using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
    int id;
    string pro_title;
    double amount;
    int month;

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

    public double Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public string Pro_title
    {
        get
        {
            return pro_title;
        }

        set
        {
            pro_title = value;
        }
    }

    public int Month
    {
        get
        {
            return month;
        }

        set
        {
            month = value;
        }
    }

    public Report()
    {
            
    }

    public List<Report> getProfessionCount(string startDate,string endDate)// מחזיר כמות תגבורים לפי מקצועות
    {
        DBServices dbsReport = new DBServices();
        List<Report> dbCountProfessionReport = dbsReport.getProfessionCount(startDate,endDate,"studentDBConnectionString");
        return dbCountProfessionReport;
    }

    public List<Report> StudentRequestsByProfession(string startDate, string endDate, string userId)// מחזיר כמות בקשות ממתינות לתלמיד לפי מקצועות
    {
        DBServices dbsReport = new DBServices();
        List<Report> dbStudentRequestsByProfession = dbsReport.StudentRequestsByProfession(startDate, endDate, "studentDBConnectionString", userId);
        return dbStudentRequestsByProfession;
    }


    public List<Report> StudentClassesByProfession(string startDate, string endDate, string userId)// מחזיר כמות בקשות ממתינות לתלמיד לפי מקצועות
    {
        DBServices dbsReport = new DBServices();
        List<Report> dbStudentRequestsByProfession = dbsReport.StudentClassesByProfession(startDate, endDate, "studentDBConnectionString", userId);
        return dbStudentRequestsByProfession;
    }


    public int getRequestsCount()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getRequestsCount("studentDBConnectionString");
        return numAffected;

    }

    public int getAttendenceFormsCount()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getAttendenceFormsCount("studentDBConnectionString");
        return numAffected;

    }

    public int getUnreadMessagesCount()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getUnreadMessagesCount("studentDBConnectionString");
        return numAffected;

    }
    public int getUnreadMessagesCountForStudent(string userID)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getUnreadMessagesCountForStudent("studentDBConnectionString",userID);
        return numAffected;

    }


    public int getUnreadMessagesCountForTeacher(string tea_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getUnreadMessagesCountForTeacher("studentDBConnectionString",tea_id);
        return numAffected;

    }


    public int getHoursCount(int tea_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getHoursCount("studentDBConnectionString", tea_id);
        return numAffected;

    }

    public int getAttendanceFormCount(int tea_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.getAttendanceFormCount("studentDBConnectionString", tea_id);
        return numAffected;

    }

    public List<Report> TeacherHoursByMonths(string startDate, string endDate, string userId)//מחזיר כמות שעות עבודה של מתגבר לפי חודשים 
    {
        DBServices dbsReport = new DBServices();
        List<Report> dbTeacherHoursByMonths = dbsReport.TeacherHoursByMonths(startDate, endDate, "studentDBConnectionString", userId);
        return dbTeacherHoursByMonths;
    }

}