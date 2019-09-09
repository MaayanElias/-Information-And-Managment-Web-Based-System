<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowRequests.aspx.cs" Inherits="ShowRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= reqGV.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[4].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[6].innerHTML.replace(/<[^>]+>/g, "");
                ele3 = grid.rows[r].cells[11].innerHTML.replace(/<[^>]+>/g, "");
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
    <style>
        .container.content {
            direction: rtl;
        }

        #straightLeft {
            position: relative;
            right: 115px;
        }

        .filterTB {
            margin-right: 20px;
            width: 200px;
        }

        .grid a {
            color: #0094ff;
        }

            .grid a:hover {
                color: #9fccdf;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li class="active">בקשות תלמידים</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>בקשות תלמידים</span>
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


        <asp:SqlDataSource ID="waitingReqDSS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand=" select Requests.req_number, number ,req_type, pro_title, stu_id,(stu_firstName + ' ' + stu_lastName) as 'student_full_name',
            (tea_firstName + ' '+ tea_lastName) as'teacher_full_name', Les_maxQuan,actLes_date, Les_startHour, Les_EndHour, Les_Day , quantity,req_is_permanent, req_status, ActLes_LesId 
            from ((((requests inner join student on req_stu_id= stu_id) inner join lesson on req_actLes_id= les_id ) inner join Teacher on Les_tea_Id= tea_Id) inner join Profession on les_pro_id= pro_id) inner join ActualLesson on ActLes_LesId= req_actLes_id where actLes_date = req_actLes_date "
            FilterExpression="(req_status)= '{0}'">
            <FilterParameters>
                <asp:ControlParameter ControlID="statusDDL" PropertyName="SelectedValue" />
            </FilterParameters>
        </asp:SqlDataSource>

        <asp:GridView ID="reqGV" EmptyDataText="אין בקשות להצגה" CssClass="grid" runat="server" DataSourceID="waitingReqDSS" DataKeyNames="number" AllowPaging="true" PageSize="20" Style="margin-left: auto; margin-top: 20px; margin-right: auto; margin-bottom: 100px; text-align: center; width: 100%" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" OnRowDataBound="reqGV_RowDataBound">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <RowStyle Height="50px" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="AproveButton" Text="אשר" runat="server" CssClass="btn btn-success btn-sm" OnClick="ApproveButton_Click" />
                        <asp:Button ID="DeclineButton" Text="דחה" runat="server" CssClass="btn btn-danger btn-sm" OnClick="DeclineButton_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="req_number" HeaderText="מזהה" InsertVisible="False" ReadOnly="True" SortExpression="req_number" />
                <asp:BoundField DataField="req_type" HeaderText="סוג הבקשה" ReadOnly="True" SortExpression="req_type" />
                <asp:BoundField DataField="stu_id" HeaderText="ת.ז" SortExpression="stu_id" />
                <asp:BoundField DataField="student_full_name" HeaderText="תלמיד" SortExpression="student_full_name" ReadOnly="True" />
                <asp:BoundField DataField="ActLes_LesId" HeaderText="מספר התגבור" SortExpression="ActLes_LesId" />
                <asp:BoundField DataField="pro_title" HeaderText="מקצוע" SortExpression="pro_title" />
                <asp:BoundField DataField="actLes_date" HeaderText="תאריך התגבור" SortExpression="actLes_date" />
                <asp:BoundField DataField="Les_Day" HeaderText="יום בשבוע" SortExpression="Les_Day" />
                <asp:BoundField DataField="Les_startHour" HeaderText="התחלה" SortExpression="Les_startHour" DataFormatString="{0:hh\:mm}" />
                <asp:BoundField DataField="Les_EndHour" HeaderText="סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}" />
                <asp:BoundField DataField="teacher_full_name" HeaderText="מתגבר" SortExpression="teacher_full_name" ReadOnly="True" />
                <asp:BoundField DataField="Les_maxQuan" HeaderText="קיבלות" SortExpression="Les_maxQuan" />
                <asp:BoundField DataField="quantity" HeaderText="רשומים" SortExpression="quantity" />
                <asp:BoundField DataField="req_status" HeaderText="סטטוס הבקשה" SortExpression="req_status" />
            </Columns>
            <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                HorizontalAlign="center" Wrap="False" />
            <PagerStyle CssClass="gvwCasesPager" Height="20px" VerticalAlign="Bottom" HorizontalAlign="Center" />

        </asp:GridView>
    </div>
</asp:Content>

