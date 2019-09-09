using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class ShowAvailability : System.Web.UI.Page
{
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

    protected void availabilityGV_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        

            if (e.Row.Cells[1].Text == "1")
                e.Row.Cells[1].Text = "א'";
            else if (e.Row.Cells[1].Text == "2")
                e.Row.Cells[1].Text = "ב'";
            else if (e.Row.Cells[1].Text == "3")
                e.Row.Cells[1].Text = "ג'";
            else if (e.Row.Cells[1].Text == "4")
                e.Row.Cells[1].Text = "ד'";
            else // יום חמישי 
                e.Row.Cells[1].Text = "ה'";
        }


    protected void availabilityGV_DataBound(object sender, EventArgs e)
    {
        for (int i = availabilityGV.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = availabilityGV.Rows[i];
            GridViewRow previousRow = availabilityGV.Rows[i - 1];
            for (int j = 0; j < row.Cells.Count; j++)
            {
                if (row.Cells[0].Text == previousRow.Cells[0].Text)
                {
                    if (previousRow.Cells[0].RowSpan == 0)
                    {
                        if (row.Cells[j].RowSpan == 0)
                        {
                            previousRow.Cells[j].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                        }
                        row.Cells[j].Visible = false;
                    }
                }
            }
        }
    }
}