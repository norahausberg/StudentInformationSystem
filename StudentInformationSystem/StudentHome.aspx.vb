Public Class StudentHome
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' 🔒 Check login and role access
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        If Session("role") <> "Student" Then
            Response.Redirect("Login.aspx?msg=access_denied")
        End If
    End Sub
End Class
