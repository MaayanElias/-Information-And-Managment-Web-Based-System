using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


public partial class ClassesForStudent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Student UserStudent = null;
            if (Session["stuUserSession"] != null)
            {
                UserStudent = (Student)(Session["stuUserSession"]);
                Session["userStudent"] = UserStudent;
            }
            else Response.Redirect("default.aspx");
        }
        if (Session["studentAction"] != null)
        {
            if (Session["studentAction"].ToString() == "1")
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('נשלחה בקשה להשתתפות בתגבור')", true);
            if (Session["studentAction"].ToString() == "2")
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('בוטלה בקשה להשתתפות בתגבור')", true);
            if (Session["studentAction"].ToString() == "3")
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('נשלחה בקשה לביטול השתתפות בתגבור')", true);
        }

        Session["studentAction"] = "0";
        Student s = (Student)(Session["userStudent"]);
        DataTable dt = this.GetRequests();
        Session["studentRequests"] = dt;
    }

    protected void classesGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[3].Text.IndexOf(" ");
            e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, index);
            string iDate = e.Row.Cells[3].Text;
            DateTime oDate = Convert.ToDateTime(iDate);
            DateTime currentDate = DateTime.Now.Date;


        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[4].Text == "1")
                e.Row.Cells[4].Text = "א'";
            else if (e.Row.Cells[4].Text == "2")
                e.Row.Cells[4].Text = "ב'";
            else if (e.Row.Cells[4].Text == "3")
                e.Row.Cells[4].Text = "ג'";
            else if (e.Row.Cells[4].Text == "4")
                e.Row.Cells[4].Text = "ד'";
            else // יום חמישי 
                e.Row.Cells[4].Text = "ה'";
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string iDate = e.Row.Cells[3].Text;
            DateTime oDate = Convert.ToDateTime(iDate);
            DateTime currentDate = DateTime.Now.Date;
            if (oDate < currentDate)
            {
                e.Row.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (Convert.ToInt32(e.Row.Cells[9].Text) < (Convert.ToInt32(e.Row.Cells[8].Text))) //במידה והסטודנט לא רשום לתגבור ויש עדיין מקום בתגבור, כלומר אם הכמות המקסימלית קטנה מהכמות הפועל, ניתן להירשם לתגבור ולכן נציג את כפתור "הגש בקשה"
            {
                Student s = (Student)(Session["userStudent"]);
                if (s.IsEntitled == true)
                {
                    Button BTN = e.Row.FindControl("requestButton") as Button;
                    if (BTN != null)
                    {
                        BTN.Text = "הגש בקשה";
                        BTN.Enabled = true;
                        BTN.Click += requestButton_Click;
                        BTN.OnClientClick = "javascript:return window.alert('בקשתך נשלחה')";
                    }
                }
                else
                {
                    Button BTN = e.Row.FindControl("requestButton") as Button;
                    if (BTN != null)
                    {

                        BTN.Text = "הגש בקשה";
                        BTN.Enabled = false;
                        BTN.ToolTip = "אינך זכאי להירשם לתגבורים";
                    }
                }

            }
            else
            {
                //e.Row.Cells[0].Text = "לא ניתן להירשם"; //במידה והסטודנט לא רשום לתגבור וגם אין עוד מקום בתגבור, לא נאפשר הרשמה לתגבור זה
                Button BTN = e.Row.FindControl("requestButton") as Button;
                if (BTN != null)
                {
                    BTN.Text = "הגש בקשה";
                    BTN.Enabled = false;
                    BTN.ToolTip = "תגבור מלא. לא ניתן להירשם";
                }
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataTable userReqs = (DataTable)(Session["studentRequests"]);
            foreach (DataRow dr in userReqs.Rows)
            {
                Student s = (Student)(Session["userStudent"]);
                double stuId = s.Stu_id;

                DateTime dat = (DateTime)(dr["req_actLes_date"]);
                string lesNum = Convert.ToString(dr["req_actLes_id"]);
                string lesDate = dat.ToString("dd/MM/yyyy");
                int req_status = Convert.ToInt32(dr["req_status"]);
                int req_type = Convert.ToInt32(dr["req_type"]);
                double ReqStuID = Convert.ToDouble(dr["req_stu_id"]);


                if ((e.Row.Cells[1].Text == lesNum) && (e.Row.Cells[3].Text == lesDate)) //במידה והסטודנט הספציפי שלח בקשה לתגבור ספציפי
                {


                    if (req_type == 0 && req_status == 2)
                    {

                        Button BTN = e.Row.FindControl("requestButton") as Button;
                        if (BTN != null)
                        {
                            BTN.Text = "בטל בקשה";
                            BTN.Enabled = true;
                            //BTN.BackColor = System.Drawing.Color.Red;
                            BTN.CssClass = "btn btn-danger btn-sm";
                            BTN.Click += cancelBTN_click;
                            BTN.OnClientClick = "javascript:return window.alert('בקשתך בוטלה')";
                        }
                    }
                    else if (req_type == 0 && req_status == 1)
                    {
                        Button BTN = e.Row.FindControl("requestButton") as Button;
                        if (BTN != null)
                        {
                            BTN.Text = "בטל השתתפות";
                            BTN.Enabled = true;
                            //BTN.BackColor = System.Drawing.Color.DarkBlue;
                            BTN.CssClass = "btn btn-primary btn-sm";
                            BTN.Click += cancelParticipationButton_Click;
                            BTN.OnClientClick = "javascript:return window.alert('בקשתך לביטול הרשמה לתגבור נשלחה')";
                        }
                    }
                    else if (req_type == 0 && req_status == 0) //אם יש לו בקשה לתגבור הזה והיא נדחחתה
                    {
                        //נרצה קודם לדעת מאיזה סיבה הבקשה שלו נדחתה אם אילוץ התנגשות- נאפשר לו להגיש בקשה ככה שאם יבטל את התגבור המתנגש, תתאפשר לו הרשמה ואם לא יבטל אז שוב פעם דחייה אוטומטית
                        // אם הסיבה לדחייה היא שיקולים של סיגל לא נאפשר הגשת בקשה נוספת לתגבור זה
                        //השדה סיבת דחייה בטבלת בקשות מאותחל ל0 עבור כל הבקשות ורק בקשות שנדחו מסיבה זו מקבלות ערך 1
                        // נביא את כל הבקשות של שהתלמיד שנדחו מהסיבה שהתגבורים הללו מתנגשים עם תגבורים אחרים שבהם התלמיד משתתף- כלומר השדה סיבת דחייה עבורם שווה ל1, ושבהם התאריך ומזהה התגבור המבוקש שווה לתאריך ומזהה וכן התגבור בכל שורה בגריד
                        Session["lesId"] = Convert.ToInt32(e.Row.Cells[1].Text);
                        DateTime tmpDate = Convert.ToDateTime(e.Row.Cells[3].Text);
                        Session["lesDate"] = tmpDate.ToString("yyyy-MM-dd");

                        DataTable RequestsForStuIdDT = this.GetRequestsForStuId();
                        if (RequestsForStuIdDT.Rows.Count != 0)//המשמעות שיש בקשות שנדחו עבור תלמיד זה בגלל אילוצי התנגשות 
                        {
                            Button BTN = e.Row.FindControl("requestButton") as Button;
                            if (BTN != null)
                            {
                                BTN.Text = "הגש בקשה";
                                BTN.Enabled = true;
                                BTN.Click += requestButton_Click;
                                BTN.OnClientClick = "javascript:return window.alert('בקשתך נשלחה')";
                            }
                        }
                        //e.Row.Cells[0].Text = "בקשתך להירשם נדחתה";
                        else
                        { //נבדוק האם נדחה כי התגבור מלא או מהסיבות של סיגל. אם נדחה כי התגבור מלא נאפשר הגשת בקשה, אחרת לא נאפשר

                            DataTable RequestsForStuIdDTFull = this.GetRequestsForStuId_fullDec();
                            Button BTN = e.Row.FindControl("requestButton") as Button;
                            if (BTN != null)
                            {
                                BTN.Text = "הגש בקשה";
                                //BTN.BackColor = System.Drawing.Color.DarkBlue;
                                BTN.CssClass = "btn btn-success btn-sm";
                                if (RequestsForStuIdDTFull.Rows.Count != 0) // המשמעות שהבקשה לתגבור נדחתה כי התגבור מלא, לכן נאפשר לו להגיש בקשה במידה ומישהו יבטל השתתפות
                                {
                                    BTN.Enabled = true;

                                }

                                else
                                {
                                    BTN.Enabled = false;
                                    BTN.ToolTip = "בקשתך  להירשם לתגבור נדחתה";
                                }
                            }
                        }

                    }

                    if (req_type == 1 && req_status == 2)
                    {
                        e.Row.Cells[0].Text = "נשלחה בקשה לביטול השתתפות";
                    }
                    else if (req_type == 1 && req_status == 1)
                    {

                        Student stu = (Student)(Session["userStudent"]);
                        if (stu.IsEntitled == true)
                        {
                            Button BTN = e.Row.FindControl("requestButton") as Button;
                            if (BTN != null)
                            {
                                BTN.Text = "הגש בקשה";
                                BTN.CssClass = "btn btn-success btn-sm";
                                BTN.Click += requestButton_Click;
                                BTN.Enabled = true;
                                BTN.OnClientClick = "javascript:return window.alert('בקשתך נשלחה')";
                            }
                        }
                        else
                        {
                            Button BTN = e.Row.FindControl("requestButton") as Button;
                            if (BTN != null)
                            {
                                BTN.Text = "הגש בקשה";
                                BTN.Enabled = false;
                                BTN.ToolTip = "אינך זכאי להירשם לתגבורים";
                                Label1.Visible = true;
                            }
                        }
                    }
                    else if (req_type == 1 && req_status == 0)
                    {
                        //e.Row.Cells[0].Text = " בקשתך לביטול השתתפות נדחתה";
                        Button BTN = e.Row.FindControl("requestButton") as Button;
                        if (BTN != null)
                        {
                            BTN.Text = "בטל השתתפות";
                            //BTN.BackColor = System.Drawing.Color.DarkBlue;
                            BTN.CssClass = "btn btn-primary btn-sm";
                            BTN.Enabled = false;
                            BTN.ToolTip = "בקשתך לביטול השתתפות בתגבור נדחתה";
                        }
                    }
                }

            }
        }


    }


    public void requestButton_Click(Object sender, EventArgs e)
    {
        Session["studentAction"] = "1";
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;

        Student s = (Student)(Session["userStudent"]);
        double reqStuId = s.Stu_id;
        Request req;
        int actLes_id = Convert.ToInt32(gvRow.Cells[1].Text);
        DateTime actlDate = Convert.ToDateTime(gvRow.Cells[3].Text);
        int req_status = 2; //the request status is now pending
        int is_permanent = 0; //ther is only one-time request for now
        int req_type = 0; // הגשת בקשה להרשמה מקבלת סוג=0
        int flg = 0;

        DataTable dt = this.GetRequests();

        foreach (DataRow dr in dt.Rows)
        {

            DateTime dat = (DateTime)(dr["req_actLes_date"]);
            int lesNum = Convert.ToInt32(dr["req_actLes_id"]);
            double ReqStuID = Convert.ToDouble(dr["req_stu_id"]);
            int reqType = Convert.ToInt32(dr["req_type"]);
            int req_decliend = Convert.ToInt32(dr["req_decliend"]);
            int req_status2 = Convert.ToInt32(dr["req_status"]);

            if (dat == actlDate && lesNum == actLes_id && ReqStuID == reqStuId && reqType == req_type && req_decliend == 0)
            {
                flg = 1;
            }

            else if(dat == actlDate && lesNum == actLes_id && ReqStuID == reqStuId && reqType == 1 && req_status2 == 1)
            {
                flg = 0;
            }
        }
        if (flg == 0)
        {
            try
            {
                DateTime reqDate = DateTime.Now.Date;
                req = new Request(actLes_id, actlDate, reqStuId, req_status, is_permanent, req_type, reqDate);
                int numEffected = req.InsertRequest();
            }
            catch (Exception ex)
            {
                Response.Write("There was an error " + ex.Message);
            }

        }
        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }


    public void cancelBTN_click(Object sender, EventArgs e)
    {
        Session["studentAction"] = "2";

        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        Student s = (Student)(Session["userStudent"]);
        double reqStuId = s.Stu_id;

        int actLes_id = Convert.ToInt32(gvRow.Cells[1].Text);
        DateTime actlDate = Convert.ToDateTime(gvRow.Cells[3].Text);

        try
        {
            Request req = new Request();
            int numEffected = req.deleteRequest(reqStuId, actLes_id, actlDate);
        }
        catch (Exception ex)
        {
            Response.Write("There was an error " + ex.Message);
        }
        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }

    public void cancelParticipationButton_Click(Object sender, EventArgs e)
    {
        Session["studentAction"] = "3";

        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;

        Student s = (Student)(Session["userStudent"]);
        double reqStuId = s.Stu_id;
        Request req;
        int actLes_id = Convert.ToInt32(gvRow.Cells[1].Text);
        DateTime actlDate = Convert.ToDateTime(gvRow.Cells[3].Text);
        int req_status = 2; //the request status is now pending
        int is_permanent = 0; //ther is only one-time request for now
        int req_type = 1; // הגשת בקשה לביטול השתתפות מקבלת סוג=1

        int flg = 0;

        DataTable dt = this.GetRequests();
        foreach (DataRow dr in dt.Rows)
        {

            DateTime dat = (DateTime)(dr["req_actLes_date"]);
            int lesNum = Convert.ToInt32(dr["req_actLes_id"]);
            double ReqStuID = Convert.ToDouble(dr["req_stu_id"]);
            int reqType = Convert.ToInt32(dr["req_type"]);

            if (dat == actlDate && lesNum == actLes_id && ReqStuID == reqStuId && reqType == req_type)
            {
                flg = 1;
            }
        }
        if (flg == 0)
        {
            try
            {
                DateTime reqDate = DateTime.Now.Date;
                req = new Request(actLes_id, actlDate, reqStuId, req_status, is_permanent, req_type, reqDate);
                int numEffected = req.InsertRequest();
                Session["studentAction"] = "3";

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
        Student s = (Student)(Session["userStudent"]);
        double reqStuId = s.Stu_id;

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from requests where req_stu_id= '" + reqStuId + "'";

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



    // נביא את כל הבקשות של שהתלמיד שנדחו מהסיבה שהתגבורים הללו מתנגשים עם תגבורים אחרים שבהם התלמיד משתתף- כלומר השדה סיבת דחייה עבורם שווה ל1, ושבהם התאריך ומזהה התגבור המבוקש שווה לתאריך ומזהה וכן התגבור בכל שורה בגריד
    private DataTable GetRequestsForStuId()
    {
        Student s = (Student)(Session["userStudent"]);
        double StuId = s.Stu_id;
        int lesID = Convert.ToInt32(Session["lesId"]);
        string lesDate = (Session["lesDate"]).ToString();

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from requests where req_stu_id= '" + StuId + "' and req_actLes_id='" + lesID + "' and [req_actLes_date]='" + lesDate + "' and req_status=0 and req_decliend=1";
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



    // נביא את כל הבקשות של שהתלמיד שנדחו מהסיבה שהתגבורים הללו ממלאים - כלומר השדה "מלא" שלהם שווה ל1, ושבהם התאריך ומזהה התגבור המבוקש שווה לתאריך ומזהה וכן התגבור בכל שורה בגריד
    private DataTable GetRequestsForStuId_fullDec()
    {
        Student s = (Student)(Session["userStudent"]);
        double StuId = s.Stu_id;
        int lesID = Convert.ToInt32(Session["lesId"]);
        string lesDate = (Session["lesDate"]).ToString();

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from requests where req_stu_id= '" + StuId + "' and req_actLes_id='" + lesID + "' and [req_actLes_date]='" + lesDate + "' and req_status=0 and dec_full=1";
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