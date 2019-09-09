using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports : System.Web.UI.Page
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

    protected void commentsGRDW_DataBound(object sender, EventArgs e)
    {

        for (int i = commentsGRDW.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = commentsGRDW.Rows[i];
            GridViewRow previousRow = commentsGRDW.Rows[i - 1];
            for (int j = 0; j < row.Cells.Count - 1; j++)
            {
                if (row.Cells[j].Text == previousRow.Cells[j].Text)
                {
                    if (j != 0)
                    {
                        if (row.Cells[j - 1].Text == previousRow.Cells[j - 1].Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
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
                    else
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
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
}