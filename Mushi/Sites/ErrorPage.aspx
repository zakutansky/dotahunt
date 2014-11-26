﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Mushi.Sites.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Error:</h2>
    <p></p>
    <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" Style="color: red"></asp:Label>

    <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
        <p>&nbsp;</p>
        <h4>Detailed Error:</h4>
        <p>
            <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
        </p>

        <h4>Error Handler:</h4>
        <p>
            <asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
        </p>

        <h4>Detailed Error Message:</h4>
        <p>
            <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
        </p>
        <p>
            <asp:Label ID="InnerTrace" runat="server" />
        </p>
    </asp:Panel>
    
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.4/jquery-ui.min.js" type="text/javascript"></script>
    <script src="<%= Fingerprint.CdnUrl("~/Scripts/login.min.js") %>" type="text/javascript"></script>

</asp:Content>
