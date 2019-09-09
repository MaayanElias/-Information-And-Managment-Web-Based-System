<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/data.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/no-data-to-display.js"></script>

    <style>
        .row {
            margin: 0 auto;
            text-align: center;
        }

        .btn {
            margin: 10px;
            background-color: #245581;
        }

            .btn:hover {
                color: white;
                background-color: #84bed6;
            }

            .btn:focus {
                background-color: #84bed6;
                color: white;
            }


        h3 {
            text-align: center;
            margin-top: 20px;
            color: #337ab7;
        }

        h5 {
            text-align: center;
            margin: 0 auto;
            color: #337ab7;
            margin-left: 5px;
        }

        #endDate, #startDate {
            direction: rtl;
        }

        #imagPrint {
            vertical-align: middle;
        }

        #emptyReport {
            margin-top: 20px;
        }

        #emptyReportLogo {
            height: 280px;
            width: auto;
            opacity: 0.2;
        }

        #emptyReportText {
            opacity: 0.2;
            margin-bottom: 10px;
        }

        tr, th {
            text-align: center;
        }

    </style>


    <script>
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

        function hideDiv() {
            document.getElementById('emptyReport').style.display = "none";
        }

        var DateFilter = {
            startDate: null,
            endDate: null
        };

        function showReport() {
            document.getElementById("CommentsForStudents").style.display = "none";
            document.getElementById("chartsDiv").style.display = "";
            //ProfessionCountChart.style.display = "";
            hideDiv();
            DateFilter.startDate = $('#startDate').val();
            DateFilter.endDate = $('#endDate').val();
            if (DateFilter.startDate == "" && DateFilter.endDate == "") {
                document.getElementById("title").innerHTML = "דוח תגבורים לפי מקצועות";

                DateFilter.startDate = "1970-01-01";
                DateFilter.endDate = "3000-01-01";

            }
            else if (DateFilter.startDate == "") {
                DateFilter.startDate = "1970-01-01";
                document.getElementById("title").innerHTML = "דוח תגבורים לפי מקצועות עד תאריך  " + DateFilter.endDate;

            }

            else if (DateFilter.endDate == "") {
                DateFilter.endDate = "3000-01-01";
                document.getElementById("title").innerHTML = "דוח תגבורים לפי מקצועות מתאריך  " + DateFilter.startDate;

            }
            else {
                document.getElementById("title").innerHTML = "דוח תגבורים לפי מקצועות בין התאריכים " + DateFilter.startDate + " - " + DateFilter.endDate;

            }



            var dataString = JSON.stringify(DateFilter);

            $.ajax({
                type: 'POST',
                url: 'WebService.asmx/getProfessionCountForReport',
                data: dataString,
                contentType: "application/json; charset=utf-8",
                traditional: true,
                success: function (data) {
                    var ProfessionCountList = $.parseJSON(data.d);
                    //יצירת גרף עמודות
                    var tbody = $('#ProfessionCountChart').find('tbody');
                    $(tbody).find('tr').remove();
                    var str1 = "";
                    ProfessionCountList.forEach(function (profession) {
                        str1 += '<tr>' +

                            '<td>' + profession.Pro_title + '</td>' +
                            '<td>' + profession.Amount + '</td>' +
                            '</tr>';
                    });
                    tbody.append(str1);


                    $(function () {
                        $('#chart').highcharts({
                            lang: {
                                noData: 'אין נתונים להציג'
                            },
                            noData: {
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '20px',
                                    color: '#303030'
                                }
                            },
                            data: {
                                table: 'ProfessionCountChart'
                            },
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: ' '
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

                    //יצירת הגרפים- עוגה
                    var tbody = $('#ProfessionCountPie').find('tbody');
                    $(tbody).find('tr').remove();
                    var str1 = "";
                    ProfessionCountList.forEach(function (profession) {
                        str1 += '<tr>' +
                            '<td>' + profession.Pro_title + '</td>' +
                            '<td>' + profession.Amount + '</td>' +
                            '</tr>';
                    });
                    tbody.append(str1);

                    $(function () {
                        $('#pie').highcharts({
                            lang: {
                                noData: 'אין נתונים להציג'
                            },
                            noData: {
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '20px',
                                    color: '#303030'
                                }
                            },
                            data: {
                                table: 'ProfessionCountPie'
                            },
                            chart: {
                                type: 'pie'
                            },
                            title: {
                                text: ' '
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
            });


        }

        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;

            window.print();
            document.body.innerHTML = originalContents;
        }

        function showtable() {
            var x = document.getElementById("ProfessionCountChart");
            hideDiv();
            if (x.style.display === "none") {
                x.style.display = "block";
                document.getElementById("display").setActive = true;
            } else {
                x.style.display = "none";
            }
        }

        function showCommentsTable() {
            document.getElementById("emptyReport").style.display = "none";
            document.getElementById("chartsDiv").style.display = "none";
            document.getElementById("CommentsForStudents").style.display = "";
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="admin_dashboard.aspx">בית</a></li>
                <li class="active">דוחות</li>
            </ol>
            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>דוחות</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>
    <div class="container">
        <div class="row content">

            <input type="text" id="endDate" readonly="readonly" placeholder="תאריך סיום" name="endDatename" title="במידה ולא נבחר תאריך, תאריך הסיום יהיה היום" />

            <input type="text" id="startDate" readonly="readonly" placeholder="תאריך התחלה" name="startDatename" style="margin-left: 5px;" />

            <h5 style="display: inline;">סנן לפי תאריך</h5>
            <i class="fa fa-filter" aria-hidden="true"></i>
        </div>
        <div class="row content">

            <input type="image" id="imagPrint" src="images/printer-service.png" alt="Print" width="30" height="30" onclick="printDiv('report_PH')" title="הדפסה" />

            <input type="button" id="display" value="הצג/הסתר טבלת נתונים" class="btn btn-lg" onclick="showtable()" />

            <input type="button" id="profession_reportBTN" value="ביקוש מקצועות" class="btn btn-lg" onclick="showReport()" />

            <input type="button" id="requests_reportBTN" value="משובי תלמידים" class="btn btn-lg" onclick="showCommentsTable()" />

            <input type="button" id="teacher_reportBTN" value="שעות מתגברים" class="btn btn-lg" onclick="" />







        </div>
        <div id="emptyReport" class="row">
            <h3 id="emptyReportText">בחר דוח להצגה</h3>
            <img id="emptyReportLogo" src="images/report.gif" />
        </div>


        <div id="report_PH" style="text-align: center;">
            <div id="CommentsForStudents" style="display: none; margin-top: 20px; text-align:center;">
                <h3>דו"ח משובי תלמידים בתגבורים</h3>
                <asp:SqlDataSource ID="commentsDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select ([Stu_FirstName] +' ' + [Stu_LastName]) as 'student_name', ([Tea_FirstName]+' '+[Tea_LastName]) as 'teacher_name', [Pro_Title] ,comments from [dbo].[signedToLesson] inner join lesson on [StLes_ActLesId]=[Les_Id] inner join [dbo].[Teacher]  on [Tea_Id]= [Les_Tea_Id] inner join student on [Stu_Id]= [StLes_stuId] inner join [dbo].[Profession] on [Pro_Id]=[Les_Pro_Id] where [comments] <> null or [comments] <> ' ' order by student_name, teacher_name, [Pro_Title]"></asp:SqlDataSource>
                <asp:GridView ID="commentsGRDW" runat="server" AutoGenerateColumns="False" CssClass="grid" EnableViewState="false" AllowSorting="false" BackColor="White" BorderColor="#838384" BorderStyle="None" BorderWidth="2px" CellPadding="4" Style="direction: rtl; margin: 0 auto; margin-top: 20px;  width: 80%" DataSourceID="commentsDS" OnDataBound="commentsGRDW_DataBound">
                     <RowStyle HorizontalAlign="Center"></RowStyle>
                    <Columns>
                        <asp:BoundField DataField="student_name" HeaderText="שם התלמיד" SortExpression="student_name" ItemStyle-Width="15%">
                            <ItemStyle Font-Bold="True" BorderWidth="1" />
                        </asp:BoundField>
                        <asp:BoundField DataField="teacher_name" HeaderText="שם המתגבר" SortExpression="teacher_name"  ItemStyle-Width="15%"/>
                        <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" SortExpression="Pro_Title" ItemStyle-Width="15%"/>
                        <asp:BoundField DataField="comments" HeaderText="הערות" SortExpression="comments" ItemStyle-Width="55%"/>
                    </Columns>
                    <HeaderStyle Font-Bold="True" />
                   

                </asp:GridView>

            </div>
            <div id="chartsDiv">
                <h3 id="title"></h3>
                <div class="row">
                    <div id="chart" style="width: 50%; margin: 0 auto; float: right;"></div>
                    <div id="pie" style="width: 50%; margin: 0 auto; float: left;"></div>
                </div>
                <div class="row" style="text-align: center">
                    <table id="ProfessionCountChart" class="table table-bordered" style="margin: 0 auto; margin-top: 20px; display: none; width: 250px; text-align: center; direction: rtl;">
                        <thead>
                            <tr>
                                <th>שם</th>
                                <th>כמות</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <table id="ProfessionCountPie" class="table table-bordered" style="display: none;">
                    <thead>
                        <tr>

                            <th>שם</th>
                            <th>כמות</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div style="margin-bottom: 70px;">&nbsp</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

