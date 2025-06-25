<%@ Page Title="Manage Enrollments" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageEnrollments.aspx.vb" Inherits="StudentInformationSystem.ManageEnrollments" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Enroll in Course</h2>

    <!-- Status label for enrollment success/warning -->
    <asp:Label ID="LblEnrollStatus" runat="server" CssClass="alert alert-info" Visible="false" />

    <!-- Show student dropdown only to Admins -->
    <asp:PlaceHolder ID="phStudentDropdown" runat="server" Visible="false">
        <div class="form-group">
            <label for="DdlStudents">Student</label>
            <asp:DropDownList ID="DdlStudents" runat="server" CssClass="form-control" />
        </div>
    </asp:PlaceHolder>

    <!-- Course selection dropdown -->
    <div class="form-group">
        <label for="DdlCourses">Course</label>
        <asp:DropDownList ID="DdlCourses" runat="server" CssClass="form-control" />
    </div>

    <!-- Enroll button -->
    <br />
    <asp:Button ID="BtnEnroll" runat="server" Text="Enroll"
                CssClass="btn btn-success"
                OnClick="BtnEnroll_Click"
                OnClientClick="return confirm('Are you sure you want to enroll?');" />

    <hr />
    <h3>Current Enrollments</h3>

    <!-- Table of current enrollments -->
    <asp:GridView ID="GvEnrollments" runat="server" AutoGenerateColumns="False"
                  CssClass="table table-bordered"
                  DataKeyNames="enrollment_id"
                  OnRowDeleting="GvEnrollments_RowDeleting">
        <Columns>
            <%-- Student name is only visible for Admins --%>
            <asp:BoundField DataField="student_name" HeaderText="Student" Visible="False" />
            <asp:BoundField DataField="course_name" HeaderText="Course" />
            <asp:BoundField DataField="enrollment_date" HeaderText="Enrolled On" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="Remove" />
        </Columns>
    </asp:GridView>
</asp:Content>
