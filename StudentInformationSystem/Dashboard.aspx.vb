Imports System.Data
Imports System.Web.Script.Serialization
Imports Npgsql

Partial Class Dashboard
    Inherits System.Web.UI.Page

    ' Redirect to login if session is not active
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("username") Is Nothing Then
            Response.Redirect("Login.aspx?msg=login_required")
        End If

        If Not IsPostBack Then
            LoadChartData()
        End If
    End Sub

    ' Loads enrollment data from the database and injects it as JS variables
    Private Sub LoadChartData()
        Dim connString As String = ConfigurationManager.ConnectionStrings("SupabaseConnection").ConnectionString

        ' SQL query: Count enrollments per course (including 0 using LEFT JOIN)
        Dim query As String = "
            SELECT c.course_name, COUNT(e.enrollment_id) AS student_count
            FROM courses c
            LEFT JOIN enrollments e ON c.course_id = e.course_id
            GROUP BY c.course_name
            ORDER BY c.course_name;
        "

        Dim courseNames As New List(Of String)()       ' List of course names
        Dim studentCounts As New List(Of Integer)()     ' List of enrollment counts

        ' Execute the query and store results
        Using conn As New NpgsqlConnection(connString)
            Using cmd As New NpgsqlCommand(query, conn)
                conn.Open()
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        courseNames.Add(reader("course_name").ToString())
                        studentCounts.Add(Convert.ToInt32(reader("student_count")))
                    End While
                End Using
            End Using
        End Using

        ' Serialize lists to JSON strings for JS use
        Dim serializer As New JavaScriptSerializer()
        Dim courseJson As String = serializer.Serialize(courseNames)
        Dim countJson As String = serializer.Serialize(studentCounts)

        ' Inject JS variables into the page to use with Chart.js
        Dim jsScript As String = "
            <script type='text/javascript'>
                var courseLabels = " & courseJson & ";
                var studentCounts = " & countJson & ";
            </script>"

        ' Register the script so it's available to client-side Chart.js
        ClientScript.RegisterStartupScript(Me.GetType(), "ChartData", jsScript)
    End Sub
End Class
