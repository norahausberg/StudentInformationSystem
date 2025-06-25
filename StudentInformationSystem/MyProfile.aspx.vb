Imports System.Data
Imports Npgsql

Partial Class MyProfile
    Inherits System.Web.UI.Page

    ' Restrict access to students only
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("username") Is Nothing OrElse Session("role").ToString().ToLower() <> "student" Then
            Response.Redirect("Login.aspx?msg=unauthorized")
            Return
        End If

        If Not IsPostBack Then
            LoadStudentProfile()
        End If
    End Sub

    ' Loads the current student's profile and enrolled courses
    Private Sub LoadStudentProfile()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

        Using conn As New NpgsqlConnection(connString)
            conn.Open()

            ' Get student's full name, email, username, and role
            Dim infoQuery As String = "SELECT s.first_name || ' ' || s.last_name AS full_name, s.email, u.username, u.role
                                       FROM users u
                                       JOIN students s ON u.student_id = s.id
                                       WHERE u.username = @username"

            Using cmd As New NpgsqlCommand(infoQuery, conn)
                cmd.Parameters.AddWithValue("@username", Session("username").ToString())
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        lblFullName.Text = reader("full_name").ToString()
                        lblEmail.Text = reader("email").ToString()
                        lblUsername.Text = reader("username").ToString()
                        lblRole.Text = reader("role").ToString()
                        pnlProfile.Visible = True
                    Else
                        lblMessage.Text = "Student profile not found."
                        lblMessage.Visible = True
                        pnlProfile.Visible = False
                        Return
                    End If
                End Using
            End Using

            ' Get student's enrolled courses
            Dim courseQuery As String = "SELECT c.course_name, e.enrollment_date
                                         FROM enrollments e
                                         JOIN courses c ON e.course_id = c.course_id
                                         WHERE e.student_id = (
                                            SELECT student_id FROM users WHERE username = @username
                                         )
                                         ORDER BY e.enrollment_date DESC"

            Using cmd As New NpgsqlCommand(courseQuery, conn)
                cmd.Parameters.AddWithValue("@username", Session("username").ToString())
                Dim dt As New DataTable()
                Dim da As New NpgsqlDataAdapter(cmd)
                da.Fill(dt)
                gvCourses.DataSource = dt
                gvCourses.DataBind()
            End Using
        End Using
    End Sub
End Class
