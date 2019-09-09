<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowLesson.aspx.cs" Inherits="ShowLesson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>

    <script>
        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= LessonTemplateGRDW.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[3].innerHTML.replace(/<[^>]+>/g, "");
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
        #addTemplateBTN {
            text-align: right;
            float: right;
        }

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
                color: #00196e;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li class="active">רשימת תבניות תגבורים</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>רשימת תבניות תגבורים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>

    <asp:SqlDataSource ID="LessonTemplateDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="	select Les_Id,Pro_Title,Les_StartHour,Les_EndHour,(Tea_FirstName + ' ' +  Tea_LastName) as 'full name', Les_MaxQuan
	from Lesson inner join Teacher on Les_Tea_Id= Tea_Id inner join Profession on Les_Pro_Id=Pro_Id"></asp:SqlDataSource>

    <div class="container content">
        <div>
            <asp:Button ID="addTemplateBTN" runat="server" Text="הוסף תבנית תגבור" CssClass="btn btn-primary rightTB btn-sm" OnClick="addTemplateBTN_Click" />
            <asp:TextBox ID="searchTB" placeholder="חיפוש" runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB"></asp:TextBox>

        </div>
        <div>
            <asp:GridView ID="LessonTemplateGRDW" CssClass="grid" runat="server" DataSourceID="LessonTemplateDS" AllowPaging="true" PageSize="20" Style="margin-left: auto; margin-top: 20px; margin-right: auto; margin-bottom: 100px; text-align: center; width: 100%" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" OnSelectedIndexChanged="LessonTemplateGRDW_SelectedIndexChanged">
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <Columns>
                    <asp:CommandField SelectText="פרטים" ShowSelectButton="True" />
                    <asp:BoundField DataField="Les_Id" HeaderText="מזהה תבנית" InsertVisible="False" ReadOnly="True" SortExpression="Les_Id" />
                    <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" SortExpression="Pro_Title" />
                    <asp:BoundField DataField="full name" HeaderText="מתגבר" SortExpression="full name" />
                    <asp:BoundField DataField="Les_StartHour" HeaderText="שעת התחלה" SortExpression="Les_StartHour" DataFormatString="{0:hh\:mm}" />
                    <asp:BoundField DataField="Les_EndHour" HeaderText="שעת סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}" />
                    <asp:BoundField DataField="Les_MaxQuan" HeaderText="כמות מקסימלית" SortExpression="Les_MaxQuan" />
                </Columns>
                            <Pagerstyle CssClass="gvwCasesPager" height="20px" verticalalign="Bottom" horizontalalign="Center"/>

            </asp:GridView>
        </div>
    </div>
    <div style="margin-bottom: 20px;">&nbsp</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

