using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;

public partial class TeacherAttendanceForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["selectedLessonNum"] == null)
        {
            Response.Redirect("ClassesForTeacher.aspx");
        }

        {
            Button butt = new Button();
            butt.ID = "sendForm";
            if (Convert.ToInt32(Session["flg"]) == 1)
            {
                //addStuTB.Visible = true;
                butt.Text = "שלח טופס";
                butt.Click += sendForm_Click;
                butt.CssClass = "btn btn-sm btn-primary margin-top20";
            }
            else
            {
                //addStuTB.Visible = false;
                butt.Text = "עדכן טופס";
                butt.Click += updateForm_Click;
                butt.CssClass = "btn btn-sm btn-primary margin-top20";
            }
            PlaceHolder2.Controls.Add(butt);

            string tea_name = (string)(Session["selectedLessonTeacher"]);
            string proffesion = (string)(Session["selectedLessonProf"]);
            string lessDate = (string)(Session["selectedLessonDate"]);
            string lessDay = Convert.ToString(Session["selectedLessonDay"]);
            if (lessDay == "1") { lessDay = "ראשון"; }
            else if (lessDay == "2") { lessDay = "שני"; }
            else if (lessDay == "3") { lessDay = "שלישי"; }
            else if (lessDay == "4") { lessDay = "רביעי"; }
            else { lessDay = "חמישי"; }
            string lessHour = (string)(Session["selectedLessonHour"]);
            lessHour = lessHour.Substring(0, lessHour.Length - 3);
            int lessID = Convert.ToInt32(Session["selectedLessonNum"]);

            teacherLBL.Text = "<b>מתגבר : </b> " + tea_name;
            profssionLBL.Text = "<b>מקצוע : </b>" + proffesion;
            dateLBL.Text = "<b>תאריך : </b> " + lessDate;
            DayLBL.Text = "<b>יום </b>" + lessDay;
            hourLBL.Text = "<b>בשעה </b> " + lessHour;

            DateTime date_less = Convert.ToDateTime(lessDate);

            SignedToLesson stl = new SignedToLesson();

            DataTable dt = stl.readStudentsList(lessID, date_less);
            Session["studentsList"] = dt;
            //DataTable studentsDT = new DataTable();
            Session["studentsNumber"] = dt.Rows.Count;
            int counter = 1;

            foreach (DataRow dr in dt.Rows)
            {
                Student s = new Student();
                double Stu_id = Convert.ToDouble(dr["StLes_stuId"]);
                DataTable dt2 = s.readSpecipicStudent(Stu_id);
                int Presence = Convert.ToInt32(dr["Presence"]);
                string comments = Convert.ToString(dr["comments"]);


                foreach (DataRow dr2 in dt2.Rows)
                {

                    string stuFirstlName = (string)(dr2["stu_firstName"]);
                    string stuLastlName = (string)(dr2["stu_lastName"]);
                    string stuFullName = stuFirstlName + " " + stuLastlName;

                    //studentsDT.Rows.Add(stuFullName);
                    Label lbl = new Label();
                    lbl.Text = stuFullName;
                    CheckBox cb = new CheckBox();
                    cb.ID = "cb" + Convert.ToString(Stu_id);
                    cb.CssClass = "regular-checkbox";
                    TextBox tb = new TextBox();
                    tb.ID = "tb" + Convert.ToString(Stu_id);
                    tb.CssClass = "comment_box";
                    tb.Attributes.Add("placeholder", "הוסף הערה לתלמיד..");
                    Button delBTN = new Button();
                    delBTN.ID = "delBTN" + Convert.ToString(Stu_id);
                    delBTN.Text = "מחק תלמיד";
                    delBTN.ToolTip = "מחיקת התלמיד מתגבור זה";
                    delBTN.CssClass = "btn btn-danger btn-sm";
                    delBTN.Click += delBTNButton_Click;

                    if (Presence == 1)
                    {
                        cb.Checked = true;
                    }
                    else
                    {
                        cb.Checked = false;
                    }
                    if (comments != null) { tb.Text = comments; }

                    //tb.placeholder = "הערות...";
                    PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td class='presenceCB'>"));
                    PlaceHolder1.Controls.Add(cb);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl(counter + ". "));
                    PlaceHolder1.Controls.Add(lbl);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    PlaceHolder1.Controls.Add(tb);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<td>"));
                    PlaceHolder1.Controls.Add(delBTN);
                    PlaceHolder1.Controls.Add(new LiteralControl("</td>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));

                    counter++;
                }

            }
        }
        if (!IsPostBack)
        {
            GetAllStudents();

        }
    }




    protected void Page_PreRender(object sender, EventArgs e)
    {



    }

    public void delBTNButton_Click(Object sender, EventArgs e)
    {
        SignedToLesson stl = new SignedToLesson();
        Button button = (Button)sender;
        string buttonId = button.ID;
        string stuId = buttonId.Substring(6);
        int delStuId = Convert.ToInt32(stuId);
        string lessDate = (Session["selectedLessonDate"]).ToString();
        DateTime lesDate = Convert.ToDateTime(lessDate);
        int lesId = (int)(Session["selectedLessonNum"]);
        int numEff = stl.deleteStudentFromLesson(delStuId, lesId, lesDate.ToString("yyyy-MM-dd"));
        ActualLesson actl = new ActualLesson();
        int quan = (int)Session["studentsNumber"] - 1;
        int numEf = actl.updateSpecificActualLesson(lesId, lesDate.ToString("yyyy-MM-dd"), quan);
        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }


    protected void sendForm_Click(object sender, EventArgs e)
    {

        int lessID = Convert.ToInt32(Session["selectedLessonNum"]);
        string lesDate = (string)(Session["selectedLessonDate"]);
        DateTime lessDate = Convert.ToDateTime(lesDate);
        ActualLesson actl = new ActualLesson();
        actl.updateActualLessonAttendancForm(lessID, lessDate);
        DataTable dt = (DataTable)(Session["studentsList"]);
        foreach (DataRow dr in dt.Rows)
        {
            double Stu_id = Convert.ToDouble(dr["StLes_stuId"]);
            CheckBox cb = (CheckBox)PlaceHolder1.FindControl("cb" + Stu_id.ToString());
            if (cb != null)
            {

                if (cb.Checked == false)
                {

                    int stuPresence = 0;
                    SignedToLesson stl = new SignedToLesson();
                    stl.updateStudentPresence(lessID, lessDate, Stu_id, stuPresence);
                    Student stud = new Student();
                    stud.updateStudentCounter(Stu_id);
                    DataTable dtCounter = stud.getStudentCounter(Stu_id);
                    DataTable manDT = this.GetManeger();

                    foreach (DataRow drMan in manDT.Rows)
                    {
                        double msg_toManagerId = Convert.ToDouble(drMan["Man_Id"]);
                        Session["msg_toManagerId"] = msg_toManagerId;
                    }

                    bool msg_has_read = false;
                    DateTime msg_date = DateTime.Now;

                    foreach (DataRow d in dtCounter.Rows)
                    {
                        if (Convert.ToInt32(d[0]) == 5)//להכניס את התלמיד לרשימת הלא זכאים
                        {
                            int updateIsEntitledTo = 0;
                            string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
                            Messages mes;
                            double msg_ManagerId = Convert.ToDouble(Session["msg_toManagerId"]);

                            string mesContent = "תלמיד יקר, עקב אי הגעתך לתגבורים אינך זכאי להגיש בקשות. אנא פנה למנהל/ת";
                            mes = new Messages(msg_ManagerId, Stu_id, "עדכון זכאות לתגבורים", mesContent, false, currnetDate);

                            Messages messageForStudent = new Messages();
                            messageForStudent.Msg_fromStudentId = Stu_id;
                            messageForStudent.Msg_toManagerId = msg_ManagerId;
                            messageForStudent.Msg_subject = "עדכון זכאות - הודעת מערכת";
                            messageForStudent.Msg_content = " זוהי הודעה אוטומטית: המערכת הכניסה את תלמיד זה לרשימת הלא זכאים, עקב אי הגעתו לתגבורים. באפשרותך להוציא אותו מהרשימה";
                            messageForStudent.Msg_hasRead = msg_has_read;
                            messageForStudent.System_msg = 1;
                            messageForStudent.Msg_date = msg_date.ToString("yyyyMMdd");

                            Session["entStu"] = Stu_id;

                            try
                            {
                                Request req = new Request();
                                int numEf = req.deleteRequests(Stu_id); // מחיקת כל הבקשות שלו להרשמה לתגבורים שעוד לא עברו

                                int numEffected = stud.updateIsEntittled(Stu_id, updateIsEntitledTo);
                                int NumEffected2 = mes.InsertMessage();


                                int numEffected11 = messageForStudent.InsertMessageSystemStudent();

                                SignedToLesson delStu = new SignedToLesson();
                                //int numEf1 = delStu.deleteStudentFromLesson(Stu_id, lessID, date);

                                //נביא את כל התגבורים שבהם התלמיד משתתף ותאריך התגבור עוד לא עבר ונמחק את התלממיד מתגבורים אלה
                                DataTable classesForStuIdDT = this.GetClassForStu();
                                foreach (DataRow ro in classesForStuIdDT.Rows)
                                {
                                    DateTime dat = (DateTime)(ro["StLes_ActLesDate"]);
                                    if (dat >= DateTime.Now)
                                    {
                                        string da = dat.ToString("yyyy-MM-dd");
                                        int lesNum = Convert.ToInt32(ro["StLes_ActLesId"]);
                                        int numEf1 = delStu.deleteStudentFromLesson(Stu_id, lesNum, da);
                                        ActualLesson actls = new ActualLesson();
                                        int quan = actls.readSpecificActualLessonQuan(lesNum, da) - 1;
                                        int numEfc = actls.updateSpecificActualLesson(lesNum, da, quan);
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
                            }
                        }
                    }

                }
            }

            TextBox tb = (TextBox)PlaceHolder1.FindControl("tb" + Stu_id.ToString());
            if (tb != null)
            {
                if (tb.Text != null)
                {
                    string stuNotes = tb.Text;
                    SignedToLesson stl = new SignedToLesson();
                    stl.updateStudentNotes(lessID, lessDate, Stu_id, stuNotes);
                }
            }
        }

        Session["newForm"] = true;
        Response.Redirect("ClassesForTeacher.aspx");
    }


    protected void updateForm_Click(object sender, EventArgs e)
    {
        int lessID = Convert.ToInt32(Session["selectedLessonNum"]);
        string lesDate = (string)(Session["selectedLessonDate"]);
        DateTime lessDate = Convert.ToDateTime(lesDate);
        DataTable dt = (DataTable)(Session["studentsList"]);
        foreach (DataRow dr in dt.Rows)
        {
            double Stu_id = Convert.ToDouble(dr["StLes_stuId"]);
            CheckBox cb = (CheckBox)PlaceHolder1.FindControl("cb" + Stu_id.ToString());
            if (cb != null)
            {
                if (cb.Checked == false)
                {
                    int stuPresence = 0;
                    SignedToLesson stl = new SignedToLesson();
                    stl.updateStudentPresence(lessID, lessDate, Stu_id, stuPresence);
                    Student stud = new Student();
                    stud.updateStudentCounter(Stu_id);
                    DataTable dtCounter = stud.getStudentCounter(Stu_id);
                    DataTable manDT = this.GetManeger();

                    foreach (DataRow drMan in manDT.Rows)
                    {
                        double msg_toManagerId = Convert.ToDouble(drMan["Man_Id"]);
                        Session["msg_toManagerId"] = msg_toManagerId;
                    }


                    bool msg_has_read = false;
                    DateTime msg_date = DateTime.Now;

                    foreach (DataRow d in dtCounter.Rows)
                    {
                        if (Convert.ToInt32(d[0]) == 5)//להכניס את התלמיד לרשימת הלא זכאים
                        {
                            int updateIsEntitledTo = 0;
                            string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
                            Messages mes;
                            double msg_ManagerId = Convert.ToDouble(Session["msg_toManagerId"]);

                            string mesContent = "תלמיד יקר, עקב אי הגעתך לתגבורים אינך זכאי להגיש בקשות. אנא פנה למנהל/ת";
                            mes = new Messages(msg_ManagerId, Stu_id, "עדכון זכאות לתגבורים", mesContent, false, currnetDate);

                            Messages messageForStudent = new Messages();
                            messageForStudent.Msg_fromStudentId = Stu_id;
                            messageForStudent.Msg_toManagerId = msg_ManagerId;
                            messageForStudent.Msg_subject = "עדכון זכאות- הודעת מערכת";
                            messageForStudent.Msg_content = " זוהי הודעה אוטומטית: המערכת הכניסה את תלמיד זה לרשימת הלא זכאים, עקב אי הגעתו לתגבורים. באפשרותך להוציא אותו מהרשימה";
                            messageForStudent.Msg_hasRead = msg_has_read;
                            messageForStudent.System_msg = 1;
                            messageForStudent.Msg_date = msg_date.ToString("yyyy-MM-dd");

                            string date = lessDate.ToString("yyyy-MM-dd");
                            Session["entStu"] = Stu_id;
                            try
                            {
                                Request req = new Request();
                                int numEf = req.deleteRequests(Stu_id); // מחיקת כל הבקשות שלו להרשמה לתגבורים שעוד לא עברו
                                int numEffected = stud.updateIsEntittled(Stu_id, updateIsEntitledTo);
                                int NumEffected2 = mes.InsertMessage();


                                int numEffected11 = messageForStudent.InsertMessageSystemStudent();
                                SignedToLesson delStu = new SignedToLesson();
                                //int numEf1 = delStu.deleteStudentFromLesson(Stu_id, lessID, date);

                                //נביא את כל התגבורים שבהם התלמיד משתתף ותאריך התגבור עוד לא עבר ונמחק את התלממיד מתגבורים אלה
                                DataTable classesForStuIdDT = this.GetClassForStu();
                                foreach (DataRow ro in classesForStuIdDT.Rows)
                                {
                                    DateTime dat = (DateTime)(ro["StLes_ActLesDate"]);
                                    if(dat >= DateTime.Now)
                                    {
                                        string da = dat.ToString("yyyy-MM-dd");
                                        int lesNum = Convert.ToInt32(ro["StLes_ActLesId"]);
                                        int numEf1 = delStu.deleteStudentFromLesson(Stu_id, lesNum, da);
                                        ActualLesson actl = new ActualLesson();
                                        int quan = actl.readSpecificActualLessonQuan(lesNum, da) - 1;
                                        int numEfc = actl.updateSpecificActualLesson(lesNum, da, quan);
                                    }

                                }


                            }
                            catch (Exception ex)
                            {
                                Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
                            }
                        }
                    }

                }
                else
                {
                    int stuPresence = 1;
                    SignedToLesson stl = new SignedToLesson();
                    stl.updateStudentPresence(lessID, lessDate, Stu_id, stuPresence);
                }
            }
            TextBox tb = (TextBox)PlaceHolder1.FindControl("tb" + Stu_id.ToString());
            if (tb != null)
            {
                string stuNotes = tb.Text;
                SignedToLesson stl = new SignedToLesson();
                stl.updateStudentNotes(lessID, lessDate, Stu_id, stuNotes);
            }
            Session["updateForm"] = true;

        }

        Response.Redirect("ClassesForTeacher.aspx");
    }

    private DataTable GetManeger()
    {

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from Manager where Man_Status= '" + 1 + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }
        return dt;
    }

    private void GetAllStudents()
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString);
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        string query;
        query = "select Stu_Id, Stu_FirstName + ' '  + Stu_LastName + ' - ' + cast(Stu_Id as nvarchar (255)) as 'fullName' from Student";
        da = new SqlDataAdapter(query, con);

        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlSearch.DataSource = ds;
            ddlSearch.DataTextField = "fullName";
            ddlSearch.DataValueField = "Stu_Id";
            ddlSearch.DataBind();
            ddlSearch.Items.Insert(0, new ListItem(" בחר תלמיד ", "0"));
            ddlSearch.SelectedIndex = 0;
            //ddlSearch.SelectedIndexChanged += ddlSearch_SelectedIndexChanged;
        }
    }


    protected void btnAddStu_onClick(object sender, EventArgs e)
    {
        string stuId = ddlSearch.SelectedValue.ToString();
        double selectedStuId = Convert.ToDouble(stuId);
        int lessID = Convert.ToInt32(Session["selectedLessonNum"]);
        DateTime lesDate = Convert.ToDateTime(Session["selectedLessonDate"]);
        string dateFormat = lesDate.ToString("yyyy-MM-dd");
        ActualLesson acl = new ActualLesson();
        SignedToLesson stl;

        int studentsNum = Convert.ToInt32(Session["studentsNumber"]);
        int actual_quan = studentsNum + 1;
        try
        {
            stl = new SignedToLesson(lessID, dateFormat, selectedStuId);
            int numEffected3 = acl.updateSpecificActualLesson(lessID, dateFormat, actual_quan); //הגדלת כמות משתתפים ב-1
            int numEffected2 = stl.InsertSigendToLesson(); //הכנסת התלמיד לטבלת "רשום לתגבור"
            Server.TransferRequest(Request.Url.AbsolutePath, false);
        }
        catch (Exception ex)
        {
            Response.Write("There was an error " + ex.Message);
        }
    }



    private DataTable GetClassForStu()
    {
        double sId = (double)(Session["entStu"]);
        string curDate = (DateTime.Now).ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from signedToLesson where [StLes_stuId]= '" + sId + "' and [StLes_ActLesDate]>='"+ curDate+"'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }
        return dt;
    }

}