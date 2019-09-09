using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class TeacherAvailability : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["TeacherID"] == null)
        {
            Response.Redirect("ShowTeacher.aspx");
        }
        else
        {
            availabilityDS.SelectParameters.Add("current_teacher_id", Session["TeacherID"].ToString());

            availabilityDS.SelectCommand = "select Ave_TeaId,Ave_day,Ave_startHour, Ave_endtHour, (tea_firstName + ' ' + tea_lastName) as 'teacher_full_name' from TeacheravAilability inner join Teacher on Ave_TeaId = Tea_Id where Ave_TeaId=@current_teacher_id";
        }
    }

    protected void submitBTN_Click(object sender, EventArgs e)
    {
        if (Session["TeacherID"] != null)
        {
            double teaId = Convert.ToDouble(Session["TeacherID"]);
            Teacher teaAvailability = new Teacher(teaId, Convert.ToInt32(dayDDL.SelectedValue), startTB.Text, endTB.Text);
            int numAffected = teaAvailability.InsertAvailability();
            Server.TransferRequest(Request.Url.AbsolutePath, false);
        }
        else
        {
            Response.Redirect("ShowTeacher.aspx");
        }

    }


    protected void availabilityGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)


            if (e.Row.Cells[0].Text == "1")
                e.Row.Cells[0].Text = "א'";
            else if (e.Row.Cells[0].Text == "2")
                e.Row.Cells[0].Text = "ב'";
            else if (e.Row.Cells[0].Text == "3")
                e.Row.Cells[0].Text = "ג'";
            else if (e.Row.Cells[0].Text == "4")
                e.Row.Cells[0].Text = "ד'";
            else // יום חמישי 
                e.Row.Cells[0].Text = "ה'";
    }



    protected void saveTeacherBTN_Click(object sender, EventArgs e)
    {
        Session["AddTeacher"] = true;
        Response.Redirect("ShowTeacher.aspx");
    }
}