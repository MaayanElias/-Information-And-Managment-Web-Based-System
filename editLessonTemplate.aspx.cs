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

public partial class editLessonTemplate : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["selectedID"] == null)
        {
            Response.Redirect("ShowLesson.aspx");
        }

        int less_id = (int)(Session["selectedID"]);
        if (!Page.IsPostBack)
        {
            GetTeachers();
            professionDDL.Items.Insert(0, "בחר מקצוע ");
            fillDetails(less_id);
          
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
        }
        else
        {
            professionDDL.Items.Insert(0, "מקצוע לא נבחר");
            professionDDL.DataBind();
        }

    }
    private void fillDetails(double less_id)
    {

        Lesson ls = new Lesson();
        DataTable dt = ls.readSpecipicLesson(less_id);
        foreach (DataRow row in dt.Rows)
        {
            dayDDL.SelectedValue = row["Les_day"].ToString();
            startTimeTB.Text = row["Les_StartHour"].ToString();
            endTimeTB.Text = row["Les_EndHour"].ToString();
            teacherDDL.SelectedValue = row["Les_Tea_Id"].ToString();
            professionDDL.SelectedValue = row["Les_Pro_Id"].ToString();
            maxQuanTB.Text = row["Les_MaxQuan"].ToString();
        }
    }

    protected void submitBTN_Click(object sender, EventArgs e)
    {
        Lesson ls = new Lesson();

        ls.Tea_id = Convert.ToDouble(teacherDDL.SelectedValue);
        ls.Pro_id = Convert.ToInt32(professionDDL.SelectedValue);
        ls.Les_maxQuan = Convert.ToInt32(maxQuanTB.Text);
        ls.Les_startHour = startTimeTB.Text;
        ls.Les_endHour = endTimeTB.Text;
        ls.Les_day = Convert.ToInt32(dayDDL.SelectedValue);
        int less_id = (int)(Session["selectedID"]);

        try
        {
            int numEffected = ls.updateSpecificLesson(less_id); ;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirmationModal", "$('#confirmationModal').modal();", true);
        }
        catch (Exception ex)
        {
            Response.Write("There was an error when trying to insert the product into the database" + ex.Message);
        }
    }
}