using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class entitled_list : System.Web.UI.Page
{
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();
    int updateIsEntitledTo;
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

    protected void transferOneBTN_Click(object sender, EventArgs e)
    {

        if (filterTB.Text != null)
            filterTB.Text = "";
        if (allLB.SelectedIndex >= 0)
        {
            for (int i = 0; i < allLB.Items.Count; i++)
            {
                if (allLB.Items[i].Selected)
                {

                    if (!arraylist1.Contains(allLB.Items[i]))
                    {
                        Session["transferBack"] = false;
                        arraylist1.Add(allLB.Items[i]);
                        updateInDB(Convert.ToDouble(allLB.Items[i].Value), updateIsEntitledTo);
                    }
                }
            }
            for (int i = 0; i < arraylist1.Count; i++)
            {
                if (!blackLB.Items.Contains(((ListItem)arraylist1[i])))
                {
                    blackLB.Items.Add(((ListItem)arraylist1[i]));
                }
                allLB.Items.Remove(((ListItem)arraylist1[i]));
            }
            blackLB.SelectedIndex = -1;
        }
        else
        {
            lbltxt.Visible = true;
            lbltxt.Text = "Please select atleast one in Listbox1 to move";
        }
    }



    protected void transferOneBackBTN_Click(object sender, EventArgs e)
    {
        lbltxt.Visible = false;
        if (blackLB.SelectedIndex >= 0)
        {
            for (int i = 0; i < blackLB.Items.Count; i++)
            {
                if (blackLB.Items[i].Selected)
                {
                    if (!arraylist2.Contains(blackLB.Items[i]))
                    {
                        Session["transferBack"] = true;
                        arraylist2.Add(blackLB.Items[i]);
                        updateIsEntitledTo = 1;
                        updateInDB(Convert.ToDouble(blackLB.Items[i].Value), updateIsEntitledTo);
                    }
                }
            }
            for (int i = 0; i < arraylist2.Count; i++)
            {
                if (!allLB.Items.Contains(((ListItem)arraylist2[i])))
                {
                    allLB.Items.Add(((ListItem)arraylist2[i]));
                }
                blackLB.Items.Remove(((ListItem)arraylist2[i]));
            }
            allLB.SelectedIndex = -1;
        }
        else
        {
            lbltxt.Visible = true;
            lbltxt.Text = "Please select atleast one in Listbox2 to move";
        }
    }

    protected void transferAllBackBTN_Click(object sender, EventArgs e)
    {
        updateIsEntitledTo = 1;
        lbltxt.Visible = false;
        while (blackLB.Items.Count != 0)
        {
            for (int i = 0; i < blackLB.Items.Count; i++)
            {
                Session["transferBack"] = true;
                updateInDB(Convert.ToDouble(blackLB.Items[i].Value), updateIsEntitledTo);
                allLB.Items.Add(blackLB.Items[i]);
                blackLB.Items.Remove(blackLB.Items[i]);
            }
        }
    }

    public void updateInDB(double id, int updateIsEntitledTo)
    {
        int countOfmissingForStudent;
        Student stud = new Student();
        if (Convert.ToBoolean(Session["transferBack"]) == true)
        {
            try
            {
                DataTable dt = stud.readSpecipicStudent(id);
                foreach (DataRow row in dt.Rows)
                {
                    countOfmissingForStudent = Convert.ToInt32(row["stu_counter"]);
                    if (countOfmissingForStudent == 5)
                    {
                        stud.updateStu_CounterToZero(id);
                    }

                }

            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to update stu_counter in the database" + ex.Message);

            }

            try
            {
                int numEffected = stud.updateIsEntittled(id, updateIsEntitledTo);
            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to update stu_isEntitled in the database" + ex.Message);
            }
        }

        else
        {
            try
            {
                int numEffected = stud.updateIsEntittled(id, updateIsEntitledTo);
                Request req = new Request();
                int numEf = req.deleteRequests(id); // מחיקת כל הבקשות שלו להרשמה לתגבורים שעוד לא עברו

                SignedToLesson delStu = new SignedToLesson();
                Session["entStu"] = id;
                //נביא את כל התגבורים שבהם התלמיד משתתף ותאריך התגבור עוד לא עבר ונמחק את התלממיד מתגבורים אלה
                DataTable classesForStuIdDT = this.GetClassForStu();
                foreach (DataRow ro in classesForStuIdDT.Rows)
                {
                    DateTime dat = (DateTime)(ro["StLes_ActLesDate"]);
                    if (dat >= DateTime.Now)
                    {
                        string da = dat.ToString("yyyy-MM-dd");
                        int lesNum = Convert.ToInt32(ro["StLes_ActLesId"]);
                        int numEf1 = delStu.deleteStudentFromLesson(id, lesNum, da);
                        ActualLesson actls = new ActualLesson(); 
                        int numEfc = actls.reduceQuan(lesNum, da);
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("There was an error when trying to update stu_isEntitled in the database" + ex.Message);
            }
        }
    }




    private DataTable GetClassForStu()
    {
        double sId = (double)(Session["entStu"]);
        string curDate = (DateTime.Now).ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from signedToLesson where [StLes_stuId]= '" + sId + "' and [StLes_ActLesDate]>='" + curDate + "'";

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