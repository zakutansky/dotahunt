<%@ OutputCache Duration="10" Location="Client" VaryByParam="None" %>

<%@ Page
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="Mushi.Login"
    Async="true" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainHeader" runat="server">
    <link href="<%= Fingerprint.CdnUrl("~/Content/tab.min.css") %>" rel="stylesheet" />
    <link href="<%= Fingerprint.CdnUrl("~/Content/ripple.min.css") %>" rel="stylesheet" />
    <link href="<%= Fingerprint.CdnUrl("~/Content/login.min.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%= Fingerprint.CdnUrl("~/Scripts/login.min.js") %>" type="text/javascript"></script>

    <div id="ksdialog" class="itemShadow">
        <div class="ksimage" onclick="RedirectToKickStarter()">
        </div>
        <div class="kscontent">
            <div class="text">
                <span class="ksname" onclick="RedirectToKickStarter()"></span>
            </div>
            <div class="pledged">
                <span></span> <%= Resources.Global.Currency %>
            </div>
            <div class="progressbar">
                <div></div>
            </div>
            <div class="pledgeinfo">
                <div class="percent">
                    <span></span>
                </div>
                <div class="daystogo">
                    <span></span> <%= Resources.Global.DaysToGo %>
                </div>
            </div>
        </div>
    </div>

    <div class="Tab">

        <asp:Panel runat="server" ID="tabHeader" ClientIDMode="Static" CssClass="tab_header">
            <div class="tabs">
                <asp:Panel runat="server" ID="tabHHireNow" onclick="TabClick(this)" CssClass="activeTab">
                    <asp:Label runat="server" Text="<%$ Resources:Global,HireAPro %>" />
                </asp:Panel>
                <asp:Panel runat="server" ID="tabHBookPro" onclick="TabClick(this)">
                    <asp:Label runat="server" Text="<%$ Resources:Global,BookAPro %>" />
                </asp:Panel>
            </div>
            <div class="tabLine"></div>
        </asp:Panel>

        <asp:Panel runat="server" ID="tabContainer" ClientIDMode="Static" CssClass="tab_body">

            <%--Hire a pro now--%>
            <asp:Panel runat="server" ID="tabHireNow" CssClass="activeTab">
                <asp:GridView ID="gvProPlayers"
                    ViewStateMode="Disabled"
                    ClientIDMode="Static"
                    runat="server"
                    OnRowDataBound="gvProPlayers_RowDataBound"
                    AutoGenerateColumns="false"
                    DataKeyNames="PlayerId"
                    ShowHeader="false"
                    GridLines="None"
                    CssClass="mushiGv"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" ImageUrl='<%# Eval("AvatarUrl") %>' CssClass="roundImage2" ToolTip="<%$ Resources:Global,DotaBuffProfile %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NickName" />
                        <asp:BoundField ItemStyle-CssClass="lblPrice" DataField="Price" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" ItemStyle-Font-Bold="true" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnCreate" runat="server" CssClass="btnCreate" Text='<%# (int)Eval("Status") == 1 ? Resources.Global.Invite : Resources.Global.Offline %>' OnClientClick='return false;' />
                                <asp:HiddenField runat="server" Value='<%# Eval("PlayerId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <%--Book a pro--%>
            <asp:Panel runat="server" ID="tabBookPro">
                <div id="bookapro">
                    <span><%= Resources.Global.BookAProText %></span>
                </div>
            </asp:Panel>

        </asp:Panel>
    </div>

    <div id="footer">
        <span>© <a href="http://www.centrigen.com">CENTRIGEN</a> - made with <span>❤</span> in Prague. Dota 2 is a registered trademark of Valve Corporation.</span>
    </div>


    <script src="<%= Fingerprint.CdnUrl("~/Scripts/ripple.min.js") %>" type="text/javascript"></script>
    <script src="<%= Fingerprint.CdnUrl("~/Scripts/tab.min.js") %>" type="text/javascript"></script>
    <script src="<%= Fingerprint.CdnUrl("~/Scripts/jquery.animate-shadow.min.js") %>" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.4/jquery-ui.min.js" type="text/javascript"></script>
</asp:Content>
