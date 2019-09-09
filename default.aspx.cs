using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class @default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                usernameTB.Text = Request.Cookies["UserName"].Value;
                passwordTB.Attributes["value"] = Request.Cookies["Password"].Value;
                rememberCB.Checked = true;
            }
        }
    }

    protected void loginBTN_Click(object sender, EventArgs e)
    {
        string username_id = usernameTB.Text;
        string password = passwordTB.Text;

        if (rememberCB.Checked == true)
        {
            //נשמור בקוקיז את שם המשתמש והסיסמה
            HttpCookie myCookie = new HttpCookie("UserName");
            Response.Cookies["UserName"].Value = username_id;
            Response.Cookies["UserName"].Expires = DateTime.Now.AddHours(24);

            HttpCookie myCookie2 = new HttpCookie("Password");
            Response.Cookies["Password"].Value = password;
            Response.Cookies["Password"].Expires = DateTime.Now.AddHours(24);
        }

        Student stu = new Student();//אם זה תלמיד
        Student getStuObj = stu.readSpecificUserStudent(Convert.ToDouble(username_id), password);

        Manager mngr = new Manager();// אם זה מנהל
        Manager getManObj = mngr.readSpecificUserManager(Convert.ToDouble(username_id), password);

        Teacher tea = new Teacher();// אם זה מתגבר
        Teacher getTeaObj = tea.readSpecificUserTeacher(Convert.ToDouble(username_id), password);

        if (getStuObj != null) //לכניסה של תלמיד
        {

            if (getStuObj.Status == false) // אם הוא משתמש לא פעיל
            {
                Label errorMem = new Label();
                errorMem.Text = "הסטטוס של משתמש:  " + getStuObj.FirstName + " " + getStuObj.LastName + " אינו פעיל ולכן לא יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
            }
            else
            {
                Session["stuUserSession"] = getStuObj;
                Label errorMem = new Label();
                errorMem.Text = getStuObj.FirstName + " " + getStuObj.LastName + " יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
                Response.Redirect("student_dashboard.aspx");
            }
        }


        else if (getManObj != null) //לכניסה של מנהל
        {

            if (getManObj.Man_status == false)// אם הוא משתמש לא פעיל
            {
                Label errorMem = new Label();
                errorMem.Text = "הסטטוס של משתמש:  " + getManObj.Man_firstName + " " + getManObj.Man_lastName + " אינו פעיל ולכן לא יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
            }
            else
            {
                Session["manUserSession"] = getManObj;
                Label errorMem = new Label();
                errorMem.Text = "יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
                Response.Redirect("admin_dashboard.aspx");
            }
        }
        else if (getTeaObj != null) //לכניסה של מתגבר
        {

            if (getTeaObj.Tea_status == false)// אם הוא משתמש לא פעיל
            {
                Label errorMem = new Label();
                errorMem.Text = "הסטטוס של משתמש:  " + getTeaObj.Tea_firstName + " " + getTeaObj.Tea_lastName + " אינו פעיל ולכן לא יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
            }
            else
            {
                Session["teaUserSession"] = getTeaObj;
                Label errorMem = new Label();
                errorMem.Text = "יכול להיכנס למערכת";
                LabelPH.Controls.Add(errorMem);
                Response.Redirect("teacher_dashboard.aspx");
            }
        }
        else //אם הוא לא רשום
        {
            Label errorMem = new Label();
            errorMem.Text = "סליחה, שם המשתמש ו/או הסיסמה שהזנת לא קיימים במערכת.";
            LabelPH.Controls.Add(errorMem);
        }

    }
}