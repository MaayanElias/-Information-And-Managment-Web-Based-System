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

public partial class EditTeacher : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        bool AddPro = Convert.ToBoolean(Session["AddPro"]);
        if (AddPro)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('המקצוע נוסף בהצלחה')", true);
            Session["AddPro"] = false;
        }
        bool Addavailability = Convert.ToBoolean(Session["AddAvailability"]);
        if (Addavailability)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('פרטי זמינות נוספו למתגבר')", true);
            Session["AddAvailability"] = false;
        }


        if (!Page.IsPostBack)
        {
            if (Session["selectedTeacherID"] == null)
            {
                Response.Redirect("ShowTeacher.aspx");
            }
            double s_tea_id = (double)(Session["selectedTeacherID"]);
            fillDetails(s_tea_id);

        }
        SqlDataSource1.SelectParameters.Add("current_teacher_id", idTB.Text);

        SqlDataSource1.SelectCommand = "SELECT Pro_Title FROM Profession pro INNER JOIN TeachBy tb ON pro.Pro_Id = tb.pro_id WHERE (tb.tea_id = @current_teacher_id)";
        Report r = new Report();
        try
        {
            int teacherId = Convert.ToInt32(idTB.Text);
            int HoursCounter = r.getHoursCount(teacherId);
            int AttendanceFormCounter = r.getAttendanceFormCount(teacherId);
            HiddenHoursCounter.Text = HoursCounter.ToString();
            HiddenAttendanceFormCounter.Text = AttendanceFormCounter.ToString();
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to get requests count" + ex.Message);

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
        availabilityDS.SelectParameters.Add("current_teacher_id", idTB.Text);

        availabilityDS.SelectCommand = "select Ave_TeaId,Ave_day,Ave_startHour, Ave_endtHour, (tea_firstName + ' ' + tea_lastName) as 'teacher_full_name' from TeacheravAilability inner join Teacher on Ave_TeaId = Tea_Id where Ave_TeaId=@current_teacher_id";
    }


    private void fillDetails(double s_tea_id)
    {

        Teacher t = new Teacher();
        DataTable dt = t.readSpecipicTeacher(s_tea_id);
        foreach (DataRow row in dt.Rows)
        {
            idTB.Text = row["tea_id"].ToString();
            fNameTB.Text = row["tea_firstName"].ToString();
            LNameTB.Text = row["tea_LastName"].ToString();
            phoneTB.Text = row["tea_PhoneNumber"].ToString();
            mailTB.Text = row["tea_Email"].ToString();
            addressTB.Text = row["tea_address"].ToString();
            int status = Convert.ToUInt16(row["tea_status"]);
            if (status == 1) { statusCB.Checked = true; }
            else { statusCB.Checked = false; }
            passwordTB.Text = row["tea_password"].ToString();

        }



    }


    protected void deleteBTN_Click(object sender, EventArgs e)
    {
        Teacher tea = new Teacher();
        int id = Convert.ToInt32(idTB.Text);
        try
        {
            int numEffected = tea.deleteTeacher(id);
            Session["DeleteTeacher"] = true;
            Response.Redirect("ShowTeacher.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }
    }


    protected void saveBTN_Click(object sender, EventArgs e)
    {

        Teacher tea = new Teacher();

        tea.Tea_id = Convert.ToInt32(idTB.Text);
        tea.Tea_firstName = fNameTB.Text;
        tea.Tea_lastName = LNameTB.Text;
        tea.Tea_phoneNumber = phoneTB.Text;
        tea.Tea_email = mailTB.Text;
        tea.Tea_address = addressTB.Text;
        tea.Tea_status = statusCB.Checked;
        tea.Tea_password = passwordTB.Text;

        try
        {
            int numEffected = tea.updateSpecificTeacher(); ;
            Session["AddTeacher"] = true;
            Response.Redirect("ShowTeacher.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }
    }
    protected void returnBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowTeacher.aspx");
    }


    protected void submitBTN_Click(object sender, EventArgs e)
    {

        double teaId = Convert.ToDouble(idTB.Text);
        Teacher teaAvailability = new Teacher(teaId, Convert.ToInt32(dayDDL.SelectedValue), startTB.Text, endTB.Text);
        int numAffected = teaAvailability.InsertAvailability();
        Session["AddAvailability"] = true;
        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }


    protected void addProBTN_Click(object sender, EventArgs e)
    {
        {
            TeachBy tb;
            int pro_id = Convert.ToInt32(ProfessionDDL.SelectedValue);


            try
            {
                tb = new TeachBy(Convert.ToDouble(idTB.Text), pro_id);
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
                Response.Redirect("EditTeacher.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
            }


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

}