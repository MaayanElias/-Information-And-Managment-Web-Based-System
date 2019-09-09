<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddInstructor.aspx.cs" Inherits="AddInstructor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            position: relative;
            left: 80px;
        }

        .modaltb {
            text-align: right;
            direction: rtl;
            width: 500px;
            margin: 0 auto;
            position: absolute;
            top: 200px;
        }

       .fieldError {
            height: 25px;
            width: auto;
            position: relative;
            float: left;
            left: 270px;
            top: -40px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li><a href="ShowInstructors.aspx">חונכים</a></li>
                <li class="active">הוספת חונך</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>הוספת חונך חדש</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>

    <!-- Starts contact form 1 -->
    <div class="container">
        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="homeContactContent">


                    <div class="form-group">
                        <i class="fa fa-envelope"></i>
                        <asp:TextBox ID="MailTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="אימייל"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server"
                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ControlToValidate="MailTB"
                            ErrorMessage="כתובת המייל הוזנה בפורמט לא נכון">
                            <img class="fieldError" src="images/requiredField.png"  title="כתובת מייל לא חוקית"/></asp:RegularExpressionValidator>
                    </div>


                    <div class="form-group">
                        <i class="fa fa-phone"></i>
                        <asp:TextBox ID="PhoneTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="טלפון"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                            ControlToValidate="PhoneTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png"  title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ErrorMessage="Enter valid Phone number"
                            ControlToValidate="PhoneTB"
                            ValidationExpression="^[0-9]{10}$">
                                <img class="fieldError" src="images/requiredField.png"  title="אנא הכנס מספר נייד בעל 10 ספרות"/>
                        </asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-home"></i>
                        <asp:TextBox ID="addressTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="כתובת"></asp:TextBox>
                    </div>


                    <div class="form-group" style="text-align: center">
                        <asp:Button ID="submitBTN" runat="server" Text="הוסף חונך" OnClick="submitBTN_Click" CssClass="btn btn-primary rightTB" data-target="#confirmationModal" />
                    </div>


                </div>
            </div>
            <div class="col-sm-6 col-xs-12" style="position: relative; right: 100px;">
                <div class="homeContactContent">

                    <div class="form-group">
                        <i class="fa fa-id-card"></i>
                        <asp:TextBox ID="IdTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="ת.ז"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="IdTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="ValidIdNum" runat="server"
                            Display="Dynamic"
                            ControlToValidate="IdTB"
                            ErrorMessage="הכנס מספר בעל 9 ספרות"
                            ValidationExpression="^[0-9]{9}$">
                            <img class="fieldError" src="images/requiredField.png"  title="הכנס מספר בעל 9 ספרות"/>
                        </asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="FirstNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם פרטי"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="FirstNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png"  title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="LastNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם משפחה"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="LastNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-unlock-alt"></i>
                        <asp:TextBox ID="PasswordTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="סיסמא"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ControlToValidate="PasswordTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png"  title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

        </div>
        <!-- Ends contact form 1 -->
    </div>
    <div id="confirmationModal" class="modal fade modal-sm modaltb" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content btn">
                <div class="modal-header">
                    <h5>החונך נוסף בהצלחה</h5>
                    &nbsp
                    <button type="button" class="btn btn-sm btn-success" data-dismiss="modal" onclick="window.location='ShowInstructors.aspx'">חזור למסך חונכים</button>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
