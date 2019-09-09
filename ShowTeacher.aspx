<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowTeacher.aspx.cs" Inherits="ShowTeacher" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= teacherGRDW.ClientID %>"));
            var terms = Obj.value.toUpperCase();
            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[1].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                ele3 = grid.rows[r].cells[3].innerHTML.replace(/<[^>]+>/g, "");
                ele4 = grid.rows[r].cells[4].innerHTML.replace(/<[^>]+>/g, "");
                //ele5 = grid.rows[r].cells[6].innerHTML.replace(/<[^>]+>/g, "");
                //ele6 = grid.rows[r].cells[7].innerHTML.replace(/<[^>]+>/g, "");
                //ele7 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                if (ele1.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele2.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele3.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else if (ele4.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                //else if (ele5.toUpperCase().indexOf(terms) >= 0)
                //    grid.rows[r].style.display = '';
                //else if (ele7.toUpperCase().indexOf(terms) >= 0)
                //    grid.rows[r].style.display = '';

                else grid.rows[r].style.display = 'none';
            }
        }
    </script>
    <style>
        #addTeacherBTN {
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

        .excel {
            margin-right: 20px;
            float: left;
            
        }
        .UploadBTN{
            width:80px;
        }
    </style>
    <script src="https://code.jquery.com/jquery.js"></script>
    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li class="active">מתגברים</li>

            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>רשימת מתגברים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>

    <div class="container content">
        <div>
            <asp:Button ID="addTeacherBTN" runat="server" Text="הוסף מתגבר" OnClick="addTeacherBTN_Click" CssClass="btn btn-primary rightTB btn-sm" />
            <asp:DropDownList ID="DropDownList1" AutoPostBack="true" runat="server" CssClass="filterTB">
                <asp:ListItem Text="כל המשתמשים" Value="" />
                <asp:ListItem Value="true">משתמשים פעילים</asp:ListItem>
                <asp:ListItem Value="false">משתמשים לא פעילים</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="searchTB" placeholder="חיפוש" runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB"></asp:TextBox>

            <%--For upload : download and install the following - https://www.microsoft.com/en-us/download/confirmation.aspx?id=23734--%>
            <asp:FileUpload ID="excelFU" runat="server" ToolTip="בחר קובץ" CssClass="btn btn-success btn-sm excel" />

            <asp:Button ID="UploadBTN" runat="server" OnClick="UploadBTN_Click" Text="ייבא" CssClass="btn btn-success btn-sm excel UploadBTN" />
            <asp:Button ID="exportBTN" runat="server" Text="שמור רשימה" OnClick="exportBTN_Click" CssClass="btn btn-success btn-sm excel" data-toggle="tooltip" data-placement="top" title="" data-original-title="שמור כקובץ Excel" />

        </div>

        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="SELECT * FROM [Teacher]" FilterExpression="(tea_status)= '{0}'">
                <FilterParameters>
                    <asp:ControlParameter ControlID="DropDownList1" PropertyName="SelectedValue" />

                </FilterParameters>
            </asp:SqlDataSource>
        </div>
        <br />
        <div>
            <asp:GridView ID="teacherGRDW" CssClass="grid" AllowPaging="true" PageSize="20" Style="margin-left: auto; margin-right: auto; margin-bottom: 70px; text-align: center; width: 100%" runat="server" DataKeyNames="Tea_Id" DataSourceID="SqlDataSource1"  OnSelectedIndexChanged="teacherGrid_SelectedIndexChanged" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="פרטים" />
                    <asp:BoundField DataField="Tea_Id" HeaderText="ת.ז" ReadOnly="True" SortExpression="Tea_Id" />
                    <asp:BoundField DataField="Tea_FirstName" HeaderText="שם פרטי" SortExpression="Tea_FirstName" />
                    <asp:BoundField DataField="Tea_LastName" HeaderText="שם משפחה" SortExpression="Tea_LastName" />
                    <asp:BoundField DataField="Tea_PhoneNumber" HeaderText="פלאפון" SortExpression="Tea_PhoneNumber" />
                    <asp:BoundField DataField="Tea_Email" HeaderText="מייל" SortExpression="Tea_Email" />
                    <asp:BoundField DataField="Tea_address" HeaderText="כתובת" SortExpression="Tea_address" />
                    <asp:CheckBoxField DataField="Tea_status" HeaderText="סטטוס" SortExpression="Tea_status" />
                </Columns>
                                    <pagerstyle cssclass="gvwCasesPager" height="20px" verticalalign="Bottom" horizontalalign="Center" />

            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>
