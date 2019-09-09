using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


public partial class ShowActualLesson_admin : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    string day;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Manager UserManager = null;
            if (Session["manUserSession"] != null)
            {
                UserManager = (Manager)(Session["manUserSession"]);
                Session["manObjectSession"] = UserManager;
            }
            else Response.Redirect("default.aspx");

            if (!Page.IsPostBack)
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //Change with your select statement .
                    using (SqlCommand cmd = new SqlCommand("Select * from Lesson le inner join Profession pro on le.Les_Pro_id=pro.Pro_Id inner join Teacher tea on tea.Tea_id=le.Les_Tea_Id", con))
                    {
                        DataTable dt = new DataTable();
                        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                        adpt.Fill(dt);
                        Dictionary<int, string> lst = new Dictionary<int, string>();


                        foreach (DataRow row in dt.Rows)
                        {
                            switch (row[6].ToString())
                            {
                                case "1":
                                    day = "ראשון";
                                    break;
                                case "2":
                                    day = "שני";
                                    break;
                                case "3":
                                    day = "שלישי";
                                    break;
                                case "4":
                                    day = "רביעי";
                                    break;
                                case "5":
                                    day = "חמישי";
                                    break;
                            }
                            //Add values to Dictionary
                            string val = row[8].ToString() + " - " + row[10].ToString() + " " + row[11].ToString() + " - " + day + " - " + row[4].ToString() + " " + row[5].ToString();
                            lst.Add(Convert.ToInt32(row[0]), val);
                        }
                        TigburDDL.DataSource = lst;
                        TigburDDL.DataTextField = "Value";
                        TigburDDL.DataValueField = "key";
                        TigburDDL.DataBind();
                        TigburDDL.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                        TigburDDL.SelectedIndex = 0;
                    }
                }

            }

        }

        bool AddActualLesson = Convert.ToBoolean(Session["AddActualLesson"]);
        if (AddActualLesson)
        {
            int wantedCounter = Convert.ToInt32(Session["wantedCounter"]);
            int lessonCounter = Convert.ToInt32(Session["LessonCounterSession"]);
            if (lessonCounter == wantedCounter)
            {
                if (lessonCounter == 1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('נוסף תגבור אחד למערכת')", true);
                }
                else if (lessonCounter > 1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('נוספו " + lessonCounter + " תגבורים למערכת')", true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.warning('בחר מספר תגבורים להוספה')", true);
                }

            }
            else
            {
                string alert = Convert.ToString(Session["alert"]);
                Response.Write(@"<script language='javascript'>alert('" + alert + "');</script>");

            }

            Session["AddActualLesson"] = false;
        }

    }

    Lesson les = new Lesson();

    private void dayOfActLesson_TextChanged(object sender, EventArgs e)
    {
        Session["day"] = dayOfActLesson.Text;



    }



    protected void generateDate_Click(object sender, EventArgs e)
    {
        Session["AddActualLesson"] = true;
        string dt = Request.Form["DatePickername"];
        int les_id = (int)(Session["LES_ID"]);
        int quantity = 0;
        DateTime start_date = Convert.ToDateTime(dt);
        int amount = Convert.ToInt32(counterTB.Text);
        int day = Convert.ToInt32(dayOfActLesson.Text);

        // נבדוק מי מלמד את התגבור הנבחר ואת השעות של תגבור זה
        DataTable dtTeaID = this.GetSelectedTeaId();
        double teaID = Convert.ToDouble(dtTeaID.Rows[0]["Les_Tea_Id"]);
        DateTime start = DateTime.Parse((dtTeaID.Rows[0]["Les_StartHour"]).ToString());
        DateTime end = DateTime.Parse((dtTeaID.Rows[0]["Les_EndHour"]).ToString());
        Session["selectedteaID"] = teaID;

        //נביא את כל התגבורים (תאריך+שעת התחלה+שעת סיום+מס תגבור) עבור מתגבר זה
        DataTable DTclassForSelectedTeaId = this.GetClassForSelectedTeaId();

        int flg = 0; //מסמן אם יש התנגשות בזמנים

        ActualLesson al;


        DataTable checkTable = new DataTable();
        checkTable.Columns.Add("flg", typeof(int));//מסמן אם ניתן להוסיף תגבור או לא- 1 ניתן 0 לא ניתן

        List<string> problemDatesList = new List<string>();
        List<string> goodDatesList = new List<string>();

        for (int i = 0; i < amount; i++)
        {
            foreach (DataRow dr in DTclassForSelectedTeaId.Rows)
            {
                if (Convert.ToDateTime(dt) == Convert.ToDateTime(dr["ActLes_date"]))
                {
                    DateTime a = DateTime.Parse((dr["Les_StartHour"]).ToString());
                    //אם יש לו תגבור באותו יום, אבל התגבור הנבחר מתחיל ומסתיים לפני התגבורים אחרים שיש לו באותו יום- ניתן לשבץ
                    if (start < DateTime.Parse((dr["Les_StartHour"]).ToString()) && end <= DateTime.Parse((dr["Les_StartHour"]).ToString()))
                    {
                        DataRow newRow = checkTable.NewRow();
                        // Set values in the columnn:
                        newRow["flg"] = 1;
                        // Add the row to the rows collection.
                        checkTable.Rows.Add(newRow);
                    }
                    //אם יש לו תגבור באותו יום, אבל התגבור הנבחר מתחיל אחרי שהתגבורים אחרים שיש לו באותו יום כבר התחילו והסתיימו- ניתן לשבץ
                    else if (start > DateTime.Parse((dr["Les_StartHour"]).ToString()) && start >= DateTime.Parse((dr["Les_EndHour"]).ToString()))
                    {
                        DataRow newRow = checkTable.NewRow();
                        // Set values in the column
                        newRow["flg"] = 1;
                        // Add the row to the rows collection.
                        checkTable.Rows.Add(newRow);
                    }
                    else //כל מצב אחר אומר שיש תגבור למתגבר באותו התאריך וגם השעות מתנגשות
                    {
                        DataRow newRow = checkTable.NewRow();
                        // Set values in the column
                        newRow["flg"] = 0;
                        // Add the row to the rows collection.
                        checkTable.Rows.Add(newRow);
                    }
                }
            }

            if (checkTable.Rows.Count > 0)
            {
                foreach (DataRow dr in checkTable.Rows)
                {
                    if (Convert.ToInt32(dr["flg"]) != 1)
                    {
                        flg = 1;
                        break;//מספיק שלפחות אחד מהתגבורים בתאריך זה מתנגש עם שעות התגבור הנבחר
                    }

                }
                if (flg == 1)
                {
                    DateTime date = Convert.ToDateTime(dt);
                    string d = date.ToString("dd'/'MM'/'yyyy");
                    problemDatesList.Add(d);
                }

                else //אף אחד מהתגבורים שקיימים כבר לא מתנגשים עם התאריך הנבחר
                {
                    al = new ActualLesson(les_id, dt, quantity, day);
                    al.insertActualLesson(); ;
                    DateTime date2 = Convert.ToDateTime(dt);
                    string good = date2.ToString("dd'/'MM'/'yyyy");
                    goodDatesList.Add(good);
                }

            }

            else //אם אין שורות בטבלה המשמעות שאין למתגבר תגבורים אחרים באותו יום ולכן אין בעיית שיבוץ וניתן להוסיף את התגבור
            {
                al = new ActualLesson(les_id, dt, quantity, day);
                al.insertActualLesson();



            }

            start_date = start_date.AddDays(7);
            dt = start_date.ToString("yyyy-MM-dd");

            checkTable.Clear();
            flg = 0;
        }

        string goodAlert = "";
        if (goodDatesList.Count >= 1)
        {

            if (goodDatesList.Count == 1)
            {
                goodAlert = "המופע שהתווסף לתגבור הנבחר חל בתאריך: " + goodDatesList[0] + ".";
            }
            else
            {
                goodAlert = "המופעים שהתווספו לתגבור הנבחר חלים בתאריכים הבאים: ";

                for (int j = 0; j < goodDatesList.Count; j++)
                {
                    if (j != (goodDatesList.Count - 1))
                        goodAlert += (goodDatesList[j] + ", ");
                    else
                        goodAlert += (goodDatesList[j] + ".");
                }
            }

            Session["goodAlert"] = goodAlert;
        }


        if (problemDatesList.Count >= 1)
        {
            string alert;
            if (problemDatesList.Count == 1)
            {
                alert = "לא ניתן להוסיף למערכת את תבנית התגבור שנבחרה בתאריך הבא: " + problemDatesList[0] + ". המתגבר משובץ לתגבור אחר בשעות התגבור שבחרת";
            }
            else
            {
                alert = "לא ניתן להוסיף את תבנית התגבור שנבחרה בתאריכים הבאים: ";

                for (int j = 0; j < problemDatesList.Count; j++)
                {
                    if (j != (problemDatesList.Count - 1))
                        alert += (problemDatesList[j] + ", ");
                    else
                        alert += (problemDatesList[j] + ".תהמתגבר משובץ כבר לתגבור אחר בתאריכים אלה בשעות התגבור שבחרת. ");
                }
            }
            if (Session["goodAlert"] != null) { alert += Convert.ToString(Session["goodAlert"]); }
            Session["alert"] = alert;
            //Response.Write(@"<script language='javascript'>alert('" + alert + "');</script>");
        }

        int lessonCounter = goodDatesList.Count;
        string wantedCounter = counterTB.Text;
        Session["LessonCounterSession"] = goodDatesList.Count;
        Session["wantedCounter"] = wantedCounter;
        Response.Redirect("ShowActualLesson_admin.aspx");
    }

    protected void TigburDDL_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LES_ID"] = Convert.ToInt32(TigburDDL.SelectedValue);
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int cancelled = 1;
        ActualLesson acl = new ActualLesson();
        SignedToLesson stl = new SignedToLesson();
        int lesId = Convert.ToInt32(gvRow.Cells[1].Text);
        DateTime tmpDate = Convert.ToDateTime(gvRow.Cells[3].Text);
        string lesDate = tmpDate.ToString("yyyy-MM-dd");
        Request stuReq = new Request();

        string proff = gvRow.Cells[2].Text;
        string start = gvRow.Cells[4].Text;
        Session["cancelledLesId"] = lesId;
        Session["cancelledLesDate"] = lesDate;
        string lesDateMES = tmpDate.ToString("dd-MM-yyyy");
        Manager m = (Manager)(Session["manUserSession"]);
        double manID = m.Man_id;
        DataTable dt = this.GetStudents();
        string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");

        Messages mes;
        string mesContent = "תלמיד יקר, שיעור " + proff + " שחל בתאריך " + lesDateMES + " ובשעה " + start + " בוטל.";
        foreach (DataRow dr in dt.Rows)
        {
            double stuID = Convert.ToDouble(dr["StLes_stuId"]);

            mes = new Messages(manID, stuID, "ביטול שיעור", mesContent, false, currnetDate);
            int NumEffected = mes.InsertMessage();
        }

        DataTable teaIDtable = this.GetTeaId();
        double teaID = Convert.ToDouble(teaIDtable.Rows[0]["Les_Tea_Id"]);

        string mesToTeaContent = "מתגבר יקר, שיעור " + proff + " שחל בתאריך " + lesDateMES + " ובשעה " + start + " בוטל.";
        Messages TeaMes = new Messages(manID, teaID, "ביטול שיעור", mesToTeaContent, false, currnetDate);
        int numEffected1 = TeaMes.InsertMessageFromManagerToTeacher();

        try
        {
            int numEffected2 = acl.cancelSpecificActualLesson(lesId, lesDate, cancelled); //עדכון שהתגבור בוטל
            int numEffected3 = stl.deleteStudentsFromLesson(lesId, lesDate);//מחיקת התלמידים שמשתתפים בתגבור זה בטבלת "רשום לתגבור"
            int numEffected4 = stuReq.deleteCancelldLessonRequests(lesId, lesDate);//מחיקת כל הבקשות להרשמה לתגבור זה
            Server.TransferRequest(Request.Url.AbsolutePath, false);
        }

        catch (Exception ex)
        {
            Response.Write("illegal values to the SignedToLesson attributes - error message is " + ex.Message);
            return;
        }
    }


    private DataTable GetStudents()//להביא את הסטודנטים שמשתתפים בתגבור שבוטל ע"מ לשלוח להם הודעה על ביטול התגבור
    {
        int lesId = Convert.ToInt32(Session["cancelledLesId"]);
        string lesDate = Convert.ToString(Session["cancelledLesDate"]);

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select * from signedToLesson where StLes_ActLesId= " + lesId + " and StLes_ActLesDate=" + "'" + lesDate + "'";

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

    private DataTable GetTeaId()//מביא את ת"ז המתגבר שמעביר את השיעור שבוטל ע"מ לעדכן אותו בביטול
    {
        int lesId = Convert.ToInt32(Session["cancelledLesId"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Tea_Id from Lesson where Les_Id= '" + lesId + "'";

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

    private DataTable GetSelectedTeaId()//מביא את ת"ז המתגברשמלמד את התבנית הנבחרת בDDL
    {
        int SelectedlesId = Convert.ToInt32(Session["LES_ID"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Tea_Id,Les_StartHour,Les_EndHour from Lesson where Les_Id= '" + SelectedlesId + "'";

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


    private DataTable GetClassForSelectedTeaId()//מביא את ת"ז המתגברשמלמד את התבנית הנבחרת בDDL
    {
        double selectedteaID = Convert.ToDouble(Session["selectedteaID"]);
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Id,Les_Tea_Id,ActLes_date, Les_StartHour, Les_EndHour from ActualLesson inner join Lesson on ActLes_LesId=Les_Id where actls_cancelled=0 AND Les_Tea_Id= '" + selectedteaID + "' order by ActLes_date";

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




    //private static void ReadOrderData(string connectionString)
    //{
    //    string queryString =
    //        "SELECT OrderID, CustomerID FROM dbo.Orders;";

    //    using (SqlConnection connection = new SqlConnection(connectionString))
    //    using (SqlCommand command = new SqlCommand(queryString, connection))
    //    {
    //        connection.Open();

    //        using (SqlDataReader reader = command.ExecuteReader())
    //        {
    //            // Call Read before accessing data.
    //            while (reader.Read())
    //            {
    //                Console.WriteLine(String.Format("{0}, {1}",
    //                reader[0], reader[1]));
    //            }
    //        }
    //    }
    //}


}