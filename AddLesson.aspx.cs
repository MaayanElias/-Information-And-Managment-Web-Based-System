using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Globalization;
public partial class AddLesson : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["manUserSession"] == null)
            {
                Response.Redirect("default.aspx");
            }

            GetTeachers();
            professionDDL.Items.Insert(0, "בחר מקצוע ");
            dayDDL.Items.Insert(0, "בחר יום בשבוע ");

        }
    }

    private void GetTeachers()
    {
        query = "select tea_id, (Tea_FirstName + ' '  + Tea_LastName + ' - ' + cast(Tea_Id as nvarchar (255))) as 'fullName'  from Teacher";
        da = new SqlDataAdapter(query, con);

        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            teacherDDL.DataSource = ds;
            teacherDDL.DataTextField = "fullName";
            teacherDDL.DataValueField = "tea_id";
            teacherDDL.DataBind();
            teacherDDL.Items.Insert(0, new ListItem(" בחר מתגבר ", "0"));
            teacherDDL.SelectedIndex = 0;
        }
    }



    protected void teacherDDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        ds.Clear();
        string get_teacherName, teacherName;
        teacherName = teacherDDL.SelectedItem.Text;
        get_teacherName = teacherDDL.SelectedValue.ToString();

        double selectedteaId = Convert.ToDouble(get_teacherName);
        Session["selectedteaId"] = selectedteaId;

        if (get_teacherName != "0")
        {
            query = "Select pro.pro_id, pro.pro_title from Profession pro inner join TeachBy tb on pro.Pro_Id= tb.pro_id where tb.tea_id='" + get_teacherName.ToString() + "'";
            da = new SqlDataAdapter(query, con);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                professionDDL.DataSource = ds;
                professionDDL.DataTextField = "pro_title";
                professionDDL.DataValueField = "pro_id";
                professionDDL.DataBind();
                professionDDL.Items.Insert(0, new ListItem("בחר מקצוע", "0"));
                professionDDL.SelectedIndex = 0;
            }

            string query2 = "Select Ave_day from TeacheravAilability where Ave_TeaId='" + get_teacherName + "'";
            da = new SqlDataAdapter(query2, con);
            da.Fill(ds2);


            if (ds2.Tables[0].Rows.Count > 0)
            {
                dayDDL.DataSource = ds2;
                dayDDL.DataTextField = "Ave_day";
                dayDDL.DataValueField = "Ave_day";
                dayDDL.DataBind();
                foreach (ListItem li in dayDDL.Items)
                {
                    if (li.Value == "1")
                    {
                        li.Text = "א'";
                    }
                    else if (li.Value == "2")
                    {
                        li.Text = "ב'";
                    }
                    else if (li.Value == "3")
                    {
                        li.Text = "ג'";
                    }
                    else if (li.Value == "4")
                    {
                        li.Text = "ד'";
                    }
                    else
                    {
                        li.Text = "ה'";
                    }
                }
                dayDDL.Items.Insert(0, new ListItem("בחר יום בשבוע", "0"));
                dayDDL.SelectedIndex = 0;


            }
            else
            {
                //professionDDL.Items.Insert(0, " מקצוע לא נבחר ");
                //professionDDL.DataBind();
                dayDDL.Items.Clear();
                dayDDL.Items.Insert(0, "בחר יום בשבוע");
                dayDDL.Items.Insert(1, "א'");
                dayDDL.Items.Insert(2, "ב'");
                dayDDL.Items.Insert(3, "ג'");
                dayDDL.Items.Insert(4, "ד'");
                dayDDL.Items.Insert(5, "ה'");
                dayDDL.DataBind();

            }

        }

    }

    protected void submitBTN_Click(object sender, EventArgs e)
    {
        Lesson les;
        int dayNum;
        if (dayDDL.SelectedItem.Text == "א'")
        {
            dayNum = 1;
        }
        else if (dayDDL.SelectedItem.Text == "ב'")
        {
            dayNum = 2;
        }
        else if (dayDDL.SelectedItem.Text == "ג'")
        {
            dayNum = 3;
        }
        else if (dayDDL.SelectedItem.Text == "ד'")
        {
            dayNum = 4;
        }
        else
        {
            dayNum = 5;
        }

        try
        {
            double teacheId = Convert.ToDouble(teacherDDL.SelectedValue);
            int proId = Convert.ToInt32(professionDDL.SelectedValue);
            les = new Lesson(teacheId, proId, Convert.ToInt32(maxQuanTB.Text), (startTimeTB.Text), (endTimeTB.Text), dayNum);
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('אנא מלא את כל הפרטים')", true);
            return;
        }

        DataTable availabiltyDT = this.GetTeachersAvailability();
        if (availabiltyDT != null)
        {
            double selectedTeaID = (double)(Session["selectedteaId"]);
            int selectedDay = Convert.ToInt32(Session["selectedDay"]);
            string selectedStartHour = startTimeTB.Text;
            string selectedEndHour = endTimeTB.Text;
            DateTime selectedStartHourDT = DateTime.ParseExact(selectedStartHour, "HH:mm", CultureInfo.InvariantCulture);
            DateTime selectedEndHourDT = DateTime.ParseExact(selectedEndHour, "HH:mm", CultureInfo.InvariantCulture);
            foreach (DataRow dr in availabiltyDT.Rows)
            {
                double teacherID = Convert.ToDouble(dr["Ave_TeaId"]);
                int day = Convert.ToInt32(dr["Ave_day"]);
                string startHour = Convert.ToString(dr["Ave_startHour"]);
                string endHour = Convert.ToString(dr["Ave_endtHour"]);
                DateTime startHourDT = DateTime.ParseExact(startHour, "HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime startEndDT = DateTime.ParseExact(endHour, "HH:mm:ss", CultureInfo.InvariantCulture);

                if (selectedTeaID == teacherID && selectedDay == day && selectedStartHourDT >= startHourDT && selectedEndHourDT <= startEndDT)
                {
                    try
                    {
                        int numEffected = les.InsertLesson();
                        Session["AddLesson"] = true;
                        Response.Redirect("ShowLesson.aspx");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);
                    }
                }

                else
                {
                    Response.Write(@"<script language='javascript'>alert('מתגבר זה אינו זמין בשעות שהזנת ביום הנבחר');</script>");
                }
            }
        }

        else
        {
            try
            {
                int numEffected = les.InsertLesson();
                Session["AddLesson"] = true;
                Response.Redirect("ShowLesson.aspx");

            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to Insert the student into the database" + ex.Message);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.error('קיימת שגיאה בשרת. אנא נסה שנית')", true);

            }
        }
    }


    protected void dayDDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedDay;
        if (dayDDL.SelectedItem.Text == "א'")
        { 
            selectedDay = 1;
        }
        else if (dayDDL.SelectedItem.Text == "ב'")
        {
            selectedDay = 2;
        }
        else if (dayDDL.SelectedItem.Text == "ג'")
        {
            selectedDay = 3;
        }
        else if (dayDDL.SelectedItem.Text == "ד'")
        {
            selectedDay = 4;
        }
        else
        {
            selectedDay = 5;
        }
        Session["selectedDay"] = selectedDay;
    }

    private DataTable GetTeachersAvailability()
    {
        double teaId = Convert.ToDouble(Session["selectedteaId"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "Select * from TeacheravAilability where Ave_TeaId='" + teaId + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }
        return dt;
    }
}
