<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMasterPage.master" AutoEventWireup="true" CodeFile="ClassesForStudent.aspx.cs" Inherits="ClassesForStudent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery.js"></script>

    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>

    <script>
        function Filter(Obj) {

            var grid = document.getElementById(("<%= classesGV.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[7].innerHTML.replace(/<[^>]+>/g, "");

                if (ele1.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele2.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else grid.rows[r].style.display = 'none';
            }
        }

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-left",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",

        }
    </script>

    <style>
        .container.content {
            direction: rtl;
        }



        .btn {
            width: 90px;
            height: 35px;
        }


    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="student_calendar.aspx">בית</a></li>
                <li class="active">תגבורים במרכז</li>

            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>תגבורים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
            <div dir="rtl" style="margin: 0 130px 20px 0">
                <asp:Label ID="Label1" Style="color: red; font-weight: bold" runat="server" Text="אינך זכאי להירשם לתגבורים. אנא פנה למנהלת המרכז" Visible="false"></asp:Label>
            </div>
        </div>
    </section>
    <div class="container content">
        <asp:TextBox ID="searchTB" placeholder="חיפוש" runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB" Style="margin: 15px 65px 15px 0;"></asp:TextBox>

        <asp:SqlDataSource ID="classesDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand=" select ActLes_LesId, Les_day,Les_Id,Pro_Title, ActLes_date,Les_StartHour,Les_EndHour,(Tea_FirstName + ' ' +  Tea_LastName) as 'full_name', Les_MaxQuan,quantity
	from Lesson inner join ActualLesson on Les_Id = ActLes_LesId inner join Teacher on Les_Tea_Id= Tea_Id inner join Profession on Les_Pro_Id=Pro_Id where actls_cancelled=0"></asp:SqlDataSource>

        <asp:GridView ID="classesGV" CssClass="grid" runat="server" AutoGenerateColumns="False" DataSourceID="classesDS" Style="margin-left: auto; margin-right: auto; margin-bottom: 100px; text-align: center; width: 90%" AllowSorting="True" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" OnRowDataBound="classesGV_RowDataBound" EnableViewState="False">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <RowStyle Height="50px" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="requestButton" Text="" runat="server" CssClass="btn btn-success btn-sm" CommandName="request" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ActLes_LesId" HeaderText="מספר תגבור" InsertVisible="False" ReadOnly="True" SortExpression="ActLes_LesId" />
                <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" SortExpression="Pro_Title"  />
                <asp:BoundField DataField="ActLes_date" HeaderText="תאריך" SortExpression="ActLes_date" />
                <asp:BoundField DataField="Les_day" HeaderText="יום בשבוע" SortExpression="Les_day" />
                <asp:BoundField DataField="Les_StartHour" HeaderText="שעת התחלה" SortExpression="Les_StartHour" DataFormatString="{0:hh\:mm}" />
                <asp:BoundField DataField="Les_EndHour" HeaderText="שעת סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}"/>
                <asp:BoundField DataField="full_name" HeaderText="שם המתגבר" ReadOnly="True" SortExpression="full_name" />
                <asp:BoundField DataField="Les_MaxQuan" HeaderText="כמות מקסימלית בתגבור" SortExpression="Les_MaxQuan" />
                <asp:BoundField DataField="quantity" HeaderText="כמות נוכחית בתגבור" SortExpression="quantity" />
            </Columns>


        </asp:GridView>
    </div>
</asp:Content>
