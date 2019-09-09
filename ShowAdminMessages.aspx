<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShowAdminMessages.aspx.cs" Inherits="ShowAdminMessages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="plugins/select2/select2.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%=studentDDL.ClientID%>").select2({
                    placeholder: "בחר תלמיד",
                    allowClear: true,
                    dir: "rtl"
                });

        });
          $(document).ready(function () {

            $("#<%=teacherDDL.ClientID%>").select2({
                    placeholder: "בחר מתגבר",
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

        .container.content {
            direction: rtl;
        }

        .contentBox {
            height: 500px;
        }

        .hiddenColumn {
            display: none;
        }

        .anti-overflow {
            word-wrap: break-word;
        }


        .AlternateRowStyle {
            height: 30px;
        }

        .createMessage {
            position: relative;
            float: left;
            background-color: #245581;
            display: inline;
            margin-right: 20px;
            position: relative;
            top: -10px;
        }

        h4 {
            color: #337ab7;
            direction: rtl;
            display: inline-block;
            margin-bottom: 0 auto;
        }

        .btn {
            background-color: #245581;
        }
        .btn-width{
            width:80px;
        }
        .leftBTN {
            float: left;
            display: inline-block;
            position: relative;
            top: -10px;
        }

        td a {
            color: #444444;
            padding-right: 20px;
        }

        .btn-danger {
            background-color: #c9302c;
            margin-right: 20px;
        }

        .studentMSGtoManager {
            margin-bottom: 40px;
        }

        .upperGRDWsection {
            margin-top: 10px;
            height: 10px;
        }
        .margin-top{
            margin-top:20px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="sectionTitle text-center">
        <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span id="headerText" runat="server">דואר נכנס</span>
            <span class="shape shape-right bg-color-4"></span>
        </h2>
    </div>


    <div class="container content">
        <div class="btn-group btn-group-sm " role="group" aria-label="send-recieve mail">
            <asp:Button ID="OutMail" CssClass="btn btn-info btn-width" runat="server" Text="דואר יוצא" OnClick="ShowOutMail_Click" />
            <asp:Button ID="InMail" CssClass="btn btn-info btn-width" runat="server" Text="דואר נכנס" OnClick="ShowInMail_Click" />

        </div>
        <div class="upperGRDWsection">
            <h4>הודעות תלמידים</h4>
            <button type="button" class="btn btn-info btn-sm createMessage" data-toggle="modal" data-target="#createMessageModal">הודעה חדשה לתלמיד</button>
            <asp:Button ID="ReadAllBTN" CssClass="leftBTN btn btn-sm btn-info" runat="server" Text="סמן הכל כנקרא" OnClick="ReadAllBTN_Click" />

        </div>

        <div class="studentMSGtoManager">

            <asp:SqlDataSource ID="InMailAdminDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select stu_id, msg_id,  Stu_FirstName + ' ' +  Stu_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content
from StudentMessages inner join Student on msg_fromStudentId=Stu_Id where msg_IsDeletedForManager=0 order by msg_id desc"></asp:SqlDataSource>

            <asp:GridView ID="InMailGRDW" EmptyDataText="אין הודעות חדשות" CssClass="grid" runat="server" Style="margin: 0 auto; margin-top: 20px; text-align: center; width: 100%"  DataSourceID="InMailAdminDS" OnRowDataBound="InMailGRDW_RowDataBound" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowPaging="True">
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <AlternatingRowStyle CssClass="AlternateRowStyle" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="showInMessageDetails" OnClick="showInMessageDetails_Click"
                                Text="פרטים"
                                CommandName="Attendance_Form"
                                runat="server" CommandArgument='<%# Container.DataItem %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Column1" ItemStyle-Width="20%" HeaderText="התקבלה מ" SortExpression="Column1" />
                    <asp:BoundField DataField="msg_date" ItemStyle-Width="10%" DataFormatString="{0:dd/MM/yyyy}" HeaderText="נשלחה בתאריך" SortExpression="msg_date" />
                    <asp:BoundField DataField="msg_subject" ItemStyle-Width="30%" HeaderText="נושא ההודעה" SortExpression="msg_subject" />
                    <asp:BoundField DataField="msg_content" ItemStyle-Width="40%" HeaderText="תוכן" SortExpression="msg_content" />
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="msg_hasRead" HeaderText="האם נקראה" SortExpression="msg_hasRead" ShowHeader="False">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="stu_id" HeaderText="מזהה תלמיד" ReadOnly="True" ShowHeader="False" SortExpression="stu_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="msg_content" ItemStyle-Width="30%" HeaderText="נושא ההודעה" SortExpression="msg_content">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="AdminMessagesDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select msg_id,  Stu_FirstName + ' ' +  Stu_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content from ManagerMessages inner join Student on msg_toStudentId=Stu_Id where msg_isDeletedForManager=0 order by msg_id desc"></asp:SqlDataSource>


            <asp:GridView ID="AdminMessagesGRDW" OnRowDataBound="AdminMessagesGRDW_RowDataBound" CssClass="grid" runat="server" Style="margin: 0 auto; margin-top: 20px; text-align: center; width: 100%;" DataSourceID="AdminMessagesDS" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowPaging="True">
            <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <AlternatingRowStyle CssClass="AlternateRowStyle" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="showOutMessageDetails" OnClick="showOutMessageDetails_Click"
                                Text="פרטים"
                                CommandName="Attendance_Form"
                                runat="server" CommandArgument='<%# Container.DataItem %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Column1" ItemStyle-Width="20%" HeaderText="הנמען" SortExpression="Column1" />
                    <asp:BoundField DataField="msg_date" ItemStyle-Width="10%" DataFormatString="{0:dd/MM/yyyy}" HeaderText="נשלחה בתאריך" SortExpression="msg_date" />
                    <asp:BoundField DataField="msg_subject" ItemStyle-Width="30%" HeaderText="נושא ההודעה" SortExpression="msg_subject" />
                    <asp:BoundField DataField="msg_content" ItemStyle-Width="40%" HeaderText="תוכן" SortExpression="msg_content" />
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                </Columns>
   
            </asp:GridView>
        </div>
        <hr />
        <div class="upperGRDWsection">
            <h4>הודעות מתגברים</h4>
            <button type="button" class="btn btn-info btn-sm createMessage" data-toggle="modal" data-target="#createMessageModalToTeacher">הודעה חדשה למתגבר</button>

            <asp:Button ID="ReadAllForTeachersBTN" CssClass="leftBTN btn btn-sm btn-info" runat="server" Text="סמן הכל כנקרא" OnClick="ReadAllForTeachersBTN_Click" />
        </div>

        <div>

            <asp:SqlDataSource ID="InMailAdminFromTeachersDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select tea_id, msg_id,  Tea_FirstName + ' ' +  Tea_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content
from TeacherMessages inner join Teacher on msg_fromTeacherId=Tea_Id where msg_IsDeletedForManager=0 order by msg_id desc"></asp:SqlDataSource>

            <asp:GridView ID="InMailFromTeachersGRDW" CssClass="grid" runat="server" Style="margin: 0 auto; margin-top: 20px; margin-bottom: 70px; text-align: center; width: 100%" DataSourceID="InMailAdminFromTeachersDS" OnRowDataBound="InMailFromTeachersGRDW_RowDataBound" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowPaging="True" >
                 <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <AlternatingRowStyle CssClass="AlternateRowStyle" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="showInMessageFromTeachersDetails" OnClick="showInMessageFromTeachersDetails_Click"
                                Text="פרטים"
                                CommandName="Attendance_Form"
                                runat="server" CommandArgument='<%# Container.DataItem %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Column1" ItemStyle-Width="20%" HeaderText="התקבלה מ" SortExpression="Column1" />
                    <asp:BoundField DataField="msg_date" ItemStyle-Width="10%" DataFormatString="{0:dd/MM/yyyy}" HeaderText="נשלחה בתאריך" SortExpression="msg_date" />
                    <asp:BoundField DataField="msg_subject" ItemStyle-Width="30%" HeaderText="נושא ההודעה" SortExpression="msg_subject" />
                    <asp:BoundField DataField="msg_content" ItemStyle-Width="40%" HeaderText="תוכן" SortExpression="msg_content" />
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="msg_hasRead" HeaderText="האם נקראה" SortExpression="msg_hasRead" ShowHeader="False">

                        <HeaderStyle CssClass="hiddenColumn" />


                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Tea_id" HeaderText="מזהה מתגבר" ReadOnly="True" ShowHeader="False" SortExpression="Tea_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>

                </Columns>

            </asp:GridView>
        </div>






        <div>

            <asp:SqlDataSource ID="OutMailAdminToTeachersDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select msg_id,  Tea_FirstName + ' ' +  Tea_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content from ManagerMessagesToTeachers inner join Teacher on msg_toTeacherId=Tea_Id where msg_isDeletedForManager=0 order by msg_id desc"></asp:SqlDataSource>

        </div>

        <div>
            <asp:GridView ID="OutMailAdminToTeachersGRDW" CssClass="grid" runat="server" Style="margin: 0 auto; margin-bottom: 50px; margin-top: 20px; text-align: center; width: 100%;"  DataSourceID="OutMailAdminToTeachersDS" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowPaging="True">
                  <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="showOutMessageFromTeachersDetails" OnClick="showOutMessageFromTeachersDetails_Click"
                                Text="פרטים"
                                CommandName="Attendance_Form"
                                runat="server" CommandArgument='<%# Container.DataItem %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Column1" ItemStyle-Width="20%" HeaderText="הנמען" SortExpression="Column1" />
                    <asp:BoundField DataField="msg_date" ItemStyle-Width="10%" DataFormatString="{0:dd/MM/yyyy}" HeaderText="נשלחה בתאריך" SortExpression="msg_date" />
                    <asp:BoundField DataField="msg_subject" ItemStyle-Width="30%" HeaderText="נושא ההודעה" SortExpression="msg_subject" />
                    <asp:BoundField DataField="msg_content" ItemStyle-Width="40%" HeaderText="תוכן" SortExpression="msg_content" />
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                </Columns>

            </asp:GridView>
        </div>




        <!-- Bootstrap Modal Dialog -->

        <div class="modal fade" id="createMessageModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header" style="text-align: center;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">
                                    <asp:Label ID="lblModalTitle" runat="server" Text="הודעה חדשה"></asp:Label></h4>
                            </div>
                            <div class="modal-body">
                                <asp:DropDownList ID="studentDDL" runat="server" CssClass="form-control border-color-4 tb" placeholder="שלח לתלמיד"></asp:DropDownList><br />
                                <asp:TextBox ID="subjectTB" runat="server" CssClass="form-control border-color-4 tb margin-top" placeholder="בנושא"></asp:TextBox><br />
                                <asp:TextBox ID="contentTB" runat="server" CssClass="form-control border-color-4 tb contentBox" TextMode="MultiLine" placeholder="תוכן ההודעה"></asp:TextBox><br />
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-sm btn-info btn-width" data-dismiss="modal" aria-hidden="true" style="float: left; ">סגור</button>
                                <div class="btn-group">
                                    <asp:Button ID="sendMessageBTN" CssClass="btn btn-sm btn-info" runat="server" Text="שלח הודעה" OnClick="sendMessageBTN_Click" />
                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="modal fade" id="createMessageModalToTeacher" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header" style="text-align: center;">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label1" runat="server" Text="הודעה חדשה"></asp:Label></h4>
                            </div>
                            <div class="modal-body">
                                <asp:DropDownList ID="teacherDDL" runat="server" CssClass="form-control border-color-4 tb" placeholder="שלח למתגבר"></asp:DropDownList><br />
                                <asp:TextBox ID="subjectTBForTeacher" runat="server" CssClass="form-control border-color-4 tb margin-top" placeholder="בנושא"></asp:TextBox><br />
                                <asp:TextBox ID="contentTBForTeacher" runat="server" CssClass="form-control border-color-4 tb contentBox" TextMode="MultiLine" placeholder="תוכן ההודעה"></asp:TextBox><br />
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-sm btn-info btn-width" data-dismiss="modal" aria-hidden="true" style="float: left; ">סגור</button>
                                <div class="btn-group">
                                    <asp:Button ID="sendMessageToTeacherBTN" CssClass="btn btn-sm btn-info btn-width" runat="server" Text="שלח הודעה" OnClick="sendMessageToTeacherBTN_Click" />
                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div id="ShowMessageContent" class="modal fade" style="direction: rtl;">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" style="text-align: center;">
                        <asp:Label ID="msgHeaderTitleLBL" runat="server" Text="Label"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body anti-overflow">
                    <asp:Label ID="displayContentLBL" runat="server" Text=""></asp:Label>

                </div>
                <div class="modal-footer">
                    <button class="btn btn-sm btn-info btn-width" data-dismiss="modal" aria-hidden="true" style="float: left; ">סגור</button>
                    <asp:Button ID="responseBTN" CssClass="btn btn-sm btn-info btn-width" runat="server" Text="השב להודעה" OnClick="responseBTN_Click" />
                    <asp:Button ID="deleteBTN" CssClass="btn btn-sm btn-danger btn-width" runat="server" Text="מחק הודעה" OnClick="deleteBTN_Click" />
                </div>
            </div>
        </div>
    </div>



</asp:Content>

