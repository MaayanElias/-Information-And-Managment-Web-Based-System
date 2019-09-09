using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teacher_specific_calendar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["teaUserSession"] == null)
        {
            Response.Redirect("default.aspx");
        }
        
        Teacher T = (Teacher)Session["teaUserSession"];
        userId.Value = T.Tea_id.ToString();
    }
}