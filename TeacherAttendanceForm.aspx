<%@ Page Title="" Language="C#" MasterPageFile="~/TeacherMasterPage.master" AutoEventWireup="true" CodeFile="TeacherAttendanceForm.aspx.cs" Inherits="TeacherAttendanceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script src="plugins/select2/select2.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script type="text/javascript">

        $("#addStuTB").click(function () {

            $("#<%=ddlSearch.ClientID%>").select2({
                placeholder: "בחר תלמיד",
                allowClear: true,
                dir: "rtl"
            });

        });


    </script>

    <style>
        #student_list {
            margin-top: 20px;
        }

        h4 {
            margin-bottom: 10px;
            color: #337ab7;
        }

        .comment_box {
            width: 300px;
            margin: 5px;
        }

        .presenceCB {
            padding-right: 10px;
        }

        #information_img {
            width: 15px;
            height: auto;
            margin-right: 10px;
        }

        input::-webkit-input-placeholder { /* WebKit browsers */
            color: #c0c0c0;
        }
        .margin-top20{
            margin-top:20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="teacher_index.aspx">בית</a></li>
                <li><a href="ClassesForTeacher.aspx">תגבורים</a></li>
                <li class="active">טופס משוב לתגבור</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>טופס נוכחות בתגבור</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>
    <div class="container">
        <div dir="rtl">

            <div id="details_placeholder" class="row">
                <div class="col-lg-12">
                    <h4>פרטי תגבור</h4>
                </div>
                <div class="col-lg-10">

                    <asp:Label ID="dateLBL" runat="server" Text=""></asp:Label>
                    <br />

                    <asp:Label ID="DayLBL" runat="server" Text=""></asp:Label>

                    <asp:Label ID="hourLBL" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-lg-2">
                    <asp:Label ID="teacherLBL" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="profssionLBL" runat="server" Text=""></asp:Label>
                    <br />
                </div>
            </div>

            <div id="student_list" class="row">
                <div class="col-lg-12">
                    <h4>רשימת תלמידים משתתפים</h4>
                    <table>
                        <tr>
                            <td style="width: 50px;"><b>נוכחות</b>
                            </td>
                            <td style="width: 150px;"><b>שם התלמיד</b>
                            </td>
                            <td style="width: 300px;"><b>הערות</b><img id="information_img" src="images/information-icon.png" title="הערות ישמרו בפרופיל האישי של התלמיד" />
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        <tr>
                            <td colspan="2" style="margin-top:20px;">
                               <h4>הוסף תלמיד</h4>
                            </td>
                            <td style="float: left; margin-top: 20px;">
                            </td>
                        </tr>
                              <tr>
                            <td colspan="2" style="margin-top:20px;">
                                <asp:DropDownList ID="ddlSearch" runat="server"></asp:DropDownList>
                                <asp:Button ID="Button1" runat="server" Text="הוסף תלמיד" OnClick="btnAddStu_onClick" CssClass="btn btn-sm btn-primary margin-top20" />
                            </td>
                            <td style="float: left; margin-top: 10px;">
                                <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>

                            </td>
                        </tr>
                    </table>

                </div>
            </div>




        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>
