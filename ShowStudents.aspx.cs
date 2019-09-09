using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShowStudents : System.Web.UI.Page
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
        bool AddStudent = Convert.ToBoolean(Session["AddStudent"]);
        if (AddStudent)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('פרטי התלמיד נשמרו במערכת')", true);
            Session["AddStudent"] = false;
        }
        bool DeleteStudent = Convert.ToBoolean(Session["DeleteStudent"]);
        if (DeleteStudent)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_message", "toastr.success('התלמיד נמחק בהצלחה')", true);
            Session["DeleteStudent"] = false;
        }
    }

    protected void addStudentBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddStudent.aspx");
    }

    protected void studentsGRDW_SelectedIndexChanged(object sender, EventArgs e)
    {
        int stud_id = Convert.ToInt32(studentsGRDW.SelectedRow.Cells[1].Text);
        Session["selectedID"] = stud_id;
        Response.Redirect("EditStudent.aspx");

    }

    protected void exportBTN_Click(object sender, EventArgs e)
    {

        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Students.xls"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        studentsGRDW.AllowPaging = false;
        studentsGRDW.Columns[0].Visible = false;
        foreach (GridViewRow row in studentsGRDW.Rows)
        {
            foreach (TableCell cell in row.Cells)
            {
                cell.Attributes.CssStyle["text-align"] = "center";
            }
        }

        //Change the Header Row back to white color
        studentsGRDW.HeaderRow.Style.Add("background-color", "#FFFFFF");
        //Applying stlye to gridview header cells
        for (int i = 0; i < studentsGRDW.HeaderRow.Cells.Count; i++)
        {
            studentsGRDW.HeaderRow.Cells[i].Style.Add("visible", "none");
        }



        studentsGRDW.RenderControl(htw);
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


        double Stu_Id;
        double Ins_Id;
        string Stu_FirstName;
        string Stu_LastName;
        string Stu_BirthDate;
        string Stu_PhoneNumber;
        string Stu_Email;
        string Stu_address;
        bool Stu_status;
        string Stu_password;
        string Stu_Grade;
        bool Stu_IsEntitled;
        string Stu_Note = null;

        Student stu;
        foreach (DataRow row in dt.Rows)
        {
            try
            {
                Stu_Id = Convert.ToDouble(row["Stu_Id"]);
                Ins_Id = Convert.ToDouble(row["Ins_Id"]);
                Stu_FirstName = row["Stu_firstName"].ToString();
                Stu_LastName = row["Stu_lastName"].ToString();
                Stu_BirthDate = row["Stu_BirthDate"].ToString();
                Stu_PhoneNumber = row["Stu_phoneNumber"].ToString();
                Stu_Email = row["Stu_Email"].ToString();
                Stu_address = row["Stu_address"].ToString();
                Stu_status = Convert.ToBoolean(row["Stu_status"]);
                Stu_password = row["Stu_password"].ToString();
                Stu_Grade = row["Stu_Grade"].ToString();
                Stu_IsEntitled = Convert.ToBoolean(row["Stu_IsEntitled"]);
                //Stu_Note=row["Stu_Note"].ToString();
                stu = new Student(Stu_Id, Ins_Id, Stu_FirstName, Stu_LastName, Stu_BirthDate, Stu_PhoneNumber, Stu_Email, Stu_address, Stu_status, Stu_password, Stu_Grade, Stu_IsEntitled, Stu_Note);
                int numEffected = stu.InsertStudent();
            }
            catch
            {
                Response.Redirect("ShowStudents.aspx");
            }
        }
    }

}