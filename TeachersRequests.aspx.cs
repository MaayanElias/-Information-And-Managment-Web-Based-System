using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class TeachersRequests : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Manager UserManager = null;
            if (Session["manUserSession"] != null)
            {
                UserManager = (Manager)(Session["manUserSession"]);
                Session["manObjectSession"] = UserManager;
            }
            else Response.Redirect("default.aspx");


            statusDDL.ClearSelection(); //making sure the previous selection has been cleared
            statusDDL.Items.FindByValue("2").Selected = true;
        }
    }

    protected void teaReqGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[8].Text == "1")
                e.Row.Cells[8].Text = "א'";
            else if (e.Row.Cells[8].Text == "2")
                e.Row.Cells[8].Text = "ב'";
            else if (e.Row.Cells[8].Text == "3")
                e.Row.Cells[8].Text = "ג'";
            else if (e.Row.Cells[8].Text == "4")
                e.Row.Cells[8].Text = "ד'";
            else if (e.Row.Cells[8].Text == "5")// יום חמישי 
                e.Row.Cells[8].Text = "ה'";
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[10].Text == "1")
                e.Row.Cells[10].Text = "אושרה";

            //else if (e.Row.Cells[10].Text == "0")
            //    e.Row.Cells[10].Text = "נדחתה";

            else
                e.Row.Cells[10].Text = "ממתינה";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[3].Text.IndexOf(" ");
            e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, index);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[10].Text != "ממתינה")
            {
                Button statusAp = (Button)e.Row.FindControl("AproveButton");
                statusAp.Visible = false;
                //Button statusDe = (Button)e.Row.FindControl("DeclineButton");
                //statusDe.Visible = false;


            }
        }
    }


    protected void ApproveButton_Click(object sender, EventArgs e)
    {
        teachersRequests req = new teachersRequests();
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        int req_num = Convert.ToInt32(gvRow.Cells[1].Text);
        int status = 1;

        int cancelled = 1;
        ActualLesson acl = new ActualLesson();
        SignedToLesson stl = new SignedToLesson();
        int lesId = Convert.ToInt32(gvRow.Cells[2].Text);
        DateTime tmpDate = Convert.ToDateTime(gvRow.Cells[3].Text);
        string lesDate = tmpDate.ToString("yyyy-MM-dd");
        Request stuReq = new Request();

        string proff = gvRow.Cells[4].Text;
        string start= gvRow.Cells[6].Text;
        Session["cancelledLesId"] = lesId;
        Session["cancelledLesDate"] = lesDate;
        string lesDateMES = tmpDate.ToString("dd-MM-yyyy");
        Manager m = (Manager)(Session["manUserSession"]);
        double manID = m.Man_id;
        DataTable dt = this.GetStudents();
        string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
        Messages mes;


        DataTable teaIDtable = this.GetTeaId();
        double teaID = Convert.ToDouble(teaIDtable.Rows[0]["Les_Tea_Id"]);
        string mesToTeaContent = "מתגבר יקר, בקשתך לביטול שיעור " + proff + " שחל בתאריך " + lesDateMES + " ובשעה " + start + " אושרה והתגבור בוטל.";
        // Messages TeaMes = new Messages(manID, teaID, "ביטול שיעור", mesToTeaContent, false, currnetDate);
        Messages TeaMes = new Messages();
        TeaMes.Msg_fromManagerId = manID;
        TeaMes.Msg_toTeacherId = teaID;
        TeaMes.Msg_subject = "ביטול שיעור";
        TeaMes.Msg_content = mesToTeaContent;
        TeaMes.Msg_hasRead = false;
        TeaMes.Msg_date = currnetDate;
        int numEffected1 = TeaMes.InsertMessageFromManagerToTeacher();

        string mesContent = "תלמיד יקר, שיעור "+ proff+ " שחל בתאריך "+ lesDateMES + " ובשעה "+ start+" בוטל.";
        foreach (DataRow dr in dt.Rows)
        {
            double stuID = Convert.ToDouble(dr["StLes_stuId"]);

            mes = new Messages(manID, stuID, "ביטול שיעור", mesContent, false, currnetDate);
            int NumEffected = mes.InsertMessage();
        }


        try
        {
            int numEffected = req.updateSpecificTeacherRequest(req_num, status);
            int numEffected2 = acl.cancelSpecificActualLesson(lesId, lesDate, cancelled);
            int numEffected3 = stl.deleteStudentsFromLesson(lesId, lesDate);
            int numEffected4 = stuReq.deleteCancelldLessonRequests(lesId, lesDate);
            Server.TransferRequest(Request.Url.AbsolutePath, false);
        }

        catch (Exception ex)
        {
            Response.Write("illegal values to the SignedToLesson attributes - error message is " + ex.Message);
            return;
        }
    }


    private DataTable GetStudents()//להביא את הסטודנטים שמשתתפים בתגבור שבוטל ע"מ לשלוח להם הודעה על ביטול התגבור
    {
        int lesId = Convert.ToInt32(Session["cancelledLesId"]);
        string lesDate = Convert.ToString(Session["cancelledLesDate"]);

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from signedToLesson where StLes_ActLesId= " + lesId + " and StLes_ActLesDate=" + "'" + lesDate+"'";

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

    private DataTable GetTeaId()//להביא את הסטודנטים שמשתתפים בתגבור שבוטל ע"מ לשלוח להם הודעה על ביטול התגבור
    {
        int lesId = Convert.ToInt32(Session["cancelledLesId"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Tea_Id from Lesson where Les_Id= '" + lesId + "'";

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








    //protected void DeclineButton_Click(object sender, EventArgs e)
    //{
    //    teachersRequests req = new teachersRequests();
    //    GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
    //    int index = gvRow.RowIndex;
    //    int req_num = Convert.ToInt32(gvRow.Cells[1].Text);
    //    int status = 0; // בקשה נדחתה= סטטוס 0
    //    int numEffected = req.updateSpecificTeacherRequest(req_num, status);
    //    //Response.Redirect("ShowRequests.aspx");

    //    string proff = gvRow.Cells[4].Text;
    //    string start = gvRow.Cells[6].Text;
    //    DateTime tmpDate = Convert.ToDateTime(gvRow.Cells[3].Text);
    //    string lesDateMES = tmpDate.ToString("dd-MM-yyyy");
    //    Manager m = (Manager)(Session["manUserSession"]);
    //    double manID = m.Man_id;
    //    string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
    //    int lesId = Convert.ToInt32(gvRow.Cells[2].Text);
    //    Session["cancelledLesId"] = lesId;
    //    DataTable teaIDtable = this.GetTeaId();
    //    double teaID = Convert.ToDouble(teaIDtable.Rows[0]["Les_Tea_Id"]);
    //    string mesToTeaContent = "מתגבר יקר, בקשתך לביטול שיעור " + proff + " שחל בתאריך " + lesDateMES + " ובשעה " + start + " נדחתה. שים לב, התגבור מתקיים כרגיל!";
    //    Messages TeaMes = new Messages(manID, teaID, "ביטול שיעור", mesToTeaContent, false, currnetDate);
    //    int numEffected1 = TeaMes.InsertMessageFromManagerToTeacher();

    //    Server.TransferRequest(Request.Url.AbsolutePath, false);

    //}
}
