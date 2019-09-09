<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="EditTeacher.aspx.cs" Inherits="EditTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://code.jquery.com/jquery.js"></script>
    <script src="plugins/jquery-ui/jquery-ui.js"></script>
    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/no-data-to-display.js"></script>
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




        var DateFilter = {
            startDate: null,
            endDate: null,
            userId: null
        };


        function showReport() {



            DateFilter.userId = document.getElementById('<%=idTB.ClientID%>').value;
            var dataString = JSON.stringify(DateFilter);

            $.ajax({
                type: 'POST',
                url: 'WebService.asmx/TeacherHoursByMonths',
                data: dataString,
                contentType: "application/json; charset=utf-8",
                traditional: true,
                success: function (data) {
                    var TeacherHoursByMonthsCountList = $.parseJSON(data.d);
                    //יצירת גרף עמודות
                    var tbody = $('#hoursChartByMonths').find('tbody');
                    $(tbody).find('tr').remove();
                    var str1 = "";
                    var monthsArray = [undefined, 'ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי', 'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'];
                    TeacherHoursByMonthsCountList.forEach(function (month) {

                        str1 += '<tr>' +
                            '<td>' + monthsArray[month.Month] + '</td>' +
                            '<td>' + month.Amount + '</td>' +
                            '</tr>';
                    });
                    tbody.append(str1);




                    $(function () {
                        $('#chart').highcharts({
                            lang: {
                                noData: 'אין נתונים עבור המתגבר'
                            },
                            noData: {
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '20px',
                                    color: '#303030'
                                }
                            },
                            data: {
                                table: 'hoursChartByMonths'
                            },
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: ' שעות מתגבר לפי חודשים'
                            },
                            yAxis: {
                                allowDecimals: true,
                                title: {
                                    text: 'כמות'
                                }
                            },
                            //xAxis:{
                            //    min:1,
                            //    categories: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11']
                            //},
                            series: [{

                                showInLegend: false,
                                color: "#6C6E87"

                            }],
                            plotOptions: {
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                },
                                column: {
                                    colorByPoint: true
                                },
                                series: {
                                    dataLabels: {
                                        //color: '#B0B0B3',
                                        enabled: true,
                                    },
                                }
                            },


                            tooltip: {

                                enabled: false
                            }
                        });
                    });

                }
            }); //End ajax call for student requests

        }
        function printDiv(divName) {
            var firstName = document.getElementById('<%=fNameTB.ClientID%>').value;
            var lastName = document.getElementById('<%=LNameTB.ClientID%>').value;
            document.getElementById("reportTitle").innerHTML = "דו\"ח שעות עבור  " + firstName + " " + lastName;

            document.getElementById("hoursChartByMonths").style.display = "";
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
            document.getElementById("hoursChartByMonths").style.display = "none";
            document.getElementById("reportTitle").innerHTML = "";

        }
        window.onload = showReport;
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
            left: 50px;
        }

        .modaltb {
            text-align: right;
            direction: rtl;
            width: 500px;
            margin: 0 auto;
            position: absolute;
            top: 200px;
        }

        .cbRight {
            text-align: right;
            direction: rtl;
            position: relative;
            right: 20px;
            bottom: 20px;
        }


        .filterBTN {
            margin: 10px;
            background-color: #9fccdf;
            width: 100px;
        }

        #report_PH {
            margin-bottom: 100px;
            text-align: center;
        }

        .filterDiv {
            height: 150px;
        }

        #imagPrint {
            vertical-align: middle;
            float: right;
            margin-right: 30px;
        }

        h4 {
            margin-bottom: 10px;
            color: #337ab7;
            direction: rtl;
        }

        h3 {
            text-align: center;
        }

        .center {
            text-align: center;
        }



        .teacherKPI {
            background-color: #245581;
            width: 130px;
            height: 110px;
            text-align: center;
            padding-top: 10px;
            padding-bottom: 10px;
            color: white;
            font-size: 30px;
            margin-left: 40%;
            margin-top: 40px;
            margin-bottom: 30px;
        }

        p {
            color: white;
            font-size: 16px;
        }

        .countersKPI {
            padding-top: 5px;
            font-size: 30px;
        }


        #formsKPI {
            background-color: #ea7066;
        }

        .ddl {
            display: inline-block;
            float: right;
        }

        .fieldError {
            height: 25px;
            width: auto;
            position: relative;
            float: left;
            left: 0px;
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
                <li class="active">עריכת פרטי מתגבר</li>
            </ol>

            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>עריכת פרטי מתגבר</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>

    <!-- Starts contact form 1 -->
    <div class="container">
        <div class="row cbRight">
            <asp:CheckBox ID="statusCB" runat="server" />
            משתמש פעיל
        </div>

        <div class="row">

            <div class="col-sm-6 col-xs-12 col-lg-4">
                <h4>מקצועות</h4>

                <asp:Button ID="addProBTN" runat="server" Text="הוסף מקצוע" CssClass="btn btn-primary btn-sm" OnClick="addProBTN_Click" />
                <asp:DropDownList ID="ProfessionDDL" runat="server" CssClass="form-control border-color-4 tb ddl" placeholder="בחר מקצוע">
                </asp:DropDownList>

                <%--                //insert datasource here--%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>"></asp:SqlDataSource>
                <br />
                <%--            //insert gridview here--%>

                <asp:GridView ID="GridView1" runat="server" EmptyDataText="אין מקצועות למתגבר" CssClass="grid" Style="margin-left: auto; margin-right: auto; margin-top: 30px; direction: rtl; text-align: center; width: 100%" AllowSorting="True" CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" AutoGenerateColumns="False">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="pro_title" HeaderText="מקצוע" />
                        <asp:CommandField ShowSelectButton="True" />

                    </Columns>

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
                <div class="form-group" style="text-align: center; position: absolute; top: 390px;">
                    <button id="deletePopUp" class="btn btn-danger" style="display: inline-block; margin-right: 20px;" type="button" data-toggle="modal" data-target="#deleteConfirmationModal"><span class="glyphicon glyphicon-trash"></span></button>

                    <asp:Button ID="saveBTN" runat="server" Text="עדכון מתגבר" OnClick="saveBTN_Click" CssClass="btn btn-primary" data-target="#confirmationModal" />
                </div>
            </div>


            <div class="col-sm-6 col-xs-12 col-lg-4" style="margin-bottom: 70px;">
                <h4>זמינויות</h4>
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
                        <asp:Button ID="submitBTN" runat="server" Text="הוסף זמינות למתגבר" OnClick="submitBTN_Click" CssClass="btn btn-primary" data-target="#confirmationModal" />
                    </div>
                </div>

                <asp:SqlDataSource ID="availabilityDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand=""></asp:SqlDataSource>
                <div style="direction: rtl; text-align: center;">
                    <asp:GridView ID="availabilityGV" runat="server" EmptyDataText="אין הגבלת זמינות למתגבר" DataSourceID="availabilityDS" AutoGenerateColumns="False" Style="text-align: center; width: 100%" CssClass="grid" OnRowDataBound="availabilityGV_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Ave_day" HeaderText="יום בשבוע" SortExpression="Ave_day" />
                            <asp:BoundField DataField="Ave_startHour" HeaderText="התחלה" SortExpression="Ave_startHour" DataFormatString="{0:hh\:mm}" />
                            <asp:BoundField DataField="Ave_endtHour" HeaderText="סיום" SortExpression="Ave_endtHour" DataFormatString="{0:hh\:mm}" />
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
                </div>
            </div>

            <div class="col-sm-6 col-xs-12 col-lg-4">
                <h4>פרטים</h4>

                <div class="homeContactContent">

                    <div class="form-group">
                        <i class="fa fa-id-card"></i>
                        <asp:TextBox ID="idTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="ת.ז" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="fNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם פרטי"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="FNRF" runat="server"
                            ControlToValidate="fNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>

                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="LNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם משפחה"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="LNRF" runat="server"
                            ControlToValidate="LNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>

                    </div>

                    <div class="form-group">
                        <i class="fa fa-unlock-alt"></i>
                        <asp:TextBox ID="passwordTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="סיסמא"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="passRF" runat="server"
                            ControlToValidate="passwordTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>

                    </div>

                    <div class="homeContactContent">
                        <div class="form-group">
                            <i class="fa fa-phone"></i>
                            <asp:TextBox ID="phoneTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="טלפון"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RFPhoneValidate" runat="server"
                                ControlToValidate="PhoneTB"
                                ErrorMessage="שדה זה הינו שדה חובה"
                                ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="PhoneValidate" runat="server"
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
                        <div class="form-group">
                            <i class="fa fa-envelope"></i>
                            <asp:TextBox ID="mailTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="אימייל"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                                ControlToValidate="MailTB"
                                ErrorMessage="כתובת המייל הוזנה בפורמט לא נכון">
                            <img class="fieldError" src="images/requiredField.png"  title="כתובת מייל לא חוקית"/></asp:RegularExpressionValidator>


                        </div>

                    </div>


                </div>
            </div>

        </div>
        <!-- Starts user dashboard -->
        <div class="row">
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>נתוני מתגבר</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
            <div class="row">

                <input type="image" id="imagPrint" src="images/printer-service.png" alt="Print" width="30" height="30" onclick="printDiv('report_PH')" title="הדפסה" />
                <div id="report_PH">
                    <h3 id="reportTitle"></h3>
                    <div class="row">
                        <div class="col-lg-6 center" id="chart" style="width: 60%; margin-top: 20px;">
                        </div>

                        <div class="col-lg-6 center" id="teacherKPIDiv" style="width: 40%;">
                            <div id="hoursKPI" class="teacherKPI">

                                <p>
                                    <asp:Label ID="HiddenHoursCounter" CssClass="countersKPI" runat="server" Text=""></asp:Label>
                                </p>
                                <p>שעות תגבורים</p>
                                <p>במרכז</p>
                            </div>

                            <div id="formsKPI" class="teacherKPI">
                                <p>
                                    <asp:Label ID="HiddenAttendanceFormCounter" CssClass="countersKPI" runat="server" Text=""></asp:Label>

                                </p>
                                <p>טפסי משוב</p>
                                <p>חסרים</p>
                            </div>
                        </div>


                    </div>
                    <%--  כמות שעות לפי חודשים למתגבר --%>

                    <table id="hoursChartByMonths" class="table table-bordered" style="margin: 0 auto; width: 200px; direction: rtl; display: none;">
                        <thead>
                            <tr>
                                <th>חודש</th>
                                <th>כמות</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>

                </div>
            </div>

        </div>

        <!-- Ends contact form 1 -->
    </div>
    <div style="margin-bottom: 20px;">&nbsp</div>
    <div id="confirmationModal" class="modal fade modal-sm modaltb" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content btn">
                <div class="modal-header">
                    <h5>המתגבר עודכן בהצלחה</h5>
                    &nbsp
                    <button type="button" class="btn btn-sm btn-success" data-dismiss="modal" onclick="window.location='ShowTeacher.aspx'">חזור למסך מתגברים</button>
                </div>
            </div>

        </div>
    </div>

    <div id="deleteConfirmationModal" class="modal fade modal-md modaltb" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content btn">
                <div class="modal-header">
                    <h5>האם ברצונך למחוק את המתגבר ?</h5>
                    &nbsp
                    <asp:Button ID="Button2" runat="server" Text="מחק" OnClick="deleteBTN_Click" CssClass="btn btn-sm btn-danger" />
                    &nbsp&nbsp
                    <button type="button" class="btn btn-sm btn-success" data-dismiss="modal">ביטול</button>
                </div>
            </div>

        </div>
    </div>

</asp:Content>

