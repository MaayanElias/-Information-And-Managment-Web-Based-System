using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


public partial class addTeacher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["manUserSession"] == null)
            {
                Response.Redirect("default.aspx");
            }

        }
    }





    protected void submitBTN_Click(object sender, EventArgs e)
    {

        Teacher t;
        bool status = true;

        try
        {
            t = new Teacher(Convert.ToDouble(idTB.Text), fNameTB.Text, LNameTB.Text, phoneTB.Text, mailTB.Text, addressTB.Text, status, passwordTB.Text);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('אנא מלא את כל הפרטים')", true);
            return;
        }
        try
        {
            int numEffected = t.InsertTeacher();
            Session["AddTeacher"] = true;
            Session["TeacherID"] = Convert.ToDouble(idTB.Text);
            Response.Redirect("addProfessionsForTeacher.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Stuert the Teacher into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);

        }
    }
    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowTeacher.aspx");
    }

}