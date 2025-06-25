<%@ Page Title="Login" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.vb" Inherits="StudentInformationSystem.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Centered login card layout -->
    <div class="d-flex justify-content-center align-items-center" style="min-height: 85vh;">
        <div class="col-md-6 col-lg-4">
            <div class="card p-4 shadow">

                <!-- Page title -->
                <h3 class="text-center mb-3">Login</h3>

                <!-- Role info box -->
                <div class="alert alert-secondary text-center small">
                    Choose your role when registering:
                    <ul class="mb-0">
                        <li><strong>Admin</strong> = Full access</li>
                        <li><strong>Student</strong> = Enroll & view info</li>
                    </ul>
                </div>

                <!-- Login status message (set in code-behind) -->
                <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info text-center d-block" Visible="false" />

                <!-- Username field -->
                <div class="form-group mb-3">
                    <label for="txtUsername">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                </div>

                <!-- Password field -->
                <div class="form-group mb-4">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                </div>

                <!-- Login button -->
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />

                <!-- Link to register page -->
                <div class="text-center mt-3">
                    <span>Don't have an account?</span>
                    <a href="Register.aspx">Register here</a>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
