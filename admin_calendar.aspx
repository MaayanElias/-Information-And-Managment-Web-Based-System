<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="admin_calendar.aspx.cs" Inherits="admin_calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <meta charset="utf-8" />

    <script src='assets/js/jquery.min.js'></script>
    <script src='assets/js/jquery-ui.min.js'></script>
    <script src="plugins/select2/select2.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script src='assets/js/moment.min.js'></script>
    <link href='assets/css/fullcalendar.css' rel='stylesheet' />
    <link href='assets/css/fullcalendar.print.css' rel='stylesheet' media='print' />
    <script src='assets/js/fullcalendar.min.js'></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />

    <script type='text/javascript'>
        // script -> webservice -> tigbur -> dbservices and all the way back
        $(document).ready(function () {

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

            var TigburById = {
                id: null,
                stuID: null,
                IsCheckNeeded: null
            }

            var tigburDate = {
                startDate: null
            }

            var tigburProf = {
                id: null,
                ProfName: null
            }

            var Metagber = {
                id: null,
                Name: null,
                Prof1: null,
                Prof2: null,
                prof3: null,
                prof4: null
            }

            var Obj4delete = {
                id: null,
                date: null,
                adminID: null,

            }

            var ActualLes = {
                id: null,
                date: null,
                quantity: null
            }


            var myArray = []; // creating a new array object
            myArray['100'] = '#4d4dff'; // setting the attribute a to blue
            myArray['101'] = '#3333ff';
            myArray['102'] = '#1a1aff';
            myArray['103'] = '#DA8700';
            myArray['104'] = '#BA7300';
            myArray['105'] = '#7C4D00';
            myArray['106'] = '#660b4a';
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
                nowIndicator: true,
                eventLimit: true, // allow "more" link when too many events
                themeSystem: 'bootstrap4', // theme
                themeName: 'Minty',
                timeFormat: 'H:mm',
                slotLabelFormat: ['H:mm'],
                nowIndicator: true,
                allDaySlot: false,
                now: $.now(),

                //disableDragging: disable_dragging,
                //disableResizing: disable_resizing,


                // getting all events from db and putting them into fullcalendar
                events: function (event) {

                    $('#calendar').fullCalendar('removeEvents', event._id)
                    $.ajax({
                        url: 'WebService.asmx/getSchedule4Admin',
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
                    });
                },

                //===============================click==========================================
                //event data popup ajax func WS -> tigburim -> dbs and all the way back
                eventClick: function (event) {

                    TigburById.id = event.id;
                    TigburById.stuID = null;
                    TigburById.IsCheckNeeded = 0;
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
                                document.getElementById("ContentPlaceHolder1_ProfessionName").innerHTML = tigubrObj.ProfName;
                                document.getElementById("ContentPlaceHolder1_timeStartLabel").innerHTML = tigubrObj.StartTime.substring(0, 5);
                                document.getElementById("ContentPlaceHolder1_timeEndLabel").innerHTML = tigubrObj.EndTime.substring(0, 5);
                                document.getElementById("ContentPlaceHolder1_TeacherLabel").innerHTML = tigubrObj.TeacherName;
                                document.getElementById("ContentPlaceHolder1_LimitLabel").innerHTML = tigubrObj.Limit;
                                document.getElementById("ContentPlaceHolder1_quantityleft").innerHTML = tigubrObj.ActualLimit;
                                Obj4delete.id = tigubrObj.TrueId;
                                Obj4delete.date = tigubrObj.TigburDate;
                            },
                            error: function (result) { alertFunc(" התגבור לא זוהה"); }
                        });

                    $("#showTigburPopupPopupDiv").dialog({
                        autoOpen: false,
                        height: 254,
                        width: 290,
                        modal: true,
                        resizable: true,
                    });

                    $('#showTigburPopupPopupDiv').dialog('open');
                    $("#showTigburPopupPopupDiv").dialog({
                        title: "פרטי תגבור",
                    });

                },



                selectable: true,
                select: function (event) {//לחיצה על יום ריק

                    var utc = new Date();
                    utc = formatDateUTC(utc);
                    var eventDate = formatDateUTC(event._d);
                    tigburDate.startDate = eventDate;
                    ActualLes.date = eventDate;
                    if (utc <= eventDate) {

                        $("#insertPopupDiv").dialog({
                            autoOpen: false,
                            height: 240,
                            width: 300,
                            modal: true,
                            resizable: true,
                        });

                        $('#insertPopupDiv').dialog('open');
                        $("#insertPopupDiv").dialog({
                            title: "בחירת תבנית תגבור",
                        });
                    }
                    else {
                        alertFunc("התאריך שנבחר עבר, אנא בחר תאריך חדש");

                    }
                    var dataString = JSON.stringify(tigburDate);
                    $.ajax
                        ({
                            url: 'WebService.asmx/getRelevantTemplate',
                            data: dataString,
                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json; charset = utf-8', // sent to the server
                            async: false,
                            success: function (data) {
                                var tigburList = $.parseJSON(data.d);
                                $("#ContentPlaceHolder1_Lesson_ddl").empty();
                                $("#ContentPlaceHolder1_Lesson_ddl").append($("<option></option>").val(0).html("בחר תבנית"));//מכניסי לתוך הדרופדאוןליסטים את רשימת התבניות שיכולים לשבץ אותם

                                $(tigburList).each(function () {
                                    $("#ContentPlaceHolder1_Lesson_ddl").append($("<option value='" + $(this).attr('TrueId') + "'>" + $(this).attr('StartTime').substring(0, 5) + "-" + $(this).attr('EndTime').substring(0, 5) + "," + $(this).attr('ProfName') + "," + $(this).attr('TeacherName') + "," + $(this).attr('Limit') + "</option>"));
                                });

                            },
                            error: function (result) { alertFunc(" משהו השתבש,אנא נסה שנית"); }
                        });

                    $('#calendar').fullCalendar('unselect');
                },
            });

            $("#ContentPlaceHolder1_show_studentlist").click(function showStudentList() {

                document.getElementById("ContentPlaceHolder1_stuListLBL").innerHTML = " ";
                var dataString = JSON.stringify(Obj4delete);
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

            $("#ContentPlaceHolder1_deleteTigbur").click(function deleteTigburFunc() {

                var dataString = JSON.stringify(Obj4delete);
                $.ajax
                    ({
                        url: 'WebService.asmx/delSpecTig',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            alertFunc(" תגבור זה נמחק בהצלחה");
                            location.reload(true);
                        },
                        error: function (result) { alertFunc(" הייתה בעיה במחיקת תגבור זה"); }
                    });
                /// ajax returns "[{\"Stu_id\":0,\"Stu_Instructor_id\":0,\"FirstName\":\"דור שפיגל\",\"LastName\":null,\"BirthDate\":null,
                ///\"PhoneNumber\":null,\"Email\":null,\"Address\":null,\"Status\":false,\"Password\":null,\"Grade\":null,
                ///\"Notes\":null,\"IsEntitled\":false},{\"Stu_id\":0,\"Stu_Instructor_id\":0,\"FirstName\":\"תומרבזרנו\",
                //\"LastName\":null,\"BirthDate\":null,\"PhoneNumber\":null,\"Email\":null,\"Address\":null,\"Status\":false,\"Password\":null,\"Grade\":null,\"Notes\":null,\"IsEntitled\":false}]"

            });

            $("#ContentPlaceHolder1_add_Tigbur").click(function addTigburFunc() {

                ActualLes.id = $('#<%=Lesson_ddl.ClientID %>').val()
                ActualLes.quantity = document.getElementById("ContentPlaceHolder1_MofaimTB").value;
                if (ActualLes.quantity < 1) { alertFunc(" מספר המופעים חייב להיות 1 ומעלה"); return; }
                if (ActualLes.quantity == "הכנס מספר מופעים") { alertFunc(" אנא הכנס מספר מופעים"); return; }
                var dataString = JSON.stringify(ActualLes);
                $.ajax
                    ({
                        url: 'WebService.asmx/insertALfromCal',
                        data: dataString,
                        type: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset = utf-8', // sent to the server
                        async: false,
                        success: function (data) {
                            countObj = $.parseJSON(data.d);
                            if (countObj == 0 || countObj == null) {
                                alertFunc(" התרחשה בעיה,אנא בדוק את פרטי התבנית הנבחרים");
                                return;
                            }
                            alertFunc(" תגבור זה נוסף בהצלחה");
                            location.reload(true);
                        },
                        error: function (result) { alertFunc(" הייתה בעיה בהוספת תגבור זה"); }
                    });

            });

            $("#<%=Lesson_ddl.ClientID%>").select2({

                placeholder: "Select Item",

                allowClear: true

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
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div id="loading">
        <img id="loading-image" src="images/loading2.gif" alt="Loading..." />
    </div>


    <div class="container">
        <div>
            <div id="pageTitle" class="sectionTitle text-center" style="display: none;">
                <h2>
                    <span class="shape shape-left bg-color-4"></span>
                    <span>מערכת שעות</span>
                    <span class="shape shape-right bg-color-4"></span>
                </h2>
            </div>

            <div id="calendar"></div>
            <img id="indexMap" src="images/admin_teacher_index_horizontal.jpg" />
            <!--lesson details start -->
            <div data-role="popup" id="showTigburPopup" class="ui-content" style="min-width: 250px;">
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
                                <asp:Label ID="LimitLabel" runat="server" Text=" "></asp:Label>
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
                                <asp:Button ID="show_studentlist" CssClass="btn btn-primary btn-xs btn-width" runat="server" Text="תלמידים רשומים" />&nbsp; 
                            </td>
                            <td>
                                <asp:Button ID="deleteTigbur" CssClass="btn btn-danger btn-sm btn-width" runat="server" Text="מחק תגבור זה" />
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>
    </div>
    <!--error alert start -->
    <div data-role="popup" id="alertModel" class="ui-content" style="min-width: 250px; width: auto">
        <div id="alertModelDIV" style="display: none;">
            <label id="alertlabel"></label>
            <button type="button" id="close_alertlabel" class="btn btn-primary btn-width" style="float: left;">אישור</button>
        </div>
    </div>

    <%--    פופאפ להכנסת תגבור--%>
    <div data-role="popup" id="insertPopup" class="ui-content" style="min-width: 400px; min-width: 400px">
        <div id="insertPopupDiv" style="display: none; text-align: center;">
            <h5>הכנס פרטי תגבור</h5>

            <table runat="server" dir="rtl">


                <tr>
                    <td class="text-center">
                        <h5>תבנית    </h5>
                    </td>
                    <td>
                        <asp:DropDownList ID="Lesson_ddl" CssClass="form-control" runat="server" style="width:180px;">
                        </asp:DropDownList></td>
                </tr>


                <tr>
                    <td class="text-center">
                        <h5>מופעים</h5>
                    </td>
                    <td>
                        <br />
                        <asp:TextBox ID="MofaimTB" runat="server" CssClass="form-control" style="width:180px;" value="הכנס מספר מופעים"></asp:TextBox>
                        <br />
                    </td>

                </tr>

                <tr class="text-center">
                    <td></td>
                    <td>
                        <asp:Button ID="add_Tigbur" CssClass="btn center-block btn-xs btn-success btn-width" runat="server" Text="הוסף תגבור" />
                    </td>

                </tr>
            </table>
        </div>
    </div>

    <!--student list details start -->
    <div data-role="popup" id="showStudentList" class="ui-content" style="min-width: 250px;">
        <div id="showStudentListrPopupPopupDiv" style="display: none; text-align: center;">
            <asp:Label ID="stuListLBL" runat="server" Text=" "></asp:Label>
        </div>
    </div>

    <script type="text/javascript">
        $(window).load(function () {
            $('#loading').hide();
            $('#pageTitle').show();
        });
    </script>
</asp:Content>

