<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMasterPage.master" AutoEventWireup="true" CodeFile="studentRequests.aspx.cs" Inherits="studentRequests" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .container.content {
            direction: rtl;
        }

        .straightLeft {
            position: relative;
            right: 65px;
        }
        .blueHeader {
            color: #0094ff;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="student_calendar.aspx">בית</a></li>
                <li class="active">הבקשות שלי</li>

            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>הבקשות שלי</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>
    <div class="container content">
        <asp:DropDownList ID="statusDDL" runat="server" AutoPostBack="true" Style="text-align: right; margin-top: 15px" CssClass="filterTB straightLeft">
            <asp:ListItem Text="כל הבקשות" Value="" />
            <asp:ListItem Value="2">בקשות ממתינות</asp:ListItem>
            <asp:ListItem Value="1">בקשות שאושרו</asp:ListItem>
            <asp:ListItem Value="0">בקשות שנדחו</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="stuReqDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select DISTINCT Requests.req_number, pro_title,
            (tea_firstName + ' '+ tea_lastName) as 'teacher_full_name',actLes_date, Les_startHour, Les_EndHour, Les_Day,req_type, req_status  
            from ((((requests inner join student on req_stu_id= stu_id) inner join lesson on req_actLes_id= les_id ) inner join Teacher on Les_tea_Id= tea_Id) inner join Profession on les_pro_id= pro_id) inner join ActualLesson on ActLes_LesId= req_actLes_id where actLes_date = req_actLes_date and stu_id=@current_stu_id AND actls_cancelled=0"
            FilterExpression="(req_status)= '{0}'">
            <FilterParameters>
                <asp:ControlParameter ControlID="statusDDL" PropertyName="SelectedValue" />
            </FilterParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="stuReqGV" runat="server"  EmptyDataText="אין בקשות להציג" CssClass="grid" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto; margin-top: 20px; margin-bottom: 70px; text-align: center; width: 90%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowPaging="True" PageSize="20" DataKeyNames="req_number" DataSourceID="stuReqDS" OnRowDataBound="stuReqGV_RowDataBound">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <RowStyle Height="30px" />
            <Columns>
                <asp:BoundField DataField="req_status" HeaderText="סטטוס הבקשה" SortExpression="req_status" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="req_type" HeaderText="סוג הבקשה" SortExpression="req_type" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="pro_title" HeaderText="מקצוע" SortExpression="pro_title" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="teacher_full_name" HeaderText="שם המתגבר" SortExpression="teacher_full_name" ReadOnly="True" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="actLes_date" HeaderText="תאריך" SortExpression="actLes_date" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="Les_Day" HeaderText="יום בשבוע" SortExpression="Les_Day" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="Les_startHour" HeaderText="שעת התחלה" SortExpression="Les_startHour" DataFormatString="{0:hh\:mm}" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="Les_EndHour" HeaderText="שעת סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}" >
                <HeaderStyle CssClass="blueHeader" />
                </asp:BoundField>
            </Columns>
                        <Pagerstyle CssClass="gvwCasesPager" height="20px" verticalalign="Bottom" horizontalalign="Center"/>

             <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                            HorizontalAlign="center" Wrap="False" />
        </asp:GridView>
    </div>

</asp:Content>
