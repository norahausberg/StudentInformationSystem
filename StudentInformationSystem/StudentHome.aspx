<%@ Page Title="Student Home" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentHome.aspx.vb" Inherits="StudentInformationSystem.StudentHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Hero section for student landing page -->
    <style>
        .student-hero {
            background: url('https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=1600&q=80') no-repeat center center;
            background-size: cover;
            color: white;
            padding: 80px 20px;
            text-align: center;
            border-radius: 8px;
        }

        .student-hero h1 {
            font-size: 3rem;
            margin-bottom: 20px;
        }

        .student-hero p {
            font-size: 1.3rem;
        }

        .btn-section {
            margin-top: 40px;
        }
    </style>

    <!-- Welcome message and action buttons -->
    <div class="student-hero">
        <h1>Welcome, <%= Session("username") %>!</h1>
        <p>Use the options below to manage your courses, check your profile, and view your progress.</p>

        <div class="btn-section">
            <a href="ManageEnrollments.aspx" class="btn btn-primary btn-lg m-2">Enroll in Courses</a>
            <a href="Dashboard.aspx" class="btn btn-success btn-lg m-2">View Dashboard</a>
            <a href="MyProfile.aspx" class="btn btn-secondary btn-lg m-2">My Profile</a>
        </div>
    </div>

</asp:Content>
