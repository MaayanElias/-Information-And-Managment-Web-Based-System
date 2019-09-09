using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class AddStudent : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["manUserSession"] == null)
            {
                Response.Redirect("default.aspx");
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString))
            {
                con.Open();
                //Change with your select statement .
                using (SqlCommand cmd = new SqlCommand("select * from Instructor", con))
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
                    InstructorDDL.DataSource = lst;
                    InstructorDDL.DataTextField = "Value";
                    InstructorDDL.DataValueField = "key";
                    InstructorDDL.DataBind();
                }
            }
        }
    }

    //protected void dateCAL_SelectionChanged(object sender, EventArgs e)
    //{
    //    BirhtDateTB.Text = dateCAL.SelectedDate.ToShortDateString();
    //}


    protected void submitBTN_Click(object sender, EventArgs e)
    {
        Student stud;
        int Ins_id = Convert.ToInt32(InstructorDDL.SelectedValue);
        bool status = true;
        bool entitled = true;
        try
        {
            stud = new Student(Convert.ToDouble(IdTB.Text), Ins_id, FirstNameTB.Text, LastNameTB.Text, BirhtDateTB.Text, PhoneTB.Text, MailTB.Text, addressTB.Text, status, PasswordTB.Text, gradeDDL.SelectedValue, entitled, NoteTB.Text);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('אנא מלא את כל הפרטים')", true);

            return;
            

        }

        try
        {
            int numEffected = stud.InsertStudent();
            Session["AddStudent"] = true;
            Response.Redirect("ShowStudents.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);
        }

    }



    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowStudents.aspx");
    }
}