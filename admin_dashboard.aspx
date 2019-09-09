<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="admin_dashboard.aspx.cs" Inherits="admin_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="assets/plugins/toastr/toastr.css" rel="stylesheet" />
    <script src="assets/plugins/toastr/toastr.js"></script>
    <script src="plugins/animateNumber/jquery.animateNumber.min.js"></script>
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
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }



        var DateFilter = {
            startDate: null,
            endDate: null
        };



        function loadSettings() {
            var RequestsNumber = document.getElementById("<%=HiddenRequestsCounter.ClientID %>").innerHTML;
            var AttenderFormsNumber = document.getElementById("<%=HiddenAttendenceFormsCounter.ClientID %>").innerHTML;
            var UnreadMessagesNumber = document.getElementById("<%=HiddenUnreadMessagesCounter.ClientID %>").innerHTML;

            $('#requestsCounter').animateNumber({ number: RequestsNumber });
            $('#attendenceCounter').animateNumber({ number: AttenderFormsNumber });
            $('#counter1').animateNumber({ number: 42 });
            $('#UnreadMessagesCounter').animateNumber({ number: UnreadMessagesNumber });



            loadGraphs()
            return false;
        };


        $(function () {
            $("#startDateTB").datepicker(
                { dateFormat: 'yy-mm-dd' }
                );
        });
        $(function () {
            $("#endDateTB").datepicker(
                { dateFormat: 'yy-mm-dd' }
                );
        });


        function loadGraphs() {
            DateFilter.startDate = $('#startDateTB').val();
            DateFilter.endDate = $('#endDateTB').val();
            if (DateFilter.startDate == "" && DateFilter.endDate == "") {
                DateFilter.startDate = "1970-01-01";
                DateFilter.endDate = "3000-01-01";

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
                                noData: 'אין נתונים להציג בטווח הזמן המבוקש'
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
                                style: {
                                    color: '#337ab7',
                                    font: '20px "Open Sans", sans-serif',
                                    
                                    
                                },
                                text: 'תגבורים לפי מקצוע',

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
                                        enabled: true,
                                    },
                                }
                            },


                            tooltip: {

                                enabled: false
                            },

                        });
                    });
                }
            });



        }

        toastr.options.onclick = function () { window.location.replace("ShowAdminMessages.aspx"); }


    </script>

    <style>
        .filterLogo {
            width: 25px;
            background-color: transparent;
            position: relative;
            top: 6px;
        }

        .kotert{
            background-color:rebeccapurple;
        }
        #filterRow {
            margin: 0 auto;
            margin-bottom: 10px;
        }

        input {
            margin-right: 10px;
            background-color: #245581;
            width: 70px;
        }

        #requestsKPI, #attendenceKPI, #KPI3, #messagesKPI {
            width: 130px;
            height: 110px;
            display: inline-block;
            text-align: center;
            padding-top: 10px;
            padding-bottom: 10px;
            color: white;
            font-size: 30px;
           
        }

        #requestsKPI {
            background-color: #245581;
        }

        #attendenceKPI {
            background-color: #245581;
        }

        #KPI3 {
            background-color: #245581;
        }

        #messagesKPI {
            background-color: #245581;
        }

            #requestsKPI:hover, #attendenceKPI:hover, #KPI3:hover, #messagesKPI:hover {
                background-color: #cac7ff;
                color: #333;
            }


        .margin-right {
            margin-right: 50px;
        }

        p {
            font-weight: bold;
            font-size: 18px;
            color: white;
        }

        .counter-text {
            font-size: 28px;
        }

        .kpi1-row, .upper_row {
            margin-bottom: 40px;
        }

        .btn:hover {
            color: white;
        }

        #filter_dayBTN:hover, #filter_weekBTN:hover, #filter_monthBTN:hover {
            color: white;
            background-color: #84bed6;
        }


        #chart {
            width: 90%;
            height: 300px;
            margin: 0 auto;
            float: left;
            text-align: center;
        }

        #endDateTB, #startDateTB {
            display: none;
        }


        .hiddenLBL {
            visibility: hidden;
        }

        .TDL {
            margin-right: 100px;
        }

        .filterBTN_leftSide {
            text-align: center;
        }

        .filterHeader {
            display: inline-block;
            margin-bottom: 0px;
            padding-right: 15px;
        }

        .KPI_box {
            border: solid 3px black;
            border-radius: 10px;
            height: 300px;
            padding: 10px;
            text-align: center;
        }

        .longText {
            width: 60px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .empty, .empty td {
            border: 0;
            background: none;
            margin: 0;
            padding: 0;
        }
        h3 {
            color: #337ab7;
            direction: rtl;
            display: inline-block;
            margin-bottom: 0 auto;
            margin-left:50px;
        }
        
        #TDL-div{
            text-align:center;
            float:initial
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="mainContent full-width clearfix featureSection">
        <div class="container" style="margin-bottom: 20px;">

            <div class="sectionTitle text-center">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>מסך מנהל</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

        </div>
    </section>
    <div class="container center-block" style="margin: 0 auto; align-content: center; direction: rtl;" >
        <div class="row" id="filterRow" style="width:59%; float:left">
            <div class="col-lg-8 filterBTN_leftSide" style=" width:100%">
                <asp:Button ID="filter_dayBTN" runat="server" Text="יום" CssClass="btn btn-sm" OnClick="filter_dayBTN_Click" />
                <asp:Button ID="filter_weekBTN" runat="server" Text="שבוע" CssClass="btn btn-sm" OnClick="filter_weekBTN_Click" />
                <asp:Button ID="filter_monthBTN" runat="server" Text="חודש" CssClass="btn btn-sm" OnClick="filter_monthBTN_Click" />
                <asp:ImageButton ID="filterBTN" CssClass="filterLogo" src="images/filterLogo.png" runat="server" OnClick="filter_clear_Click" ToolTip="נקה בחירה" />
                <h4 id="chartTitle" class="filterHeader" runat="server"></h4>

            </div>
            
        </div>
        
        <div class="row kpi1-row" >
            <div class="col-lg-8 center-block">
                <div id="chart" style="padding-left:25px"></div>
            </div>

            <div id="TDL-div" class="col-lg-4"">
                <h3 class="TDL">מדדי ביצוע</h3>
            </div>

            <div class="col-lg-4 " >
                <div class="KPI_box form-control border-color-4  ">
                    <div class="upper_row">
                        <%--upper section--%>

                        <a href="ShowRequests.aspx?id=1">
                            <div id="requestsKPI" class="btn">
                                <p><span id="requestsCounter" class="counter-text">0</span></p>
                                <p>בקשות</p>
                                <p>ממתינות</p>
                            </div>
                        </a>

                        <div id="attendenceKPI" class="btn margin-right">
                            <p><span id="attendenceCounter" class="counter-text">0</span></p>
                            <p>טפסי משוב</p>
                            <p>חסרים</p>
                        </div>
                    </div>


                    <div class="lower_row">
                        <%--lower section--%>
                        <a href="ShowAdminMessages.aspx">

                            <div id="messagesKPI" class="btn">

                                <p><span id="UnreadMessagesCounter" class="counter-text">0</span></p>
                                <p>הודעות</p>
                                <p>חדשות</p>
                            </div>
                        </a>
                        <div id="KPI3" class="btn margin-right">
                            <p><span id="counter1" class="counter-text">0</span></p>
                            <p>תגבורים</p>
                            <p>עתידיים</p>
                        </div>

                    </div>
                </div>
            </div>

        </div>
        <div class="row" >
            <div class="col-lg-6" style="width:48%; margin-left:25px">
                <h3 class=" ">הודעות אחרונות</h3>
                <asp:SqlDataSource ID="manMessagesDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select top(10) msg_date, Stu_FirstName + ' ' + Stu_LastName as 'from' , msg_subject , msg_content from StudentMessages  inner join Student on Student.Stu_Id=StudentMessages.msg_fromStudentId where msg_hasRead=0 order by msg_id DESC"></asp:SqlDataSource>
                <asp:GridView ID="manMessagesGRDW" EmptyDataText="אין הודעות חדשות" CssClass="grid" runat="server" OnRowDataBound="manMessagesGRDW_RowDataBound" Style="overflow: hidden; margin: 0 auto; margin-top: 20px; margin-bottom: 50px; text-align: center; width: 100%;" DataSourceID="manMessagesDS" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                    <Columns>
                        <asp:BoundField DataField="msg_date" HeaderText="תאריך" ItemStyle-Width="15%" SortExpression="msg_date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="from" HeaderText="מאת" ItemStyle-Width="15%" SortExpression="from" />
                        <asp:BoundField DataField="msg_subject" HeaderText="נושא" ItemStyle-Width="30%" SortExpression="msg_subject" />
                        <asp:BoundField DataField="msg_content" HeaderText="תוכן" ItemStyle-Width="40%" SortExpression="msg_content" ItemStyle-CssClass="longText">
                            <ItemStyle CssClass="longText" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataRowStyle Font-Size="12pt" ForeColor="Black"
                        HorizontalAlign="center" Wrap="False" />
       
                </asp:GridView>

            </div>
            <div class="col-lg-6" style="direction: rtl; float:initial; width:48%">
                <h3 style="text-align: center;">תגבורים קרובים</h3>
                <asp:SqlDataSource ID="upcomingLessonsDS" runat="server" ConnectionString="<%$ ConnectionStrings:studentDBConnectionString %>" SelectCommand="select top(10) actLes_date,Pro_Title,(Tea_FirstName + ' ' +  Tea_LastName) as 'full name',Les_StartHour,Les_EndHour,Les_MaxQuan,quantity from Lesson inner join ActualLesson on Les_Id = ActLes_LesId inner join Teacher on Les_Tea_Id= Tea_Id inner join Profession on Les_Pro_Id=Pro_Id where ActLes_date >= GETDATE()-1  AND actls_cancelled=0 order by ActLes_date"></asp:SqlDataSource>
                <asp:GridView ID="upcomingLessonsGRDW" CssClass="grid" runat="server" Style="margin: 0 auto; margin-top: 20px; margin-bottom: 50px; text-align: center; width: 100%" DataSourceID="upcomingLessonsDS" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" >
               <AlternatingRowStyle BackColor="#F7F7F7" />
                <RowStyle Height="30px" />
                    <Columns>
                        <asp:BoundField DataField="actLes_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="תאריך" SortExpression="actLes_date" />
                        <asp:BoundField DataField="Pro_Title" HeaderText="מקצוע" SortExpression="Pro_Title" />
                        <asp:BoundField DataField="full name" HeaderText="מתגבר" SortExpression="full name" />
                        <asp:BoundField DataField="Les_StartHour" HeaderText="התחלה" SortExpression="Les_StartHour"  DataFormatString="{0:hh\:mm}"/>
                        <asp:BoundField DataField="Les_EndHour" HeaderText="סיום" SortExpression="Les_EndHour"  DataFormatString="{0:hh\:mm}"/>
                        <asp:BoundField DataField="Les_MaxQuan" HeaderText="קיבולת" SortExpression="Les_MaxQuan" />
                        <asp:BoundField DataField="quantity" HeaderText="רשומים" SortExpression="quantity" />
                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>
    <input type="text" id="endDateTB" placeholder="בחר תאריך סיום" name="endDatename" value="<%= this.inputEndValue %>" />
    <input type="text" id="startDateTB" placeholder="בחר תאריך התחלה" name="startDatename" value="<%= this.inputStartValue %>" />


    <table id="ProfessionCountChart" class="table table-bordered" style="display: none; margin: 0 auto; width: 200px; direction: rtl;">
        <thead>
            <tr>
                <th>שם</th>
                <th>כמות</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
    <asp:Label ID="HiddenRequestsCounter" runat="server" Text="" CssClass="hiddenLBL"></asp:Label>
    <asp:Label ID="HiddenAttendenceFormsCounter" runat="server" Text="" CssClass="hiddenLBL"></asp:Label>
    <asp:Label ID="HiddenUnreadMessagesCounter" runat="server" Text="" CssClass="hiddenLBL"></asp:Label>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jsPlaceHolder" runat="Server">
</asp:Content>

