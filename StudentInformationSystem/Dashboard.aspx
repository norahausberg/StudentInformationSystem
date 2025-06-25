<%@ Page Title="Dashboard" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.vb" Inherits="StudentInformationSystem.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Student Enrollment per Course</h2>

    <!-- Chart container -->
    <canvas id="enrollmentChart" width="800" height="400"></canvas>

    <!-- Load Chart.js library from CDN -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <!-- Chart rendering script (gets data from server-injected JS variables) -->
    <script type="text/javascript">
        window.onload = function () {
            var ctx = document.getElementById('enrollmentChart').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: courseLabels, // Provided by backend
                    datasets: [{
                        label: 'Number of Enrollments',
                        data: studentCounts, // Provided by backend
                        backgroundColor: 'rgba(40, 167, 69, 0.5)', // Bootstrap green
                        borderColor: 'rgba(40, 167, 69, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true // Y-axis starts at 0
                        }
                    }
                }
            });
        };
    </script>

</asp:Content>
