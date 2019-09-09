using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
// REMEMBER TO ADD THIS NAMESPACE
using System.Web.Script.Serialization;
using System.Web.Script.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]

    public string getSpecificSchedule(string id)// הבאת תגבורים לתלמיד
    {
        Tigburim newTigbur = new Tigburim();
        List<Tigburim> listTigbur = newTigbur.getTigburimListForStudent("studentDBConnectionString","Lesson",id);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringgetSchedule = js.Serialize(listTigbur);
        return jsonStringgetSchedule;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getSchedule(string id)//הבאת תגבורים
    {
        Tigburim newTigbur = new Tigburim();
        List<Tigburim> listTigbur = newTigbur.getTigburimList("studentDBConnectionString", "Lesson");
        listTigbur = newTigbur.checkIfSignedFunc(id, listTigbur);
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringgetSchedule = js.Serialize(listTigbur);
        return jsonStringgetSchedule;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getSchedule4Admin()//הבאת תגבורים
    {
        Tigburim newTigbur = new Tigburim();
        List<Tigburim> listTigbur = newTigbur.getTigburimList("studentDBConnectionString", "Lesson");
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringgetSchedule = js.Serialize(listTigbur);
        return jsonStringgetSchedule;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string delSpecTig(string id, string date)
    {
        double manID = 9999; // manager id
        double TeaId = 0;
        string prof = null;
        string hour = null;
        string todayStr = DateTime.Now.ToString("yyyy-MM-dd");//today's date for insert
        Request req = new Request();
        Tigburim tig = new Tigburim();
        List<Tigburim> tigList = new List<Tigburim>();

        //getting student list and teacher id
        tigList = tig.getStudentList4Messages(id, date);
        DateTime tempDate = Convert.ToDateTime(date);
        string trueDate = tempDate.ToString("dd-MM-yyyy");

        //calling for messages to stu
        foreach (Tigburim tigbur in tigList)
        {

            if (TeaId == 0)
            {
                TeaId = Convert.ToDouble(tigbur.TeacherId);
                prof = tigbur.ProfName;
                hour = Convert.ToString(tigbur.StartTime);
                string[] splitStr = hour.Split(':');
                hour = splitStr[0] + ":" + splitStr[1];
            }


            string content = "תלמיד יקר, שיעור " + tigbur.ProfName + " שחל בתאריך " + trueDate + " ובשעה " + hour + " בוטל.";
            Messages mes = new Messages(manID, Convert.ToDouble(tigbur.StuId), "ביטול שיעור", content, false, todayStr);
            int NumEffected = mes.InsertMessage();
            if (NumEffected == 0) return null;
        }

        if (TeaId == 0)
        {
            Teacher t = new Teacher();
            TeaId = (double)t.getTeacherId4delete(id);
        }

        //calling for messages to teacher
        string mesToTeaContent = "מתגבר יקר, בוטל תגבור " + prof + " שחל בתאריך " + trueDate + " ובשעה " + hour + " , עמך הסליחה.";
        Messages TeaMes = new Messages();
        TeaMes.Msg_fromManagerId = manID;
        TeaMes.Msg_toTeacherId = TeaId;
        TeaMes.Msg_subject = "ביטול תגבור";
        TeaMes.Msg_content = mesToTeaContent;
        TeaMes.Msg_hasRead = false;
        TeaMes.Msg_date = todayStr;
        int numEffectedTeach = TeaMes.InsertMessageFromManagerToTeacher();
        if (numEffectedTeach == 0) return null;

        //updating status to 1 in AL table and deleting requests
        int delSTL = tig.deleteSTL(id, date);
        int delReq = req.deleteRequests(id, date);
        int delTig = tig.delSpecTig(id, date);
        if (delTig == 0) return null;

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringgetSchedule = js.Serialize(delTig);
        return jsonStringgetSchedule;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getRelevantTemplate(string startDate)
    {
        Tigburim newTigbur = new Tigburim();
        List<Tigburim> listTigbur = newTigbur.getTigburimTemplate(startDate);
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetTurtering = js.Serialize(listTigbur);
        return jsonStringGetTurtering;

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string InsertRequest(string id,string date,string stuId,string status,string perm,string sub_date,string type)
    {
        int newReq = (new Request()).InsertRequestCal(id,date,stuId,status,perm,sub_date,type);
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        if (newReq == 0) return null;
        if (newReq == -1) return "-1";
        string jsonStringGetTurtering = js.Serialize(newReq);
        return jsonStringGetTurtering;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string delRequest(string id, string date, string stuId, string status, string perm, string sub_date)
    {
        int newReq = (new Request()).deleteRequest(Convert.ToDouble(stuId), Convert.ToInt16(id), Convert.ToDateTime(date));
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        if (newReq == 0) return null;
        string jsonStringGetTurtering = js.Serialize(newReq);
        return jsonStringGetTurtering;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getStudentList4CAL(string id, string date)
    {
        Student stu = new Student();
        List<Student> stuList = stu.getStuListCAL(id, date);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        if (stuList == null) return null;
        string jsonStringGetTurtering = js.Serialize(stuList);
        return jsonStringGetTurtering;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string insertALfromCal(string id, string date,string quantity)
    {
        int numEffected = 0;
        string dt = date;
        int les_id = Convert.ToInt16(id);
        int Limitquantity = 0;
        DateTime start_date = Convert.ToDateTime(dt);

        String MyString = date;
        DateTime MyDateTime = new DateTime();
        MyDateTime = DateTime.ParseExact(MyString, "yyyy-MM-dd", null);
        int day = (int)MyDateTime.DayOfWeek + 1;

        int amount = Convert.ToInt32(quantity);
        ActualLesson al;
        for (int i = 0; i < amount; i++)
        {
            al = new ActualLesson(les_id, dt, Limitquantity,day);
            numEffected += al.insertActualLesson();
            start_date = start_date.AddDays(7);
            dt = start_date.ToString("yyyy-MM-dd");
        }
        if (numEffected < 1) return null;
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetTurtering = js.Serialize(numEffected);
        return jsonStringGetTurtering;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getTigburById(string id, string stuID, string IsCheckNeeded)
    {
        int Check = Convert.ToInt16(IsCheckNeeded);
        Tigburim newTigbur = (new Tigburim()).getTigburById(id);
        if (Check == 1)
        {
            List<Tigburim> tiglist = new List<Tigburim>();
            tiglist.Add(newTigbur);
            tiglist = newTigbur.checkIfSignedFunc(stuID, tiglist);
            foreach (Tigburim t in tiglist)
            {
                newTigbur = t;
            }
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetTurtering = js.Serialize(newTigbur);
        return jsonStringGetTurtering;
    }

    //Reports Web methods
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getProfessionCountForReport(string startDate, string endDate)//מחזיר כמות תגבורים לפי מקצוע בתאריכים הנתונים
    {

        Report r = new Report();
        List<Report> ProfessionCount = r.getProfessionCount(startDate, endDate);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetProfession = js.Serialize(ProfessionCount);
        return jsonStringGetProfession;
    }

    //Student Request by Profession Web methods
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string StudentRequestsByProfession(string startDate, string endDate,string userId)//מחזיר כמות בקשות לפי מקצוע בתאריכים הנתונים
    {

        Report r = new Report();
        List<Report> StudentRequestsByProfession = r.StudentRequestsByProfession(startDate, endDate, userId);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetProfession = js.Serialize(StudentRequestsByProfession);
        return jsonStringGetProfession;
    }

    //Student classes by Profession Web methods
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string StudentClassesByProfession(string startDate, string endDate, string userId)//מחזיר כמות בקשות לפי מקצוע בתאריכים הנתונים
    {

        Report r = new Report();
        List<Report> StudentClassesByProfession = r.StudentClassesByProfession(startDate, endDate, userId);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetProfession = js.Serialize(StudentClassesByProfession);
        return jsonStringGetProfession;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getTigburByIdTeacher(string id)
    {

        Tigburim newTigbur = (new Tigburim()).getTigburById(id);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetTurtering = js.Serialize(newTigbur);
        return jsonStringGetTurtering;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getSpecificScheduleTeacher(string id)// הבאת תגבורים למתגבר
    {
        Tigburim newTigbur = new Tigburim();
        List<Tigburim> listTigbur = newTigbur.getTigburimListForTeacher("studentDBConnectionString", "Lesson", id);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringgetSchedule = js.Serialize(listTigbur);
        return jsonStringgetSchedule;
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string InsertRequestTeacher(string id, string date, string teaId, string status, string sub_date, string reason)
    {
        int reqStat = Convert.ToInt32(status);
        double TeaID = Convert.ToDouble(teaId);
        int lesId = Convert.ToInt32(id);
        DateTime trueDate = Convert.ToDateTime(date);
        DateTime reqTrueDate = Convert.ToDateTime(sub_date);
        teachersRequests teaReq = new teachersRequests(lesId, trueDate, TeaID, reqStat, reqTrueDate, reason);
        int isReq = teaReq.CheckRequestCAL();
        if (isReq > 0)
        {
            return null;
        }
        int numEffected = teaReq.InsertTeacherRequest();
        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        if (numEffected == 0) return null;
        string jsonStringGetTurtering = js.Serialize(numEffected);
        return jsonStringGetTurtering;
    }

    //TeacherHoursByMonths Web methods
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string TeacherHoursByMonths(string startDate, string endDate, string userId)//מחזיר כמות בקשות לפי מקצוע בתאריכים הנתונים
    {

        Report r = new Report();
        List<Report> TeacherHoursByMonths = r.TeacherHoursByMonths(startDate, endDate, userId);

        JavaScriptSerializer js = new JavaScriptSerializer();
        // serialize to string
        string jsonStringGetProfession = js.Serialize(TeacherHoursByMonths);
        return jsonStringGetProfession;
    }


}
