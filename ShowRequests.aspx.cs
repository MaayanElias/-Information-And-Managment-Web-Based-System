using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class ShowRequests : System.Web.UI.Page
{
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

            string id = Request.QueryString["id"];
            if (id == "1")
                statusDDL.SelectedValue = "2";

            statusDDL.ClearSelection(); //making sure the previous selection has been cleared
            statusDDL.Items.FindByValue("2").Selected = true;
        }
        if (Session["manUserSession"] == null)
        {
            Response.Redirect("default.aspx");
        }


        bool declineReq = Convert.ToBoolean(Session["declineReq"]);
        if (declineReq)
        {
            Session["declineReq"] = false;
            Response.Write(@"<script language='javascript'>alert('לא ניתן לאשר את הבקשה עבור תלמיד זה. ישנה התנגשות בין התגבור המבוקש לבין תגבור אחר שהוא משובץ אליו. נשלחה הודעה לתלמיד');</script>");

        }

    }

    protected void reqGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[8].Text == "1")
                e.Row.Cells[8].Text = "א'";
            else if (e.Row.Cells[8].Text == "2")
                e.Row.Cells[8].Text = "ב'";
            else if (e.Row.Cells[8].Text == "3")
                e.Row.Cells[8].Text = "ג'";
            else if (e.Row.Cells[8].Text == "4")
                e.Row.Cells[8].Text = "ד'";
            else // יום חמישי 
                e.Row.Cells[8].Text = "ה'";


            if (e.Row.Cells[14].Text == "1")
                e.Row.Cells[14].Text = "אושרה";

            else if (e.Row.Cells[14].Text == "0")
                e.Row.Cells[14].Text = "נדחתה";

            else
                e.Row.Cells[14].Text = "ממתינה";


            if (e.Row.Cells[2].Text == "0")
                e.Row.Cells[2].Text = "הרשמה לתגבור";

            else if (e.Row.Cells[2].Text == "1")
                e.Row.Cells[2].Text = "ביטול השתתפות";
            else
                e.Row.Cells[2].Text = "";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[7].Text.IndexOf(" ");
            e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, index);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[14].Text != "ממתינה")
            {
                Button statusAp = (Button)e.Row.FindControl("AproveButton");
                statusAp.Visible = false;
                Button statusDe = (Button)e.Row.FindControl("DeclineButton");
                statusDe.Visible = false;


            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text != "ביטול השתתפות")
            {
                if ((Convert.ToInt32(e.Row.Cells[13].Text) == (Convert.ToInt32(e.Row.Cells[12].Text))) && e.Row.Cells[14].Text == "ממתינה")
                {
                    Server.TransferRequest(Request.Url.AbsolutePath, false);

                    ActualLesson acl = new ActualLesson();
                    double stuId = Convert.ToDouble(e.Row.Cells[3].Text);
                    int lesId = Convert.ToInt32(e.Row.Cells[5].Text);
                    DateTime tmpDate = Convert.ToDateTime(e.Row.Cells[7].Text);
                    string lesDate = tmpDate.ToString("yyyy-MM-dd");
                    Request req = new Request();

                    int req_num = Convert.ToInt32(e.Row.Cells[1].Text);

                    Session["lessonId"] = lesId;
                    Session["lessonDate"] = lesDate;
                    string proffTitle = e.Row.Cells[6].Text;
                    Session["proffTitle"] = proffTitle;

                    string LesDateFormat = tmpDate.ToString("dd/MM/yyyy");
                    string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
                    Messages mes;
                    Manager m = (Manager)(Session["manUserSession"]);
                    double manID = m.Man_id;
                    string selectedLesSHour = e.Row.Cells[9].Text;

                    DataTable dt = this.GetAlternativeActualLesson();
                    if (dt != null)
                    {
                        //first alternative
                        //string firstAlternativeProff = Convert.ToString(dt.Rows[0]["Pro_Title"]);
                        if (dt.Rows.Count == 2)
                        {
                            DateTime tempFirstAlternativeDate = Convert.ToDateTime(dt.Rows[0]["actLes_date"]);
                            string firstAlternativeDate = tempFirstAlternativeDate.ToString("dd/MM/yyyy");
                            string firstAlternativeSHour = Convert.ToString(dt.Rows[0]["Les_StartHour"]);
                            //string firstAlternativeEHour = Convert.ToString(dt.Rows[0]["Les_EndHour"]);
                            int firstAlternativeIntDay = Convert.ToInt32(dt.Rows[0]["Les_day"]);
                            string firstAlternativeDay;
                            if (firstAlternativeIntDay == 1) firstAlternativeDay = "ראשון";
                            else if (firstAlternativeIntDay == 2) firstAlternativeDay = "שני";
                            else if (firstAlternativeIntDay == 3) firstAlternativeDay = "שלישי";
                            else if (firstAlternativeIntDay == 4) firstAlternativeDay = "רביעי";
                            else firstAlternativeDay = "חמישי";

                            //second alternative
                            //string secondAlternativeProff = Convert.ToString(dt.Rows[1]["Pro_Title"]);//לא צריך כי אותה מספר תגבור זה אותו מזהה מקצוע והשאילתא לפי מספר תגבור
                            DateTime tempSecondAlternativeDate = Convert.ToDateTime(dt.Rows[1]["actLes_date"]);
                            string secondAlternativeDate = tempSecondAlternativeDate.ToString("dd/MM/yyyy");
                            string secondAlternativeSHour = Convert.ToString(dt.Rows[1]["Les_StartHour"]);
                            //string secondAlternativeEHour = Convert.ToString(dt.Rows[1]["Les_EndHour"]);
                            int secondAlternativeIntDay = Convert.ToInt32(dt.Rows[1]["Les_day"]);
                            string secondAlternativeDay;
                            if (secondAlternativeIntDay == 1) secondAlternativeDay = "ראשון";
                            else if (secondAlternativeIntDay == 2) secondAlternativeDay = "שני";
                            else if (secondAlternativeIntDay == 3) secondAlternativeDay = "שלישי";
                            else if (secondAlternativeIntDay == 4) secondAlternativeDay = "רביעי";
                            else secondAlternativeDay = "חמישי";


                            //string mesContent = "תלמיד יקר, שיעור " + proffTitle + " שחל בתאריך " + LesDateFormat + ", בשעה " + selectedLesSHour + " מלא. באפשרותך להירשם לאחד המתגבורים הבאים: " + "<br><br>" + proffTitle + " " + firstAlternativeDate + ", יום " + firstAlternativeDay + ", משעה " + firstAlternativeSHour + " עד " + firstAlternativeEHour + "<br>" + proffTitle + " " + secondAlternativeDate + ", יום " + secondAlternativeDay + " משעה " + secondAlternativeSHour + " עד " + secondAlternativeEHour;
                            string mesContent = "תלמיד יקר, שיעור " + proffTitle + " שחל בתאריך " + LesDateFormat + ", בשעה " + selectedLesSHour + " מלא. באפשרותך להירשם לאחד המתגבורים הבאים: " + "<br><br>" + proffTitle + " " + firstAlternativeDate + ", יום " + firstAlternativeDay + ", בשעה " + firstAlternativeSHour + "<br>" + proffTitle + " " + secondAlternativeDate + ", יום " + secondAlternativeDay + " בשעה " + secondAlternativeSHour;
                            mes = new Messages(manID, stuId, "בקשתך להירשם לתגבור נדחתה", mesContent, false, currnetDate);
                            int NumEffected = mes.InsertMessage();
                        }

                        else if (dt.Rows.Count == 1)
                        {
                            DateTime tempFirstAlternativeDate = Convert.ToDateTime(dt.Rows[0]["actLes_date"]);
                            string firstAlternativeDate = tempFirstAlternativeDate.ToString("dd/MM/yyyy");
                            string firstAlternativeSHour = Convert.ToString(dt.Rows[0]["Les_StartHour"]);
                            string firstAlternativeEHour = Convert.ToString(dt.Rows[0]["Les_EndHour"]);
                            int firstAlternativeIntDay = Convert.ToInt32(dt.Rows[0]["Les_day"]);
                            string firstAlternativeDay;
                            if (firstAlternativeIntDay == 1) firstAlternativeDay = "ראשון";
                            else if (firstAlternativeIntDay == 2) firstAlternativeDay = "שני";
                            else if (firstAlternativeIntDay == 3) firstAlternativeDay = "שלישי";
                            else if (firstAlternativeIntDay == 4) firstAlternativeDay = "רביעי";
                            else firstAlternativeDay = "חמישי";

                            string mesContent = "תלמיד יקר, שיעור " + proffTitle + " שחל בתאריך " + LesDateFormat + ", בשעה " + selectedLesSHour + " מלא. באפשרותך להירשם לתגבור הבא: " + proffTitle + " שחל בתאריך " + firstAlternativeDate + ", ביום " + firstAlternativeDay + " החל מהשעה " + firstAlternativeSHour + " ועד שעה " + firstAlternativeEHour;
                            mes = new Messages(manID, stuId, "בקשתך להירשם לתגבור נדחתה", mesContent, false, currnetDate);
                            int NumEffected = mes.InsertMessage();
                        }

                    }
                    else//אם אין תגבורים מתאימים (בעתיד) כרגע
                    {
                        string mesContent = "תלמיד יקר, שיעור " + proffTitle + " שחל בתאריך " + LesDateFormat + ", בשעה " + selectedLesSHour + " מלא ולכן לא ניתן להירשם לתגבור. עמך הסליחה";
                        mes = new Messages(manID, stuId, "בקשתך להירשם לתגבור נדחתה", mesContent, false, currnetDate);
                        int NumEffected = mes.InsertMessage();

                    }
                    int status = 0;
                    int decFull = 1;
                    int numEffected = req.updateSpecificRequest(req_num, status);
                    int numEff = req.updateDecfullField(req_num, decFull);

                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime currentDate = DateTime.Now.Date;
            DateTime lesDate = Convert.ToDateTime(e.Row.Cells[7].Text);
            if (lesDate < currentDate && e.Row.Cells[14].Text == "ממתינה")
            {
                int reqId = Convert.ToInt32(e.Row.Cells[1].Text);
                Request req = new Request();
                int status = 0; // נדחה אוטומטית בקשות ממתינות לתגבור שכבר עבר
                int numEffected = req.updateSpecificRequest(reqId, status);
            }

            //if (lesDate < currentDate)// לא נציג בקשות לתגבור שכבר עבר
            //{
            //    e.Row.Visible = false;
            //}

        }

    }



    protected void ApproveButton_Click(object sender, EventArgs e)
    {Session["flgSendReq"] = "1";
        Request req = new Request();
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int req_num = Convert.ToInt32(gvRow.Cells[1].Text);

        ActualLesson acl = new ActualLesson();
        SignedToLesson stl;
        double stuId = Convert.ToDouble(gvRow.Cells[3].Text);
        int lesId = Convert.ToInt32(gvRow.Cells[5].Text);
        DateTime tmpDate = Convert.ToDateTime(gvRow.Cells[7].Text);
        string lesDate = tmpDate.ToString("yyyy-MM-dd");

        int req_type = 1;
        string req_typeName = gvRow.Cells[2].Text;
        if (req_typeName != "ביטול השתתפות")
        {
            req_type = 0;
        }
        else //נאשר בקשה לביטול השתתפות- נשנה סטטוס בקשה ל-1
        {
            int status = 1;
            int numEffected = req.updateSpecificRequest(req_num, status);

            try
            {
                stl = new SignedToLesson(lesId, lesDate, stuId);
            }
            catch (Exception ex)
            {
                Response.Write("illegal values to the SignedToLesson attributes - error message is " + ex.Message);
                return;
            }

            int actual_quan = Convert.ToInt32(gvRow.Cells[13].Text) - 1;

            try
            {
                int numEffected3 = acl.updateSpecificActualLesson(lesId, lesDate, actual_quan); //הקטנת כמות משתתפים ב-1
                int numEffected2 = stl.deleteStudentFromLesson(stuId, lesId, lesDate); //הוצאת התלמיד מטבלת "רשום לתגבור"
                Server.TransferRequest(Request.Url.AbsolutePath, false);
            }
            catch (Exception ex)
            {
                Response.Write("There was an error " + ex.Message);
            }
        }

        int flg = 0; //מסמן שאין התנגשות בין תגבור מבוקש לבין תגבור אחר כלשהו שהתלמיד משובץ אליו
        DataTable checkTable = new DataTable();
        checkTable.Columns.Add("flg", typeof(int));//מסמן אם ניתן להוסיף תגבור או לא- 1 ניתן 0 לא ניתן

        if (req_type != 1) //אם זו בקשה להרשמה (זו לא בקשה לביטול השתתפות) 
        {

            //ניקח את שעות התגבור המבוקש ע"מ לראות אם באותם שעות בתאריך התגבור, התלמיד משובץ לתגבור אחר
            DateTime reqLesStart = DateTime.Parse((gvRow.Cells[9].Text).ToString());
            DateTime reqLesEnd = DateTime.Parse((gvRow.Cells[10].Text).ToString());

            Session["reqStuId"] = stuId;
            Session["reqLesDate"] = lesDate;

            //נביא את כל התגבורים (תאריך+שעת התחלה+שעת סיום+מס תגבור) עבור מתגבר זה
            DataTable DTclassForReqStuId = this.GetClassForReqStuId();
            if (DTclassForReqStuId.Rows.Count > 0)//המשמעות שהתלמיד המבקש משובץ לתגבורים אחרים בתאריך המבוקש- נבדוק התנגשות בשעות
            {
                foreach (DataRow dr in DTclassForReqStuId.Rows)
                {
                    //אם יש לו תגבור באותו יום, אבל התגבור הנבחר מתחיל ומסתיים לפני התגבורים אחרים שיש לו באותו יום- ניתן לשבץ
                    if (reqLesStart < DateTime.Parse((dr["Les_StartHour"]).ToString()) && reqLesEnd <= DateTime.Parse((dr["Les_StartHour"]).ToString()))
                    {
                        DataRow newRow = checkTable.NewRow();
                        // Set values in the columnn:
                        newRow["flg"] = 1;
                        // Add the row to the rows collection.
                        checkTable.Rows.Add(newRow);
                    }
                    //אם יש לו תגבור באותו יום, אבל התגבור הנבחר מתחיל אחרי שהתגבורים אחרים שיש לו באותו יום כבר התחילו והסתיימו- ניתן לשבץ
                    else if (reqLesStart > DateTime.Parse((dr["Les_StartHour"]).ToString()) && reqLesStart >= DateTime.Parse((dr["Les_EndHour"]).ToString()))
                    {
                        DataRow newRow = checkTable.NewRow();
                        // Set values in the column
                        newRow["flg"] = 1;
                        // Add the row to the rows collection.
                        checkTable.Rows.Add(newRow);
                    }
                    else //כל מצב אחר המשמעות שיש תגבור לתלמיד באותו התאריך וגם השעות מתנגשות
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
            }

            if (flg == 0) //אין התנגשות, אז נבדוק האם יש מקום בתגבור ואם כן נאשר
            {
                if (Convert.ToInt32(gvRow.Cells[13].Text) < (Convert.ToInt32(gvRow.Cells[12].Text)))//אם יש מקום בתגבור הבקשה תאושר והתלמיד נרשם לתגבור
                {
                    
                    int status = 1;
                    int numEffected = req.updateSpecificRequest(req_num, status);

                    try
                    {
                        stl = new SignedToLesson(lesId, lesDate, stuId);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("illegal values to the SignedToLesson attributes - error message is " + ex.Message);
                        return;
                    }

                    int actual_quan = Convert.ToInt32(gvRow.Cells[13].Text) + 1;



                    try
                    {
                        int numEffected3 = acl.updateSpecificActualLesson(lesId, lesDate, actual_quan); //הגדלת כמות משתתפים ב-1
                        int numEffected2 = stl.InsertSigendToLesson(); //הכנסת התלמיד לטבלת "רשום לתגבור"
                        Server.TransferRequest(Request.Url.AbsolutePath, false);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("There was an error " + ex.Message);
                    }

                }
            }
            else //התלמיד משובץ כבר לתגבור אחר שמתנגש עם התגבור המבוקש- נדחה את הבקשה עם סיבת דחייה לתלמיד ואלרט למנהלת
            {
                int status = 0;
                int Decliend = 1;
                int numEffected = req.updateSpecificRequest(req_num, status); //נעדכן שהבקשה נדחתה
                int numEffected4 = req.updateReqDecliendField(req_num, Decliend);//נעדכן שהסיבה לדחיית הבקשה היא "אילוץ" ההתנגשות


                string proffTitle = gvRow.Cells[6].Text;
                string start = gvRow.Cells[9].Text;
                string LesDateFormat = tmpDate.ToString("dd/MM/yyyy");
                string currnetDate = DateTime.Now.ToString("yyyy-MM-dd");
                Messages mes;
                Manager m = (Manager)(Session["manUserSession"]);
                double manID = m.Man_id;
   

                string mesContent = "תלמיד יקר, שיעור " + proffTitle + " שחל בתאריך " + LesDateFormat + ", בשעה " + reqLesStart.ToString("HH:mm") + " מתנגש עם תגבור אחר שאתה משובץ אליו בתאריך זה";
                mes = new Messages(manID, stuId, "בקשתך להירשם לתגבור נדחתה", mesContent, false, currnetDate);
                int NumEffected = mes.InsertMessage();

                
                Session["declineReq"] = true;
                Server.TransferRequest(Request.Url.AbsolutePath, false);
            }

        }
    }


    protected void DeclineButton_Click(object sender, EventArgs e)
    {
        Request req = new Request();
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        int req_num = Convert.ToInt32(gvRow.Cells[1].Text);
        int status = 0;
        int numEffected = req.updateSpecificRequest(req_num, status);
        //Response.Redirect("ShowRequests.aspx");
        Server.TransferRequest(Request.Url.AbsolutePath, false);

    }

    private DataTable GetAlternativeActualLesson()
    {
        string lessomProff = Convert.ToString(Session["proffTitle"]);
        string lessonDate = Convert.ToString(Session["lessonDate"]);

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select top 2 ActLes_date,Pro_Title,Les_day,Les_StartHour,Les_EndHour from ActualLesson inner join Lesson on ActLes_LesId=Les_Id inner join Profession on Pro_Id= Les_Pro_Id where ActLes_date >='" + lessonDate + "' and Pro_Title='" + lessomProff + "' and actls_cancelled=0 and quantity<Les_MaxQuan order by ActLes_date ";
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





    private DataTable GetClassForReqStuId()//הבאת כל התגבורים שהתלמיד שהגיש בקשה משתתף בהם, באותו תאריך של התגבור המבוקש
    {
        double reqStuId = Convert.ToDouble(Session["reqStuId"]);
        string reqLesDate = (Session["reqLesDate"]).ToString();
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["studentDBConnectionString"].ConnectionString;
        string sql = "select Les_Id, StLes_stuId, ActLes_date, Les_StartHour, Les_EndHour from signedToLesson inner join ActualLesson on ActLes_date= StLes_ActLesDate and ActLes_LesId = StLes_ActLesId inner join lesson on ActLes_LesId = Les_Id where StLes_ActLesDate = '" + reqLesDate + "' and StLes_stuId = '" + reqStuId + "'";
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




