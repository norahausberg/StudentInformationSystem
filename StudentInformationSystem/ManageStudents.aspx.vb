Imports System.Data
Imports Npgsql
Imports System.Text.RegularExpressions

Partial Class ManageStudents
    Inherits System.Web.UI.Page

    ' Session check and student table load
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        If Session("role") <> "Admin" Then
            Response.Redirect("Login.aspx?msg=access_denied")
        End If

        If Not IsPostBack Then
            txtEnrollmentDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            lblStatus.Visible = False
            LoadStudents()
        End If
    End Sub

    ' Load all students into GridView
    Protected Sub LoadStudents()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "SELECT id, first_name, last_name, email, enrollment_date FROM students ORDER BY id"
            Dim cmd As New NpgsqlCommand(query, conn)
            Dim da As New NpgsqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            gvStudents.DataSource = dt
            gvStudents.DataBind()
        End Using
    End Sub

    ' Add new student to the database
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim firstName As String = txtFirstName.Text.Trim()
        Dim lastName As String = txtLastName.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()
        Dim enrollmentDateStr As String = txtEnrollmentDate.Text.Trim()

        Dim namePattern As String = "^[A-Za-zæøåÆØÅ' \-]+$"

        ' Validate names
        If Not Regex.IsMatch(firstName, namePattern) OrElse Not Regex.IsMatch(lastName, namePattern) Then
            lblStatus.Text = "Invalid name. Use only letters, hyphens, apostrophes, and spaces."
            lblStatus.CssClass = "alert alert-danger"
            lblStatus.Visible = True
            Return
        End If

        ' Validate email
        If Not email.Contains("@") OrElse email.Length < 5 Then
            lblStatus.Text = "Invalid email address."
            lblStatus.CssClass = "alert alert-danger"
            lblStatus.Visible = True
            Return
        End If

        ' Validate date
        Dim enrollmentDate As Date
        If Not Date.TryParse(enrollmentDateStr, enrollmentDate) Then
            lblStatus.Text = "Invalid enrollment date."
            lblStatus.CssClass = "alert alert-danger"
            lblStatus.Visible = True
            Return
        End If

        ' Insert into DB
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "INSERT INTO students (first_name, last_name, email, enrollment_date) VALUES (@first, @last, @mail, @date)"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@first", firstName)
            cmd.Parameters.AddWithValue("@last", lastName)
            cmd.Parameters.AddWithValue("@mail", email)
            cmd.Parameters.AddWithValue("@date", enrollmentDate)
            cmd.ExecuteNonQuery()
        End Using

        ' Reset form
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtEmail.Text = ""
        txtEnrollmentDate.Text = DateTime.Now.ToString("yyyy-MM-dd")

        LoadStudents()
        lblStatus.Text = "Student added successfully!"
        lblStatus.CssClass = "alert alert-success"
        lblStatus.Visible = True
    End Sub

    ' Start editing a row
    Protected Sub gvStudents_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvStudents.EditIndex = e.NewEditIndex
        LoadStudents()
    End Sub

    ' Cancel edit mode
    Protected Sub gvStudents_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvStudents.EditIndex = -1
        LoadStudents()
    End Sub

    ' Update student info in DB
    Protected Sub gvStudents_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim id As Integer = Convert.ToInt32(gvStudents.DataKeys(e.RowIndex).Value)
        Dim row As GridViewRow = gvStudents.Rows(e.RowIndex)

        Dim firstName As String = CType(row.Cells(1).Controls(0), TextBox).Text
        Dim lastName As String = CType(row.Cells(2).Controls(0), TextBox).Text
        Dim email As String = CType(row.Cells(3).Controls(0), TextBox).Text
        Dim enrollmentDate As String = CType(row.Cells(4).Controls(0), TextBox).Text

        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "UPDATE students SET first_name = @first, last_name = @last, email = @mail, enrollment_date = @date WHERE id = @id"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@first", firstName)
            cmd.Parameters.AddWithValue("@last", lastName)
            cmd.Parameters.AddWithValue("@mail", email)
            cmd.Parameters.AddWithValue("@date", Date.Parse(enrollmentDate))
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using

        gvStudents.EditIndex = -1
        LoadStudents()
        lblStatus.Text = "Student updated successfully!"
        lblStatus.CssClass = "alert alert-success"
        lblStatus.Visible = True
    End Sub

    ' Delete student and related data
    Protected Sub gvStudents_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim id As Integer = Convert.ToInt32(gvStudents.DataKeys(e.RowIndex).Value)

            Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                ' Delete enrollments first
                Dim cmdEnrollments As New NpgsqlCommand("DELETE FROM enrollments WHERE student_id = @id", conn)
                cmdEnrollments.Parameters.AddWithValue("@id", id)
                cmdEnrollments.ExecuteNonQuery()

                ' Delete user account if any
                Dim cmdUsers As New NpgsqlCommand("DELETE FROM users WHERE student_id = @id", conn)
                cmdUsers.Parameters.AddWithValue("@id", id)
                cmdUsers.ExecuteNonQuery()

                ' Delete student record
                Dim cmdStudent As New NpgsqlCommand("DELETE FROM students WHERE id = @id", conn)
                cmdStudent.Parameters.AddWithValue("@id", id)
                cmdStudent.ExecuteNonQuery()
            End Using

            lblStatus.Text = "Student deleted successfully!"
            lblStatus.CssClass = "alert alert-warning"
            lblStatus.Visible = True
            LoadStudents()
        Catch ex As Exception
            lblStatus.Text = "Error deleting student: " & ex.Message
            lblStatus.CssClass = "alert alert-danger"
            lblStatus.Visible = True
        End Try
    End Sub

    ' Confirm deletion via JavaScript prompt
    Protected Sub gvStudents_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each ctrl As Control In e.Row.Cells(5).Controls
                If TypeOf ctrl Is Button AndAlso CType(ctrl, Button).CommandName = "Delete" Then
                    CType(ctrl, Button).OnClientClick = "return confirm('Are you sure you want to delete this student?');"
                End If
            Next
        End If
    End Sub
End Class
