Imports System.Data
Imports Npgsql

Partial Class ManageEnrollments
    Inherits System.Web.UI.Page

    ' Check session and load dropdowns/grid on first page load
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        If Not IsPostBack Then
            ' Admins see student dropdown
            If Session("role").ToString().ToLower() = "admin" Then
                LoadStudents()
                phStudentDropdown.Visible = True
            End If

            LoadCourses()
            LoadEnrollments()
        End If

        ' Show/hide student column in GridView
        GvEnrollments.Columns(0).Visible = (Session("role").ToString().ToLower() = "admin")
    End Sub

    ' Load students into dropdown (admin only)
    Protected Sub LoadStudents()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim query As String = "SELECT id, first_name || ' ' || last_name AS full_name FROM students"
            Dim cmd As New NpgsqlCommand(query, conn)
            conn.Open()
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()
            DdlStudents.DataSource = reader
            DdlStudents.DataTextField = "full_name"
            DdlStudents.DataValueField = "id"
            DdlStudents.DataBind()
        End Using
    End Sub

    ' Load all available courses into dropdown
    Protected Sub LoadCourses()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim query As String = "SELECT course_id, course_name FROM courses"
            Dim cmd As New NpgsqlCommand(query, conn)
            conn.Open()
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()
            DdlCourses.DataSource = reader
            DdlCourses.DataTextField = "course_name"
            DdlCourses.DataValueField = "course_id"
            DdlCourses.DataBind()
        End Using
    End Sub

    ' Load enrollments depending on user role
    Protected Sub LoadEnrollments()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim query As String
            Dim cmd As NpgsqlCommand

            If Session("role").ToString().ToLower() = "student" Then
                ' Students only see their enrollments
                query = "SELECT e.enrollment_id, c.course_name, e.enrollment_date
                         FROM enrollments e
                         JOIN courses c ON e.course_id = c.course_id
                         WHERE e.student_id = (SELECT student_id FROM users WHERE username = @username)
                         ORDER BY e.enrollment_date DESC"
                cmd = New NpgsqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@username", Session("username").ToString())
            Else
                ' Admins see all enrollments
                query = "SELECT e.enrollment_id, s.first_name || ' ' || s.last_name AS student_name, 
                                c.course_name, e.enrollment_date
                         FROM enrollments e
                         JOIN students s ON e.student_id = s.id
                         JOIN courses c ON e.course_id = c.course_id
                         ORDER BY e.enrollment_date DESC"
                cmd = New NpgsqlCommand(query, conn)
            End If

            Dim da As New NpgsqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            GvEnrollments.DataSource = dt
            GvEnrollments.DataBind()
        End Using
    End Sub

    ' Handle new enrollment
    Protected Sub BtnEnroll_Click(sender As Object, e As EventArgs)
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim studentId As Integer

            ' Admin selects from dropdown, students auto-linked
            If Session("role").ToString().ToLower() = "admin" Then
                studentId = Convert.ToInt32(DdlStudents.SelectedValue)
            Else
                Dim queryStudentId As String = "SELECT student_id FROM users WHERE username = @username"
                Using cmd As New NpgsqlCommand(queryStudentId, conn)
                    cmd.Parameters.AddWithValue("@username", Session("username").ToString())
                    studentId = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End If

            Dim courseId As Integer = Convert.ToInt32(DdlCourses.SelectedValue)

            ' Prevent duplicate enrollments
            Dim checkQuery As String = "SELECT COUNT(*) FROM enrollments WHERE student_id = @student_id AND course_id = @course_id"
            Using checkCmd As New NpgsqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@student_id", studentId)
                checkCmd.Parameters.AddWithValue("@course_id", courseId)

                Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If exists > 0 Then
                    LblEnrollStatus.CssClass = "alert alert-warning"
                    LblEnrollStatus.Text = "This student is already enrolled in the selected course."
                    LblEnrollStatus.Visible = True
                    Return
                End If
            End Using

            ' Insert new enrollment
            Dim insertQuery As String = "INSERT INTO enrollments (student_id, course_id, enrollment_date)
                                         VALUES (@student_id, @course_id, CURRENT_DATE)"
            Using cmd As New NpgsqlCommand(insertQuery, conn)
                cmd.Parameters.AddWithValue("@student_id", studentId)
                cmd.Parameters.AddWithValue("@course_id", courseId)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        LblEnrollStatus.CssClass = "alert alert-success"
        LblEnrollStatus.Text = "Enrollment successful."
        LblEnrollStatus.Visible = True
        LoadEnrollments()
    End Sub

    ' Handle enrollment deletion
    Protected Sub GvEnrollments_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id As Integer = Convert.ToInt32(GvEnrollments.DataKeys(e.RowIndex).Value)
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "DELETE FROM enrollments WHERE enrollment_id = @id"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using

        LblEnrollStatus.CssClass = "alert alert-warning"
        LblEnrollStatus.Text = "Enrollment removed."
        LblEnrollStatus.Visible = True
        LoadEnrollments()
    End Sub
End Class
