Imports Microsoft.AspNet.Identity
Imports System.Web.UI.HtmlControls

Public Class SiteMaster
    Inherits MasterPage

    ' Constants for XSRF protection
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String

    ' Handles Anti-XSRF token creation and persistence
    Protected Sub Page_Init(sender As Object, e As EventArgs)
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid

        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {
                .HttpOnly = True,
                .Value = _antiXsrfTokenValue
            }

            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
                responseCookie.Secure = True
            End If

            Response.Cookies.[Set](responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    ' Validates Anti-XSRF token before page load
    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)
        Else
            If DirectCast(ViewState(AntiXsrfTokenKey), String) <> _antiXsrfTokenValue OrElse
               DirectCast(ViewState(AntiXsrfUserNameKey), String) <> (If(Context.User.Identity.Name, String.Empty)) Then
                Throw New InvalidOperationException("Validation of Anti-XSRF token failed.")
            End If
        End If
    End Sub

    ' Handles user-based logic: navigation visibility, user label, and home link redirect
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("username") IsNot Nothing AndAlso Session("role") IsNot Nothing Then
                Dim username As String = Session("username").ToString()
                Dim role As String = Session("role").ToString().ToLower()

                ' Show logged-in username and logout button
                litUserInfo.Text = $"<span class='navbar-text me-3 text-white'>Logged in as: <strong>{username}</strong> ({role})</span>" &
                                   $"<a href='Login.aspx?logout=1' class='btn btn-sm btn-outline-light'>Logout</a>"

                ' Update home link depending on role
                Dim homeLink = TryCast(FindControlRecursive(Me, "homeLink"), HtmlAnchor)
                If homeLink IsNot Nothing Then
                    homeLink.HRef = If(role = "student", "StudentHome.aspx", "Default.aspx")
                End If

                ' If student, hide admin links and show My Profile
                If role = "student" Then
                    Dim liManageStudents = FindControlRecursive(Me, "liManageStudents")
                    Dim liManageCourses = FindControlRecursive(Me, "liManageCourses")
                    If liManageStudents IsNot Nothing Then liManageStudents.Visible = False
                    If liManageCourses IsNot Nothing Then liManageCourses.Visible = False

                    Dim phProfile = FindControlRecursive(Me, "phStudentNav")
                    If phProfile IsNot Nothing Then phProfile.Visible = True
                End If
            End If
        End If
    End Sub

    ' Recursive utility method for finding controls across master/content pages
    Private Function FindControlRecursive(root As Control, id As String) As Control
        If root.ID = id Then Return root
        For Each c As Control In root.Controls
            Dim t = FindControlRecursive(c, id)
            If t IsNot Nothing Then Return t
        Next
        Return Nothing
    End Function

    ' Default logging out event (not actively used)
    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
End Class
