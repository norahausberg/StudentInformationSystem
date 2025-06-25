<%@ Page Title="Home" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="StudentInformationSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* Hero section styling */
        .hero {
            background: url('https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=1600&q=80') no-repeat center center;
            background-size: cover;
            color: white;
            padding: 80px 20px;
            text-align: center;
            border-radius: 8px;
        }

        .hero h1 {
            font-size: 3rem;
            margin-bottom: 20px;
            color: white;
        }

        .hero p {
            font-size: 1.2rem;
            color: white;
            max-width: 800px;
            margin: 0 auto 30px auto;
        }

        .btn-section {
            margin-top: 30px;
        }
    </style>

    <!-- Admin Home Page Content -->
    <div class="hero">
        <h1>Welcome to the Student Information System</h1>
        <p>
            This web application allows you to manage key aspects of student data at your institution. 
            You can add, edit, or remove student records, create and organize course offerings, 
            enroll students in courses, and view enrollment statistics through interactive charts.
            Use the navigation buttons below to get started.
        </p>

        <!-- Quick access navigation buttons -->
        <div class="btn-section">
            <a href="ManageStudents.aspx" class="btn btn-primary btn-lg m-2">Manage Students</a>
            <a href="ManageCourses.aspx" class="btn btn-success btn-lg m-2">Manage Courses</a>
            <a href="ManageEnrollments.aspx" class="btn btn-warning btn-lg m-2">Manage Enrollments</a>
            <a href="Dashboard.aspx" class="btn btn-info btn-lg m-2">View Dashboard</a>
        </div>
    </div>

</asp:Content>
