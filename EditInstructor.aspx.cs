using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class EditInstructor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["selectedID"] == null)
        {
            Response.Redirect("ShowInstructors.aspx");
        }
        double ins_id = (double)(Session["selectedID"]);


        if (!Page.IsPostBack)
        {
            fillDetails(ins_id);
        }

    }

    private void fillDetails(double ins_id)
    {

        Instructor ins = new Instructor();
        DataTable dt = ins.readSpecipicInstructor(ins_id);
        foreach (DataRow row in dt.Rows)
        {
            IdTB.Text = row["Ins_id"].ToString();
            FirstNameTB.Text = row["Ins_firstName"].ToString();
            LastNameTB.Text = row["Ins_LastName"].ToString();
            PhoneTB.Text = row["Ins_PhoneNumber"].ToString();
            MailTB.Text = row["Ins_Email"].ToString();
            addressTB.Text = row["Ins_address"].ToString();
            passwordTB.Text = row["Ins_password"].ToString();
            int status = Convert.ToUInt16(row["Ins_status"]);
            if (status == 1) { statusCB.Checked = true; }
            else { statusCB.Checked = false; }
        }
    }

    protected void deleteBTN_Click(object sender, EventArgs e)
    {
        Instructor inst = new Instructor();
        double id = Convert.ToInt32(IdTB.Text);
        try
        {
            int numEffected1 = inst.deleteInstructorFromStudent(id);
            int numEffected2 = inst.deleteInstructor(id);
            Session["DeleteInstructor"] = true;
            Response.Redirect("ShowInstructors.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }
    }

    protected void saveBTN_Click(object sender, EventArgs e)
    {
        Instructor inst = new Instructor();
        inst.Ins_id = Convert.ToDouble(IdTB.Text);
        inst.Ins_firstName = FirstNameTB.Text;
        inst.Ins_lastName = LastNameTB.Text;
        inst.Ins_phoneNumber = PhoneTB.Text;
        inst.Ins_email = MailTB.Text;
        inst.Ins_addres = addressTB.Text;
        inst.Ins_password = passwordTB.Text;
        inst.Ins_status = statusCB.Checked;

        try
        {
            int numEffected = inst.updateSpecificInstructor();
            Session["AddInstuctor"] = true;
            Response.Redirect("ShowInstructors.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }

    }
    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowInstructors.aspx");
    }
}