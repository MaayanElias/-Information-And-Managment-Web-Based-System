using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Manager
/// </summary>
public class Manager
{
    private double man_id;
    private string man_firstName;
    private string man_lastName;
    private string man_phoneNumber;
    private string man_email;
    private string man_address;
    private bool man_status;
    private string man_password;

    public Manager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public double Man_id
    {
        get
        {
            return man_id;
        }

        set
        {
            man_id = value;
        }
    }

    public string Man_firstName
    {
        get
        {
            return man_firstName;
        }

        set
        {
            man_firstName = value;
        }
    }

    public string Man_lastName
    {
        get
        {
            return man_lastName;
        }

        set
        {
            man_lastName = value;
        }
    }

    public string Man_phoneNumber
    {
        get
        {
            return man_phoneNumber;
        }

        set
        {
            man_phoneNumber = value;
        }
    }

    public string Man_email
    {
        get
        {
            return man_email;
        }

        set
        {
            man_email = value;
        }
    }

    public string Man_address
    {
        get
        {
            return man_address;
        }

        set
        {
            man_address = value;
        }
    }

    public bool Man_status
    {
        get
        {
            return man_status;
        }

        set
        {
            man_status = value;
        }
    }

    public string Man_password
    {
        get
        {
            return man_password;
        }

        set
        {
            man_password = value;
        }
    }


    public Manager readSpecificUserManager(double username_id, string password)
    {
        DBServices dbs = new DBServices();
        Manager DBuser = dbs.readSpecificUserManagerDB(username_id, password, "studentDBConnectionString", "Manager");

        if (DBuser == null)
            return null;
        else
            return DBuser;
    }

    public DataTable readManager(double ManId)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.readManagerDB("studentDBConnectionString", "[Manager]", ManId);
        return dbs.dt;
    }

}