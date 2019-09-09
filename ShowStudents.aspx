<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowStudents.aspx.cs" Inherits="ShowStudents" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        //for serching gridview on keyup
        function Filter(Obj) {

            var grid = document.getElementById(("<%= studentsGRDW.ClientID %>"));
            var terms = Obj.value.toUpperCase();

            for (var r = 1; r < grid.rows.length; r++) {
                ele1 = grid.rows[r].cells[1].innerHTML.replace(/<[^>]+>/g, "");
                ele2 = grid.rows[r].cells[2].innerHTML.replace(/<[^>]+>/g, "");
                ele3 = grid.rows[r].cells[3].innerHTML.replace(/<[^>]+>/g, "");
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
        #addStudentBTN {
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

        .Textbox_width {
            margin-right: 20px;
            width: 100px;
        }

        .UploadBTN {
            width: 80px;
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
                <li class="active">תלמידים</li>

            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>רשימת תלמידים</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>

    <div class="container content">
        <div>
            <asp:Button ID="addStudentBTN" runat="server" Text="הוסף תלמיד" OnClick="addStudentBTN_Click" CssClass="btn btn-primary rightTB btn-sm" />
            <asp:TextBox ID="searchTB" placeholder="חיפוש" runat="server" AutoPostBack="true" onkeyup="Filter(this)" CssClass="filterTB"></asp:TextBox>


            <asp:DropDownList ID="gradeDDL" runat="server" AutoPostBack="true" Style="text-align: right" CssClass="filterTB">
                <asp:ListItem Text="בחר כיתה" Value="" />
                <asp:ListItem>כיתה ז</asp:ListItem>
                <asp:ListItem>כיתה ח</asp:ListItem>
                <asp:ListItem>כיתה ט</asp:ListItem>
                <asp:ListItem>כיתה י</asp:ListItem>
                <asp:ListItem>כיתה יא</asp:ListItem>
                <asp:ListItem>כיתה יב</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownList1" AutoPostBack="true" runat="server" CssClass="filterTB">
                <asp:ListItem Text="כל המשתמשים" Value="" />
                <asp:ListItem Value="true">משתמשים פעילים</asp:ListItem>
                <asp:ListItem Value="false">משתמשים לא פעילים</asp:ListItem>
            </asp:DropDownList>

            <%--For upload : download and install the following - https://www.microsoft.com/en-us/download/confirmation.aspx?id=23734--%>
            <asp:FileUpload ID="excelFU" runat="server" ToolTip="בחר קובץ" CssClass="btn btn-success btn-sm excel" />

            <asp:Button ID="UploadBTN" runat="server" OnClick="UploadBTN_Click" Text="ייבא" CssClass="btn btn-success btn-sm excel UploadBTN" />
            <asp:Button ID="exportBTN" runat="server" Text="שמור רשימה" OnClick="exportBTN_Click" CssClass="btn btn-success btn-sm excel" data-toggle="tooltip" data-placement="top" title="" data-original-title="שמור כקובץ Excel" />

        </div>

        <div>

            <asp:SqlDataSource ID="studentsDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand=" SELECT [stu_id], [stu_firstName], [stu_lastName], [stu_birthDate], [stu_phoneNumber], [stu_email],[stu_address],[stu_status],[stu_password],[stu_grade],[stu_IsEntitled],[stu_Note], (Ins_FirstName + ' ' +  Ins_LastName) as 'full_name'
                        FROM [Student] inner join Instructor on Student.Ins_Id= Instructor.Ins_Id"
                FilterExpression="stu_grade like '{0}s' or (stu_status)= '{1}'">
                <FilterParameters>
                    <asp:ControlParameter Name="stu_grade" ControlID="gradeDDL" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="DropDownList1" PropertyName="SelectedValue" />

                </FilterParameters>
            </asp:SqlDataSource>

        </div>

        <br />
        <div>
            <asp:GridView ID="studentsGRDW" CssClass="grid" runat="server" AllowPaging="true" PageSize="20" Style="margin: 0 auto; margin-top: 20px; text-align: center; width: 100%" DataKeyNames="stu_id" DataSourceID="studentsDS" OnSelectedIndexChanged="studentsGRDW_SelectedIndexChanged" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="פרטים" />
                    <asp:BoundField DataField="stu_id" HeaderText="ת.ז תלמיד" ReadOnly="True" SortExpression="stu_id" />
                    <asp:BoundField DataField="stu_firstName" HeaderText="שם פרטי" SortExpression="stu_firstName" />
                    <asp:BoundField DataField="stu_lastName" HeaderText="שם משפחה" SortExpression="stu_lastName" />
                    <asp:BoundField DataField="stu_birthDate" HeaderText="תאריך לידה" SortExpression="stu_birthDate" />
                    <asp:BoundField DataField="stu_grade" HeaderText="כיתה" SortExpression="stu_grade" />
                    <asp:BoundField DataField="full_name" HeaderText="שם המדריך" SortExpression="full_name" />
                    <asp:BoundField DataField="stu_phoneNumber" HeaderText="פלאפון" SortExpression="stu_phoneNumber" />
                    <asp:BoundField DataField="stu_email" HeaderText="מייל" SortExpression="stu_email" />
                    <asp:BoundField DataField="stu_address" HeaderText="כתובת" SortExpression="stu_address" />
                    <asp:CheckBoxField DataField="stu_IsEntitled" HeaderText="זכאי" SortExpression="stu_IsEntitled" />
                    <asp:CheckBoxField DataField="stu_status" HeaderText="סטטוס" SortExpression="stu_status" />
                    <asp:BoundField DataField="stu_Note" HeaderText="הערות" SortExpression="stu_Note" />


                </Columns>
                <PagerStyle CssClass="gvwCasesPager" Height="20px" VerticalAlign="Bottom" HorizontalAlign="Center" />

            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

