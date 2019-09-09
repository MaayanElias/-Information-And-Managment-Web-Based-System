<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowActualLesson_admin.aspx.cs" Inherits="ShowActualLesson_admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>

    <script>
        var weekday = new Array();
        weekday[7] = "1";
        weekday[1] = "2";
        weekday[2] = "3";
        weekday[3] = "4";
        weekday[4] = "5";
        weekday[5] = "6";
        weekday[6] = "7";

        $(function () {
            $("#datepicker").datepicker(
                {
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (dateText, inst) {
                        var date = $(this).datepicker('getDate');
                        var dayOfWeek = weekday[date.getUTCDay() + 1];
                        $('#<%= dayOfActLesson.ClientID %>').val(dayOfWeek);
                    }
                });

        });

        $(function () {

            // Initialize and change language to hebrew
            $('#datepicker').datepicker($.datepicker.regional["he"]);

        });


        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= lessonsGRDW.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[6].innerHTML.replace(/<[^>]+>/g, "");
                if (ele1.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele2.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';

                else grid.rows[r].style.display = 'none';
            }
        }
    </script>

    <script>
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

    <script src="plugins/select2/select2.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script type="text/javascript">

            $(document).ready(function () {

                $("#<%=TigburDDL.ClientID%>").select2({
                    placeholder: "בחר תבנית תגבור",
                    allowClear: true,
                    dir: "rtl"
                });

            });

    </script>


    <style>
        .container.content {
            direction: rtl;
        }


        .boxes {
            position: relative;
            top: 2px;
            margin-left: 20px;
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
                <li class="active">תגבורים</li>

            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>רשימת תגבורים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>

    <div class="container content">
        <div>
            <asp:SqlDataSource ID="lessonsDS" runat="server"
                ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>"
                SelectCommand="	select Les_Id,Pro_Title, ActLes_date,Les_StartHour,Les_EndHour,(Tea_FirstName + ' ' +  Tea_LastName) as 'full name', Les_MaxQuan,quantity from Lesson inner join ActualLesson on Les_Id = ActLes_LesId inner join Teacher on Les_Tea_Id= Tea_Id inner join Profession on Les_Pro_Id=Pro_Id  where ActLes_date >= GETDATE()-1  AND actls_cancelled=0 order by ActLes_date"></asp:SqlDataSource>

        </div>
        <div>
            <asp:DropDownList ID="TigburDDL" runat="server" AutoPostBack="true" CssClass="form-control border-color-4" Style="width: 450px; margin-bottom: 10px; float: right;" OnSelectedIndexChanged="TigburDDL_SelectedIndexChanged">
            </asp:DropDownList>
            <input type="text" id="datepicker"  class="boxes" style="margin-right: 20px;" readonly="readonly" placeholder="בחר תאריך התחלה" name="DatePickername" />
            <asp:TextBox ID="counterTB" type="number" runat="server" CssClass=" boxes" placeholder="הזן כמות מופעים" ></asp:TextBox>
            <asp:Button ID="generateDate" runat="server" Text="הוסף למערכת" OnClick="generateDate_Click" CssClass="btn btn-primary rightTB btn-sm slight_left" />
        </div>
        <br />
        <div>
            <asp:TextBox ID="searchTB" placeholder="חיפוש ..." runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB"></asp:TextBox>

        </div>
        <div>
            <asp:GridView ID="lessonsGRDW" CssClass="grid" runat="server" DataSourceID="lessonsDS" Style="margin-left: auto; margin-top: 20px; margin-right: auto; margin-bottom: 100px; text-align: center; width: 100%" AllowSorting="True" AllowPaging="true" PageSize="20" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
                 <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="50px" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="cancelButton" Text="בטל תגבור" runat="server" CssClass="btn btn-success btn-sm" OnClick="cancelButton_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Les_Id" HeaderText="מזהה תבנית" InsertVisible="False" SortExpression="Les_Id" />
                    <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" SortExpression="Pro_Title" />
                    <asp:BoundField DataField="ActLes_date" HeaderText="תאריך" SortExpression="ActLes_date" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Les_StartHour" HeaderText="שעת התחלה" SortExpression="Les_StartHour" DataFormatString="{0:hh\:mm}" />
                    <asp:BoundField DataField="Les_EndHour" HeaderText="שעת סיום" SortExpression="Les_EndHour" DataFormatString="{0:hh\:mm}" />
                    <asp:BoundField DataField="full name" HeaderText="שם המתגבר" SortExpression="full name" />
                    <asp:BoundField DataField="Les_MaxQuan" HeaderText="קיבולת מקסימלית" SortExpression="Les_MaxQuan" />
                    <asp:BoundField DataField="quantity" HeaderText="כמות רשומים" SortExpression="quantity" />
                </Columns>
                            <Pagerstyle CssClass="gvwCasesPager" height="20px" verticalalign="Bottom" horizontalalign="Center"/>

            </asp:GridView>
        </div>
    </div>

    <asp:TextBox ID="dayOfActLesson" style="display:none;" runat="server"></asp:TextBox>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

