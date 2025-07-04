﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RegisterExternalLogin.aspx.vb" Inherits="StudentInformationSystem.RegisterExternalLogin" Async="true" %>

<%@ Import Namespace="StudentInformationSystem" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main>
        <h3>Register with your <%: ProviderName %> account</h3>
        <asp:PlaceHolder runat="server">
            <div>
                <h4>Association Form</h4>
                <hr />
                <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                <p class="text-info">
                    You've authenticated with <strong><%: ProviderName %></strong>. Please enter an email below for the current site
                    and click the Log in button.
                </p>

                <div class="row">
                    <asp:Label runat="server" AssociatedControlID="email" CssClass="col-md-2 col-form-label">Email</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="email"
                            Display="Dynamic" CssClass="text-danger" ErrorMessage="Email is required" />
                        <asp:ModelErrorMessage runat="server" ModelStateKey="email" CssClass="text-error" />
                    </div>
                </div>

                <div class="row">
                    <div class="offset-md-2 col-md-10">
                        <asp:Button runat="server" Text="Log in" CssClass="btn btn-outline-dark" OnClick="LogIn_Click" />
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
    </main>
</asp:Content>
