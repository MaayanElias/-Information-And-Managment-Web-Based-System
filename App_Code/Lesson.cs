using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Lesson
/// </summary>
public class Lesson
{

    private double tea_id;
    private int pro_id;
    private int les_maxQuan;
    private string les_startHour;
    private string les_endHour;
    private int les_day;

    public double Tea_id
    {
        get
        {
            return tea_id;
        }

        set
        {
            tea_id = value;
        }
    }

    public int Pro_id
    {
        get
        {
            return pro_id;
        }

        set
        {
            pro_id = value;
        }
    }

    public int Les_maxQuan
    {
        get
        {
            return les_maxQuan;
        }

        set
        {
            les_maxQuan = value;
        }
    }

    public string Les_startHour
    {
        get
        {
            return les_startHour;
        }

        set
        {
            les_startHour = value;
        }
    }

    public string Les_endHour
    {
        get
        {
            return les_endHour;
        }

        set
        {
            les_endHour = value;
        }
    }

    public int Les_day
    {
        get
        {
            return les_day;
        }

        set
        {
            les_day = value;
        }
    }

    public Lesson()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Lesson(double _tea_id,int _pro_id,int _les_maxQuan,string _les_startHour, string _les_endHour, int _les_day) 
    {
        Tea_id = _tea_id;
        Pro_id = _pro_id;
        Les_maxQuan = _les_maxQuan;
        Les_startHour = _les_startHour;
        Les_endHour = _les_endHour;
        Les_day = _les_day;

    }

    public int InsertLesson()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertLesson(this);
        return numAffected;

    }

    public int deleteLessonFromRequest(int act_les_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteLessonFromRequest(act_les_id);
        return numAffected;

    }

    public int deleteLesson(int act_les_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteLesson(act_les_id);
        return numAffected;

    }

    public DataTable readSpecipicLesson(double _less_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.ReadSpecipicLessonDB("studentDBConnectionString", "[Lesson]", _less_id);
        return dbs.dt;
    }

    public int updateSpecificLesson(int less_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificLesson(this, less_id);
        return numAffected;

    }
}