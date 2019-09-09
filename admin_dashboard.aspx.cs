using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_dashboard : System.Web.UI.Page
{
    public string inputStartValue;
    public string inputEndValue;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["manUserSession"] == null)
            {
                Response.Redirect("default.aspx");
            }
        }
        Report r = new Report();
        try
        {
            int requestsCounter = r.getRequestsCount();
            HiddenRequestsCounter.Text = requestsCounter.ToString();
            int attendenceFormsCounter = r.getAttendenceFormsCount();
            HiddenAttendenceFormsCounter.Text = attendenceFormsCounter.ToString();
            int unreadMessagesCounter = r.getUnreadMessagesCount();
            HiddenUnreadMessagesCounter.Text = unreadMessagesCounter.ToString();
            if (!IsPostBack)
            {
                if (unreadMessagesCounter==1)
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
        finally
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { loadSettings(); });", true);
        }

     

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (manMessagesGRDW.Rows.Count == 0)
            manMessagesGRDW.CssClass = "empty";
    }


    protected void filter_dayBTN_Click(object sender, EventArgs e)
    {
        string startDate = DateTime.Now.ToString("yyyy-MM-dd");
        inputStartValue = startDate;
        inputEndValue = startDate;
        chartTitle.InnerHtml = "סינון: יום";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { loadGraphs(); });", true);

    }

    protected void filter_weekBTN_Click(object sender, EventArgs e)
    {
        DateTime date = DateTime.Now;
        string startDate = DateTime.Now.ToString("yyyy-MM-dd");
        string endDate = date.AddDays(7).ToString("yyyy-MM-dd");
        inputStartValue = startDate;
        inputEndValue = endDate;
        chartTitle.InnerHtml = "סינון: שבוע";

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { loadGraphs(); });", true);
    }

    protected void filter_monthBTN_Click(object sender, EventArgs e)
    {
        DateTime date = DateTime.Now;
        string startDate = DateTime.Now.ToString("yyyy-MM-dd");
        string endDate = date.AddMonths(1).ToString("yyyy-MM-dd");
        inputStartValue = startDate;
        inputEndValue = endDate;
        chartTitle.InnerHtml = "סינון: חודש";

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { loadGraphs(); });", true);
    }

    protected void filter_clear_Click(object sender, EventArgs e)
    {    
        inputStartValue = "";
        inputEndValue = "";
        chartTitle.InnerHtml = "";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { loadGraphs(); });", true);
    }

    protected void manMessagesGRDW_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["OrigData"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 30) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0, 30) + "...";
                e.Row.Cells[3].ToolTip = ViewState["OrigData"].ToString();
            }
            ViewState["OrigData"] = e.Row.Cells[2].Text;
            if (e.Row.Cells[2].Text.Length >= 20) //Just change the value of 30 based on your requirements
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Substring(0, 20) + "...";
                e.Row.Cells[2].ToolTip = ViewState["OrigData"].ToString();
            }


        }


    }
}