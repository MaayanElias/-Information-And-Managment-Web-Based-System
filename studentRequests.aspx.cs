using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class studentRequests : System.Web.UI.Page
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

            Student s = (Student)(Session["userStudent"]);
            string reqStuId = Convert.ToString(s.Stu_id);
            stuReqDS.SelectParameters.Add("current_stu_id", reqStuId);


            statusDDL.ClearSelection(); //making sure the previous selection has been cleared
            statusDDL.Items.FindByValue("2").Selected = true;
        }

    }

    protected void stuReqGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[1].Text == "0")
                e.Row.Cells[1].Text = "הרשמה לתגבור";

            else if (e.Row.Cells[1].Text == "1")
                e.Row.Cells[1].Text = "ביטול השתתפות";
            else
                e.Row.Cells[1].Text = "";

            if (e.Row.Cells[5].Text == "1")
                e.Row.Cells[5].Text = "א'";
            else if (e.Row.Cells[5].Text == "2")
                e.Row.Cells[5].Text = "ב'";
            else if (e.Row.Cells[5].Text == "3")
                e.Row.Cells[5].Text = "ג'";
            else if (e.Row.Cells[5].Text == "4")
                e.Row.Cells[5].Text = "ד'";
            else // יום חמישי 
                e.Row.Cells[5].Text = "ה'";


            if (e.Row.Cells[0].Text == "1")
                e.Row.Cells[0].Text = "אושרה";

            else if (e.Row.Cells[0].Text == "0")
                e.Row.Cells[0].Text = "נדחתה";

            else
                e.Row.Cells[0].Text = "ממתינה";

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[4].Text.IndexOf(" ");
            e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, index);
        }
    }
}