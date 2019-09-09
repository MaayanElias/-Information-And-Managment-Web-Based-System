<%@ Page Title="" Language="C#" MasterPageFile="~/TeacherMasterPage.master" AutoEventWireup="true" CodeFile="ClassesForTeacher.aspx.cs" Inherits="ClassesForTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />


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

    <style>
        .btn {
            width: 130px;
        }

        @media (min-width: 1200px) {
            .container {
                width: 95%;
            }
        }

        .grid .declinedRequest {
            color: red;
        }

        .grid .whitetext {
            color: white;
        }


        .SentForm {
            color: #4cae4c;
        }
         .gvwCasesPager a
            {
                margin-left:10px;
                margin-right:10px;
            }

         .headerText{
            color: #0094ff
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="sectionTitle text-center">
        <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span>התגבורים שלי</span>
            <span class="shape shape-right bg-color-4"></span>
        </h2>
    </div>
    <div class="container content">
        <asp:GridView ID="teacherClasses" CssClass="grid" runat="server" Style="margin-left: auto; margin-right: auto; margin-bottom: 100px; text-align: center; width: 100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" EnableViewState="false" OnRowDataBound="teacherClasses_RowDataBound" AllowPaging="True" PageSize="20" OnPageIndexChanging="gridView_PageIndexChanging">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <RowStyle Height="30px" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1"
                            Text="שלח טופס נוכחות"
                            CommandName="Attendance_Form"
                            runat="server" CommandArgument='<%# Container.DataItem %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
          <HeaderStyle  CssClass="headerText"  />
            <Pagerstyle CssClass="gvwCasesPager" height="20px" verticalalign="Bottom" horizontalalign="Center"/>
        </asp:GridView>
        <div>
            <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">בקשה לביטול תגבור</h4>
                        </div>
                        
                            <div class="col-lg-12 col-sm-12 col-md-12 col-xs-12">
                                <div class="form-group">
                                    <asp:Label ID="lblLesonId" runat="server" Text="מספר תגבור : "></asp:Label>
                                    <asp:Label ID="lbl1" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblLesonDate" runat="server" Text="תאריך תגבור: "></asp:Label>
                                    <asp:Label ID="lbl2" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblReason" runat="server" Text="סיבת ביטול: "></asp:Label>
                                    <asp:TextBox ID="txtReason" runat="server" TabIndex="3" MaxLength="100" CssClass="form-control"
                                        placeholder="הזן סיבת ביטול..."></asp:TextBox>
                                </div>
                            </div>
                        
                        <div class="modal-footer">
                            <asp:Button ID="btnSend" runat="server" Text="שלח" CssClass="btn btn-info" OnClick="btnSend_Click" />
                            <button type="button" class="btn btn-info" data-dismiss="modal">ביטול</button>
                        </div>
                    </div>
                </div>
                <script type='text/javascript'>
                    function openModal() {
                        $('[id*=myModal]').modal('show');
                    }
                </script>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>
