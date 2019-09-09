<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMasterPage.master" AutoEventWireup="true" CodeFile="student_calendar.aspx.cs" Inherits="student_calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <meta charset="utf-8" />

    <script src='assets/js/jquery.min.js'></script>
    <script src='assets/js/jquery-ui.min.js'></script>

    <script src='assets/js/moment.min.js'></script>
    <link href='assets/css/fullcalendar.css' rel='stylesheet' />
    <link href='assets/css/fullcalendar.print.css' rel='stylesheet' media='print' />
    <script src='assets/js/fullcalendar.min.js'></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />

    <script type='text/javascript'>
        // script -> webservice -> tigbur -> dbservices and all the way back



        $(document).ready(function () {
            var userID = $("#" + '<%= userId.ClientID %>').val();

            //error popup alert func
            function alertFunc(message_alert) {
                $('#alertlabel').empty();
                $('#alertlabel').append(message_alert);

                $("#alertModelDIV").dialog({
                    autoOpen: false,

                });
                $('#alertModelDIV').dialog('open');
                $("#alertModelDIV").dialog({
                    title: " הודעה",
                });
            }

            $('#close_alertlabel').click(function () {
                $('#alertModelDIV').dialog('close');
            });



            function formatDate(date) {
                var dateArr = date.split("-");
                year = dateArr[0];
                day = dateArr[1];
                month = dateArr[2];

                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day;
                return (year + "-" + month + "-" + day);

            }

            function formatDateUTC(date) {// yyyy-mm-dd
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();
                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day;
                return (year + "-" + month + "-" + day);
            }

            // page is now ready, initialize the calendar...
            var eventData = {
                id: null,
                title: null,
                start: null,
                end: null,
                startTime: null,
                endTime: null,
                teacherId: null,
                teacherName: null,
                profId: null,
                limit: null,
                actualLimit: null,
                color: null,
                type: null

            };

            var reqData = {
                id: null,
                date: null,
                stuId: null,
                status: null,
                perm: null,
                sub_date: null,
                type: null
            };

            var TigburById = {
                id: null,
                stuID: null,
                IsCheckNeeded: null
            };

            var amountCheck = {
                signed2: null,
            }

            var tigData4StuList = {
                id: null,
                date: null
            }

            var checkIfSigned = {
                id: null
            }







            var myArray = []; // creating a new array object
            myArray['100'] = '#4d4dff'; // setting the attribute a to blue
            myArray['101'] = '#3333ff';
            myArray['102'] = '#1a1aff';
            myArray['103'] = '#DA8700';
            myArray['104'] = '#BA7300';
            myArray['105'] = '#7C4D00';
            myArray['106'] = '#660b4a';
            myArray['signed'] = '#009F02';
            myArray['full'] = 'grey';

            $('#calendar').fullCalendar({
                utc: true,
                axisFormat: 'HH:mm',
                timeFormat: 'HH:mm',
                minTime: "14:00:00",
                maxTime: "21:00:00",
                nowIndicator: true,
                height: 600,
                header: {
                    left: 'next,prev today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },

                selectable: true,
                editable: false,
                isRTL: true,
                monthNames: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר"],
                monthNamesShort: ['ינואר', 'פברואר', 'מרץ', 'אפריל', 'מאי', 'יוני', 'יולי', 'אוגוסט', 'ספטמבר', 'אוקטובר', 'נובמבר', 'דצמבר'],
                dayNames: ['ראשון', 'שני', 'שלישי', 'רביעי', 'חמישי', 'שישי', 'שבת'],
                dayNamesShort: ['ראשון', 'שני', 'שלישי', 'רביעי', 'חמישי', 'שישי', 'שבת'],
                buttonText: {
                    today: 'היום',
                    month: 'חודש',
                    week: 'שבוע',
                    day: 'יום'
                },// datetabs names init
                defaultView: 'agendaWeek',
                //  allDaySlot: false,
                hiddenDays: [5, 6],
                slotDuration: '00:15:00',
                slotEventOverlap: false,
                eventLimit: true, // allow "more" link when too many events
                allDaySlot: false,


                // getting all events from db and putting them into fullcalendar
                events: function (event) {
                    checkIfSigned.id = userID;
                    var dataString = JSON.stringify(checkIfSigned);
                    $('#calendar').fullCalendar('removeEvents', event._id)
                    $.ajax({
                        url: 'WebService.asmx/getSchedule',
                        data: dataString,
                        type: 'POST', // Send post data
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8',
                        async: false,
                        success: function (obj) {
                            var obj = $.parseJSON(obj.d);
                            cache: true;
                            $(obj).each(function () {
                                var setcolor = myArray[$(this).attr('ProfId')];
                                if (parseInt($(this).attr('ActualLimit')) == 0) { setcolor = myArray['full']; };
                                if (parseInt($(this).attr('IsSigned')) == 1) { setcolor = myArray['signed']; };
                                eventData = {
                                    id: $(this).attr('Id'),
                                    title: $(this).attr('ProfName') + "," + $(this).attr('TeacherName'),
                                    start: $(this).attr('TigburDate') + "T" + $(this).attr('StartTime'),
                                    end: $(this).attr('TigburDate') + "T" + $(this).attr('EndTime'),
                                    profId: $(this).attr('ProfId'),
                                    color: setcolor,
                                    type: "tutering"


                                };
                                $('#calendar').fullCalendar('renderEvent', eventData, true);
                            });

                        },
                        error: function (result) { alertFunc(" הייתה בעיה בהבאת התגבורים"); }
                    });
                },


                //===============================click==========================================
                //event data popup ajax func WS -> tigburim -> dbs and all the way back
                eventClick: function (event) {

                    TigburById.id = event.id;
                    TigburById.stuID = userID;
                    TigburById.IsCheckNeeded = 1;
                    var dataString = JSON.stringify(TigburById);
                    $.ajax
                        ({
                            url: 'WebService.asmx/getTigburById',
                            data: dataString,
                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json; charset = utf-8', // sent to the server
                            async: false,
                            success: function (data) {
                                var tigubrObj = $.parseJSON(data.d);
                                $("#" + '<%= add_Bakasha_Bitul.ClientID %>').hide() //fadeOut('fast'); or fadeIn('fast'); for effects
                                $("#" + '<%= add_Bakasha.ClientID %>').show();
                                $("#" + '<%= del_Bakasha.ClientID %>').show();
                                if (parseInt($(tigubrObj).attr('IsSigned')) == 1) {
                                    $("#" + '<%= add_Bakasha_Bitul.ClientID %>').show();
                                    $("#" + '<%= add_Bakasha.ClientID %>').hide();
                                    $("#" + '<%= del_Bakasha.ClientID %>').hide();

                                }
                                document.getElementById("ContentPlaceHolder1_ProfessionName").innerHTML = tigubrObj.ProfName;
                                document.getElementById("ContentPlaceHolder1_timeStartLabel").innerHTML = tigubrObj.StartTime.substring(0, 5);
                                document.getElementById("ContentPlaceHolder1_timeEndLabel").innerHTML = tigubrObj.EndTime.substring(0, 5);
                                document.getElementById("ContentPlaceHolder1_TeacherLabel").innerHTML = tigubrObj.TeacherName;
                                document.getElementById("ContentPlaceHolder1_LimitLabel").innerHTML = tigubrObj.Limit;
                                document.getElementById("ContentPlaceHolder1_quantityleft").innerHTML = tigubrObj.ActualLimit;
                                amountCheck.signed2 = tigubrObj.ActualLimit;
                                reqData.id = tigubrObj.TrueId;
                                reqData.date = tigubrObj.TigburDate
                                tigData4StuList.id = tigubrObj.TrueId;
                                tigData4StuList.date = tigubrObj.TigburDate

                            },
                            error: function (result) { alertFunc(" התגבור לא זוהה"); }
                        });
                    // ajax return format:
                    //"{\"Id\":142,\"TeacherId\":191919,\"TeacherName\":\"שלום לוי\",\"ProfId\":100,\"ProfName\":\"מתמטיקה 3 יחידות\",\"Limit\":50,
                    //\"ActualLimit\":50,\"StartTime\":\"12:00:00\",\"EndTime\":\"16:00:00\",\"TigburDate\":\"2018-04-04\",\"TrueId\":120}"

                    $("#showTigburPopupPopupDiv").dialog({
                        autoOpen: false,
                        height: 315,
                        width: 295,
                        modal: true,
                        resizable: true,
                    });

                    $('#showTigburPopupPopupDiv').dialog('open');
                    $("#showTigburPopupPopupDiv").dialog({
                        title: "פרטי תגבור",
                    });

                },
            });

            //===============================request submission==========================================
            $("#ContentPlaceHolder1_add_Bakasha").click(function addRequest() {

                reqData.perm = 0;
                reqData.status = 2; // status default is 2.
                reqData.type = 0;
                var utc = new Date();
                utc = formatDateUTC(utc);
                if (reqData.date <= utc) {
                    alertFunc(" לא ניתן להגיש בקשה בתאריך שעבר");
                    return;
                }
                if (parseInt(amountCheck.signed2) < 1) {
                    alertFunc(" התגבור מלא");
                    return;
                }
                reqData.sub_date = utc;
                reqData.stuId = userID; // stub! need to figure cookie stuff out
                var dataString = JSON.stringify(reqData);
                $.ajax
                    ({
                        url: 'WebService.asmx/InsertRequest',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            alertFunc(" הגשת התגבור הושלמה בהצלחה");
                        },
                        error: function (result) { alertFunc(" הוגשה בקשה לתגבור זה"); }
                    });


            });
            //===============================request deletion==========================================
            $("#ContentPlaceHolder1_del_Bakasha").click(function addRequest() {
                var utc = new Date();
                utc = formatDateUTC(utc);
                if (reqData.date <= utc) {
                    alertFunc(" לא ניתן למחוק בקשה בתאריך שעבר");
                    return;
                }
                reqData.stuId = userID; // stub! need to figure cookie stuff out
                var dataString = JSON.stringify(reqData);
                $.ajax
                    ({
                        url: 'WebService.asmx/delRequest',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            if ($.parseJSON(data.d) == null) {
                                alertFunc(" לא הוגשה בקשה לתגבור זה");
                                return;
                            }
                            alertFunc(" מחיקת הבקשה הושלמה בהצלחה");
                        },
                        error: function (result) { alertFunc(" הפעולה לא הושלמה"); }
                    });


            });

            //===============================get student list==========================================
            $("#ContentPlaceHolder1_show_studentlist").click(function showStudentList() {

                document.getElementById("ContentPlaceHolder1_stuListLBL").innerHTML = " ";
                var dataString = JSON.stringify(tigData4StuList);
                $.ajax
                    ({
                        url: 'WebService.asmx/getStudentList4CAL',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            stuObj = $.parseJSON(data.d);
                            document.getElementById("ContentPlaceHolder1_stuListLBL").innerHTML += ":שם התלמיד<br/>";
                            $.each(stuObj, function (i, row) {
                                document.getElementById("ContentPlaceHolder1_stuListLBL").innerHTML += row.FirstName + "<br/>";
                            });
                        },
                        error: function (result) { alertFunc(" הייתה בעיה בהבאת רשימת תלמידים"); }
                    });
                /// ajax returns "[{\"Stu_id\":0,\"Stu_Instructor_id\":0,\"FirstName\":\"דור שפיגל\",\"LastName\":null,\"BirthDate\":null,
                ///\"PhoneNumber\":null,\"Email\":null,\"Address\":null,\"Status\":false,\"Password\":null,\"Grade\":null,
                ///\"Notes\":null,\"IsEntitled\":false},{\"Stu_id\":0,\"Stu_Instructor_id\":0,\"FirstName\":\"תומרבזרנו\",
                //\"LastName\":null,\"BirthDate\":null,\"PhoneNumber\":null,\"Email\":null,\"Address\":null,\"Status\":false,\"Password\":null,\"Grade\":null,\"Notes\":null,\"IsEntitled\":false}]"

                $("#showStudentListrPopupPopupDiv").dialog({
                    autoOpen: false,
                    height: 254,
                    width: 290,
                    modal: true,
                    resizable: true,

                });

                $('#showStudentListrPopupPopupDiv').dialog('open');
                $("#showStudentListrPopupPopupDiv").dialog({
                    title: "תלמידים רשומים לתגבור",
                });


            });

            //===============================request submission==========================================
            $("#ContentPlaceHolder1_add_Bakasha_Bitul").click(function addRequest() {
                reqData.perm = 0;
                reqData.status = 2; // status default is 2.
                reqData.type = 1;
                var utc = new Date();
                utc = formatDateUTC(utc);
                if (reqData.date <= utc) {
                    alertFunc(" לא ניתן להגיש בקשה לביטול השתתפות בתאריך שעבר");
                    return;
                }
                reqData.sub_date = utc;
                reqData.stuId = userID; // stub! need to figure cookie stuff out
                var dataString = JSON.stringify(reqData);
                $.ajax
                    ({
                        url: 'WebService.asmx/InsertRequest',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            alertFunc(" הגשת בקשה לביטול השתתפות הושלמה");
                        },
                        error: function (result) { alertFunc("לא ניתן להגיש בקשה לביטול פעמיים"); }
                    });
            });


        });



    </script>
    <style>
        #calendar {
            width: 75%;
            margin: 0 auto;
            margin-bottom: 80px;
        }

        .ui-dialog .ui-dialog-title {
            text-align: center;
        }

        .fc-button {
        }

        #loading {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            position: fixed;
            display: block;
            opacity: 0.7;
            z-index: 99;
            text-align: center;
        }

        #loading-image {
            position: absolute;
            top: 40%;
            left: 45%;
            z-index: 100;
        }

        .fc-widget-header {
            background-color: rgba(0, 139, 201, 0.9);
            color: white;
        }

        .fc-state-default {
            background-color: rgba(0, 139, 201, 0.9);
            color: white;
            background: rgba(0, 139, 201, 0.9);
        }


        .ui-dialog, ui-widget {
            height: 400px;
            width: 350px;
        }

        ui-dialog-titlebar {
            background-color: rgba(0, 72, 146, 0.9);
            color: white;
        }

        div.ui-dialog-titlebar.ui-widget-header.ui-corner-all.ui-helper-clearfix.ui-draggable-handle {
            background-color: rgba(0, 139, 201, 0.9);
            background: rgba(0, 139, 201, 0.9);
            color: white;
        }

        .fc-state-active {
            background-color: rgb(0, 86, 189);
        }

        .fc-unthemed hr, .fc-unthemed .fc-popover .fc-header {
            background: rgba(0, 139, 201, 0.9);
        }

        #indexMap {
            position: absolute;
            top: 240px;
            left: 85%;
            width:180px;
            height:auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="loading">
        <img id="loading-image" src="images/loading2.gif" alt="Loading..." />
    </div>

    <section class="mainContent full-width clearfix featureSection">
        <div class="container">
            <ol class="breadcrumb">
                <li><a href="student_dashboard.aspx">בית</a></li>
                <li class="active">מערכת כללית</li>

            </ol>
            <div id="pageTitle" class="sectionTitle text-center" style="display: none;">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>מערכת כללית</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>
        </div>
    </section>


    <div class="container">


        <div id="calendar"></div>
        <img id="indexMap" src="images/student_index_horizontal.jpg" />
        <!--lesson details start -->
        <div data-role="popup" id="showTigburPopup" class="ui-content">
            <div id="showTigburPopupPopupDiv" style="display: none; text-align: center;">

                <table runat="server" dir="rtl">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="מקצוע :"></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="ProfessionName" runat="server" Text=" "></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="שעת התחלה :"></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="timeStartLabel" runat="server" Text=" "></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="שעת סיום :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="timeEndLabel" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="מתגבר :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="TeacherLabel" runat="server" Text=" "></asp:Label>

                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="מקומות :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label class="ui-button" ID="LimitLabel" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="מקומות פנויים :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="quantityleft" runat="server" Text=" "></asp:Label>
                        </td>

                    </tr>
                    <tr style="text-align: center;">
                        <td>
                         <br />
                            <asp:Button ID="show_studentlist" CssClass="btn btn-primary btn-xs btn-width" runat="server" Text="תלמידים רשומים" />&nbsp;&nbsp;
                        </td>
                        <td>
                           <br />
                            <asp:Button ID="add_Bakasha_Bitul" CssClass="btn btn-danger btn-sm btn-width" runat="server" Text="בטל השתתפות" />
                        </td>
                    </tr>
                    <tr style="text-align: center;">
                        <td>
                           
                            <asp:Button CssClass="btn btn-success btn-xs btn-width" ID="add_Bakasha" runat="server" Text="הגש בקשה" />
                           
                        </td>
                        <td>
                            
                            <asp:Button ID="del_Bakasha" CssClass="btn btn-danger btn-xs btn-width" runat="server" Text="בטל בקשה" />
                         
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="userId" runat="server" />

    <!--student list details start -->
    <div data-role="popup" id="showStudentList" class="ui-content" style="min-width: 250px;">
        <div id="showStudentListrPopupPopupDiv" style="display: none; text-align: center;">
            <asp:Label ID="stuListLBL" runat="server" Text=" "></asp:Label>
        </div>
    </div>

    <!--error alert start -->
    <div data-role="popup" id="alertModel" class="ui-content" style="min-width: 250px; width: auto">
        <div id="alertModelDIV" style="display: none;">
            <label id="alertlabel"></label>
            <button type="button" id="close_alertlabel" class="btn btn-primary btn-width" style="float: left;">אישור</button>
        </div>

    </div>

    <script type="text/javascript">
        $(window).load(function () {
            $('#loading').hide();
            $('#pageTitle').show();
        });
    </script>

</asp:Content>

