using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TeacherMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Teacher UserTeacher = null;
            if (Session["TeaUserSession"] != null)
            {
                UserTeacher = (Teacher)(Session["TeaUserSession"]);
                double TeaId = UserTeacher.Tea_id;
                getTeacherName(TeaId);
            }

        }
    }


    private void getTeacherName(double TeaId)
    {
        Teacher teacher = new Teacher();
        DataTable dt = teacher.readSpecipicTeacher(TeaId);
        foreach (DataRow row in dt.Rows)
        {
            TeaNameLBL.Text = "משתמש : " + row["tea_FirstName"].ToString() + " " + row["tea_LastName"].ToString();
            
        }

    }
}
