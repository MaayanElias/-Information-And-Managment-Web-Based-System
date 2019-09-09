using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShowAdminMessages : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Manager UserManager = null;
            if (Session["manUserSession"] != null)
            {
                UserManager = (Manager)(Session["manUserSession"]);
                Session["manUserSession"] = UserManager;
            }
            else Response.Redirect("default.aspx");


            InMailGRDW.Visible = true;
            InMailFromTeachersGRDW.Visible = true;
            AdminMessagesGRDW.Visible = false;
            OutMailAdminToTeachersGRDW.Visible = false;
            Session["MailStatus"] = true;

        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString))
            {
                con.Open();
                //Change with your select statement .
                using (SqlCommand cmd = new SqlCommand("select * from student", con))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    Dictionary<int, string> lst = new Dictionary<int, string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        //Add values to Dictionary
                        string val = row[2].ToString() + "  " + row[3].ToString() + " - " + row[0].ToString();
                        lst.Add(Convert.ToInt32(row[0]), val);
                    }
                    studentDDL.DataSource = lst;
                    studentDDL.DataTextField = "Value";
                    studentDDL.DataValueField = "key";
                    studentDDL.DataBind();
                }

                using (SqlCommand cmd = new SqlCommand("select * from Teacher", con))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    Dictionary<int, string> lst = new Dictionary<int, string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        //Add values to Dictionary
                        string val = row[1].ToString() + "  " + row[2].ToString() + " - " + row[0].ToString();
                        lst.Add(Convert.ToInt32(row[0]), val);
                    }
                    teacherDDL.DataSource = lst;
                    teacherDDL.DataTextField = "Value";
                    teacherDDL.DataValueField = "key";
                    teacherDDL.DataBind();
                }
            }


            if (Convert.ToInt32(Session["isClicked"]) == 1)
            {
                Messages message = new Messages();
                string msg_content;
                int msdID = Convert.ToInt32(Session["msg_id"]);
                DataTable dt = message.getMessageContent(msdID);
                foreach (DataRow row in dt.Rows)
                {
                    msg_content = Convert.ToString(Session["msg_content"]);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowMessageContent", "$('#ShowMessageContent').modal();", true);
                    displayContentLBL.Text = msg_content;
                    msgHeaderTitleLBL.Text = Convert.ToString(Session["msg_subject"]);
                }
            }
            Session["isClicked"] = 0;
        }
    }

    //////שליחת הודעה ממנהלת לתלמיד
    protected void sendMessageBTN_Click(object sender, EventArgs e)
    {
        Messages message;
        Manager m = (Manager)(Session["manUserSession"]);
        double msg_fromAddminId = m.Man_id;
        double msg_toStudentId = Convert.ToDouble(studentDDL.SelectedValue);
        //double msg_toTeacherId = Convert.ToDouble(teacherDDL.SelectedValue);
        string msg_subject = subjectTB.Text;
        string msg_content = contentTB.Text;
        bool msg_has_read = false;
        DateTime msg_date = DateTime.Now;


        try
        {

            message = new Messages(msg_fromAddminId, msg_toStudentId, msg_subject, msg_content, msg_has_read, (msg_date).ToString("yyyyMMdd"));
        }
        catch (Exception ex)
        {
            return;
        }

        try
        {
            int numEffected = message.InsertMessage();

        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);
        }

        Response.Redirect("ShowAdminMessages.aspx");

    }

    //////שליחת הודעה ממנהלת למתגבר
    protected void sendMessageToTeacherBTN_Click(object sender, EventArgs e)
    {
        Manager m = (Manager)(Session["manUserSession"]);
        double msg_fromAddminId = m.Man_id;
        double msg_toTeacherId = Convert.ToDouble(teacherDDL.SelectedValue);
        string msg_subject = subjectTBForTeacher.Text;
        string msg_content = contentTBForTeacher.Text;
        bool msg_has_read = false;
        DateTime msg_date = DateTime.Now;

        try
        {
            Messages messageToTeacher = new Messages();
            messageToTeacher.Msg_fromManagerId = msg_fromAddminId;
            messageToTeacher.Msg_toTeacherId = msg_toTeacherId;
            messageToTeacher.Msg_subject = msg_subject;
            messageToTeacher.Msg_content = msg_content;
            messageToTeacher.Msg_hasRead = msg_has_read;
            messageToTeacher.Msg_date = msg_date.ToString("yyyyMMdd");

            int numEffected = messageToTeacher.InsertMessageFromManagerToTeacher();
        }

        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);
        }

        Response.Redirect("ShowAdminMessages.aspx");

    }
    //הצגת דואר נכנס של המנהלת
    protected void ShowInMail_Click(object sender, EventArgs e)
    {
        Session["MailStatus"] = true;
        InMailGRDW.Visible = true;
        InMailFromTeachersGRDW.Visible = true;
        AdminMessagesGRDW.Visible = false;
        OutMailAdminToTeachersGRDW.Visible = false;
        headerText.InnerText = "דואר נכנס";
        ReadAllBTN.Visible = true;
        ReadAllForTeachersBTN.Visible = true;
    }

    //הצגת דואר יוצא של המנהלת
    protected void ShowOutMail_Click(object sender, EventArgs e)
    {
        Session["MailStatus"] = false;
        AdminMessagesGRDW.Visible = true;
        OutMailAdminToTeachersGRDW.Visible = true;
        InMailFromTeachersGRDW.Visible = false;
        InMailGRDW.Visible = false;
        headerText.InnerText = "דואר יוצא";
        ReadAllBTN.Visible = false;
        ReadAllForTeachersBTN.Visible = false;
    }

    /////////פונקציות של דואר נכנס מהתלמיד

    //הצגת פרטי הודעת דואר נכנס שנשלח מתלמיד
    protected void showInMessageDetails_Click(object sender, EventArgs e)
    {

        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        string msg_subject = Convert.ToString(gridViewRow.Cells[3].Text);
        Session["msg_subject"] = msg_subject;
        string msg_content = Convert.ToString(gridViewRow.Cells[8].Text);
        Session["msg_content"] = msg_content;
        Session["stuID"] = Convert.ToString(gridViewRow.Cells[7].Text);
        message.UpdateMessageStatus(msg_id);

        Session["msg_id"] = msg_id;

        if (Convert.ToBoolean(gridViewRow.Cells[6].Text) == false)
        {
            for (int i = 0; i < 6; i++)
            {
                gridViewRow.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "");
            }
        }
        Session["isClicked"] = 1;
        Session["messageFromStudent"] = true;

        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

    //הדגשת הודעות שלא נקראו עדיין בטבלת הודעות נכנסות למנהלת שהתקבלו מתלמידים
    protected void InMailGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(e.Row.Cells[6].Text) == false)
            {
                for (int i = 0; i < 6; i++)
                {
                    e.Row.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[4].Text;
            if (e.Row.Cells[4].Text.Length >= 60) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 60) + "...";
                e.Row.Cells[4].ToolTip = ViewState["OrigData"].ToString();
            }

        }
    }

    protected void showInMessageFromTeachersDetails_Click(object sender, EventArgs e)
    {
        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        string msg_subject = Convert.ToString(gridViewRow.Cells[3].Text);
        Session["msg_subject"] = msg_subject;
        string msg_content = Convert.ToString(gridViewRow.Cells[4].Text);
        Session["msg_content"] = msg_content;
        Session["TeaId"] = Convert.ToString(gridViewRow.Cells[7].Text);
        message.UpdateMessageStatusAtTeacherMessages(msg_id);

        Session["msg_id"] = msg_id;

        if (Convert.ToBoolean(gridViewRow.Cells[6].Text) == false)
        {
            for (int i = 0; i < 6; i++)
            {
                gridViewRow.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "");
            }
        }
        Session["isClicked"] = 1;
        Session["messageFromStudent"] = false;

        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }

    //הדגשת הודעות שלא נקראו עדיין בטבלת הודעות נכנסות למנהלת שהתקבלו ממתגברים
    protected void InMailFromTeachersGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(e.Row.Cells[6].Text) == false)
            {
                for (int i = 0; i < 6; i++)
                {
                    e.Row.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[4].Text;
            if (e.Row.Cells[4].Text.Length >= 60) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 60) + "...";
                e.Row.Cells[4].ToolTip = ViewState["OrigData"].ToString();
            }

        }
    }

    //סמן הכל כנקרא עבור הודעות שהגיעו מתלמידים
    protected void ReadAllBTN_Click(object sender, EventArgs e)
    {
        Messages message = new Messages();
        message.UpdateHasReadForAllRecordsForManager();
        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

    /////////פונקציות של דואר יוצא מהתלמיד

    //הצגת פרטי הודעת דואר יוצא שנשלח לתלמיד
    protected void showOutMessageDetails_Click(object sender, EventArgs e)
    {

        responseBTN.Visible = false;
        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        Session["msg_id"] = msg_id;

        msgHeaderTitleLBL.Text = Convert.ToString(gridViewRow.Cells[3].Text);
        string msg_content;
        DataTable dt = message.getOutMessageContent(msg_id);
        foreach (DataRow row in dt.Rows)
        {
            msg_content = Convert.ToString(row["msg_content"]);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowMessageContent", "$('#ShowMessageContent').modal();", true);
            displayContentLBL.Text = msg_content;
        }
        Session["messageFromStudent"] = true;

    }

    protected void responseBTN_Click(object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Session["messageFromStudent"]))
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "createMessageModal", "$('#createMessageModal').modal();", true);
            string msg_subject = "תגובה ל: " + Convert.ToString(Session["msg_subject"]);
            subjectTB.Text = msg_subject;
            string studentId = Session["stuID"].ToString();
            studentDDL.SelectedValue = studentId;
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "createMessageModalToTeacher", "$('#createMessageModalToTeacher').modal();", true);
            string msg_subject = "תגובה ל: " + Convert.ToString(Session["msg_subject"]);
            subjectTBForTeacher.Text = msg_subject;
            string teacherId = Session["TeaId"].ToString();
            teacherDDL.SelectedValue = teacherId;
        }
    }

    protected void AdminMessagesGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[4].Text;
            if (e.Row.Cells[4].Text.Length >= 60) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 60) + "...";
                e.Row.Cells[4].ToolTip = ViewState["OrigData"].ToString();
            }

        }

    }

    protected void showOutMessageFromTeachersDetails_Click(object sender, EventArgs e)
    {
        responseBTN.Visible = false;
        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        Session["msg_id"] = msg_id;
        msgHeaderTitleLBL.Text = Convert.ToString(gridViewRow.Cells[3].Text);
        string msg_content;
        DataTable dt = message.getOutMessageContentFromManagerMessagesToTeachers(msg_id);
        foreach (DataRow row in dt.Rows)
        {
            msg_content = Convert.ToString(row["msg_content"]);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowMessageContent", "$('#ShowMessageContent').modal();", true);
            displayContentLBL.Text = msg_content;
        }

    }


    protected void deleteBTN_Click(object sender, EventArgs e)
    {
        Messages message = new Messages();
        int msg_id = Convert.ToInt32(Session["msg_id"]);

        //מחיקת דואר נכנס מתלמיד
        if (Convert.ToBoolean(Session["MailStatus"]) == true && Convert.ToBoolean(Session["messageFromStudent"]) == true)
        {
            message.UpdateIsDeletedInMessageForManager(msg_id);
        }

        //מחיקת דואר יוצא לתלמיד
        else if (Convert.ToBoolean(Session["MailStatus"]) == false && Convert.ToBoolean(Session["messageFromStudent"]) == true)
        {
            message.UpdateIsDeletedOutMessageForManager(msg_id);
        }

        //מחיקת דואר נכנס ממתגבר
        else if (Convert.ToBoolean(Session["MailStatus"]) == true && Convert.ToBoolean(Session["messageFromStudent"]) == false)
        {
            message.UpdateIsDeletedInMessageForManagerAtTeacherMessages(msg_id);
        }

        //מחיקת דואר יוצא למתגבר
        else
        {
            message.UpdateIsDeletedOutMessageForManagerToTeacher(msg_id);

        }

        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }

    protected void ReadAllForTeachersBTN_Click(object sender, EventArgs e)
    {
        Messages message = new Messages();
        message.UpdateHasReadForAllRecordsForManagerFromTeachers();
        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }
}
