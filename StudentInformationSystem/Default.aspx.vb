Public Class _Default
    Inherits Page

    ' On page load, verify that the user is logged in and is an admin
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Redirect to login if session is missing
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        ' Redirect to student dashboard if role is not admin
        If Session("role") Is Nothing OrElse Session("role").ToString().ToLower() <> "admin" Then
            Response.Redirect("StudentHome.aspx")
        End If
    End Sub
End Class
