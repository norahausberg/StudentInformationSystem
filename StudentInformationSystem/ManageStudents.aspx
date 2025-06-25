<%@ Page Title="Manage Students" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStudents.aspx.vb" Inherits="StudentInformationSystem.ManageStudents" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mt-4">Add New Student</h2>

    <!-- Status label for displaying success or error messages -->
    <asp:Label ID="lblStatus" runat="server" CssClass="alert alert-info mt-3 d-block" Visible="false" />

    <!-- Input fields for student creation -->
    <div class="form-group">
        <label for="txtFirstName">First Name</label>
        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label for="txtLastName">Last Name</label>
        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label for="txtEmail">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label for="txtEnrollmentDate">Enrollment Date</label>
        <asp:TextBox ID="txtEnrollmentDate" runat="server" CssClass="form-control" Width="150px" TextMode="Date" />
    </div>

    <!-- Add student button -->
    <asp:Button ID="btnAdd" runat="server" Text="Add Student"
                CssClass="btn btn-success mt-2"
                OnClick="btnAdd_Click"
                OnClientClick="return confirm('Are you sure you want to add this student?');" />

    <hr />
    <h2 class="mt-4">All Students</h2>

    <!-- Student table with CRUD functionality -->
    <asp:GridView ID="gvStudents" runat="server"
                  CssClass="table table-bordered table-hover"
                  AutoGenerateColumns="False"
                  DataKeyNames="id"
                  EnableViewState="true"
                  OnRowEditing="gvStudents_RowEditing"
                  OnRowCancelingEdit="gvStudents_RowCancelingEdit"
                  OnRowUpdating="gvStudents_RowUpdating"
                  OnRowDeleting="gvStudents_RowDeleting"
                  OnRowDataBound="gvStudents_RowDataBound">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="first_name" HeaderText="First Name" />
            <asp:BoundField DataField="last_name" HeaderText="Last Name" />
            <asp:BoundField DataField="email" HeaderText="Email" />
            <asp:BoundField DataField="enrollment_date" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                              EditText="Edit" DeleteText="Delete"
                              ButtonType="Button"
                              CausesValidation="false" />
        </Columns>
    </asp:GridView>

</asp:Content>
