<%@ Page Title="" Language="C#" MasterPageFile="~/TeacherMasterPage.master" AutoEventWireup="true" CodeFile="ShowTeacherMessages.aspx.cs" Inherits="ShowTeacherMessages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

        .RowStyle {
            height: 30px;
        }

        .AlternateRowStyle {
            height: 30px;
        }

        .createMessage {
            margin-top: 20px;
            position: relative;
            float: left;
            background-color: #245581;
        }


        .btn {
            background-color: #245581;
        }

        .leftBTN {
            float: left;
        }

        td a {
            color: #444444;
            padding-right: 20px;
        }

        .btn-danger {
            background-color: #c9302c;
            margin-right: 20px;
        }
    </style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="sectionTitle text-center">
        <h2>
            <span class="shape shape-left bg-color-4"></span>
            <span id="headerText" runat="server">דואר נכנס</span>
            <span class="shape shape-right bg-color-4"></span>
        </h2>
    </div>


    <div class="container content">

        <div class="btn-group btn-group-sm " role="group" aria-label="send-recieve mail">
            <asp:Button ID="OutMail" CssClass="btn btn-info" runat="server" Text="דואר יוצא" OnClick="ShowOutMail_Click" />
            <asp:Button ID="InMail" CssClass="btn btn-info" runat="server" Text="דואר נכנס" OnClick="ShowInMail_Click" />

        </div>
        <asp:Button ID="ReadAllBTN" CssClass="leftBTN btn btn-sm btn-info" runat="server" Text="סמן הכל כנקרא" OnClick="ReadAllBTN_Click" />


        <div>

            <asp:SqlDataSource ID="InMailTeacherDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select msg_id,  man_FirstName + ' ' +  man_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content
from ManagerMessagesToTeachers inner join Manager on msg_fromManagerId=Man_id where msg_toTeacherId = @current_tea_id and msg_IsDeletedForTeacher=0 order by msg_id desc"></asp:SqlDataSource>

        </div>

        <div>
            <asp:GridView ID="InMailGRDW" EmptyDataText="אין הודעות להצגה" CssClass="grid" runat="server" Style="text-align:center;margin: 0 auto; margin-top: 20px; text-align: center; width: 100%" DataSourceID="InMailTeacherDS" OnRowDataBound="InMailGRDW_RowDataBound" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
               <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
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
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id" >
                    <HeaderStyle CssClass="hiddenColumn" />
                    <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="msg_hasRead" HeaderText="האם נקראה" SortExpression="msg_hasRead" ShowHeader="False">
                        <HeaderStyle CssClass="hiddenColumn" />
                        <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>

                </Columns>
                  <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                            HorizontalAlign="center" Wrap="False" />

            </asp:GridView>
        </div>


        <div>

            <asp:SqlDataSource ID="TeacherMessagesDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select msg_id,  Man_FirstName + ' ' +  Man_LastName , msg_subject ,msg_date ,msg_hasRead,msg_content
from TeacherMessages inner join Manager on msg_toManagerId=Man_id where msg_fromTeacherId = @tea_id and msg_IsDeletedForTeacher=0 order by msg_id desc  "></asp:SqlDataSource>

        </div>

        <div>
            <asp:GridView ID="TeacherMessagesGRDW" EmptyDataText="אין הודעות להצגה" CssClass="grid"  OnRowDataBound="TeacherMessagesGRDW_RowDataBound" runat="server" Style="margin: 0 auto; margin-top: 20px; text-align: center; width: 100%"  DataSourceID="TeacherMessagesDS" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
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
                    <asp:BoundField DataField="msg_id" HeaderText="מזהה הודעה" InsertVisible="False" ReadOnly="True" SortExpression="msg_id" >

                    <HeaderStyle CssClass="hiddenColumn" />
                    <ItemStyle CssClass="hiddenColumn" />
                    </asp:BoundField>

                </Columns>
                  <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                            HorizontalAlign="center" Wrap="False" />

            </asp:GridView>
        </div>

        <button type="button" class="btn btn-info btn-lg createMessage" data-toggle="modal" data-target="#createMessageModal">צור הודעה חדשה</button>



        <!-- Bootstrap Modal Dialog -->

        <div class="modal fade" id="createMessageModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="text-align:center;">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">
                            <asp:Label ID="lblModalTitle" runat="server" Text="שלח הודעה לאחראית המרכז"></asp:Label></h4>
                    </div>
                    <div class="modal-body">

                        <asp:TextBox ID="subjectTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="בנושא"></asp:TextBox><br />
                        <asp:TextBox ID="contentTB" runat="server" CssClass="form-control border-color-4 tb contentBox" TextMode="MultiLine" placeholder="תוכן ההודעה"></asp:TextBox><br />
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-sm btn-info" data-dismiss="modal" aria-hidden="true" style="float: left;">סגור</button>
                        <div class="btn-group">
                            <asp:Button ID="sendMessageBTN" CssClass="btn btn-sm btn-info" runat="server" Text="שלח הודעה" OnClick="sendMessageBTN_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div id="ShowMessageContent" class="modal fade" style="direction: rtl;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="text-align:center;">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                            &times;</button>
                        <h4 class="modal-title">
                            <asp:Label ID="msgHeaderTitleLBL" runat="server" Text="Label"></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-body anti-overflow ">
                        <asp:Label ID="displayContentLBL" runat="server" Text=""></asp:Label>

                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-sm btn-info" data-dismiss="modal" aria-hidden="true" style="float: left;">סגור</button>
                        <asp:Button ID="responseBTN" CssClass="btn btn-sm btn-info" runat="server" Text="השב להודעה" OnClick="responseBTN_Click" />
                        <asp:Button ID="deleteBTN" CssClass="btn btn-sm btn-danger " runat="server" Text="מחק הודעה" OnClick="deleteBTN_Click" />

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

