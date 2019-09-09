using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Stutructor
/// </summary>
public class Instructor
{
    public Instructor()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private double ins_id;
    private string ins_firstName;
    private string ins_lastName;
    private string ins_phoneNumber;
    private string ins_email;
    private string ins_addres;
    private bool ins_status;
    private string ins_password;

    public double Ins_id
    {
        get
        {
            return ins_id;
        }

        set
        {
            ins_id = value;
        }
    }

    public string Ins_firstName
    {
        get
        {
            return ins_firstName;
        }

        set
        {
            ins_firstName = value;
        }
    }

    public string Ins_lastName
    {
        get
        {
            return ins_lastName;
        }

        set
        {
            ins_lastName = value;
        }
    }

    public string Ins_phoneNumber
    {
        get
        {
            return ins_phoneNumber;
        }

        set
        {
            ins_phoneNumber = value;
        }
    }

    public string Ins_email
    {
        get
        {
            return ins_email;
        }

        set
        {
            ins_email = value;
        }
    }

    public string Ins_addres
    {
        get
        {
            return ins_addres;
        }

        set
        {
            ins_addres = value;
        }
    }

    public bool Ins_status
    {
        get
        {
            return ins_status;
        }

        set
        {
            ins_status = value;
        }
    }

   

    public string Ins_password
    {
        get
        {
            return ins_password;
        }

        set
        {
            ins_password = value;
        }
    }


    public Instructor(double _Ins_id, string _firstName, string _lastName, string _phoneNumber, string _email, string _addres, bool _status, string _password)
    {
        Ins_id = _Ins_id;
        Ins_firstName = _firstName;
        Ins_lastName = _lastName;
        Ins_phoneNumber = _phoneNumber;
        Ins_email = _email;
        Ins_addres = _addres;
        Ins_status = _status;
        Ins_password = _password;
    }

    public int InsertInstructor()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertInstructor(this);
        return numAffected;

    }

    public int deleteInstructorFromStudent(double id)
    {

        DBServices dbs = new DBServices();
        int numAffected1 = dbs.deleteInstructorFromStudent(id);
        return numAffected1;

    }

    public int deleteInstructor(double id)
    {

        DBServices dbs = new DBServices();
        int numAffected2 = dbs.deleteInstructor(id);
        return numAffected2;

    }

    public int updateSpecificInstructor()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificInstructor(this);
        return numAffected;

    }



    public DataTable readSpecipicInstructor(double _Ins_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.ReadSpecipicInstructorDB("studentDBConnectionString", "[Instructor]", _Ins_id);
        return dbs.dt;
    }
}