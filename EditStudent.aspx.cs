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

public partial class EditStudent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      

        if (!Page.IsPostBack)
        {

            if (Session["selectedID"] == null)
            {
                Response.Redirect("ShowLesson.aspx");
            }
            int stud_id = (int)(Session["selectedID"]);
            fillDetails(stud_id);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString))
            {
                con.Open();
                //Change with your select statement .
                using (SqlCommand cmd = new SqlCommand("select * from instructor", con))
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
                    instructorDDL.DataSource = lst;
                    instructorDDL.DataTextField = "Value";
                    instructorDDL.DataValueField = "key";
                    instructorDDL.DataBind();
                }
            }


            Student s = new Student();
            int classesCounter = s.GetNumOfClassForStu(stud_id); //נביא את מספר התלמידים שהתלמיד משתתף בהם וכבר התקיימו
            total.Text = classesCounter.ToString();
            int PresenceCounter = s.GetNumOfPresenceForStu(stud_id);
            pres.Text = PresenceCounter.ToString();
            int notPresCounter = s.GetNumnotPresForStu(stud_id);
            notPres.Text = notPresCounter.ToString();
        }

    }



    private void fillDetails(double stud_id)
    {

        Student s = new Student();
        DataTable dt = s.readSpecipicStudent(stud_id);
        foreach (DataRow row in dt.Rows)
        {
            IdTB.Text = row["stu_id"].ToString();
            instructorDDL.SelectedValue = row["Ins_Id"].ToString();
            FirstNameTB.Text = row["stu_firstName"].ToString();
            LastNameTB.Text = row["stu_LastName"].ToString();
            BirhtDateTB.Text = row["stu_birthDate"].ToString();
            PhoneTB.Text = row["stu_PhoneNumber"].ToString();
            MailTB.Text = row["stu_Email"].ToString();
            addressTB.Text = row["stu_address"].ToString();
            int status = Convert.ToUInt16(row["stu_status"]);
            if (status == 1) { statusCB.Checked = true; }
            else { statusCB.Checked = false; }
            gradeDDL.SelectedValue = row["stu_grade"].ToString();
            int IsEntitled = Convert.ToUInt16(row["stu_isEntitled"]);
            if (IsEntitled == 1) { isEntitledCB.Checked = true; }
            else { isEntitledCB.Checked = false; }
            PasswordTB.Text = row["stu_password"].ToString();
            NoteTB.Text = row["stu_note"].ToString();

        }
        string stu_id = IdTB.Text;
        commentsForStudentDS.SelectParameters.Add("stu_id", stu_id);


    }


    protected void deleteBTN_Click(object sender, EventArgs e)
    {
        Student stud = new Student();
        int id = Convert.ToInt32(IdTB.Text);
        try
        {
            int numEffected = stud.deleteStudent(id);
            Session["DeleteStudent"] = true;
            Response.Redirect("ShowStudents.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }
    }


    protected void saveBTN_Click(object sender, EventArgs e)
    {

        Student stud = new Student();

        stud.Stu_id = Convert.ToInt32(IdTB.Text);
        stud.Stu_Instructor_id = Convert.ToInt32(instructorDDL.SelectedValue);
        stud.FirstName = FirstNameTB.Text;
        stud.LastName = LastNameTB.Text;
        stud.BirthDate = BirhtDateTB.Text;
        stud.PhoneNumber = PhoneTB.Text;
        stud.Email = MailTB.Text;
        stud.Address = addressTB.Text;
        stud.Status = statusCB.Checked;
        stud.Grade = gradeDDL.SelectedValue;
        stud.IsEntitled = isEntitledCB.Checked;
        stud.Password = PasswordTB.Text;
        stud.Notes = NoteTB.Text;

        try
        {
            int numEffected = stud.updateSpecificStudent(); ;
            Session["AddStudent"] = true;
            Response.Redirect("ShowStudents.aspx");
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('אנא מלא את כל הפרטים')", true);
        }
    }

    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowStudents.aspx");
    }


}