<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowAvailability.aspx.cs" Inherits="ShowAvailability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>זמינות מתגברים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>

    <asp:SqlDataSource ID="availabilityDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select Ave_TeaId,Ave_day,Ave_startHour, Ave_endtHour,
            (tea_firstName + ' '+ tea_lastName) as 'teacher_full_name' from TeacheravAilability inner join Teacher on Ave_TeaId=Tea_Id"></asp:SqlDataSource>

    <asp:GridView ID="availabilityGV" runat="server" DataSourceID="availabilityDS" Gridlines="Both" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto; margin-top: 20px ; margin-bottom:70px; text-align: center; width: 80%; direction:rtl;"  CssClass="grid" OnRowDataBound="availabilityGV_RowDataBound" OnDataBound="availabilityGV_DataBound" CellPadding="4" ForeColor="#333333">
        <Columns>
            <asp:BoundField DataField="teacher_full_name" HeaderText="שם המתגבר" SortExpression="teacher_full_name" />
            <asp:BoundField DataField="Ave_day" HeaderText="יום בשבוע" SortExpression="Ave_day" />
            <asp:BoundField DataField="Ave_startHour" HeaderText="משעה" SortExpression="Ave_startHour" DataFormatString="{0:hh\:mm}"/>
            <asp:BoundField DataField="Ave_endtHour" HeaderText="עד שעה" SortExpression="Ave_endtHour" DataFormatString="{0:hh\:mm}"/>
          </Columns>

        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

    </asp:GridView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

