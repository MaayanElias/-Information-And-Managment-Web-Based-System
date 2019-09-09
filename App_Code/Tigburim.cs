using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Tigburim
/// </summary>
public class Tigburim
{
    private int id;
    private int trueId;
    private double teacherId;
    private string teacherName;
    private int profId;
    private string profName;
    private int limit;
    private int actualLimit;
    private string startTime;
    private string endTime;
    private string tigburDate;
    private int isSigned;
    private int stuId;

    public int StuId
    {
        get
        {
            return stuId;
        }

        set
        {
            stuId = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public double TeacherId
    {
        get
        {
            return teacherId;
        }

        set
        {
            teacherId = value;
        }
    }

    public string TeacherName
    {
        get
        {
            return teacherName;
        }

        set
        {
            teacherName = value;
        }
    }

    public int ProfId
    {
        get
        {
            return profId;
        }

        set
        {
            profId = value;
        }
    }

    public string ProfName
    {
        get
        {
            return profName;
        }

        set
        {
            profName = value;
        }
    }

    public int Limit
    {
        get
        {
            return limit;
        }

        set
        {
            limit = value;
        }
    }

    public int ActualLimit
    {
        get
        {
            return actualLimit;
        }

        set
        {
            actualLimit = value;
        }
    }

    public string StartTime
    {
        get
        {
            return startTime;
        }

        set
        {
            startTime = value;
        }
    }

    public string EndTime
    {
        get
        {
            return endTime;
        }

        set
        {
            endTime = value;
        }
    }

    public string TigburDate
    {
        get
        {
            return tigburDate;
        }

        set
        {
            tigburDate = value;
        }
    }

    public int TrueId
    {
        get
        {
            return trueId;
        }

        set
        {
            trueId = value;
        }
    }

    public int IsSigned
    {
        get
        {
            return isSigned;
        }

        set
        {
            isSigned = value;
        }
    }

    public Tigburim()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<Tigburim> getTigburimList(string conStr, string tableName)
    {
        DBServices dbs = new DBServices();
        List<Tigburim> dblist = dbs.getTigburimList(conStr, tableName);
        return dblist;
    }

    public List<Tigburim> checkIfSignedFunc(string id, List<Tigburim> tList)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.checkIfSignedFunc(id);
        List<Tigburim> TigburimList = new List<Tigburim>();
        foreach (Tigburim tig in tList)
        {
            foreach (DataRow dr in dbs.dt.Rows)
            {   // Read till the end of the data into a row               
                string[] dateFormat1 = { };
                dateFormat1 = Convert.ToString(dr["StLes_ActLesDate"]).Split(' ');
                //date formatting
                string[] dateFormat2 = dateFormat1[0].Split('-');
                string checkDate = dateFormat2[2] + "-" + dateFormat2[1] + "-" + dateFormat2[0];
                if (tig.TrueId == (int)(dr["StLes_ActLesId"]) && tig.TigburDate == checkDate)
                {
                    tig.IsSigned = 1;
                    break;
                }
            }
            TigburimList.Add(tig);
        }
        return TigburimList;
    }



    public List<Tigburim> getTigburimListForStudent(string conString, string tableName, string id)
    {
        DBServices dbs = new DBServices();
        List<Tigburim> dblist = dbs.getTigburimListForStudent(conString, tableName, id);
        return dblist;
    }


    public Tigburim getTigburById(string tigID)
    {
        int TigID = Convert.ToInt16(tigID);
        DBServices dbs = new DBServices();
        Tigburim DBTigbur = dbs.getTigburById(TigID, "studentDBConnectionString", "Lesson");
        if (DBTigbur == null) return null;
        else return DBTigbur;
    }

    public int delSpecTig(string id, string date)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteTigbur(id, date);
        return numAffected;
    }

    public int deleteSTL(string id, string date)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteSTL(id, date);
        return numAffected;
    }



    public List<Tigburim> getTigburimTemplate(string date)
    {
        DBServices dbs = new DBServices();
        List<Tigburim> dblist = dbs.getTigburimListTemplate(date);
        return dblist;
    }


    public List<Tigburim> getTigburimListForTeacher(string conString, string tableName, string id)
    {
        DBServices dbs = new DBServices();
        List<Tigburim> dblist = dbs.getTigburimListForTeacher(conString, tableName, id);
        return dblist;
    }


    public List<Tigburim> getStudentList4Messages(string tigId, string date)
    {
        DBServices dbs = new DBServices();
        List<Tigburim> dblist = dbs.getStudentList4MessagesDB(tigId, date);
        return dblist;
    }

}