Imports System.Data
Imports Npgsql
Imports System.Web.Security

Partial Class Login
    Inherits System.Web.UI.Page

    ' Triggered when the page is first loaded
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' If logout was requested via URL query
            If Request.QueryString("logout") = "1" Then
                Session.Clear() ' Clear session data
                Session.Abandon() ' Fully terminate session
                lblMessage.Text = "You have been successfully logged out."
                lblMessage.CssClass = "alert alert-success w-100 text-center"
                lblMessage.Visible = True
            End If
        End If
    End Sub

    ' Triggered when user clicks the login button
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        ' Database connection and query
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Dim query As String = "SELECT username, role FROM users WHERE username = @user AND password_hash = crypt(@pass, password_hash)"

        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@user", username)
                cmd.Parameters.AddWithValue("@pass", password)

                conn.Open()
                Dim reader As NpgsqlDataReader = cmd.ExecuteReader()

                ' If user credentials are valid
                If reader.Read() Then
                    Dim role As String = reader("role").ToString()

                    ' Set session for logged in user
                    Session("username") = username
                    Session("role") = role

                    ' Redirect based on role
                    If role = "Admin" Then
                        Response.Redirect("Default.aspx")
                    ElseIf role = "Student" Then
                        Response.Redirect("StudentHome.aspx")
                    Else
                        lblMessage.Text = "Unknown role type."
                        lblMessage.CssClass = "alert alert-warning w-100 text-center"
                        lblMessage.Visible = True
                    End If

                Else
                    ' Invalid login credentials
                    lblMessage.Text = "Invalid username or password."
                    lblMessage.CssClass = "alert alert-danger w-100 text-center"
                    lblMessage.Visible = True
                End If
            End Using
        End Using
    End Sub
End Class
