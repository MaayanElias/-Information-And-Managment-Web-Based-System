using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
/// <summary>
/// Summary description for SignedToLesson
/// </summary>
public class SignedToLesson
{

    private int sigToLes_ActLesId;
    private string sigToLes_ActLesDate;
    private double sigToLess_stuId;
    private int presence;
    public SignedToLesson()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int SigToLes_ActLesId
    {
        get
        {
            return sigToLes_ActLesId;
        }

        set
        {
            sigToLes_ActLesId = value;
        }
    }

    public string SigToLes_ActLesDate
    {
        get
        {
            return sigToLes_ActLesDate;
        }

        set
        {
            sigToLes_ActLesDate = value;
        }
    }

    public double SigToLess_stuId
    {
        get
        {
            return sigToLess_stuId;
        }

        set
        {
            sigToLess_stuId = value;
        }
    }

    public int Presence
    {
        get
        {
            return presence;
        }

        set
        {
            presence = value;
        }
    }

    public SignedToLesson(int LesId, string LesDate, double stuId)
    {

        SigToLes_ActLesId = LesId; ;
        SigToLes_ActLesDate = LesDate;
        SigToLess_stuId = stuId;
    }

    public int InsertSigendToLesson()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertSigendToLesson(this);
        return numAffected;
    }

    public DataTable readStudentsList(int lessId, DateTime lessDate)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.readStudentsListDB("studentDBConnectionString", "[signedToLesson]", lessId, lessDate);
        return dbs.dt;
    }

    public int updateStudentPresence(int lesId, DateTime lesDate, double stuId, int stuPresence)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateStudentPresenceDB(lesId, lesDate, stuId, stuPresence);
        return numAffected;

    }


    public int updateStudentNotes(int lesId, DateTime lesDate, double stuId, string stuNotes)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateStudentNotesDB(lesId, lesDate, stuId, stuNotes);
        return numAffected;

    }

    public int deleteStudentsFromLesson(int LesId, string lesDate)
    {

        DBServices dbs = new DBServices();
        int numAffected1 = dbs.deleteStudentsFromLessonDB(LesId, lesDate);
        return numAffected1;

    }

    //מחיקת תלמיד מהשתתפות בתגבור
    public int deleteStudentFromLesson(double stuId , int LesId, string lesDate)
    {

        DBServices dbs = new DBServices();
        int numAffected1 = dbs.deleteStudentFromLessonDB(stuId, LesId, lesDate);
        return numAffected1;

    }


}
