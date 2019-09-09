using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Student
/// </summary>
public class Student
{


    public Student()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private double stu_id;
    private double stu_instructor_id;
    private string firstName;
    private string lastName;
    private string birthDate;
    private string phoneNumber;
    private string email;
    private string address;
    private bool status;
    private string password;
    private string grade;
    private bool isEntitled;
    private string notes;

    public double Stu_id
    {
        get
        {
            return stu_id;
        }

        set
        {
            stu_id = value;
        }
    }

    public double Stu_Instructor_id
    {
        get
        {
            return stu_instructor_id;
        }

        set
        {
            stu_instructor_id = value;
        }
    }

    public string FirstName
    {
        get
        {
            return firstName;
        }

        set
        {
            firstName = value;
        }
    }

    public string LastName
    {
        get
        {
            return lastName;
        }

        set
        {
            lastName = value;
        }
    }

    public string BirthDate
    {
        get
        {
            return birthDate;
        }

        set
        {
            birthDate = value;
        }
    }

    public string PhoneNumber
    {
        get
        {
            return phoneNumber;
        }

        set
        {
            phoneNumber = value;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }

        set
        {
            email = value;
        }
    }

    public string Address
    {
        get
        {
            return address;
        }

        set
        {
            address = value;
        }
    }

    public bool Status
    {
        get
        {
            return status;
        }

        set
        {
            status = value;
        }
    }

    public string Password
    {
        get
        {
            return password;
        }

        set
        {
            password = value;
        }
    }

    public string Grade
    {
        get
        {
            return grade;
        }

        set
        {
            grade = value;
        }
    }



    public string Notes
    {
        get
        {
            return notes;
        }

        set
        {
            notes = value;
        }
    }

    public bool IsEntitled
    {
        get
        {
            return isEntitled;
        }

        set
        {
            isEntitled = value;
        }
    }

    public Student(double _id, double _Instructor_id, string _firstName, string _lastName, string _birthDate, string _phoneNumber, string _email, string _address, bool _status, string _password, string _grade, bool _isEntitled, string _notes)
    {

        Stu_id = _id; ;
        Stu_Instructor_id = _Instructor_id;
        FirstName = _firstName;
        LastName = _lastName;
        BirthDate = _birthDate;
        PhoneNumber = _phoneNumber;
        Email = _email;
        Address = _address;
        Status = _status;
        Password = _password;
        Grade = _grade;
        IsEntitled = _isEntitled;
        Notes = _notes;

    }




    public int InsertStudent()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertStudent(this);
        return numAffected;

    }

    public int deleteStudent(double id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteStudent(id);
        return numAffected;

    }

    public int updateSpecificStudent()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificStudent(this);
        return numAffected;

    }

    public DataTable readSpecipicStudent(double _stud_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.ReadSpecipicStudentDB("studentDBConnectionString", "[Student]", _stud_id);
        return dbs.dt;
    }

    public int updateIsEntittled(double stu_id, int updateIsEntitledTo)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateIsEntittled(stu_id, updateIsEntitledTo);
        return numAffected;

    }

    public Student readSpecificUserStudent(double username_id, string password)
    {
        DBServices dbs = new DBServices();
        Student DBuser = dbs.readSpecificUserStudentDB(username_id, password, "studentDBConnectionString", "Student");

        if (DBuser == null)
            return null;
        else
            return DBuser;
    }

    public List<Student> getStuListCAL(string id, string date)
    {
        DBServices dbs = new DBServices();
        List<Student> stuList = dbs.getStuListCAL(id, date);
        if (stuList == null) return null;
        else return stuList;
    }

    public int GetNumOfClassForStu(double stuId) //מספר התגבורים שהתלמיד רשום אליהם והתקיימו כבר
    {

        DBServices dbs = new DBServices();
        int num = dbs.GetNumOfClassForStuDB(stuId, "studentDBConnectionString");
        return num;
    }

    public int GetNumOfPresenceForStu(double stuId) //מספר התגבורים שהתלמיד נכח בהם ושהתקיימו כבר
    {

        DBServices dbs = new DBServices();
        int num = dbs.GetNumOfPresenceForStuDB(stuId, "studentDBConnectionString");
        return num;
    }


    public int GetNumnotPresForStu(double stuId) //מספר התגבורים שהתלמיד נכח בהם ושהתקיימו כבר
    {

        DBServices dbs = new DBServices();
        int num = dbs.GetNumnotPresForStuDB(stuId, "studentDBConnectionString");
        return num;
    }

    public DataTable getStudentCounter(double _stud_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getStudentCounterDB("studentDBConnectionString", "[Student]", _stud_id);
        return dbs.dt;
    }


    public int updateStudentCounter(double stu_id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.updateStudentCounterDB(stu_id);
        return numAffected;

    }

    public int updateStu_CounterToZero(double stu_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateStu_CounterToZero(stu_id);
        return numAffected;

    }

}