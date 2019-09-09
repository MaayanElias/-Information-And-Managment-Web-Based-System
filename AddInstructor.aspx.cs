using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddInstructor : System.Web.UI.Page
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
        Instructor Inst;
        bool status = true;

        try
        {

            Inst = new Instructor(Convert.ToDouble(IdTB.Text), FirstNameTB.Text, LastNameTB.Text, PhoneTB.Text, MailTB.Text,addressTB.Text,status,PasswordTB.Text);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('אנא מלא את כל הפרטים')", true);
            return;
        }

        try
        {
            int numEffected = Inst.InsertInstructor();
            Session["AddInstuctor"] = true;
            Response.Redirect("ShowInstructors.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Insert the product into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);

        }

    }

    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowInstructors.aspx");
    }

}