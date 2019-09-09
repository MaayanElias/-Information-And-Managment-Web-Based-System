using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TeachBy
/// </summary>
public class TeachBy
{
    double tea_Id;
    int pro_Id;

    public double Tea_Id
    {
        get
        {
            return tea_Id;
        }

        set
        {
            tea_Id = value;
        }
    }

    public int Pro_Id
    {
        get
        {
            return pro_Id;
        }

        set
        {
            pro_Id = value;
        }
    }

    public TeachBy()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public TeachBy(double tea_id,int pro_id)
    {
        Tea_Id = tea_id;
        Pro_Id = pro_id;
    }

    public int InsertTeachBy()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertTeachBy(this);
        return numAffected;
    }

}