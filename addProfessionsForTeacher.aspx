<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="addProfessionsForTeacher.aspx.cs" Inherits="addProfessionsForTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="plugins/select2/select2.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%=ProfessionDDL.ClientID%>").select2({
                placeholder: "בחר מקצוע להוספה",
                allowClear: true,
                dir: "rtl"
            });

        });

    </script>

    <style>
        ::-webkit-input-placeholder {
            text-align: right;
        }

        .tb {
            text-align: right;
            direction: rtl;
            width: 400px;
            margin-left: auto;
        }

        .rightTB {
            margin-right: 20px;
        }

        .modaltb {
            text-align: right;
            direction: rtl;
            width: 500px;
            margin: 0 auto;
            position: absolute;
            top: 200px;
        }

        .ddl-style {
            display: inline-block;
            float: right;
        }

        .add-teacher {
            text-align: center;
            position: relative;
            float:left;
        }
                .progress_bar {
            width: 430px;
            height: auto;
            margin: auto;
            position:relative;
            top:-10px;
        }

        #progress_bar{
            text-align:center;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li><a href="ShowTeacher.aspx">מתגברים</a></li>
                <li class="active">הוספת מתגבר</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>הוספת מתגבר חדש</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>
    <div class="container">
        <div id="progress_bar">
            <img class="progress_bar" src="images/progress2.jpg" />
        </div>

        <div id="teacherProfessionRow" class="row" style="direction: rtl;" runat="server">
            <h3 dir="rtl" style="margin-right: 20px;">הוסף מקצועות למתגבר:</h3>


            <div class="col-lg-6">
                <asp:Button ID="submitBTN" runat="server" Text="המשך - בחר זמינויות" OnClick="submitBTN_Click" CssClass="btn btn-primary add-teacher" />

                <%--//insert datasource here--%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>"></asp:SqlDataSource>
                <%--//insert gridview here--%>

                <asp:GridView ID="GridView1" runat="server" EmptyDataText="מקצועות שתוסיף יופיעו כאן" CssClass="grid" Style="position: relative; direction: rtl; text-align: center; width: 50%" AllowSorting="True" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="pro_title" HeaderText="מקצוע" />
                        <asp:CommandField ShowSelectButton="True" SelectText="מחק" />

                    </Columns>
                    <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                        HorizontalAlign="center" Wrap="False" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#245581" Font-Bold="True" ForeColor="#F7F7F7" HorizontalAlign="Center" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
            </div>
            <div class="col-lg-6">
                <asp:DropDownList ID="ProfessionDDL" runat="server" CssClass="form-control border-color-4 tb smallwidth ddl-style" placeholder="בחר מקצוע">
                </asp:DropDownList>
                <asp:Button ID="addProBTN" runat="server" Text="הוסף מקצוע" CssClass="btn btn-primary rightTB btn-sm" OnClick="addProBTN_Click" />

            </div>
        </div>

    </div>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

