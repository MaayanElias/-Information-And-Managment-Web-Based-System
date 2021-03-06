﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="addTeacher.aspx.cs" Inherits="addTeacher" %>

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
            margin: 0 auto;
        }

        .progress_bar {
            width: 430px;
            height: auto;
            margin: auto;
            position: relative;
            top: -10px;
        }

        #progress_bar {
            text-align: center;
            margin-bottom: 20px;
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

    <!-- Starts contact form 1 -->
    <div class="container">
        <div id="progress_bar">
            <img class="progress_bar" src="images/progress1.jpg" />
        </div>
        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="homeContactContent">


                    <div class="form-group">
                        <i class="fa fa-envelope"></i>
                        <asp:TextBox ID="mailTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="אימייל"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server"
                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ControlToValidate="MailTB"
                            ErrorMessage="כתובת המייל הוזנה בפורמט לא נכון">
                            <img class="fieldError" src="images/requiredField.png"  title="כתובת מייל לא חוקית"/></asp:RegularExpressionValidator>
                    </div>


                    <div class="form-group">
                        <i class="fa fa-phone"></i>
                        <asp:TextBox ID="phoneTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="טלפון"></asp:TextBox>
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
                    <div class="form-group" style="text-align: center; position: relative; left: 20px;">
                        <asp:Button ID="submitBTN" runat="server" Text="המשך - הוספת מקצועות" OnClick="submitBTN_Click" CssClass="btn btn-primary add-teacher" />
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="homeContactContent">

                    <div class="form-group">
                        <i class="fa fa-id-card"></i>
                        <asp:TextBox ID="idTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="ת.ז"></asp:TextBox>
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
                        <asp:TextBox ID="fNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם פרטי"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="fNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="LNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם משפחה"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="LNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-unlock-alt"></i>
                        <asp:TextBox ID="passwordTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="סיסמא"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ControlToValidate="passwordTB"
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

    <%-- Confirmation modal - comment out since we have the toastr notifications --%>

    <%--    <div id="confirmationModal" class="modal fade modal-sm modaltb" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content btn">
                <div class="modal-header">
                    <h5>המתגבר נוסף בהצלחה</h5>
                    &nbsp
                    <button type="button" class="btn btn-sm btn-success" data-dismiss="modal" onclick="window.location='ShowTeacher.aspx'">חזור למסך מתגברים</button>
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
