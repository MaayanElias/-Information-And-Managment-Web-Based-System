using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class ShowStudentMessages : System.Web.UI.Page
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

            InMailGRDW.Visible = true;
            StudentMessagesGRDW.Visible = false;
            Session["MailStatus"] = true;

        }


        Student s = (Student)(Session["userStudent"]);
        string msg_studentId = Convert.ToString(s.Stu_id);
        InMailStudentDS.SelectParameters.Add("current_stu_id", msg_studentId);

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Student s = (Student)(Session["userStudent"]);
            string msg_studentId = Convert.ToString(s.Stu_id);
            StudentMessagesDS.SelectParameters.Add("stu_id", msg_studentId);
            if (Convert.ToInt32(Session["isClicked"]) == 1)
            {
                Messages message = new Messages();
                string msg_content;
                int msdID = Convert.ToInt32(Session["msg_id"]);
                DataTable dt = message.getMessageContentForStudent(msdID);
                foreach (DataRow row in dt.Rows)
                {
                    msg_content = Convert.ToString(row["msg_content"]);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowMessageContent", "$('#ShowMessageContent').modal();", true);
                    displayContentLBL.Text = msg_content;
                    msgHeaderTitleLBL.Text = Convert.ToString(Session["msg_subject"]);

                }
            }
            Session["isClicked"] = 0;
        }
    }

    protected void ShowInMail_Click(object sender, EventArgs e)
    {
        Session["MailStatus"] = true;
        InMailGRDW.Visible = true;
        StudentMessagesGRDW.Visible = false;
        ReadAllBTN.Visible = true;
        headerText.InnerText = "דואר נכנס";

    }

    protected void ShowOutMail_Click(object sender, EventArgs e)
    {
        Session["MailStatus"] = false;
        StudentMessagesGRDW.Visible = true;
        InMailGRDW.Visible = false;
        ReadAllBTN.Visible = false;
        headerText.InnerText = "דואר יוצא";

    }



    protected void sendMessageBTN_Click(object sender, EventArgs e)
    {
        Messages messageForStudent = new Messages();
        Student s = (Student)(Session["userStudent"]);
        double msg_fromStudentId = s.Stu_id;

        DataTable dt = this.GetManeger();

        foreach (DataRow dr in dt.Rows)
        {
            double msg_toManagerId = Convert.ToDouble(dr["Man_Id"]);
            Session["msg_toManagerId"] = msg_toManagerId;
        }

        string msg_subject = subjectTB.Text;
        string msg_content = contentTB.Text;
        bool msg_has_read = false;
        DateTime msg_date = DateTime.Now;


        try
        {
            double msg_toManagerId = Convert.ToDouble(Session["msg_toManagerId"]);
            messageForStudent.Msg_fromStudentId = msg_fromStudentId;
            messageForStudent.Msg_toManagerId = msg_toManagerId;
            messageForStudent.Msg_subject = msg_subject;
            messageForStudent.Msg_content = msg_content;
            messageForStudent.Msg_hasRead = msg_has_read;
            messageForStudent.Msg_date = msg_date.ToString("yyyyMMdd");

        }
        catch (Exception ex)
        {

            return;
        }

        try
        {
            int numEffected = messageForStudent.InsertMessageForStudent();

        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);
        }

        Response.Redirect("ShowStudentMessages.aspx");


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


    protected void showInMessageDetails_Click(object sender, EventArgs e)
    {

        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        string msg_subject = Convert.ToString(gridViewRow.Cells[3].Text);
        Session["msg_subject"] = msg_subject;
        string msg_content = Convert.ToString(gridViewRow.Cells[4].Text);
        Session["msg_content"] = msg_content;
        message.UpdateMessageStatusForStudent(msg_id);

        Session["msg_id"] = msg_id;

        if (Convert.ToBoolean(gridViewRow.Cells[6].Text) == false)
        {
            for (int i = 0; i < 6; i++)
            {
                gridViewRow.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "");

            }
        }

        Session["isClicked"] = 1;

        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

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

    protected void StudentMessagesGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void showOutMessageDetails_Click(object sender, EventArgs e)
    {
        responseBTN.Visible = false;
        Messages message = new Messages();
        GridViewRow gridViewRow = (GridViewRow)(sender as Control).Parent.Parent;
        int msg_id = Convert.ToInt32(gridViewRow.Cells[5].Text);
        Session["msg_id"] = msg_id;
        msgHeaderTitleLBL.Text = Convert.ToString(gridViewRow.Cells[3].Text);

        string msg_content;
        DataTable dt = message.getOutMessageContentForStudent(msg_id);
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
        if (Convert.ToBoolean(Session["MailStatus"]) == true)
        {
            message.UpdateIsDeletedInMessageForStudent(msg_id);
        }
        else
        {
            message.UpdateIsDeletedOutMessageForStudent(msg_id);
        }

        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

    protected void ReadAllBTN_Click(object sender, EventArgs e)
    {
        Student s = (Student)(Session["userStudent"]);
        double msg_studentId = Convert.ToDouble(s.Stu_id);
        Messages message = new Messages();
        message.UpdateHasReadForAllRecordsForStudent(msg_studentId);
        Server.TransferRequest(Request.Url.AbsolutePath, false);
    }

    protected void responseBTN_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "createMessageModal", "$('#createMessageModal').modal();", true);
        string msg_subject = "תגובה ל: " + Convert.ToString(Session["msg_subject"]);
        subjectTB.Text = msg_subject;
    }
}