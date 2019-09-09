<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="default" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <!-- SITE TITTLE -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>התחברות למערכת</title>

    <!-- PLUGINS CSS STYLE -->
    <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet"/>
<%--    <link rel="stylesheet" type="text/css" href="plugins/rs-plugin/css/settings.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="plugins/selectbox/select_option1.css" />
    <link rel="stylesheet" type="text/css" href="plugins/owl-carousel/owl.carousel.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="plugins/isotope/jquery.fancybox.css" />
    <link rel="stylesheet" type="text/css" href="plugins/isotope/isotope.css" />--%>

    <!-- GOOGLE FONT -->
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css' />
    <link href='https://fonts.googleapis.com/css?family=Dosis:400,300,600,700' rel='stylesheet' type='text/css' />

    <!-- CUSTOM CSS -->
    <link href="css/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/default.css" id="option_color" />

    <!-- Icons -->
    <link rel="shortcut icon" href="images/favicon.ico" />
    <script>
        function login() {
            window.location = "admin_dashboard.aspx";
        }
        function sendMail() {
            alert('נסה את מספר הטלפון שלך, ללא רווחים');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Header -->
        <section class="mainContent full-width clearfix">
            <div class="container">
                <img class="center-block" src="images/logo.png" />
                <div class="sectionTitle text-center">
                    <h2>
                        <span class="shape shape-left bg-color-4"></span>
                        <span> מרכז למידה - מבואות ים</span>
                        <span class="shape shape-right bg-color-4"></span>
                    </h2>
                </div>
            </div>
        </section>
        <!--Start Login Form -->
        <div class="row">
            <div class="login">
                <div class="panel panel-default formPanel">
                    <div class="panel-heading bg-color-4 border-color-4">
                        <h3 class="panel-title">התחברות למערכת</h3>
                    </div>
                    <div class="panel-body">

                        <div class="form-group formField">
                            <asp:TextBox ID="usernameTB" runat="server" CssClass="form-control txtright" placeholder="שם משתמש"></asp:TextBox>
                        </div>
                        <div class="form-group formField">
                            <asp:TextBox ID="passwordTB" runat="server" TextMode="Password" CssClass="form-control txtright" placeholder="סיסמא"></asp:TextBox>
                        </div>
                        <div class="form-group formField">
                            <asp:Button ID="loginBTN" runat="server" Text="התחבר" CssClass="btn btn-primary btn-block bg-color-7 border-color-7" OnClick="loginBTN_Click" />

                        </div>
                        <div>
                            <asp:CheckBox ID="rememberCB" runat="server" />
                            זכור אותי
                        </div>
                        <div class="form-group formField">
                            <p class="help-block" onclick="sendMail()"><a href="#">? שכחת סיסמא</a></p>
                        </div>
                          <asp:Label ID="LabelPH" runat="server" Text=""></asp:Label> <%--תוויות לתוכה תיכנס הודעה במידה והמשתמש לא קיים במערכת--%>
                    </div>
                </div>
            </div>
        </div>
        <!--Ends Login Form -->
    </form>
</body>
</html>
