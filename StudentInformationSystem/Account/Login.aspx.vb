Imports Npgsql

Partial Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("msg") = "login_required" Then
                lblMessage.Text = "Please log in to continue."
                lblMessage.CssClass = "text-warning"
            ElseIf Request.QueryString("msg") = "access_denied" Then
                lblMessage.Text = "Access denied. You do not have permission to view that page."
                lblMessage.CssClass = "text-danger"
            End If
        End If
    End Sub

    Protected Sub btnLogin_Click1(sender As Object, e As EventArgs)
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Dim query As String = "SELECT username, role FROM users WHERE username = @user AND password_hash = crypt(@pass, password_hash)"

        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@user", username)
                cmd.Parameters.AddWithValue("@pass", password)

                conn.Open()
                Dim reader As NpgsqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim role As String = reader("role").ToString()

                    ' Store info in session
                    Session("username") = username
                    Session("role") = role

                    ' Redirect based on role
                    If role = "Admin" Then
                        Response.Redirect("Dashboard.aspx")
                    ElseIf role = "Student" Then
                        Response.Redirect("StudentHome.aspx")
                    End If
                Else
                    lblMessage.Text = "Invalid username or password."
                    lblMessage.CssClass = "text-danger"
                    lblMessage.Visible = True
                End If
            End Using
        End Using
    End Sub
End Class
