<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="TeacherAvailability.aspx.cs" Inherits="TeacherAvailability" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery.js"></script>
    <script src="../plugins/bootstrap/js/bootstrap.min.js"></script>
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

        .saveBTN {
            margin-top: 20px;

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
        }

        .save_div {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li><a href="AddTeacher.aspx">מתגברים</a></li>
                <li class="active">הוספת זמינות למתגבר </li>
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
    <div class="container" dir="rtl">

        <div id="progress_bar">
            <img class="progress_bar" src="images/progress3.jpg" />
        </div>

        <!-- Starts contact form 1 -->

        <div class="row">
            <h3 dir="rtl" style="margin-right: 20px;">הוסף זמינויות למתגבר :</h3>

            <div class="col-lg-6">
                <p>*לידיעתך - במידה ולא תשמור זמינויות עבור המתגבר, יהיה באפשרותך לשבץ אותו במהלך כל הימים וכל השעות</p>
                <p>ניתן לחזור ולערוך את זמינות המתגבר במסך <a href="ShowTeacher.aspx">המתגברים</a></p>

                <asp:SqlDataSource ID="availabilityDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand=""></asp:SqlDataSource>

                <asp:GridView ID="availabilityGV" runat="server" EmptyDataText="אין הגבלת זמינות למתגבר" DataSourceID="availabilityDS" AutoGenerateColumns="False" Style="margin-left: auto; text-align: center; width: 80%" CssClass="grid" OnRowDataBound="availabilityGV_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Ave_day" HeaderText="יום בשבוע" SortExpression="Ave_day" />
                        <asp:BoundField DataField="Ave_startHour" HeaderText="התחלה" SortExpression="Ave_startHour"  DataFormatString="{0:hh\:mm}"/>
                        <asp:BoundField DataField="Ave_endtHour" HeaderText="סיום" SortExpression="Ave_endtHour"  DataFormatString="{0:hh\:mm}"/>
                    </Columns>
                    <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                        HorizontalAlign="center" Wrap="False" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#245581" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
                <div class="save_div">
                    <asp:Button ID="saveTeacherBTN" runat="server" Text="שמור מתגבר" CssClass="btn btn-success rightTB saveBTN" OnClick="saveTeacherBTN_Click" />
                </div>
            </div>
            <div class="col-lg-6 ">
                <div class="homeContactContent">
                    <div class="form-group">
                        <asp:DropDownList ID="dayDDL" runat="server" CssClass="form-control border-color-4 tb">
                            <asp:ListItem Text="יום בשבוע" Value="" />
                            <asp:ListItem Value="1">א'</asp:ListItem>
                            <asp:ListItem Value="2">ב'</asp:ListItem>
                            <asp:ListItem Value="3">ג'</asp:ListItem>
                            <asp:ListItem Value="4">ד'</asp:ListItem>
                            <asp:ListItem Value="5">ה'</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <asp:TextBox ID="startTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שעת התחלה"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="endTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שעת סיום"></asp:TextBox>
                    </div>

                    <div class="form-group" style="text-align: center">
                        <asp:Button ID="submitBTN" runat="server" Text="הוסף זמינות למתגבר" OnClick="submitBTN_Click" CssClass="btn btn-primary rightTB" data-target="#confirmationModal" />
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

