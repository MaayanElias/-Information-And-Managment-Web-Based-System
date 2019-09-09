using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class ShowTeacher : System.Web.UI.Page
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
        bool AddTeacher = Convert.ToBoolean(Session["AddTeacher"]);
        if (AddTeacher)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('פרטי המתגבר נשמרו במערכת')", true);
            Session["AddTeacher"] = false;
        }
        bool DeleteTeacher = Convert.ToBoolean(Session["DeleteTeacher"]);
        if (DeleteTeacher)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('המתגבר נמחק בהצלחה')", true);
            Session["DeleteTeacher"] = false;
        }
    }

    protected void addTeacherBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddTeacher.aspx");
    }

    protected void teacherGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        double s_tea_id = Convert.ToInt32(teacherGRDW.SelectedRow.Cells[1].Text);
        Session["selectedTeacherID"] = s_tea_id;
        Response.Redirect("EditTeacher.aspx");
    }
    protected void exportBTN_Click(object sender, EventArgs e)
    {

        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Teachers.xls"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        teacherGRDW.AllowPaging = false;
        teacherGRDW.Columns[0].Visible = false;
        foreach (GridViewRow row in teacherGRDW.Rows)
        {
            foreach (TableCell cell in row.Cells)
            {
                cell.Attributes.CssStyle["text-align"] = "center";
            }
        }

        //Change the Header Row back to white color
        teacherGRDW.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //Applying stlye to gridview header cells
        for (int i = 0; i < teacherGRDW.HeaderRow.Cells.Count; i++)
        {
            teacherGRDW.HeaderRow.Cells[i].Style.Add("visible", "none");
        }

        teacherGRDW.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    
    protected void UploadBTN_Click(object sender, EventArgs e)
    {
        if (excelFU.HasFile)
        {
            string FileName = Path.GetFileName(excelFU.PostedFile.FileName);
            string Extension = Path.GetExtension(excelFU.PostedFile.FileName);
            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            string FilePath = Server.MapPath(FolderPath + FileName);
            excelFU.SaveAs(FilePath);
            string isHDR = "Yes";
            Import_To_Grid(FilePath, Extension, isHDR);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('לא נבחר קובץ')", true);

        }
    }

    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                         .ConnectionString;
                break;
            case ".xlsx": //Excel 07
                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                          .ConnectionString;
                break;
        }
        conStr = String.Format(conStr, FilePath, isHDR);
        System.Data.OleDb.OleDbConnection connExcel = new System.Data.OleDb.OleDbConnection(conStr);
        System.Data.OleDb.OleDbCommand cmdExcel = new System.Data.OleDb.OleDbCommand();
        System.Data.OleDb.OleDbDataAdapter oda = new System.Data.OleDb.OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        //Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();


        double tea_id;
        string tea_firstName;
        string tea_lastName;
        string tea_phoneNumber;
        string tea_email;
        string tea_address;
        bool tea_status;
        string tea_password;

        Teacher tea;
        foreach (DataRow row in dt.Rows)
        {
            try
            {
                tea_id = Convert.ToDouble(row["tea_Id"]);
                tea_firstName = row["tea_firstName"].ToString();
                tea_lastName = row["tea_lastName"].ToString();
                tea_phoneNumber = row["tea_phoneNumber"].ToString();
                tea_email = row["tea_Email"].ToString();
                tea_address = row["tea_address"].ToString();
                tea_status = Convert.ToBoolean(row["tea_status"]);
                tea_password = row["tea_password"].ToString();
                tea = new Teacher(tea_id, tea_firstName, tea_lastName, tea_phoneNumber, tea_email, tea_address, tea_status, tea_password);
                int numEffected = tea.InsertTeacher();
            }
            catch
            {
                Response.Redirect("ShowTeachers.aspx");
            }
        }
    }
}