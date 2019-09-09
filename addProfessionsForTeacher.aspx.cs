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

public partial class addProfessionsForTeacher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AddPro"]!=null)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('מקצוע נשמר למתגבר')", true);

        }
        SqlDataSource1.SelectParameters.Add("current_teacher_id", Session["TeacherID"].ToString());

        SqlDataSource1.SelectCommand = "SELECT Pro_Title FROM Profession pro INNER JOIN TeachBy tb ON pro.Pro_Id = tb.pro_id WHERE (tb.tea_id = @current_teacher_id)";
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString))
        {
            con.Open();
            //Change with your select statement .
            using (SqlCommand cmd = new SqlCommand("select * from Profession", con))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
                Dictionary<int, string> lst = new Dictionary<int, string>();
                foreach (DataRow row in dt.Rows)
                {
                    //Add values to Dictionary
                    string val = row[1].ToString();
                    lst.Add(Convert.ToInt32(row[0]), val);
                }
                ProfessionDDL.DataSource = lst;
                ProfessionDDL.DataTextField = "Value";
                ProfessionDDL.DataValueField = "key";
                ProfessionDDL.DataBind();
            }
        }

    }

    protected void addProBTN_Click(object sender, EventArgs e)
    {
        {
            TeachBy tb;
            int pro_id = Convert.ToInt32(ProfessionDDL.SelectedValue);


            try
            {
                tb = new TeachBy(Convert.ToDouble(Session["TeacherID"]), pro_id);
            }
            catch (Exception ex)
            {
                Response.Write("illegal values to the Student attributes - error message is " + ex.Message);
                return;
            }

            try
            {
                int numEffected = tb.InsertTeachBy();
                Session["AddPro"] = true;
                Response.Redirect("addProfessionsForTeacher.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            }


        }
    }

    protected void submitBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("TeacherAvailability.aspx");
    }
}