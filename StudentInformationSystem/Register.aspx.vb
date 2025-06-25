Imports System.Data
Imports Npgsql
Imports System.Text.RegularExpressions
Imports System.Web

Partial Class Register
    Inherits System.Web.UI.Page

    ' Handles registration logic when the button is clicked
    Protected Sub btnRegister_Click(sender As Object, e As EventArgs)
        ' If any validation fails, exit
        If Not Page.IsValid Then
            lblRegisterMessage.Text = "There are errors on the form. Please review and correct them."
            lblRegisterMessage.CssClass = "text-danger"
            Return
        End If

        ' Get inputs
        Dim username As String = HttpUtility.HtmlEncode(txtUsername.Text.Trim())
        Dim password As String = txtPassword.Text.Trim()
        Dim role As String = ddlRole.SelectedValue
        Dim fullName As String = txtFullName.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()

        ' Extra server-side validation
        If password.Length < 6 Then
            lblRegisterMessage.Text = "Password must be at least 6 characters long."
            lblRegisterMessage.CssClass = "text-danger"
            Return
        End If

        If password.ToLower() = username.ToLower() Then
            lblRegisterMessage.Text = "Password cannot be the same as username."
            lblRegisterMessage.CssClass = "text-danger"
            Return
        End If

        If Not Regex.IsMatch(fullName, "^[A-Za-zæøåÆØÅ\s\-]+$") Then
            lblRegisterMessage.Text = "Full name can only contain letters and spaces."
            lblRegisterMessage.CssClass = "text-danger"
            Return
        End If

        If Not Regex.IsMatch(email, "^[^@\s]+@[^@\s]+\.[^@\s]+$") Then
            lblRegisterMessage.Text = "Invalid email format."
            lblRegisterMessage.CssClass = "text-danger"
            Return
        End If

        ' DB operations
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()

            ' 1. Check if username already exists
            Dim checkUserQuery As String = "SELECT COUNT(*) FROM users WHERE username = @user"
            Using checkCmd As New NpgsqlCommand(checkUserQuery, conn)
                checkCmd.Parameters.AddWithValue("@user", username)
                If Convert.ToInt32(checkCmd.ExecuteScalar()) > 0 Then
                    lblRegisterMessage.Text = "Username already exists."
                    lblRegisterMessage.CssClass = "text-danger"
                    Return
                End If
            End Using

            ' 2. Match student record (by full name + email)
            Dim studentId As Integer = -1
            Dim studentQuery As String = "SELECT id FROM students WHERE LOWER(email) = LOWER(@mail) AND LOWER(first_name || ' ' || last_name) = LOWER(@fullname)"
            Using studentCmd As New NpgsqlCommand(studentQuery, conn)
                studentCmd.Parameters.AddWithValue("@mail", email)
                studentCmd.Parameters.AddWithValue("@fullname", fullName)

                Dim result = studentCmd.ExecuteScalar()
                If result Is Nothing Then
                    lblRegisterMessage.Text = "No matching student found for that name and email."
                    lblRegisterMessage.CssClass = "text-danger"
                    Return
                End If
                studentId = Convert.ToInt32(result)
            End Using

            ' 3. Insert new user with hashed password
            Dim insertQuery As String = "INSERT INTO users (username, password_hash, role, student_id) VALUES (@user, crypt(@pass, gen_salt('bf')), @role, @student_id)"
            Using insertCmd As New NpgsqlCommand(insertQuery, conn)
                insertCmd.Parameters.AddWithValue("@user", username)
                insertCmd.Parameters.AddWithValue("@pass", password)
                insertCmd.Parameters.AddWithValue("@role", role)
                insertCmd.Parameters.AddWithValue("@student_id", studentId)
                insertCmd.ExecuteNonQuery()
            End Using
        End Using

        ' Success message + redirect to login
        lblRegisterMessage.CssClass = "text-success"
        lblRegisterMessage.Text = "User registered successfully! Redirecting to login..."
        Response.AddHeader("REFRESH", "2;URL=Login.aspx")
    End Sub
End Class
