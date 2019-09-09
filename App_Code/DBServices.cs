using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for DBServices
/// </summary>
public class DBServices
{

    public SqlDataAdapter da;
    public DataTable dt;

    public DBServices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    //read users from student table
    public Student readSpecificUserStudentDB(double userId, string userPass, string conString, string tableName)
    {
        Student s = new Student();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName + " where stu_id='" + userId + "' and stu_password='" + userPass + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            int flg = 1;

            while (dr.Read())
            {   // Read till the end of the data into a row
                s.Stu_id = (double)dr["Stu_Id"];
                s.Stu_Instructor_id = (double)dr["ins_id"];
                s.FirstName = (string)dr["stu_FirstName"];
                s.LastName = (string)dr["stu_LastName"];
                s.BirthDate = (string)dr["stu_BirthDate"];
                s.PhoneNumber = (string)dr["stu_phoneNumber"];
                s.Email = (string)dr["Stu_Email"];
                s.Address = (string)dr["Stu_Address"];
                s.Status = (bool)dr["Stu_Status"];
                s.Password = (string)dr["Stu_Password"];
                s.Grade = (string)dr["Stu_Grade"];
                s.IsEntitled = (bool)dr["Stu_IsEntitled"];
                s.Notes = (string)dr["stu_note"];

                flg = 0; // only one row->only one student will return
            }

            if (flg == 0)
            {
                return s;
            }
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //read users from student table
    public Manager readSpecificUserManagerDB(double userId, string userPass, string conString, string tableName)
    {
        Manager m = new Manager();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName + " where Man_Id='" + userId + "' and Man_Password='" + userPass + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            int flg = 1;

            while (dr.Read())
            {   // Read till the end of the data into a row
                m.Man_id = (double)dr["Man_Id"];
                m.Man_firstName = (string)dr["Man_FirstName"];
                m.Man_lastName = (string)dr["Man_LastName"];
                m.Man_phoneNumber = (string)dr["Man_PhoneNumber"];
                m.Man_email = (string)dr["Man_Email"];
                m.Man_address = (string)dr["Man_Address"];
                m.Man_status = (bool)dr["Man_Status"];
                m.Man_password = (string)dr["Man_Password"];

                flg = 0; // only one row
            }

            if (flg == 0)
            {
                return m;
            }
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }



    //Insert student to the db
    public int InsertStudent(Student student)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandStudent(student);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandStudent(Student stud)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}' ,'{5}','{6}', '{7}', '{8}', '{9}','{10}','{11}', '{12}')", stud.Stu_id, stud.Stu_Instructor_id, stud.FirstName, stud.LastName, stud.BirthDate, stud.PhoneNumber, stud.Email, stud.Address, Convert.ToSByte(Convert.ToInt16(stud.Status)), stud.Password, stud.Grade, Convert.ToSByte(Convert.ToInt16(stud.IsEntitled)), stud.Notes);
        String prefix = "INSERT INTO Student  " + "( Stu_Id , Ins_Id, Stu_FirstName , Stu_LastName ,Stu_BirthDate, Stu_PhoneNumber , Stu_Email, stu_address, stu_status ,Stu_Password, Stu_grade ,Stu_IsEntitled  ,Stu_Note ) ";
        command = prefix + sb.ToString();

        return command;
    }

    //delete student from db
    public int deleteStudent(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteStudentCommandStudent(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String deleteStudentCommandStudent(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Student where stu_id=" + id;

        return command;
    }


    //update specific student 
    public int updateSpecificStudent(Student student)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandStudent(student);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandStudent(Student stud)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Student SET stu_id='" + stud.Stu_id + "', Ins_id='" + stud.Stu_Instructor_id + "', stu_firstName='" + stud.FirstName + "', stu_lastName='"
            + stud.LastName + "',stu_birthDate='" + stud.BirthDate + "', stu_phoneNumber='" + stud.PhoneNumber + "', stu_email='" + stud.Email + "', stu_address='" + stud.Address + "', stu_status='" + stud.Status + "', stu_password='" + stud.Password + "', stu_grade='"
            + stud.Grade + "',stu_isEntitled='" + stud.IsEntitled + "',stu_note='" + stud.Notes + "' WHERE stu_id='" + stud.Stu_id + "'";
        return command;
    }

    public DBServices ReadSpecipicStudentDB(string conString, string tableName, double _stud_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE Stu_Id = '" + _stud_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }


    public int InsertInstructor(Instructor Instructor)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandInstructor(Instructor);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a Instructor command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandInstructor(Instructor inst)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}','{6}','{7}')", inst.Ins_id, inst.Ins_firstName, inst.Ins_lastName, inst.Ins_phoneNumber, inst.Ins_email, inst.Ins_addres, inst.Ins_status, inst.Ins_password);
        String prefix = "INSERT INTO Instructor  " + "(Ins_Id , Ins_FirstName , Ins_LastName , Ins_PhoneNumber , Ins_Email,Ins_address,Ins_status,Ins_password) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int deleteInstructorFromStudent(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuilddeleteInstructorFromStudentCommand(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a Instractur command String
    //--------------------------------------------------------------------
    private String BuilddeleteInstructorFromStudentCommand(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from student where Stu_id= " + id;

        return command;
    }


    public int deleteInstructor(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuilddeleteInstructorCommand(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------
    // Build the delete a Instructor command tring
    //--------------------------------------------------------------------
    private String BuilddeleteInstructorCommand(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Instructor where Ins_id= " + id;

        return command;
    }

    public int updateSpecificInstructor(Instructor Instructor)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandInstructor(Instructor);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update Instructor command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandInstructor(Instructor Inst)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Instructor SET Ins_id='" + Inst.Ins_id + "', Ins_firstName='" + Inst.Ins_firstName + "', Ins_lastName='" + Inst.Ins_lastName + "', Ins_phoneNumber='" + Inst.Ins_phoneNumber + "', Ins_email='" + Inst.Ins_email + "',Ins_address='" + Inst.Ins_addres + "',Ins_password='" + Inst.Ins_password + "',Ins_status='" + Inst.Ins_status + "' WHERE Ins_id='" + Inst.Ins_id + "'";
        return command;
    }


    public DBServices ReadSpecipicInstructorDB(string conString, string tableName, double _Ins_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE Ins_Id = '" + _Ins_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public int InsertTeacher(Teacher t)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandTeacher(t);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    private String BuildInsertCommandTeacher(Teacher t)
    {
        String command;

        StringBuilder sb = new StringBuilder();

        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}' ,'{5}','{6}', '{7}')", t.Tea_id, t.Tea_firstName, t.Tea_lastName, t.Tea_phoneNumber, t.Tea_email, t.Tea_address, Convert.ToSByte(Convert.ToInt16(t.Tea_status)), t.Tea_password);
        String prefix = "InsERT INTO Teacher " + "( Tea_Id, Tea_FirstName, Tea_LastName, Tea_PhoneNumber, Tea_Email, Tea_Address, Tea_Status, Tea_Password )";
        command = prefix + sb.ToString();

        return command;
    }


    public DBServices ReadSpecipicTeacherDB(string conString, string tableName, double _tea_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE tea_Id = '" + _tea_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }



    //--------------------------------------------------------------------
    // Build the delete a teacher command String
    //--------------------------------------------------------------------
    private String deleteTeacherCommand(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Teacher where tea_id=" + id;

        return command;
    }

    //--------------------------------------------------------------------
    // Build the update a Teacher command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandTeacher(Teacher tea)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Teacher SET tea_id='" + tea.Tea_id + "', tea_firstName='" + tea.Tea_firstName + "', tea_lastName='"
            + tea.Tea_lastName + "', tea_phoneNumber='" + tea.Tea_phoneNumber + "', tea_email='" + tea.Tea_email + "', tea_address='" + tea.Tea_address + "', tea_status='" + tea.Tea_status + "', tea_password='" + tea.Tea_password + "' WHERE tea_id='" + tea.Tea_id + "'";
        return command;
    }

    public int deleteTeacher(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteTeacherCommand(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public int updateSpecificTeacher(Teacher teacher)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandTeacher(teacher);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //read users from teacher table
    public Teacher readSpecificUserTeacherDB(double userId, string userPass, string conString, string tableName)
    {
        Teacher t = new Teacher();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName + " where tea_Id='" + userId + "' and Tea_Password='" + userPass + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            int flg = 1;

            while (dr.Read())
            {   // Read till the end of the data into a row
                t.Tea_id = (double)dr["tea_Id"];
                t.Tea_firstName = (string)dr["tea_FirstName"];
                t.Tea_lastName = (string)dr["tea_LastName"];
                t.Tea_phoneNumber = (string)dr["tea_PhoneNumber"];
                t.Tea_email = (string)dr["tea_Email"];
                t.Tea_address = (string)dr["tea_Address"];
                t.Tea_status = (bool)dr["tea_Status"];
                t.Tea_password = (string)dr["tea_Password"];

                flg = 0; // only one row
            }

            if (flg == 0)
            {
                return t;
            }
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //Insert TeachBy to the db
    public int InsertTeachBy(TeachBy tb)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandTeachBy(tb);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTeachBy(TeachBy tb)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}')", tb.Tea_Id, tb.Pro_Id);
        String prefix = "INSERT INTO TeachBy  " + "(tea_id , pro_id) ";
        command = prefix + sb.ToString();

        return command;
    }

    //פונקציה שמביאה רשימה של תגבורים
    public List<Tigburim> getTigburimList(string conString, string tableName)
    {

        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT al.number as 'SingID', t.[Tea_Id], (t.[Tea_FirstName] +' '+ t.[Tea_LastName]) as 'full_name', pro.Pro_Id, pro.[Pro_Title], les.[Les_MaxQuan], al.quantity, les.[Les_StartHour], les.[Les_EndHour], al.[ActLes_date], al.[actls_cancelled], les.Les_Id" +
                " FROM " + tableName + " les INNER JOIN [ActualLesson] al on les.Les_Id = al.ActLes_LesId INNER JOIN [Profession] pro on pro.Pro_Id = les.Les_Pro_Id INNER JOIN [Teacher] t ON t.Tea_Id = les.Les_Tea_Id WHERE al.[actls_cancelled] = 0 ";

            //SELECT *
            //FROM Lesson INNER JOIN[dbo].[Teacher]
            //ON Lesson.teacher_id = [dbo].[Teacher].[Tea_Id] INNER Join [dbo].[Profession] ON Lesson.[pro_id] = [dbo].[Profession].[pro_id]


            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Tigburim t = new Tigburim();
                string[] dateFormat1 = { };
                dateFormat1 = Convert.ToString(dr["ActLes_date"]).Split(' ');


                //object building
                t.Id = Convert.ToInt16(dr["SingID"]);
                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.EndTime = Convert.ToString(dr["Les_EndHour"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);
                t.Limit = (int)(dr["Les_MaxQuan"]);
                t.ProfId = (int)(dr["Pro_Id"]);
                t.TeacherName = Convert.ToString(dr["full_name"]);
                t.TeacherId = Convert.ToDouble(dr["Tea_Id"]);
                t.ActualLimit = (int)(dr["Les_MaxQuan"]) - (int)(dr["quantity"]);
                t.TrueId = Convert.ToInt16(dr["Les_Id"]);
                t.IsSigned = 0;
                //date formatting
                string[] dateFormat2 = dateFormat1[0].Split('-');

                t.TigburDate = dateFormat2[2] + "-" + dateFormat2[1] + "-" + dateFormat2[0];



                TigburimList.Add(t);

            }
            return TigburimList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //פונקציה שמביאה רשימה של תגבורים
    public List<Tigburim> getTigburimListForStudent(string conString, string tableName, string id)
    {
        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT DISTINCT al.number as 'SingID', t.[Tea_Id], (t.[Tea_FirstName] +' '+ t.[Tea_LastName]) as 'full_name', al.[actls_cancelled], pro.Pro_Id, pro.[Pro_Title], les.[Les_MaxQuan], al.quantity, les.[Les_StartHour], les.[Les_EndHour], al.[ActLes_date],StLes_stuId FROM Lesson les INNER JOIN[ActualLesson] al on les.Les_Id = al.ActLes_LesId INNER JOIN[Profession] pro on pro.Pro_Id = les.Les_Pro_Id INNER JOIN[Teacher] t ON t.Tea_Id = les.Les_Tea_Id INNER JOIN[signedToLesson] stl on stl.StLes_ActLesId = al.ActLes_LesId inner join student on stl.StLes_stuId = stu_id where stu_Id = '" + id + "' and al.actLes_date = StLes_ActLesDate and al.[actls_cancelled] = 0";

            //SELECT *
            //FROM Lesson INNER JOIN[dbo].[Teacher]
            //ON Lesson.teacher_id = [dbo].[Teacher].[Tea_Id] INNER Join [dbo].[Profession] ON Lesson.[pro_id] = [dbo].[Profession].[pro_id]


            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Tigburim t = new Tigburim();
                string[] dateFormat1 = { };
                dateFormat1 = Convert.ToString(dr["ActLes_date"]).Split(' ');


                //object building
                t.Id = Convert.ToInt16(dr["SingID"]);
                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.EndTime = Convert.ToString(dr["Les_EndHour"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);
                t.Limit = (int)(dr["Les_MaxQuan"]);
                t.ProfId = (int)(dr["Pro_Id"]);
                t.TeacherName = Convert.ToString(dr["full_name"]);
                t.TeacherId = Convert.ToDouble(dr["Tea_Id"]);
                t.ActualLimit = (int)(dr["Les_MaxQuan"]) - (int)(dr["quantity"]);

                //date formatting
                string[] dateFormat2 = dateFormat1[0].Split('-');

                t.TigburDate = dateFormat2[2] + "-" + dateFormat2[1] + "-" + dateFormat2[0];



                TigburimList.Add(t);

            }
            return TigburimList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public Tigburim getTigburById(int tigID, string conString, string tableName)
    {

        Tigburim t = new Tigburim();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT al.number, Les_Id, t.[Tea_Id], (t.[Tea_FirstName] +' '+ t.[Tea_LastName]) as 'full_name', pro.Pro_Id, pro.[Pro_Title], les.[Les_MaxQuan],al.[actls_cancelled], al.quantity, les.[Les_StartHour], les.[Les_EndHour], al.[ActLes_date]" +
                " FROM " + tableName + " les INNER JOIN [ActualLesson] al on les.Les_Id = al.ActLes_LesId INNER JOIN [Profession] pro on pro.Pro_Id = les.Les_Pro_Id INNER JOIN [Teacher] t ON t.Tea_Id = les.Les_Tea_Id WHERE al.[actls_cancelled] = 0 and al.number='" + tigID + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            //bool status, string startTime, string endTime, string descEvent,int eventTypeID, double latitude, double longitude, int guardingID
            while (dr.Read())
            {   // Read till the end of the data into a row               


                string[] dateFormat1 = { };
                dateFormat1 = Convert.ToString(dr["ActLes_date"]).Split(' ');


                //object building
                t.Id = Convert.ToInt16(dr["number"]);
                t.TrueId = Convert.ToInt16(dr["Les_Id"]);
                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.EndTime = Convert.ToString(dr["Les_EndHour"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);
                t.Limit = (int)(dr["Les_MaxQuan"]);
                t.ProfId = (int)(dr["Pro_Id"]);
                t.TeacherName = Convert.ToString(dr["full_name"]);
                t.TeacherId = Convert.ToDouble(dr["Tea_Id"]);
                t.ActualLimit = (int)(dr["Les_MaxQuan"]) - (int)(dr["quantity"]);

                //date formatting
                string[] dateFormat2 = dateFormat1[0].Split('-');

                t.TigburDate = dateFormat2[2] + "-" + dateFormat2[1] + "-" + dateFormat2[0];

            }
            return t;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public int updateIsEntittled(double stu_id, int updateIsEntitledTo)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandStudentIsEntitled(stu_id, updateIsEntitledTo);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandStudentIsEntitled(double stu_id, int updateIsEntitledTo)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Student SET stu_isEntitled= '" + updateIsEntitledTo + "' WHERE stu_id='" + stu_id + "'";
        return command;
    }

    public int InsertLesson(Lesson les)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandLesson(les);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a lesson command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandLesson(Lesson les)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}' ,'{5}')", les.Tea_id, les.Pro_id, les.Les_maxQuan, les.Les_startHour, les.Les_endHour, les.Les_day);
        String prefix = "INSERT INTO Lesson  " + "( Les_Tea_Id, Les_Pro_Id, Les_MaxQuan, Les_StartHour, Les_EndHour, Les_day) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int deleteLesson(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuilddeleteCommandLesson(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a lesson command String
    //--------------------------------------------------------------------
    private String BuilddeleteCommandLesson(int id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from ActualLesson where ActLes_LesId=" + id;

        return command;
    }

    public int deleteLessonFromRequest(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuilddeleteCommandLessonFromRequest(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a LessonFromRequest command String
    //--------------------------------------------------------------------
    private String BuilddeleteCommandLessonFromRequest(int id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Requests where req_actLes_id=" + id;

        return command;
    }

    public DBServices ReadSpecipicLessonDB(string conString, string tableName, double _less_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE Les_Id = '" + _less_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    //update specific Lesson 
    public int updateSpecificLesson(Lesson les, int less_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandLesson(les, less_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a Lesson command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandLesson(Lesson les, int less_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Lesson SET Les_Tea_id='" + les.Tea_id + "', Les_Pro_Id='" + les.Pro_id + "', Les_MaxQuan='" + les.Les_maxQuan + "',Les_StartHour='" + les.Les_startHour + "',Les_EndHour='" + les.Les_endHour + "', Les_Day='" + les.Les_day + "' WHERE Les_Id='" + less_id + "'";
        return command;
    }

    public int insertActualLesson(ActualLesson al)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandActualLesson(al);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a Actual lesson command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandActualLesson(ActualLesson al)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}','{3}')", al.Les_id, al.Act_les_date, al.Quantity, al.Day);
        String prefix = "INSERT INTO ActualLesson  " + "( ActLes_LesId,ActLes_date, quantity,actls_day)";
        command = prefix + sb.ToString();

        return command;
    }

    //read Specific Reques from tDB
    public DBServices readSpecificRequestDB(string conString, string tableName, int req_num)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE req_number = '" + req_num + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    // insert request 
    public int InsertRequest(Request re)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandRequest(re);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a request command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandRequest(Request re)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}', '{5}','{6}')", re.Req_actLes_id, (re.Req_actLes_date).ToString("yyyy-MM-dd"), re.Req_stu_id, re.Req_status, re.Req_is_permanent, re.Req_type, (re.Req_date).ToString("yyyy-MM-dd"));
        String prefix = "INSERT INTO Requests  " + "( req_actLes_id , req_actLes_date , req_stu_id ,req_status, req_is_permanent, req_type, request_date) ";
        command = prefix + sb.ToString();

        return command;
    }

    // insert request to fullCalendar
    public int InsertRequestCal(Request re)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandRequestCal(re);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a request command String (for fullcalendar)
    //--------------------------------------------------------------------
    private String BuildInsertCommandRequestCal(Request re)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}' , '{5}' , '{6}')", re.Req_actLes_id, (re.Req_actLes_date).ToString("yyyy-MM-dd"), re.Req_stu_id, re.Req_status, re.Req_is_permanent, re.Req_type, Convert.ToString(re.Req_dateSTR));//inserting a request from calendar is always an a request to join a tigbur
        String prefix = "INSERT INTO Requests  " + "( req_actLes_id , req_actLes_date , req_stu_id ,req_status, req_is_permanent, req_type,request_date) ";
        command = prefix + sb.ToString();

        return command;
    }

    // update a request
    public int updateSpecificRequest(int req_num, int status)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandRequest(req_num, status);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandRequest(int req_num, int status)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Requests SET req_status='" + status + "' WHERE req_number='" + req_num + "'";
        return command;
    }

    
    // update a request Decliend Field
    public int updateReqDecliendFieldDB(int req_num, int req_decliend)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildupdateReqDecliendFieldDBCommand(req_num, req_decliend);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildupdateReqDecliendFieldDBCommand(int req_num, int req_decliend)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Requests SET req_decliend='" + req_decliend + "' WHERE req_number='" + req_num + "'";
        return command;
    }

    public int updatedDecfullFieldDB(int req_num, int decFull)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildupdatedDecfullFieldDBCommand(req_num, decFull);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildupdatedDecfullFieldDBCommand(int req_num, int decFull)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Requests SET dec_full='" + decFull + "' WHERE req_number='" + req_num + "'";
        return command;
    }
    //Insert signedToLesson to the db
    public int InsertSigendToLesson(SignedToLesson stl)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandSigendToLesson(stl);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //delete request from db
    public int deleteRequest(double stu_id, int les_id, DateTime act_les_date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteRequestommand(stu_id, les_id, act_les_date);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public DBServices checkIfSignedFunc(string id)
    {
        DBServices dbS = new DBServices(); // create a helper class
        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "select StLes_ActLesId,StLes_ActLesDate from [dbo].[signedToLesson] where Stles_stuId =" + id;
            //need to get a fix on a decent query



            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataAdapter da = new SqlDataAdapter(selectSTR, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //delete requests when deleting tigbur from db
    public int deleteRequests(string id, string date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = "delete from [dbo].[Requests] where req_actLes_id =" + id + " and req_actLes_date ='" + date + "'";

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String deleteRequestommand(double stu_id, int les_id, DateTime act_les_date)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Requests where req_stu_id=" + stu_id + " and req_actLes_id=" + les_id + " and req_actLes_date=" + "'" + (act_les_date).ToString("yyyy-MM-dd") + "'";

        return command;
    }

    public int deleteTigbur(string id, string date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = "UPDATE ActualLesson SET [actls_cancelled] = 1 WHERE ActLes_LesId = '" + id + "' and ActLes_date = '" + date + "'";

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //delete request from db
    public int deleteSTL(string id, string date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = "delete from [dbo].[signedToLesson] where StLes_ActLesId =" + id + " and StLes_ActLesDate ='" + date + "'";

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------
    // Build the Insert a sigendToLesson command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandSigendToLesson(SignedToLesson stl)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}')", stl.SigToLes_ActLesId, stl.SigToLes_ActLesDate, stl.SigToLess_stuId);
        String prefix = "INSERT INTO signedToLesson  " + "(StLes_ActLesId , StLes_ActLesDate, StLes_stuId) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int updateSpecificActualLesson(int lesId, string lesDate, int actual_quan)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandActualLesson(lesId, lesDate, actual_quan);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandActualLesson(int les_numer, string lesDate, int actual_quan)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ActualLesson SET quantity='" + actual_quan + "' WHERE ActLes_LesId='" + les_numer + "' AND ActLes_date='" + lesDate + "'";
        return command;
    }

    //read specific request
    public ActualLesson readSpecificActualLesson(int actl_num, string conString, string tableName)
    {
        ActualLesson a = new ActualLesson();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName + " where number='" + actl_num + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            int flg = 1;

            while (dr.Read())
            {   // Read till the end of the data into a row

                a.Les_id = (int)dr["ActLes_LesId"];
                a.Act_les_date = dr["ActLes_date"].ToString();
                a.Quantity = (int)dr["quantity"];

                flg = 0; // only one row
            }

            if (flg == 0)
            {
                return a;
            }
            else return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------
    // Reports DBS functions
    //--------------------------------------------------------------------
    public List<Report> getProfessionCount(string startDate, string endDate, string conString)
    {

        List<Report> ProfessionCountReport = new List<Report>();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = " select Pro_Id, Pro_Title, count(1) as 'amount' from Profession inner join Lesson on Les_Pro_Id = Pro_Id inner join ActualLesson on ActLes_LesId = Les_Id WHERE actLes_date BETWEEN '" + startDate + "' AND '" + endDate + "' group by Pro_Id, Pro_Title";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                Report r = new Report();
                r.Id = Convert.ToInt32(dr["Pro_Id"]);
                r.Pro_title = (string)dr["Pro_title"];
                r.Amount = Convert.ToDouble(dr["amount"]);

                ProfessionCountReport.Add(r);

            }
            return ProfessionCountReport;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------
    // User Request by Profession - User mini dashboard
    //--------------------------------------------------------------------
    public List<Report> StudentRequestsByProfession(string startDate, string endDate, string conString, string userId)
    {

        List<Report> StudentRequestsByProfessionCountList = new List<Report>();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR = "select Pro_Id, Pro_Title, count(1) as 'amount' from Requests inner join Lesson on req_actLes_id = Les_Id inner join Profession on Pro_Id = Les_Pro_Id WHERE req_stu_id = '" + userId + "' AND req_status = '2' and request_date BETWEEN '" + startDate + "' AND '" + endDate + "' group by Pro_Id, Pro_Title ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                Report r = new Report();
                r.Id = Convert.ToInt32(dr["Pro_Id"]);
                r.Pro_title = (string)dr["Pro_title"];
                r.Amount = Convert.ToDouble(dr["amount"]);

                StudentRequestsByProfessionCountList.Add(r);

            }
            return StudentRequestsByProfessionCountList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }



    //--------------------------------------------------------------------
    // User Classes by Profession - User mini dashboard
    //--------------------------------------------------------------------
    public List<Report> StudentClassesByProfession(string startDate, string endDate, string conString, string userId)
    {

        List<Report> StudentClassesByProfessionCountList = new List<Report>();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR2 = "select Pro_Id, Pro_Title, count(1) as 'amount' from Requests inner join Lesson on req_actLes_id = Les_Id inner join Profession on Pro_Id = Les_Pro_Id WHERE req_stu_id = '" + userId + "' AND req_status = '1' and request_date BETWEEN '" + startDate + "' AND '" + endDate + "' group by Pro_Id, Pro_Title ";
            SqlCommand cmd = new SqlCommand(selectSTR2, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                Report r = new Report();
                r.Id = Convert.ToInt32(dr["Pro_Id"]);
                r.Pro_title = (string)dr["Pro_title"];
                r.Amount = Convert.ToDouble(dr["amount"]);

                StudentClassesByProfessionCountList.Add(r);

            }
            return StudentClassesByProfessionCountList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------
    // Requests counter for admin dashboard
    //--------------------------------------------------------------------
    public Int32 getRequestsCount(string conString)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count(1) as 'counter' from Requests where req_status='2'";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------
    // Attendence forms counter for admin dashboard
    //--------------------------------------------------------------------
    public Int32 getAttendenceFormsCount(string conString)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count(1) as 'amount' from [dbo].[ActualLesson] where Attendance_Form='0' AND ActLes_date < GETDATE()";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------
    // Unread messages counter for admin dashboard
    //--------------------------------------------------------------------
    public Int32 getUnreadMessagesCount(string conString)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count(1) as 'amount' from [dbo].[StudentMessages] where msg_hasRead='0'";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public DBServices getStudentCounterDB(string conString, string tableName, double _stud_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT stu_counter FROM " + tableName + " WHERE Stu_Id = '" + _stud_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public int updateStu_CounterToZero(double stu_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandStudentStu_CounterToZero(stu_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandStudentStu_CounterToZero(double stu_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Student SET stu_counter= 0 WHERE stu_id='" + stu_id + "'";
        return command;
    }


    public int updateStudentCounterDB(double stu_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildupdateStudentCounterCommand(stu_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildupdateStudentCounterCommand(double stu_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE Student SET stu_counter= stu_counter + 1 WHERE stu_id='" + stu_id + "'";
        return command;
    }


    //--------------------------------------------------------------------
    // Unread messages counter for student dashboard
    //--------------------------------------------------------------------
    public Int32 getUnreadMessagesCountForStudent(string conString, string stu_id)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count(1) as 'amount' from [dbo].[managerMessages] where msg_hasRead='0' AND msg_toStudentId='" + stu_id + "'";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }



    //--------------------------------------------------------------------
    // Unread messages counter for teacher dashboard
    //--------------------------------------------------------------------
    public Int32 getUnreadMessagesCountForTeacher(string conString, string tea_id)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count(1) as 'amount' from [dbo].[managerMessagesToTeachers] where msg_hasRead='0' AND msg_toTeacherId='" + tea_id + "'";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }



    //Read student list from DB for specific lesson (Used by teacher user)
    public DBServices readStudentsListDB(string conString, string tableName, int lessId, DateTime lessDate)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT StLes_stuId,Presence,comments FROM " + tableName + " WHERE StLes_ActLesId = '" + lessId + "' and StLes_ActLesDate='" + lessDate.ToString("yyyy-MM-dd") + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    // insert teacher request 
    public int InsertTeacherRequestDB(teachersRequests re)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandTeacherRequest(re);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a teacher request command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandTeacherRequest(teachersRequests re)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}', '{5}')", re.LesId, (re.LesDate).ToString("yyyy-MM-dd"), re.TeaId, re.ReqStatus, re.ReqReason, (re.ReqDate).ToString("yyyy-MM-dd"));
        String prefix = "INSERT INTO TeacherRequests  " + "( TeaReq__LessId , TeaReq__LessDate , TeaReq_TeaId ,TeaReq_status, reason, TeaReq_reqDate) ";
        command = prefix + sb.ToString();

        return command;
    }


    //delete request from db
    public int deleteTeacherRequestDB(double tea_id, int les_id, DateTime les_date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteTeacherRequestommand(tea_id, les_id, les_date);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String deleteTeacherRequestommand(double tea_id, int les_id, DateTime les_date)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from TeacherRequests where TeaReq_TeaId=" + tea_id + " and TeaReq__LessId=" + les_id + " and TeaReq__LessDate=" + "'" + (les_date).ToString("yyyy-MM-dd") + "'";

        return command;
    }

    // update teacher request
    public int updateSpecificTeacherRequestDB(int req_num, int status)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandTeacherRequest(req_num, status);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandTeacherRequest(int req_num, int status)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE TeacherRequests SET TeaReq_status='" + status + "' WHERE TeaReq_number='" + req_num + "'";
        return command;
    }


    //cancel lesson
    public int cancelSpecificActualLessonDB(int lesId, string lesDate, int cancel)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildCancelActualLesson(lesId, lesDate, cancel);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the cancel lesson command String
    //--------------------------------------------------------------------
    private String BuildCancelActualLesson(int les_numer, string lesDate, int cancel)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ActualLesson SET actls_cancelled='" + cancel + "' WHERE ActLes_LesId='" + les_numer + "' AND ActLes_date='" + lesDate + "'";
        return command;
    }

    //delete students from db
    public int deleteStudentsFromLessonDB(int les_id, string act_les_date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteStudentsCommand(les_id, act_les_date);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private String deleteStudentsCommand(int les_id, string StLes_ActLesDate)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from signedToLesson where StLes_ActLesId=" + les_id + " and StLes_ActLesDate=" + "'" + StLes_ActLesDate + "'";

        return command;
    }

    //delete student from lesson
    public int deleteStudentFromLessonDB(double stuId, int les_id, string act_les_date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteStudentCommand(stuId, les_id, act_les_date);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private String deleteStudentCommand(double stuId, int les_id, string StLes_ActLesDate)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from signedToLesson where StLes_ActLesId=" + les_id + " and StLes_ActLesDate=" + "'" + StLes_ActLesDate + "' and StLes_stuId='" + stuId + "'";

        return command;
    }

    //מחיקת בקשות תלמידים עבור תגבור שבוטל
    public int deleteCancelldLessonRequestsDB(int les_id, string act_les_date)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteRequestsCommand(les_id, act_les_date);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a req command String
    //--------------------------------------------------------------------
    private String deleteRequestsCommand(int les_id, string act_les_date)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from Requests where req_actLes_id=" + les_id + " and req_actLes_date=" + "'" + act_les_date + "'";

        return command;
    }




    //כאשר מתגבר ממלא טופס נוכחות נשנה בהתאם עבור תגבור זה את השדה attendanc_form 
    public int updateActualLessonAttendancFormDB(int lesId, DateTime lesDate)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandActualLessonAttendance(lesId, lesDate);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    public int deleteInMessageForManager(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildCommandDeleteInMessageForManager(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String BuildCommandDeleteInMessageForManager(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from studentMessages where msg_id=" + id;

        return command;
    }

    public int deleteInMessageForStudent(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildCommandDeleteInMessageForStudent(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String BuildCommandDeleteInMessageForStudent(double id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        command = "delete from ManagerMessages where msg_id=" + id;

        return command;
    }


    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandActualLessonAttendance(int les_numer, DateTime lesDate)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ActualLesson SET Attendance_Form='" + 1 + "' WHERE ActLes_LesId='" + les_numer + "' AND ActLes_date='" + lesDate.ToString("yyyy-MM-dd") + "'";
        return command;
    }
    public int updateStudentPresenceDB(int lesId, DateTime lesDate, double stuId, int stuPresence)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandupdateStudentPresence(lesId, lesDate, stuId, stuPresence);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandupdateStudentPresence(int les_numer, DateTime lesDate, double stuId, int stuPresence)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE signedToLesson SET Presence='" + stuPresence + "' WHERE StLes_ActLesId='" + les_numer + "' AND StLes_ActLesDate='" + lesDate.ToString("yyyy-MM-dd") + "' AND StLes_stuId='" + stuId + "'";
        return command;
    }

    public int updateStudentNotesDB(int lesId, DateTime lesDate, double stuId, string stuNotes)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandupdateStudentNotes(lesId, lesDate, stuId, stuNotes);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandupdateStudentNotes(int les_numer, DateTime lesDate, double stuId, string stuNotes)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE signedToLesson SET comments='" + stuNotes + "' WHERE StLes_ActLesId='" + les_numer + "' AND StLes_ActLesDate='" + lesDate.ToString("yyyy-MM-dd") + "' AND StLes_stuId='" + stuId + "'";
        return command;
    }

    //פונקציה שמביאה רשימה של תגבורים
    public List<Tigburim> getTigburimListTemplate(string date)
    {
        // String to DateTime
        String MyString = date;
        //MyString = "1999-04-25" example;  //Depends on your regional settings

        DateTime MyDateTime = new DateTime();
        MyDateTime = DateTime.ParseExact(MyString, "yyyy-MM-dd", null);
        int day = (int)MyDateTime.DayOfWeek + 1;
        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT L.Les_Id,(T.Tea_FirstName + ' ' + T.Tea_LastName) as 'full_name',P.Pro_Title,L.Les_StartHour,L.Les_EndHour,L.Les_day,L.Les_MaxQuan FROM  [dbo].[Lesson] L with(nolock) inner join [dbo].[Profession] P with(nolock) on P.Pro_Id= L.Les_Pro_Id  inner join [dbo].[Teacher] T with(nolock) on L.Les_Tea_Id=T.Tea_Id WHERE L.Les_day =" + Convert.ToInt16(day);



            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Tigburim t = new Tigburim();

                //object building

                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.EndTime = Convert.ToString(dr["Les_EndHour"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);
                t.Limit = (int)(dr["Les_MaxQuan"]);
                t.TeacherName = Convert.ToString(dr["full_name"]);
                t.TrueId = Convert.ToInt16(dr["Les_Id"]);



                TigburimList.Add(t);

            }
            return TigburimList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    //===============================check if request has been made already==========================================
    public int checkRequest(Request re)
    {
        string tempDate = (re.Req_actLes_date).ToString("yyyy-MM-dd");
        string checkDate = " ";
        Request check = new Request();
        int doesExist = 0;
        SqlConnection con = null;
        try
        {
            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select * from dbo.Requests where req_stu_id ='" + Convert.ToInt16(re.Req_stu_id) + "' and req_actLes_date = '" + tempDate + "' and req_actLes_id = '" + Convert.ToInt16(re.Req_actLes_id) + "' and req_type='" + Convert.ToInt16(re.Req_type) + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                check.Req_actLes_id = (int)dr["req_actLes_id"];
                check.Req_stu_id = (int)dr["req_stu_id"];
                DateTime temp = Convert.ToDateTime(dr["req_actLes_date"]);
                checkDate = temp.ToString("yyyy-MM-dd");
                check.Req_type = Convert.ToInt16(dr["req_type"]);

            }

            if (check.Req_stu_id == re.Req_stu_id && check.Req_actLes_id == re.Req_actLes_id && tempDate == checkDate && re.Req_type == check.Req_type)
            {
                doesExist = 1;
            }

            return doesExist;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //פונקציה שמביאה רשימה של תלמידים לתגבור ספציפי
    public List<Student> getStuListCAL(string id, string date)
    {
        List<Student> stuList = new List<Student>();
        SqlConnection con = null;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT (S.Stu_FirstName +' '+ S.Stu_LastName) as 'full_name' from [dbo].[Student] S inner join  [dbo].[signedToLesson] STL on STL.StLes_stuId = S.Stu_Id inner join ActualLesson A on A.ActLes_LesId = STL.StLes_ActLesId and A.ActLes_date = STL.StLes_ActLesDate where A.ActLes_date ='" + date + "' and A.ActLes_LesId =" + Convert.ToInt16(id) + " ";
            

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Student stu = new Student();
                stu.FirstName = dr["full_name"].ToString();
                stuList.Add(stu);
            }
            return stuList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public int InsertAvailabilityDB(Teacher teaAvailability)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertAvailabilityCommand(teaAvailability);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a lesson command String
    //--------------------------------------------------------------------
    private String BuildInsertAvailabilityCommand(Teacher teaAvailability)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", teaAvailability.Tea_id, teaAvailability.Day, teaAvailability.StartHour, teaAvailability.EndHour);
        String prefix = "INSERT INTO TeacherAvailability  " + "( Ave_TeaId, Ave_day, Ave_startHour, Ave_endtHour) ";
        command = prefix + sb.ToString();

        return command;
    }



    public Int32 GetNumOfClassForStuDB(double stu_id, string conString) //מספר התגבורים שהתלמיד רשום אליהם וכבר עברו
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count (1) from signedToLesson where [StLes_stuId]= '" + stu_id + "' and [StLes_ActLesDate] <= GETDATE()-1";
            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public Int32 GetNumOfPresenceForStuDB(double stu_id, string conString)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count (1) from signedToLesson where [StLes_stuId]= '" + stu_id + "' and [StLes_ActLesDate] <= GETDATE()-1 and Presence=1";
            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public Int32 GetNumnotPresForStuDB(double stu_id, string conString)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select count (1) from signedToLesson where [StLes_stuId]= '" + stu_id + "' and [StLes_ActLesDate] <= GETDATE()-1 and Presence=0";
            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int counter = (int)myCommand.ExecuteScalar();
            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //.....................................................................
    //תחילת פונקציות מודול הודעות
    //.....................................................................

    //insert message to ManagerMessages table
    public int InsertMessage(Messages message)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandMessage(message);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandMessage(Messages message)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}')", message.Msg_fromManagerId, message.Msg_toStudentId, message.Msg_subject, message.Msg_content, message.Msg_hasRead, message.Msg_date);
        String prefix = "INSERT INTO ManagerMessages  " + "( Msg_fromManagerId , Msg_toStudentId, Msg_subject, Msg_content ,Msg_hasRead, Msg_date) ";
        command = prefix + sb.ToString();

        return command;
    }


    //insert message to StudentMessages table
    public int InsertMessageForStudent(Messages message)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandMessageForStudent(message);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandMessageForStudent(Messages message)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}')", message.Msg_fromStudentId, message.Msg_toManagerId, message.Msg_subject, message.Msg_content, message.Msg_hasRead, message.Msg_date);
        String prefix = "INSERT INTO StudentMessages  " + "( Msg_fromStudentId , Msg_toManagerId, Msg_subject, Msg_content ,Msg_hasRead, Msg_date) ";
        command = prefix + sb.ToString();

        return command;
    }

    //insert message to TeacherMessages table
    public int InsertMessageForTeacher(Messages message)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandMessageForTeacher(message);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandMessageForTeacher(Messages message)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}')", message.Msg_fromTeacherId, message.Msg_toManagerId, message.Msg_subject, message.Msg_content, message.Msg_hasRead, message.Msg_date);
        String prefix = "INSERT INTO TeacherMessages  " + "( Msg_fromTeacherId , Msg_toManagerId, Msg_subject, Msg_content ,Msg_hasRead, Msg_date) ";
        command = prefix + sb.ToString();

        return command;
    }

    //insert message to ManagerMessagesToTeachers table
    public int InsertMessageFromManagerToTeacher(Messages message)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandMessageFromManagerToTeacher(message);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a student command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandMessageFromManagerToTeacher(Messages message)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}')", message.Msg_fromManagerId, message.Msg_toTeacherId, message.Msg_subject, message.Msg_content, message.Msg_hasRead, message.Msg_date);
        String prefix = "INSERT INTO ManagerMessagesToTeachers  " + "( Msg_fromManagerId , Msg_toTeacherId, Msg_subject, Msg_content ,Msg_hasRead, Msg_date) ";
        command = prefix + sb.ToString();

        return command;
    }

    public DBServices getMessageContent(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public DBServices getMessageContentForStudent(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public DBServices getMessageContentForTeacher(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public int UpdateMessageStatus(int msg_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageStatus(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageStatus(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE StudentMessages SET msg_hasRead= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }

    public int UpdateMessageStatusForStudent(int msg_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageStatusForStudent(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageStatusForStudent(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessages SET msg_hasRead= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }

    public int UpdateMessageStatusForTeacher(int msg_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageStatusForTeacher(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageStatusForTeacher(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessagesToTeachers SET msg_hasRead= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateMessageStatusAtTeacherMessages(int msg_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageStatusAtTeacherMessages(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageStatusAtTeacherMessages(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE TeacherMessages SET msg_hasRead= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public DBServices getOutMessageContent(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }


    public DBServices getOutMessageContentFromManagerMessagesToTeachers(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
    }


    public DBServices getOutMessageContentForStudent(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }

    public DBServices getOutMessageContentForTeacher(string conString, string tableName, int msg_id)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT msg_content FROM " + tableName + " WHERE msg_id = '" + msg_id + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public int UpdateIsDeletedInMessageForManager(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE StudentMessages SET msg_IsDeletedForManager= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateIsDeletedInMessageForManagerAtTeacherMessages(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageIsDeletedForManagerAtTeacherMessages(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageIsDeletedForManagerAtTeacherMessages(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE TeacherMessages SET msg_IsDeletedForManager= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }

    public int UpdateIsDeletedOutMessageForManager(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandOutMessageIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandOutMessageIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessages SET msg_IsDeletedForManager= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateIsDeletedOutMessageForManagerToTeacher(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandOutMessageIsDeletedForManagerToTeacher(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandOutMessageIsDeletedForManagerToTeacher(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessagesToTeachers SET msg_IsDeletedForManager= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateIsDeletedInMessageForStudent(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandMessageForStudentIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandMessageForStudentIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessages SET msg_IsDeletedForStudent= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateIsDeletedOutMessageForStudent(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandOutMessageForStudentIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandOutMessageForStudentIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE StudentMessages SET msg_IsDeletedForStudent= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }

    public int UpdateIsDeletedInMessageForTeacher(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandInMessageForTeacherIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandInMessageForTeacherIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessagesToTeachers SET msg_IsDeletedForTeacher= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateIsDeletedOutMessageForTeacher(int msg_id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandOutMessageForTeacherIsDeleted(msg_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandOutMessageForTeacherIsDeleted(double msg_id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE TeacherMessages SET msg_IsDeletedForTeacher= '" + 1 + "' WHERE msg_id='" + msg_id + "'";
        return command;
    }


    public int UpdateHasReadForAllRecordsForManager()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandHasReadForAllRecordsForManager();      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandHasReadForAllRecordsForManager()
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE StudentMessages SET msg_hasRead= 1 ";
        return command;
    }

    public int UpdateHasReadForAllRecordsForManagerFromTeachers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandHasReadForAllRecordsForManagerFromTeachers();      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandHasReadForAllRecordsForManagerFromTeachers()
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE TeacherMessages SET msg_hasRead= 1 ";
        return command;
    }

    public int UpdateHasReadForAllRecordsForStudent(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandHasReadForAllRecordsForStudent(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandHasReadForAllRecordsForStudent(double id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessages SET msg_hasRead= 1 where msg_toStudentId = '" + id + "'";
        return command;
    }

    public int UpdateHasReadForAllRecordsForTeacher(double id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommandHasReadForAllRecordsForTeacher(id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a student entitled status command String
    //--------------------------------------------------------------------
    private String BuildUpdateCommandHasReadForAllRecordsForTeacher(double id)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ManagerMessagesToTeachers SET msg_hasRead= 1 where msg_toTeacherId = '" + id + "'";
        return command;
    }


    //הכנסת הודעת מערכת חדשה למנהלת מה"תלמיד"
    public int InsertMessageSystemStudentDB(Messages message)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommandMessageSystemStudent(message);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert a system msg command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandMessageSystemStudent(Messages message)
    {
        String command;


        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}' , '{4}','{5}', '{6}')", message.Msg_fromStudentId, message.Msg_toManagerId, message.Msg_subject, message.Msg_content, message.Msg_hasRead, message.Msg_date, message.System_msg);
        String prefix = "INSERT INTO StudentMessages  " + "( Msg_fromStudentId , Msg_toManagerId, Msg_subject, Msg_content ,Msg_hasRead, Msg_date, system_msg) ";
        command = prefix + sb.ToString();

        return command;
    }

    //.....................................................................
    //סיום פונקציות מודול הודעות
    //.....................................................................

    // getting metagber tigburim by id
    public List<Tigburim> getTigburimListForTeacher(string conString, string tableName, string id)
    {
        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT DISTINCT al.number as 'SingID', t.[Tea_Id], (t.[Tea_FirstName] +' '+ t.[Tea_LastName]) as 'full_name', al.[actls_cancelled], pro.Pro_Id, pro.[Pro_Title], les.[Les_MaxQuan], al.quantity, les.[Les_StartHour], les.[Les_EndHour], al.[ActLes_date] FROM Lesson les INNER JOIN[ActualLesson] al on les.Les_Id = al.ActLes_LesId INNER JOIN[Profession] pro on pro.Pro_Id = les.Les_Pro_Id INNER JOIN[Teacher] t ON t.Tea_Id = les.Les_Tea_Id where t.[Tea_Id]= '" + id + "' and al.[actls_cancelled] = 0";




            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Tigburim t = new Tigburim();
                string[] dateFormat1 = { };
                dateFormat1 = Convert.ToString(dr["ActLes_date"]).Split(' ');


                //object building
                t.Id = Convert.ToInt16(dr["SingID"]);
                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.EndTime = Convert.ToString(dr["Les_EndHour"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);
                t.Limit = (int)(dr["Les_MaxQuan"]);
                t.ProfId = (int)(dr["Pro_Id"]);
                t.TeacherName = Convert.ToString(dr["full_name"]);
                t.TeacherId = Convert.ToDouble(dr["Tea_Id"]);
                t.ActualLimit = (int)(dr["Les_MaxQuan"]) - (int)(dr["quantity"]);

                //date formatting
                string[] dateFormat2 = dateFormat1[0].Split('-');

                t.TigburDate = dateFormat2[2] + "-" + dateFormat2[1] + "-" + dateFormat2[0];



                TigburimList.Add(t);

            }
            return TigburimList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //===============================check if request has been made already==========================================
    public int CheckTeacherRequestCAL(teachersRequests tr)
    {
        string tempDate = (tr.LesDate).ToString("yyyy-MM-dd");
        SqlConnection con = null;
        int counter = 0;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "SELECT * FROM [dbo].[TeacherRequests] WHERE TeaReq__LessId ='" + tr.LesId + "' and TeaReq_TeaId ='" + tr.TeaId + "' and TeaReq__LessDate = '" + tempDate + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row  
                counter++;
            }

            return counter;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }


    public int getHoursCount(string conString, double tea_id)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "Select ISNULL(sum(DateDiff(SECOND, Les_StartHour, Les_EndHour) / 3600.0),0) From Lesson inner join ActualLesson on Les_Id = ActLes_LesId WHERE Les_Tea_Id = '" + tea_id + "' and actls_cancelled = 0 and ActLes_date<= GETDATE()";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int SumOfHours = Convert.ToInt32(myCommand.ExecuteScalar());
            return SumOfHours;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public int getAttendanceFormCount(string conString, double tea_id)
    {
        SqlConnection con = connect(conString);
        try
        {
            string myScalarQuery = "select ISNULL(count(1),0) from Lesson inner join ActualLesson on Les_Id=ActLes_LesId where ActLes_date<=GETDATE() and actls_cancelled=0 and Attendance_Form=0 and Les_Tea_Id='" + tea_id + "'";

            SqlCommand myCommand = new SqlCommand(myScalarQuery, con);
            int SumOfAttendanceForm = Convert.ToInt32(myCommand.ExecuteScalar());
            return SumOfAttendanceForm;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<Report> TeacherHoursByMonths(string startDate, string endDate, string conString, string userId)
    {

        List<Report> TeacherHoursByMonthsCountList = new List<Report>();
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR2 = "Select Les_Tea_Id, month(ActLes_date) as 'month' ,sum(DateDiff(SECOND,Les_StartHour, Les_EndHour) /3600.0) as 'amount' From Lesson inner join ActualLesson on Les_Id=ActLes_LesId WHERE Les_Tea_Id = '" + userId + "' and actls_cancelled=0 and ActLes_date<=GETDATE() group by month(ActLes_date),Les_Tea_Id ";
            SqlCommand cmd = new SqlCommand(selectSTR2, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                Report r = new Report();
                r.Id = Convert.ToInt32(dr["Les_Tea_Id"]);
                r.Month = (Int32)dr["month"];
                r.Amount = Convert.ToDouble(dr["amount"]);

                TeacherHoursByMonthsCountList.Add(r);

            }
            return TeacherHoursByMonthsCountList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //getting student list and 
    public List<Tigburim> getStudentList4MessagesDB(string id, string date)
    {

        List<Tigburim> TigburimList = new List<Tigburim>();
        SqlConnection con = null;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "select StLes_stuId,Les_Tea_Id,Pro_Title,Les_StartHour from [dbo].[signedToLesson] inner join [dbo].[Lesson] on Les_Id = StLes_ActLesId inner join Profession on Les_Pro_Id = Pro_Id where StLes_ActLesId ='" + id + "' and StLes_ActLesDate = '" + date + "'";




            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end




            while (dr.Read())
            {   // Read till the end of the data into a row               
                Tigburim t = new Tigburim();


                //object building
                t.StartTime = Convert.ToString(dr["Les_StartHour"]);
                t.TeacherId = Convert.ToDouble(dr["Les_Tea_Id"]);
                t.StuId = Convert.ToInt32(dr["StLes_stuId"]);
                t.ProfName = Convert.ToString(dr["Pro_Title"]);

                TigburimList.Add(t);

            }
            return TigburimList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public double getTeacherId4delete(string id)
    {

        double teachId = 0;
        SqlConnection con = null;
        try
        {

            con = connect("studentDBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            String selectSTR;

            selectSTR = "select Les_Tea_Id from [dbo].[Lesson] where Les_Id ='" + id + "'";




            SqlCommand cmd = new SqlCommand(selectSTR, con);
            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row               
                teachId = Convert.ToDouble(dr["Les_Tea_Id"]);

            }

            return teachId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public DBServices readManagerDB(string conString, string tableName, double ManId)
    {

        DBServices dbS = new DBServices(); // create a helper class
        SqlConnection con = null;

        try
        {
            con = dbS.connect(conString); // open the connection to the database

            String selectStr = "SELECT * FROM " + tableName + " WHERE Man_Id = '" + ManId + "'";
            SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);  // Fill the datatable (in the dataset), using the Select command

            DataTable dt = ds.Tables[0];

            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;

            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }


        }
    }


    //מחיקת כל הבקשות להרשמה לתגבור, שהן עדיין ממתינות, עבור תלמיר שהופך ללא זכאי 
    public int deleteRequestsDB(double stu_id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = deleteRequestsCommand(stu_id);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the delete a student command String
    //--------------------------------------------------------------------
    private String deleteRequestsCommand(double stu_id)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        
        command = "delete from requests where req_stu_id= '" + stu_id + "' and ([req_status] = 2 or[req_status] = 1) and req_actLes_date >='"+(DateTime.Now).ToString("yyyy-MM-dd")+"'";
        return command;
    }





    public int readSpecificActualLessonQuanDB(int lesId, string lesDate)
    {
        SqlConnection con = null;
        int quan = 0;

        try
        {
            con = connect("studentDBConnectionString"); // open the connection to the database

            String selectStr = "SELECT [quantity] FROM [dbo].[ActualLesson] WHERE [ActLes_LesId] = '" + lesId + "' and [ActLes_date]='" + lesDate + "'";
            SqlCommand cmd = new SqlCommand(selectStr, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
               quan = (int)dr["[quantity]"];

            }
            return quan;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public int reduceQuanDB(int lesId, string lesDate)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("studentDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildreduceQuanCommand(lesId, lesDate);      // helper method to build the Insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the update a request command String
    //--------------------------------------------------------------------
    private String BuildreduceQuanCommand(int les_numer, string lesDate)
    {
        // use a string builder to create the dynamic string
        String command = "UPDATE ActualLesson SET quantity=quantity-1 WHERE ActLes_LesId='" + les_numer + "' AND ActLes_date='" + lesDate + "'";
        return command;
    }
}