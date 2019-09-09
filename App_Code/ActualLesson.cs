using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ActualLesson
/// </summary>
public class ActualLesson
{
    int les_id;
    string act_les_date;
    int quantity;
    int day;


    public int Les_id
    {
        get
        {
            return les_id;
        }

        set
        {
            les_id = value;
        }
    }

    public string Act_les_date
    {
        get
        {
            return act_les_date;
        }

        set
        {
            act_les_date = value;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    public int Day
    {
        get
        {
            return day;
        }

        set
        {
            day = value;
        }
    }

    public ActualLesson()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public ActualLesson(int les_id,string act_les_date, int quantity, int day)
    {
        Les_id = les_id;
        Act_les_date = act_les_date;
        Quantity = quantity;
        Day = day;
    }
    public ActualLesson(int les_id, string act_les_date, int quantity)
    {
        Les_id = les_id;
        Act_les_date = act_les_date;
        Quantity = quantity;
    }

    public int insertActualLesson()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.insertActualLesson(this);
        return numAffected;

    }

    public int updateSpecificActualLesson(int lesId, string lesDate, int actual_quan)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificActualLesson(lesId, lesDate, actual_quan);
        return numAffected;

    }

    public ActualLesson readSpecificActualLesson(int actl_num)
    {
        DBServices dbs = new DBServices();
        ActualLesson DBuser = dbs.readSpecificActualLesson(actl_num, "studentDBConnectionString", "ActualLesson");

        if (DBuser == null)
            return null;
        else
            return DBuser;
    }

    public int readSpecificActualLessonQuan (int lesId, string lesDate)
    {

        DBServices dbs = new DBServices();
        int quan = dbs.readSpecificActualLessonQuanDB(lesId, lesDate);
        return quan;

    }

    public int updateActualLessonAttendancForm(int lesId, DateTime lesDate)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateActualLessonAttendancFormDB(lesId, lesDate);
        return numAffected;

    }

    public int cancelSpecificActualLesson(int lesId, string lesDate, int cancel)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.cancelSpecificActualLessonDB(lesId, lesDate, cancel);
        return numAffected;

    }

    public int reduceQuan(int lesId, string lesDate)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.reduceQuanDB(lesId, lesDate);
        return numAffected;

    }
}