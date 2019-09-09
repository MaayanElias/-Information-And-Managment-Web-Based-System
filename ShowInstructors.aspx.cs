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


public partial class ShowInstructors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["manUserSession"] == null)
        {
            Response.Redirect("default.aspx");
        }

        bool AddInstructor = Convert.ToBoolean(Session["AddInstuctor"]);
        if (AddInstructor)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('פרטי החונך נשמרו במערכת')", true);
            Session["AddInstuctor"] = false;
        }

        bool DeleteInstructor = Convert.ToBoolean(Session["DeleteInstructor"]);
        if (DeleteInstructor)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('החונך נמחק בהצלחה')", true);
            Session["DeleteInstructor"] = false;
        }
    }



    protected void addInstructorBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddInstructor.aspx");

    }



    protected void instructorsGRDW_SelectedIndexChanged(object sender, EventArgs e)
    {
        double ins_id = Convert.ToDouble(instructorsGRDW.SelectedRow.Cells[1].Text);
        Session["selectedID"] = ins_id;
        Response.Redirect("EditInstructor.aspx");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void exportBTN_Click(object sender, EventArgs e)
    {

        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Instructors.xls"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        instructorsGRDW.AllowPaging = false;
        instructorsGRDW.Columns[0].Visible = false;
        instructorsGRDW.Columns[7].Visible = false;
        foreach (GridViewRow row in instructorsGRDW.Rows)
        {
            foreach (TableCell cell in row.Cells)
            {
                cell.Attributes.CssStyle["text-align"] = "center";
            }
        }

        //Change the Header Row back to white color
        instructorsGRDW.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //Applying stlye to gridview header cells
        for (int i = 0; i < instructorsGRDW.HeaderRow.Cells.Count; i++)
        {
            instructorsGRDW.HeaderRow.Cells[i].Style.Add("visible", "none");
        }



        instructorsGRDW.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
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


        double Ins_Id;
        string Ins_FirstName;
        string Ins_LastName;
        string Ins_PhoneNumber;
        string Ins_Email;
        string ins_address;
        bool ins_status;
        string ins_password;

        Instructor inst;
        foreach (DataRow row in dt.Rows)
        {
            try
            {
                Ins_Id = Convert.ToDouble(row["Ins_Id"]);
                Ins_FirstName = row["ins_firstName"].ToString();
                Ins_LastName = row["ins_lastName"].ToString();
                Ins_PhoneNumber = row["ins_phoneNumber"].ToString();
                Ins_Email = row["ins_Email"].ToString();
                ins_address = row["ins_address"].ToString();
                ins_status = Convert.ToBoolean(row["ins_status"]);
                ins_password = row["ins_password"].ToString();
                inst = new Instructor(Ins_Id, Ins_FirstName, Ins_LastName, Ins_PhoneNumber, Ins_Email, ins_address, ins_status, ins_password);
                int numEffected = inst.InsertInstructor();
            }
            catch
            {
                Response.Redirect("ShowInstructors.aspx");
            }
        }
    }

}