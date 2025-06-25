Imports System.Data
Imports System.Text.RegularExpressions
Imports Npgsql

Partial Class ManageCourses
    Inherits System.Web.UI.Page

    ' On page load: check access and load courses
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        If Session("role") <> "Admin" Then
            Response.Redirect("Login.aspx?msg=access_denied")
        End If

        If Not IsPostBack Then
            LoadCourses()
        End If
    End Sub

    ' Load all course records from database
    Protected Sub LoadCourses()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim query As String = "SELECT course_id, course_name, ects, hours, format, instructor FROM courses ORDER BY course_id"
            Dim cmd As New NpgsqlCommand(query, conn)
            Dim da As New NpgsqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            gvCourses.DataSource = dt
            gvCourses.DataBind()
        End Using
    End Sub

    ' Handle course creation
    Protected Sub btnAddCourse_Click(sender As Object, e As EventArgs)
        Dim name = txtCourseName.Text.Trim()
        Dim instructor = txtInstructor.Text.Trim()

        ' Allow letters and whitespace for names
        Dim letterRegex As New Regex("^[A-Za-zÆØÅæøå\s]+$")

        ' Field-level validation
        If name = "" Or txtECTS.Text = "" Or txtHours.Text = "" Or ddlFormat.SelectedValue = "" Or instructor = "" Then
            ShowMessage("Please fill out all fields.")
            Return
        End If

        If Not letterRegex.IsMatch(name) Then
            ShowMessage("Course name must only contain letters and spaces.")
            Return
        End If

        If Not letterRegex.IsMatch(instructor) Then
            ShowMessage("Instructor name must only contain letters and spaces.")
            Return
        End If

        Dim ects As Integer
        If Not Integer.TryParse(txtECTS.Text, ects) OrElse ects < 1 OrElse ects > 15 Then
            ShowMessage("ECTS must be a number between 1 and 15.")
            Return
        End If

        Dim hours As Integer
        If Not Integer.TryParse(txtHours.Text, hours) OrElse hours < 1 OrElse hours > 100 Then
            ShowMessage("Hours must be a number between 1 and 100.")
            Return
        End If

        ' Insert into DB
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "INSERT INTO courses (course_name, ects, hours, format, instructor) VALUES (@name, @ects, @hours, @format, @instructor)"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@name", name)
            cmd.Parameters.AddWithValue("@ects", ects)
            cmd.Parameters.AddWithValue("@hours", hours)
            cmd.Parameters.AddWithValue("@format", ddlFormat.SelectedValue)
            cmd.Parameters.AddWithValue("@instructor", instructor)
            cmd.ExecuteNonQuery()
        End Using

        ' Clear inputs and refresh grid
        txtCourseName.Text = ""
        txtECTS.Text = ""
        txtHours.Text = ""
        txtInstructor.Text = ""
        ShowMessage("Course added successfully!", isSuccess:=True)
        LoadCourses()
    End Sub

    ' Enable row editing
    Protected Sub gvCourses_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvCourses.EditIndex = e.NewEditIndex
        LoadCourses()
    End Sub

    ' Cancel editing
    Protected Sub gvCourses_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvCourses.EditIndex = -1
        LoadCourses()
    End Sub

    ' Handle updates to a course
    Protected Sub gvCourses_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim id As Integer = Convert.ToInt32(gvCourses.DataKeys(e.RowIndex).Value)
        Dim row As GridViewRow = gvCourses.Rows(e.RowIndex)

        Dim name = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
        Dim ectsText = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
        Dim hoursText = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
        Dim format = CType(row.Cells(4).Controls(0), TextBox).Text.Trim()
        Dim instructor = CType(row.Cells(5).Controls(0), TextBox).Text.Trim()

        Dim letterRegex As New Regex("^[A-Za-zÆØÅæøå\s]+$")

        ' Validation again during update
        If Not letterRegex.IsMatch(name) Then
            ShowMessage("Course name must only contain letters and spaces.")
            Return
        End If

        If Not letterRegex.IsMatch(instructor) Then
            ShowMessage("Instructor name must only contain letters and spaces.")
            Return
        End If

        Dim ects As Integer
        If Not Integer.TryParse(ectsText, ects) OrElse ects < 1 OrElse ects > 15 Then
            ShowMessage("ECTS must be a number between 1 and 15.")
            Return
        End If

        Dim hours As Integer
        If Not Integer.TryParse(hoursText, hours) OrElse hours < 1 OrElse hours > 100 Then
            ShowMessage("Hours must be a number between 1 and 100.")
            Return
        End If

        ' Update DB record
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "UPDATE courses SET course_name = @name, ects = @ects, hours = @hours, format = @format, instructor = @instructor WHERE course_id = @id"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@name", name)
            cmd.Parameters.AddWithValue("@ects", ects)
            cmd.Parameters.AddWithValue("@hours", hours)
            cmd.Parameters.AddWithValue("@format", format)
            cmd.Parameters.AddWithValue("@instructor", instructor)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using

        gvCourses.EditIndex = -1
        ShowMessage("Course updated successfully!", isSuccess:=True)
        LoadCourses()
    End Sub

    ' Handle deletion of a course
    Protected Sub gvCourses_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id As Integer = Convert.ToInt32(gvCourses.DataKeys(e.RowIndex).Value)
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            conn.Open()
            Dim query As String = "DELETE FROM courses WHERE course_id = @id"
            Dim cmd As New NpgsqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using

        ShowMessage("Course deleted.", isSuccess:=True)
        LoadCourses()
    End Sub

    ' Helper function to show success/error messages
    Private Sub ShowMessage(message As String, Optional isSuccess As Boolean = False)
        lblCourseStatus.Text = message
        lblCourseStatus.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        lblCourseStatus.Visible = True
    End Sub
End Class
