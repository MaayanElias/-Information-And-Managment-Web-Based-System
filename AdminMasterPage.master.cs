using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Manager UserManager = null;
            if (Session["manUserSession"] != null)
            {
                UserManager = (Manager)(Session["manUserSession"]);
                double manId = UserManager.Man_id;
                getManagerName(manId);
            }
           
        }
       
    }

    private void getManagerName(double manId)
    {
        Manager manager = new Manager();
        DataTable dt = manager.readManager(manId);
        foreach (DataRow row in dt.Rows)
        {
            ManNameLBL.Text ="משתמש : "+ row["Man_FirstName"].ToString() +" " + row["Man_LastName"].ToString();
           
        }

    }

}
