using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Student UserStudent = null;
            if (Session["stuUserSession"] != null)
            {
                UserStudent = (Student)(Session["stuUserSession"]);
                double stuId = UserStudent.Stu_id;
                getStudentName(stuId);
            }

        }
    }


    private void getStudentName(double stuId)
    {
        Student student = new Student();
        DataTable dt = student.readSpecipicStudent(stuId);
        foreach (DataRow row in dt.Rows)
        {
            StuNameLBL.Text = "משתמש : " + row["Stu_FirstName"].ToString() + " " + row["Stu_LastName"].ToString();

        }

    }
}
