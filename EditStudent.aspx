<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="EditStudent.aspx.cs" Inherits="EditStudent" %>

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

        $(function () {
            $("#startDate").datepicker(
                { dateFormat: 'yy-mm-dd' }
            );
        });
        $(function () {
            $("#endDate").datepicker(
                { dateFormat: 'yy-mm-dd' }
            );
        });

        var DateFilter = {
            startDate: null,
            endDate: null,
            userId: null
        };


        function showReport() {
            DateFilter.startDate = $('#startDate').val();
            DateFilter.endDate = $('#endDate').val();
            if (DateFilter.startDate == "" && DateFilter.endDate == "") {
                DateFilter.startDate = "1970-01-01";
                DateFilter.endDate = "3000-01-01";
            }
            else if (DateFilter.startDate == "") {
                DateFilter.startDate = "1970-01-01";
            }

            else if (DateFilter.endDate == "") {
                DateFilter.endDate = "3000-01-01";
            }


            DateFilter.userId = document.getElementById('<%=IdTB.ClientID%>').value;
            var dataString = JSON.stringify(DateFilter);

            $.ajax({
                type: 'POST',
                url: 'WebService.asmx/StudentRequestsByProfession',
                data: dataString,
                contentType: "application/json; charset=utf-8",
                traditional: true,
                success: function (data) {
                    var StudentRequestsByProfessionCountList = $.parseJSON(data.d);
                    //יצירת גרף עמודות
                    var tbody = $('#requestsChartbyProfession').find('tbody');
                    $(tbody).find('tr').remove();
                    var str1 = "";
                    StudentRequestsByProfessionCountList.forEach(function (profession) {
                        str1 += '<tr>' +

                            '<td>' + profession.Pro_title + '</td>' +
                            '<td>' + profession.Amount + '</td>' +
                            '</tr>';
                    });
                    tbody.append(str1);


                    $(function () {
                        $('#chart').highcharts({
                            lang: {
                                noData: 'אין בקשות ממתינות עבור התלמיד'
                            },
                            noData: {
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '20px',
                                    color: '#303030'
                                }
                            },
                            data: {
                                table: 'requestsChartbyProfession'
                            },
                            chart: {
                                type: 'column'
                            },
                            title: {
                                style: {
                                    color: '#337ab7',
                                    font: '20px "Open Sans", sans-serif'
                                },
                                text: ' בקשות ממתינות לפי מקצועות'
                            },
                            yAxis: {
                                allowDecimals: false,
                                title: {
                                    text: 'כמות'
                                }
                            },
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

            $.ajax({
                type: 'POST',
                url: 'WebService.asmx/StudentClassesByProfession',
                data: dataString,
                contentType: "application/json; charset=utf-8",
                traditional: true,
                success: function (data) {
                    var StudentClassesByProfessionCountList = $.parseJSON(data.d);


                    //יצירת הגרפים- עוגה
                    var tbody = $('#ClassesbyProfessionCountPie').find('tbody');
                    $(tbody).find('tr').remove();
                    var str2 = "";
                    StudentClassesByProfessionCountList.forEach(function (profession) {
                        str2 += '<tr>' +
                            '<td>' + profession.Pro_title + '</td>' +
                            '<td>' + profession.Amount + '</td>' +
                            '</tr>';
                    });
                    tbody.append(str2);

                    $(function () {
                        $('#pie').highcharts({
                            lang: {
                                noData: 'אין תגבורים עבור התלמיד'
                            },
                            noData: {
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '20px',
                                    color: '#303030'
                                }
                            },
                            data: {
                                table: 'ClassesbyProfessionCountPie'
                            },
                            chart: {
                                type: 'pie'
                            },
                            title: {
                                style: {
                                    color: '#337ab7',
                                    font: '20px "Open Sans", sans-serif'
                                },
                                text: 'תגבורים לפי מקצועות '
                            },
                            series: [{

                                showInLegend: true


                            }],
                            legend: {
                                rtl: true
                            },
                            plotOptions: {
                                pie: {
                                    innerSize: 100,
                                    depth: 45,
                                    allowPointSelect: false,
                                    cursor: 'pointer',
                                    dataLabels: {
                                        enabled: false,
                                        depth: 35,
                                        format: '<b>{point.name}</b><br/>: {point.y}',
                                        style: {
                                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                        },

                                    }
                                }
                            },


                            tooltip: {
                                enabled: true,
                                rtl: true,
                                formatter: function () {
                                    var name = this.point.name;
                                    var quan = this.y;
                                    var str = name + " - כמות  : " + quan + "";
                                    return str;
                                }
                            }
                        });
                    });
                }
            });  //End ajax call for student classes

        }

        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;

            window.print();
            document.body.innerHTML = originalContents;
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

        #endDate, #startDate {
            direction: rtl;
            margin: 0 auto;
            float: right;
            margin-bottom: 100px;
            margin-left: 40px;
        }

        .filterBTN {
            margin: 10px;
            background-color: #9fccdf;
            width: 100px;
        }

        #report_PH {
            margin-bottom: 100px;
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

        .center {
            text-align: center;
        }

        .presenceKPI {
            background-color: #245581;
            width: 130px;
            height: 110px;
            display: inline-block;
            text-align: center;
            padding-top: 10px;
            padding-bottom: 10px;
            color: white;
            font-size: 30px;
        }

        p {
            color: white;
            font-size: 16px;
        }

        .countersKPI {
            padding-top: 5px;
            font-size: 30px;
        }

        #presenceKPI {
            background-color: #8cb332;
        }

        #notpresenceKPI {
            background-color: #ea7066;
        }

        .fieldError {
            height: 25px;
            width: auto;
            position: relative;
            float: left;
            left: 240px;
            top: -40px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_calendar.aspx">בית</a></li>
                <li><a href="ShowStudents.aspx">תלמידים</a></li>
                <li class="active">עריכת פרטי תלמיד</li>
            </ol>

            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>עריכת פרטי תלמיד</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>

    <!-- Starts contact form -->
    <div class="container">
        <div class="row cbRight">
            <asp:CheckBox ID="isEntitledCB" runat="server" />
            זכאות
            &nbsp&nbsp
                <asp:CheckBox ID="statusCB" runat="server" />
            משתמש פעיל
        </div>

        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="homeContactContent">

                    <div class="form-group">

                        <asp:DropDownList ID="instructorDDL" runat="server" CssClass="form-control border-color-4 tb" placeholder="בחר מדריך">
                        </asp:DropDownList>
                      
                        <asp:RequiredFieldValidator ID="insValidator" runat="server"
                            ControlToValidate="instructorDDL"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            InitialValue="בחר מדריך"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" />
                        </asp:RequiredFieldValidator>
                   
                        </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="LastNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם משפחה"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="LNValidator" runat="server"
                            ControlToValidate="LastNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                       </asp:RequiredFieldValidator>
                    
                    </div>

                    <div class="form-group">
                        <i class="fa fa-phone"></i>
                        <asp:TextBox ID="PhoneTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="טלפון"></asp:TextBox>
                        
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
                        <i class="fa fa-comments" aria-hidden="true"></i>
                        <asp:TextBox ID="NoteTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="הערות"></asp:TextBox>
                    </div>

                    <div class="form-group" style="text-align: center; padding-left: 170px;">
                        <button id="deletePopUp" class="btn btn-danger" style="display: inline-block; margin-right: 20px;" type="button" data-toggle="modal" data-target="#deleteConfirmationModal"><span class="glyphicon glyphicon-trash"></span></button>

                        <asp:Button ID="saveBTN" runat="server" Text="עדכון תלמיד" OnClick="saveBTN_Click" CssClass="btn btn-primary " data-target="#confirmationModal" />
                    </div>

                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="homeContactContent">

                    <div class="form-group">
                        <i class="fa fa-id-card"></i>
                        <asp:TextBox ID="IdTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="ת.ז" ReadOnly="true"></asp:TextBox>


                    </div>

                    <div class="form-group">
                        <i class="fa fa-user"></i>
                        <asp:TextBox ID="FirstNameTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="שם פרטי"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="FNRF" runat="server"
                            ControlToValidate="FirstNameTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-birthday-cake"></i>
                        <asp:TextBox ID="BirhtDateTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="תאריך לידה dd/mm/yyyy"></asp:TextBox>


                        <asp:RegularExpressionValidator ID="dateRegex" runat="server"
                            ErrorMessage="Enter valid date"
                            ControlToValidate="BirhtDateTB"
                            ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$">
                                <img class="fieldError" src="images/requiredField.png"  title="אנא הכנס תאריך תקין"/>
                        </asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <i class="fa fa-envelope"></i>
                        <asp:TextBox ID="MailTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="אימייל"></asp:TextBox>
                       
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server"
                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                            ControlToValidate="MailTB"
                            ErrorMessage="כתובת המייל הוזנה בפורמט לא נכון">
                            <img class="fieldError" src="images/requiredField.png"  title="כתובת מייל לא חוקית"/></asp:RegularExpressionValidator>

                    </div>

                    <div class="form-group">
                        <i class="fa fa-unlock-alt"></i>
                        <asp:TextBox ID="PasswordTB" runat="server" CssClass="form-control border-color-4 tb" placeholder="סיסמא"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="passRF" runat="server"
                            ControlToValidate="PasswordTB"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה"/>
                        </asp:RequiredFieldValidator>

                    </div>

                    <div class="form-group">
                        <asp:DropDownList ID="gradeDDL" runat="server" Style="text-align: right" CssClass="form-control border-color-4 tb">
                            <asp:ListItem>בחר כיתה</asp:ListItem>
                            <asp:ListItem>כיתה ז</asp:ListItem>
                            <asp:ListItem>כיתה ח</asp:ListItem>
                            <asp:ListItem>כיתה ט</asp:ListItem>
                            <asp:ListItem>כיתה י</asp:ListItem>
                            <asp:ListItem>כיתה יא</asp:ListItem>
                            <asp:ListItem>כיתה יב</asp:ListItem>
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="gradeRF" runat="server"
                            ControlToValidate="gradeDDL"
                            ErrorMessage="שדה זה הינו שדה חובה"
                            InitialValue="בחר כיתה"
                            ForeColor="Red">
                            <img class="fieldError" src="images/requiredField.png" title="שדה זה הינו שדה חובה" />
                        </asp:RequiredFieldValidator>
                   
                        </div>
                </div>
            </div>

        </div>
        <!-- Starts user dashboard -->
        <div class="row">
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>נתוני תלמיד</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
            <div class="row">
                <input type="text" id="startDate" readonly="readonly" placeholder="בחר תאריך התחלה" name="startDatename" />
                <input type="text" id="endDate" readonly="readonly" placeholder="בחר תאריך סיום" name="endDatename" title="במידה ולא נבחר תאריך, תאריך הסיום יהיה היום" />
                <input type="button" id="profession_reportBTN" value="סנן" class="btn btn-sm filterBTN" onclick="showReport()" style="float: right; margin: -5px;" />
                <input type="image" id="imagPrint" src="images/printer-service.png" alt="Print" width="30" height="30" onclick="printDiv('report_PH')" title="הדפסה" />
            </div>
            <div id="report_PH" style="position: relative; top: -100px;">
                <div>

                    <h3 id="title"></h3>
                    <div class="row">
                        <div class="col-lg-6" id="chart" style="width: 50%; margin-top: 20px;"></div>
                        <br />
                        <div class="col-lg-6" id="pie" style="width: 50%;"></div>
                    </div>
                    <%--  כמות בקשות לפי מקצוע לתלמיד --%>

                    <table id="requestsChartbyProfession" class="table table-bordered" style="display: none; margin: 0 auto; width: 200px; direction: rtl;">
                        <thead>
                            <tr>
                                <th>מקצוע</th>
                                <th>כמות</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>

                    <%--   כמות תגבורים שהתלמיד נרשם אליהם ואושרו לפי מקצוע--%>
                    <table id="ClassesbyProfessionCountPie" class="table table-bordered" style="display: none; margin: 0 auto; width: 200px; direction: rtl;">
                        <thead>
                            <tr>

                                <th>מקצוע</th>
                                <th>כמות</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>

                <div class="row" style="position: relative; top: 30px;">
                    <div class="col-lg-6 center">
                        <h4>הערות מתגברים
                        </h4>


                        <asp:SqlDataSource ID="commentsForStudentDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select StLes_stuId, ActLes_date,Pro_Title,(Tea_FirstName +' '+ Tea_LastName) as 'teacher name' ,comments from signedToLesson inner join ActualLesson on ActLes_LesId=StLes_ActLesId and ActLes_date=StLes_ActLesDate inner join lesson on Les_Id = ActLes_LesId inner join Profession on Pro_Id = Les_Pro_Id  inner join Teacher on Les_Tea_Id=tea_id where StLes_stuId=@stu_id and (comments is NOT NULL AND comments <> '') "></asp:SqlDataSource>

                        <asp:GridView ID="commentsForStudentGRDW" EmptyDataText="אין הערות עבור התלמיד" runat="server" CssClass="grid" DataSourceID="commentsForStudentDS" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Style="direction: rtl; margin-top: 20px; margin-bottom: 100px; text-align: center; width: 100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
                            <AlternatingRowStyle BackColor="#F7F7F7" />

                            <Columns>
                                <asp:BoundField DataField="ActLes_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="תאריך" ItemStyle-Width="15%" SortExpression="ActLes_date" />
                                <asp:BoundField DataField="teacher name" HeaderText="מתגבר" ItemStyle-Width="20%" SortExpression="teacher name" />
                                <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" ItemStyle-Width="20%" SortExpression="Pro_Title" />
                                <asp:BoundField DataField="comments" HeaderText="הערה" ItemStyle-Width="40%" SortExpression="comments" />
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
                    <div class="col-lg-6 center">
                        <h4>נוכחות בתגבורים
                        </h4>
                        <div id="notpresenceKPI" class="presenceKPI">

                            <p>
                                <asp:Label ID="notPres" CssClass="countersKPI" runat="server" Text=""></asp:Label>
                            </p>
                            <p>תגבורים בהם</p>
                            <p>התלמיד לא נכח</p>
                        </div>
                        <div id="presenceKPI" class="presenceKPI">

                            <p>
                                <asp:Label ID="pres" CssClass="countersKPI" runat="server" Text=""></asp:Label>
                            </p>
                            <p>תגבורים בהם</p>
                            <p>התלמיד נכח</p>
                        </div>

                        <div id="totalKPI" class="presenceKPI">

                            <p>
                                <asp:Label ID="total" CssClass="countersKPI" runat="server" Text=""></asp:Label>

                            </p>
                            <p>סך הכל</p>
                            <p>תגבורים</p>
                        </div>
                    </div>

                </div>
                <!-- Ends user dashboard -->

                <!-- Ends contact form -->


            </div>
        </div>
    </div>


    <div id="deleteConfirmationModal" class="modal fade modal-md modaltb" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content btn">
                <div class="modal-header">
                    <h5>האם ברצונך למחוק את התלמיד ?</h5>
                    &nbsp
                    <asp:Button ID="deleteBTN" runat="server" Text="מחק" OnClick="deleteBTN_Click" CssClass="btn btn-sm btn-danger" />
                    &nbsp&nbsp
                    <button type="button" class="btn btn-sm btn-success" data-dismiss="modal">ביטול</button>
                </div>
            </div>

        </div>
    </div>

</asp:Content>

