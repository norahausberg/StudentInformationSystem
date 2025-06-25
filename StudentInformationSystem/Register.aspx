<%@ Page Title="Register" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.vb" Inherits="StudentInformationSystem.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center mt-5">
        <div class="col-md-6">
            <h2 class="text-center">Create Account</h2>

            <!-- Show summary of validation errors -->
            <asp:ValidationSummary ID="vsErrors" runat="server" CssClass="text-danger" HeaderText="Please fix the following:" />

            <!-- Registration message (success or error) -->
            <asp:Label ID="lblRegisterMessage" runat="server" CssClass="text-danger" />

            <!-- Username field -->
            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Password field -->
            <div class="form-group mt-2">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Full name field -->
            <div class="form-group mt-2">
                <label for="txtFullName">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required." CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtFullName"
                    ErrorMessage="Name can only contain letters and spaces."
                    ValidationExpression="^[A-Za-zæøåÆØÅ\s\-]+$" CssClass="text-danger" Display="Dynamic" />
            </div>

            <!-- Email field -->
            <div class="form-group mt-2">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Invalid email format." CssClass="text-danger" Display="Dynamic"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            </div>

            <!-- Role dropdown -->
            <div class="form-group mt-2">
                <label for="ddlRole">Role</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Student" Value="Student" />
                    <asp:ListItem Text="Admin" Value="Admin" />
                </asp:DropDownList>
            </div>

            <!-- Submit button -->
            <asp:Button ID="btnRegister" runat="server" Text="Register"
                        CssClass="btn btn-success mt-3"
                        OnClick="btnRegister_Click" />
        </div>
    </div>
</asp:Content>
