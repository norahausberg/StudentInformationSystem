<%@ Page Title="Manage Courses" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCourses.aspx.vb" Inherits="StudentInformationSystem.ManageCourses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page Heading -->
    <h2>Add New Course</h2>

    <!-- Status label for success/error messages -->
    <asp:Label ID="lblCourseStatus" runat="server" CssClass="alert alert-info" Visible="false" />

    <!-- Input form for course details -->
    <div class="form-group">
        <label>Course Name</label>
        <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>ECTS</label>
        <asp:TextBox ID="txtECTS" runat="server" CssClass="form-control" TextMode="Number" />
    </div>

    <div class="form-group">
        <label>Hours</label>
        <asp:TextBox ID="txtHours" runat="server" CssClass="form-control" TextMode="Number" />
    </div>

    <div class="form-group">
        <label>Format</label>
        <asp:DropDownList ID="ddlFormat" runat="server" CssClass="form-control">
            <asp:ListItem Text="In-person" Value="In-person" />
            <asp:ListItem Text="Online" Value="Online" />
            <asp:ListItem Text="Hybrid" Value="Hybrid" />
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <label>Instructor</label>
        <asp:TextBox ID="txtInstructor" runat="server" CssClass="form-control" />
    </div>

    <!-- Submit button -->
    <br />
    <asp:Button ID="btnAddCourse" runat="server" Text="Add Course" CssClass="btn btn-success"
        OnClick="btnAddCourse_Click"
        OnClientClick="return confirm('Are you sure you want to add this course?');" />

    <!-- Course list -->
    <hr />
    <h2>All Courses</h2>
    <asp:GridView ID="gvCourses" runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-bordered"
        DataKeyNames="course_id"
        OnRowEditing="gvCourses_RowEditing"
        OnRowUpdating="gvCourses_RowUpdating"
        OnRowCancelingEdit="gvCourses_RowCancelingEdit"
        OnRowDeleting="gvCourses_RowDeleting">
        <Columns>
            <asp:BoundField DataField="course_id" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="course_name" HeaderText="Course Name" />
            <asp:BoundField DataField="ects" HeaderText="ECTS" />
            <asp:BoundField DataField="hours" HeaderText="Hours" />
            <asp:BoundField DataField="format" HeaderText="Format" />
            <asp:BoundField DataField="instructor" HeaderText="Instructor" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ButtonType="Button">
                <ItemStyle CssClass="btn-group" />
                <ControlStyle CssClass="btn btn-sm btn-outline-secondary mx-1" />
            </asp:CommandField>
        </Columns>
    </asp:GridView>

</asp:Content>
