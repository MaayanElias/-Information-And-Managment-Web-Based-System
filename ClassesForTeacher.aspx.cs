using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

public partial class ClassesForTeacher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            Teacher UserSTeacher = null;
            if (Session["teaUserSession"] != null)
            {
                UserSTeacher = (Teacher)(Session["teaUserSession"]);
                Session["teaUserSession"] = UserSTeacher;
            }
            else Response.Redirect("default.aspx");
        }
        bool AddForm = Convert.ToBoolean(Session["newForm"]);
        if (AddForm)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('טופס נוכחות נשמר בהצלחה')", true);
            Session["newForm"] = false;
        }
        bool updateForm = Convert.ToBoolean(Session["updateForm"]);
        if (updateForm)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('הטופס עודכן בהצלחה')", true);
            Session["updateForm"] = false;
        }

        Teacher t = (Teacher)(Session["teaUserSession"]);
        double TeaId = t.Tea_id;
        Session["TeaId"] = t.Tea_id;

        string searchExpression = "Tea_id =" + TeaId;
        Session["searchExpression"] = searchExpression;

        DataTable dt1 = this.GetDetails();
        dt1.Columns.Remove("Les_Tea_id");

        teacherClasses.DataSource = dt1;
        teacherClasses.DataBind();
    }


    private DataTable GetDetails()
    {
        double tea_id = (double)(Session["TeaId"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Tea_id ,ActLes_LesId as 'מספר תגבור',Pro_Title as 'מקצוע', ActLes_date as 'תאריך', Les_day as 'יום בשבוע', Les_StartHour as 'שעת התחלת', Les_EndHour as 'שעת סיום', Les_MaxQuan as 'כמות מקסימלית', quantity as 'כמות נוכחית', Attendance_Form as 'טופס נוכחות', actls_cancelled from Lesson inner join ActualLesson on Les_Id = ActLes_LesId inner join Profession on Les_Pro_Id = Pro_Id where les_tea_id=" + tea_id + "order by ActLes_date";

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


        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = dt.Rows[i];
            if (Convert.ToInt32(dr["actls_cancelled"]) == 1)
                dr.Delete();
        }
        dt.Columns.Remove("actls_cancelled");

        return dt;
    }

    protected void teacherClasses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[3].Text.IndexOf(" ");
            e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, index);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[9].Text != "1")
            {
                e.Row.Cells[9].Text = "לא נשלח";
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                e.Row.Cells[9].CssClass = "SentForm";
                e.Row.Cells[9].Text = "נשלח";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string iDate = e.Row.Cells[3].Text;
            DateTime oDate = Convert.ToDateTime(iDate);
            DateTime currentDate = DateTime.Now.Date;
            string endHour = e.Row.Cells[5].Text;
            DateTime eHour = Convert.ToDateTime(endHour);
            TimeSpan end_hour = eHour.TimeOfDay;
            TimeSpan currentHour = DateTime.Now.TimeOfDay;

            if ((e.Row.Cells[9].Text == "לא נשלח" && oDate < currentDate) || e.Row.Cells[9].Text == "לא נשלח" && oDate == currentDate && end_hour > currentHour)
            {
                LinkButton linkBTN = e.Row.FindControl("LinkButton1") as LinkButton;
                linkBTN.Text = "שלח טופס נוכחות";
                linkBTN.Command += LinkButton1_Command;
            }
            if (e.Row.Cells[9].Text != "לא נשלח" && oDate < currentDate || e.Row.Cells[9].Text != "לא נשלח" && oDate == currentDate)
            {
                LinkButton linkBTN = e.Row.FindControl("LinkButton1") as LinkButton;
                linkBTN.Text = "הצג טופס נוכחות";
                linkBTN.Command += LinkButton1_Command_ShowForm;
            }
            if (oDate > currentDate)
            {
                LinkButton linkBTN = e.Row.FindControl("LinkButton1") as LinkButton;
                linkBTN.Text = "בטל תגבור";
                linkBTN.Enabled = true;
                linkBTN.Command += LinkButton1_Command_request;

                DataTable dt = this.GetRequests();

                foreach (DataRow dr in dt.Rows)
                {
                    double teaID = (double)(Session["TeaId"]);
                    int lessNumber = Convert.ToInt32(e.Row.Cells[1].Text);

                    DateTime dat = (DateTime)(dr["TeaReq__LessDate"]);
                    int lesNum = Convert.ToInt32(dr["TeaReq__LessId"]);
                    double ReqTeaID = Convert.ToDouble(dr["TeaReq_TeaId"]);
                    int req_status = Convert.ToInt32(dr["TeaReq_status"]);

                    if (dat == oDate && lesNum == lessNumber && ReqTeaID == teaID)// אם המתגבר כבר שלח בקשה לביטול התגבור (לפי מס' תגבור ותאריך התגבור) לא נאפשר לשלוח שוב בקשה
                    {
                        if (req_status == 2)//אם סטטוס הבקשה שלו ממתינה, נאפשר לו לבטל את הבקשה והבקשה תימחק מהדאטה בייס
                        {
                            LinkButton link_BTN = e.Row.FindControl("LinkButton1") as LinkButton;
                            link_BTN.Text = "בטל בקשה לביטול תגבור";
                            linkBTN.CssClass = "declinedRequest";
                            link_BTN.Enabled = true;
                            link_BTN.Command += LinkButton1_Command_cancelrequest;
                            link_BTN.OnClientClick = "javascript:return window.alert('בקשתך בוטלה')";
                        }

                        else if (req_status == 0)//הבקשה נדחתה
                        {
                            //אופציה מס' 1
                            LinkButton link_BTN = e.Row.FindControl("LinkButton1") as LinkButton;
                            link_BTN.Text = "בקשתך לביטול התגבור נדחתה";
                            link_BTN.Enabled = false;

                            link_BTN.ForeColor = System.Drawing.Color.Black;


                        }
                    }
                }
            }
        }
    }


    public void LinkButton1_Command(Object sender, CommandEventArgs e) //טיפול במצב שבו התגבור כבר התקיים ועדיין לא נשלח טופס נוכחות
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int selectedLessonNum = Convert.ToInt32(gvRow.Cells[1].Text);
        string selectedLessonDate = (gvRow.Cells[3].Text);
        Session["selectedLessonNum"] = selectedLessonNum;
        Session["selectedLessonDate"] = selectedLessonDate;
        Session["selectedLessonProf"] = gvRow.Cells[2].Text;
        Session["selectedLessonDay"] = Convert.ToInt32(gvRow.Cells[4].Text);
        Session["selectedLessonHour"] = gvRow.Cells[5].Text;
        Teacher t = (Teacher)(Session["teaUserSession"]);
        string teaName = t.Tea_firstName + " " + t.Tea_lastName;
        Session["selectedLessonTeacher"] = teaName;
        int flg = 1; //נסמן שהגענו מכפתור "שלח טופס" מה שמסמל שהתגבור עבר וטופס נוכחות עוד לא נשלח עבורו
        Session["flg"] = flg;
        Response.Redirect("TeacherAttendanceForm.aspx");


    }



    public void LinkButton1_Command_request(Object sender, CommandEventArgs e)//מטפל במצב שבו התגבור עוד לא התקיים והוגשה בקשה לביטול התגבור (מצד המתגבר)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int selectedLessonNum = Convert.ToInt32(gvRow.Cells[1].Text);
        string selectedLessonDate = (gvRow.Cells[3].Text);
        DateTime LessonDate = Convert.ToDateTime(selectedLessonDate);
        DateTime requestDate = DateTime.Now;

        Session["selectedLessonNum"] = selectedLessonNum;
        Session["LessonDate"] = selectedLessonDate;
        Session["requestDate"] = requestDate;
        //Response.Redirect("ClassesForTeacher.aspx");//לטפל


        lbl1.Text = Convert.ToString(selectedLessonNum);
        lbl2.Text = selectedLessonDate;
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);




    }
    public void LinkButton1_Command_ShowForm(Object sender, CommandEventArgs e)//מטפל במצב שבו התגבור התקיים והטופס נוכחות מלא ורוצים לראות את הטופס
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int selectedLessonNum = Convert.ToInt32(gvRow.Cells[1].Text);
        string selectedLessonDate = (gvRow.Cells[3].Text);
        Session["selectedLessonNum"] = selectedLessonNum;
        Session["selectedLessonDate"] = selectedLessonDate;
        Session["selectedLessonProf"] = gvRow.Cells[2].Text;
        Session["selectedLessonDay"] = Convert.ToInt32(gvRow.Cells[4].Text);
        Session["selectedLessonHour"] = gvRow.Cells[5].Text;
        Teacher t = (Teacher)(Session["teaUserSession"]);
        string teaName = t.Tea_firstName + " " + t.Tea_lastName;
        Session["selectedLessonTeacher"] = teaName;
        int flg = 0; //נסמן שהגענו מכפתור "הצג טופס" מה שמסמל השתגבור עבר וטופס נוכחות נשלח עבורו
        Session["flg"] = flg;
        Response.Redirect("TeacherAttendanceForm.aspx");//לטפל

    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        double teaID = (double)(Session["TeaId"]);
        int lessNumber = Convert.ToInt32(Session["selectedLessonNum"]);
        DateTime lessDate = Convert.ToDateTime(Session["LessonDate"]);
        DateTime reqDate = Convert.ToDateTime(Session["requestDate"]);
        int reqStatus = 2;
        string reqReason = txtReason.Text;
        int flg = 0;
        teachersRequests teaReq;


        DataTable dt = this.GetRequests();

        foreach (DataRow dr in dt.Rows)
        {

            DateTime dat = (DateTime)(dr["TeaReq__LessDate"]);
            int lesNum = Convert.ToInt32(dr["TeaReq__LessId"]);
            double ReqTeaID = Convert.ToDouble(dr["TeaReq_TeaId"]);

            if (dat == lessDate && lesNum == lessNumber && ReqTeaID == teaID)
            {
                flg = 1;
            }
        }
        if (flg == 0)
        {
            try
            {
                teaReq = new teachersRequests(lessNumber, lessDate, teaID, reqStatus, reqDate, reqReason);
                int numEffected = teaReq.InsertTeacherRequest();
            }
            catch (Exception ex)
            {
                Response.Write("There was an error " + ex.Message);
            }
        }


        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }


    private DataTable GetRequests()
    {
        double teaId = (double)(Session["TeaId"]);

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from TeacherRequests where TeaReq_TeaId= '" + teaId + "'";

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


    protected void LinkButton1_Command_cancelrequest(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        Teacher t = (Teacher)(Session["teaUserSession"]);
        double reqTeaId = t.Tea_id;

        int lesID = Convert.ToInt32(gvRow.Cells[1].Text);
        DateTime LesDate = Convert.ToDateTime(gvRow.Cells[3].Text);

        try
        {
            teachersRequests req = new teachersRequests();
            int numEffected = req.deleteTeacherRequest(reqTeaId, lesID, LesDate);
        }
        catch (Exception ex)
        {
            Response.Write("There was an error " + ex.Message);
        }
        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        teacherClasses.PageIndex = e.NewPageIndex;
        teacherClasses.DataBind();
    }
}