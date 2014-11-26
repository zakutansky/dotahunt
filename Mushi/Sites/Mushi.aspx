<%@ Page
    Title=""
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeBehind="Mushi.aspx.cs"
    Inherits="Mushi.Sites.Mushi"
    EnableViewState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHeader" runat="server">
    <link href="http://az683697.vo.msecnd.net/dotahuntcdn/tab.min.css" rel="stylesheet" />    
    <link href="http://az683697.vo.msecnd.net/dotahuntcdn/ripple.min.css" rel="stylesheet" />    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager runat="server" />

    <div class="Tab">

        <asp:Panel runat="server" ID="tabHeader" ClientIDMode="Static" CssClass="tab_header">
            <div class="tabs">
                <asp:Panel runat="server" ID="tabHHireList" onclick="TabClick(this)" CssClass="activeTab">
                    <asp:Label runat="server" Text="<%$ Resources:Global,HireList %>" />
                </asp:Panel>
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
            <%--Hire orders--%>
            <asp:Panel runat="server" ID="tabHireList" CssClass="activeTab">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row player-row">
                            <div class="col-md-4">
                                <asp:CheckBox ID="cbAvailability"
                                    Text="Set as Available"
                                    runat="server"
                                    ViewStateMode="Enabled"
                                    OnCheckedChanged="SetAsAvailable"
                                    AutoPostBack="true"
                                    ClientIDMode="AutoID" />
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblPrice"
                                    runat="server"
                                    Text="Set price" />
                                <asp:TextBox ID="tbPrice"
                                    runat="server"
                                    ClientIDMode="Static"
                                    onKeyDown="return Digitonly(this)" />
                                <asp:Button ID="btnSetPrice"
                                    ViewStateMode="Enabled"
                                    runat="server"
                                    Text="Set price"
                                    OnClick="SetPrice_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:GridView ID="gvAmateurPlayers"
                    ClientIDMode="Static"
                    ViewStateMode="Disabled"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="PlayerId"
                    ShowHeaderWhenEmpty="true"
                    GridLines="None"
                    CssClass="mushiGv"
                    BorderStyle="None"
                    EmptyDataText="No Players"
                    Width="100%">
                    <Columns>
                        <asp:ImageField DataImageUrlField="AvatarUrl" />
                        <asp:BoundField HeaderText="Nick" DataField="NickName" />
                        <asp:BoundField ItemStyle-CssClass="lblStatus" HeaderText="Status" DataField="StatusName" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" CssClass="btnAccept" Text="Accept" OnClientClick='<%# "AcceptOrder(\""+ Eval("PlayerId") + "\"); return false;" %>' Style='<%# (int)Eval("Status") == 2 ? "": "display: none" %>' />
                                <asp:Button runat="server" CssClass="btnAbort" Text="Abort" OnClientClick='<%# "AbortOrder(\""+ Eval("PlayerId") + "\"); return false;" %>' Style='<%# (int)Eval("Status") < 5 ? "": "display: none" %>' />
                                <asp:HiddenField runat="server" Value='<%# Eval("PlayerId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle Height="100px" HorizontalAlign="Center" Font-Size="20px" />
                </asp:GridView>
            </asp:Panel>

            <%--Hire a pro now--%>
            <asp:Panel runat="server" ID="tabHireNow" CssClass="activeTab">
                <asp:GridView ID="gvProPlayers"
                    OnRowDataBound="gvProPlayers_RowDataBound"
                    ViewStateMode="Disabled"
                    ClientIDMode="Static"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="PlayerId"
                    ShowHeader="false"
                    GridLines="None"
                    CssClass="mushiGv"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgAvatar" runat="server" ImageUrl='<%# Eval("AvatarUrl") %>' CssClass="roundImage2" ToolTip="<%$ Resources:Global,DotaBuffProfile %>" onclick='<%# "PlayerInfo(\"" + Eval("ProfileUrl") + "\",\"" + Resources.Global.ConfirmPlayerProfile + "\");" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NickName" />
                        <asp:BoundField ItemStyle-CssClass="lblPrice" DataField="Price" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" ItemStyle-Font-Bold="true" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnCreate" CssClass="btnCreate" Text='<%# (int)Eval("Status") == 1 ? Resources.Global.Invite : Resources.Global.Offline %>' OnClientClick='<%# "CreateOrder(\""+ Eval("PlayerId") + "\"); return false;" %>' Style='<%# (int)Eval("Status") > 1 ? "display: none": "" %>' />
                                <asp:Button runat="server" CssClass="btnPay greenBtn" Text="<%$ Resources:Global,Pay %>" OnClientClick='<%# "PayOrder(\""+ Eval("PlayerId") + "\"); return false;" %>' Style='<%# (int)Eval("Status") == 3 ? "": "display: none" %>' />
                                <asp:Button runat="server" CssClass="btnAbort" Text="<%$ Resources:Global,Abort %>" OnClientClick='<%# "AbortOrder(\""+ Eval("PlayerId") + "\"); return false;" %>' Style='<%# (int)Eval("Status") > 1 && (int)Eval("Status") < 4 ? "": "display: none" %>' />
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
    <!--Script references. -->
    <!--Reference the jQuery library. -->
    <script src="https://ajax.aspnetcdn.com/ajax/signalr/jquery.signalr-2.1.1.min.js" type="text/javascript"></script>
    <script src="http://az683697.vo.msecnd.net/dotahuntcdn/ripple.min.js" type="text/javascript"></script>
    <script src="http://az683697.vo.msecnd.net/dotahuntcdn/tab.min.js" type="text/javascript"></script>
    <script src="http://az683697.vo.msecnd.net/dotahuntcdn/mushi.min.js" type="text/javascript"></script>
    <script src="../signalr/hubs"></script>
</asp:Content>
