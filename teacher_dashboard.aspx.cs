using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teacher_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        Teacher UserTeacher = null;

        if (Session["teaUserSession"] != null)
        {
            UserTeacher = (Teacher)(Session["teaUserSession"]);
            userId.Value = UserTeacher.Tea_id.ToString();
            Session["teaUserSession"] = UserTeacher;
        }
        else Response.Redirect("default.aspx");
        if (!IsPostBack)
        {

            Teacher t = (Teacher)(Session["teaUserSession"]);
            string TeaId = Convert.ToString(t.Tea_id);
            upcomingLessonsForTeacherDS.SelectParameters.Add("current_tea_id", TeaId);
        }

        Report r = new Report();
        try
        {

            int unreadMessagesCounter = r.getUnreadMessagesCountForTeacher(userId.Value);
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
        upcomingLessonsForTeacherGRDW.Font.Size = FontUnit.Small;
        InMailGRDW.Font.Size = FontUnit.Small;


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Teacher t = (Teacher)(Session["teaUserSession"]);
            string TeaId = Convert.ToString(t.Tea_id);
            InMailTeacherDS.SelectParameters.Add("current_tea_id", TeaId);

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