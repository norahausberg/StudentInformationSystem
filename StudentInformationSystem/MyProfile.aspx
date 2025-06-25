<%@ Page Title="My Profile" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.vb" Inherits="StudentInformationSystem.MyProfile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page heading -->
    <h2>My Profile</h2>

    <!-- Message label for error notifications -->
    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger" Visible="false" />

    <!-- Main profile panel, shown only if data loads successfully -->
    <asp:Panel ID="pnlProfile" runat="server" Visible="false">
        <!-- Student Info Card -->
        <div class="card mb-4">
            <div class="card-header bg-primary text-white">Student Info</div>
            <div class="card-body">
                <p><strong>Full Name:</strong> <asp:Label ID="lblFullName" runat="server" /></p>
                <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" /></p>
                <p><strong>Username:</strong> <asp:Label ID="lblUsername" runat="server" /></p>
                <p><strong>Role:</strong> <asp:Label ID="lblRole" runat="server" /></p>
            </div>
        </div>

        <!-- Enrolled Courses Table -->
        <h4>Enrolled Courses</h4>
        <asp:GridView ID="gvCourses" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="course_name" HeaderText="Course Name" />
                <asp:BoundField DataField="enrollment_date" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
