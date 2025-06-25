Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Clear session and redirect
        Session.Clear()
        Session.Abandon()
        Response.Redirect("Login.aspx?logout=1")
    End Sub

End Class
