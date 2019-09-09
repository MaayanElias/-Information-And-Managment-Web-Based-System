using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Student UserStudent = null;

        if (Session["stuUserSession"] != null)
        {
            UserStudent = (Student)(Session["stuUserSession"]);
            userId.Value = UserStudent.Stu_id.ToString();
            Session["userStudent"] = UserStudent;
        }
        else Response.Redirect("default.aspx");
        if (!IsPostBack)
        {

            Student s = (Student)(Session["userStudent"]);
            string StuId = Convert.ToString(s.Stu_id);
            upcomingLessonsForStudentDS.SelectParameters.Add("current_stu_id", StuId);
        }

        Report r = new Report();
        try
        {
            
            int unreadMessagesCounter = r.getUnreadMessagesCountForStudent(userId.Value);
            HiddenUnreadMessagesCounter.Text = unreadMessagesCounter.ToString();
            if (!IsPostBack)
            {
                if (unreadMessagesCounter == 1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.info('יש לך הודעה חדשה אחת')", true);
                }
                else if (unreadMessagesCounter > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.info('יש לך " + unreadMessagesCounter + " הודעות חדשות')", true);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to get requests count" + ex.Message);
        }
        upcomingLessonsForStudentGRDW.Font.Size = FontUnit.Small;
        InMailGRDW.Font.Size = FontUnit.Small;


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Student s = (Student)(Session["userStudent"]);
            string StuId = Convert.ToString(s.Stu_id);
            InMailStudentDS.SelectParameters.Add("current_stu_id", StuId);

        }
    }




    protected void InMailGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToBoolean(e.Row.Cells[5].Text) == false)
            {
                for (int i = 0; i < 6; i++)
                {
                    e.Row.Cells[i].Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 40) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 40) + "...";
                e.Row.Cells[3].ToolTip = ViewState["OrigData"].ToString();
            }

        }
    }
}