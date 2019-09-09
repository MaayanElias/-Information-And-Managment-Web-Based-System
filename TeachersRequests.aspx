<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="TeachersRequests.aspx.cs" Inherits="TeachersRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .grid a {
            color: #0094ff;
        }

            .grid a:hover {
                color: #9fccdf;
            }
    </style>
    <script>
        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= teaReqGV.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[4].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[3].innerHTML.replace(/<[^>]+>/g, "");
                ele3 = grid.rows[r].cells[5].innerHTML.replace(/<[^>]+>/g, "");
                if (ele1.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele2.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele3.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';

                else grid.rows[r].style.display = 'none';
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li class="active">בקשות מתגברים</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>בקשות מתגברים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>
    <div class="container content">
        <asp:DropDownList ID="statusDDL" runat="server" AutoPostBack="true" Style="text-align: right" CssClass="filterTB">
            <asp:ListItem Text="כל הבקשות" Value="" />
            <asp:ListItem Value="2">בקשות ממתינות</asp:ListItem>
            <asp:ListItem Value="1">בקשות שאושרו</asp:ListItem>
            <asp:ListItem Value="0">בקשות שנדחו</asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="searchTB" placeholder="חיפוש" runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB"></asp:TextBox>


        <asp:SqlDataSource ID="teaReqDSS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select DISTINCT TeaReq_number,ActLes_LesId, ActLes_date, pro_title, Tea_id,(Tea_firstName + ' ' + Tea_lastName) as 'teacher_full_name',
           Les_startHour, Les_EndHour, Les_Day , quantity, reason,TeaReq_status 
            from ((((TeacherRequests inner join teacher on TeaReq_TeaId= tea_id) inner join lesson on TeaReq__LessId= les_id ) inner join Profession on les_pro_id= pro_id) inner join ActualLesson on ActLes_LesId= TeaReq__LessId) where ActLes_date = TeaReq__LessDate"
            FilterExpression="(TeaReq_status)= '{0}'">
            <FilterParameters>
                <asp:ControlParameter ControlID="statusDDL" PropertyName="SelectedValue" />
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="teaReqGV" runat="server" CssClass="grid" DataSourceID="teaReqDSS" AllowPaging="true" PageSize="20" Style="margin-left: auto; margin-top: 20px; margin-right: auto; margin-bottom: 100px; text-align: center; width: 100%" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" OnRowDataBound="teaReqGV_RowDataBound">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <RowStyle Height="50px" />

            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="AproveButton" Text="אשר" runat="server" CssClass="btn btn-success btn-sm" OnClick="ApproveButton_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TeaReq_number" HeaderText="מס' בקשה" InsertVisible="False" ReadOnly="True" SortExpression="TeaReq_number" />
                <asp:BoundField DataField="ActLes_LesId" HeaderText="מס' תגבור" InsertVisible="False" ReadOnly="True" SortExpression="ActLes_LesId" />
                <asp:BoundField DataField="ActLes_date" HeaderText="תאריך תגבור" SortExpression="ActLes_date" />
                <asp:BoundField DataField="pro_title" HeaderText="מקצוע" SortExpression="pro_title" />
                <asp:BoundField DataField="teacher_full_name" HeaderText="מתגבר" SortExpression="teacher_full_name" ReadOnly="True" />
                <asp:BoundField DataField="Les_startHour" HeaderText="התחלה" SortExpression="Les_startHour" DataFormatString="{0:hh\:mm}" />
                <asp:BoundField DataField="Les_EndHour" HeaderText="סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}" />
                <asp:BoundField DataField="Les_Day" HeaderText="יום בשבוע" SortExpression="Les_Day" />
                <asp:BoundField DataField="quantity" HeaderText="רשומים" SortExpression="quantity" />
                <asp:BoundField DataField="TeaReq_status" HeaderText="סטטוס הבקשה" SortExpression="TeaReq_status" />
                <asp:BoundField DataField="reason" HeaderText="סיבת ביטול" ReadOnly="True" SortExpression="reason" />
            </Columns>
            <PagerStyle CssClass="gvwCasesPager" Height="20px" VerticalAlign="Bottom" HorizontalAlign="Center" />

        </asp:GridView>
    </div>
</asp:Content>


