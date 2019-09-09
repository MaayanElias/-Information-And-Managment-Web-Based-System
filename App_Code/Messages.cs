using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Messages
/// </summary>
public class Messages
{
    public Messages()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    // fields of ManagerMessages table
    private double msg_fromManagerId;
    private double msg_toStudentId;

    //// fields of StudentMessages table
    private double msg_fromStudentId;
    private double msg_toManagerId;

    //// fields of TeacherMessages table
    private double msg_toTeacherId;
    private double msg_fromTeacherId;

    //// fields of System messages
    private int system_msg;

    //fields of both
    private string msg_subject;
    private string msg_content;
    private bool msg_hasRead;
    private string msg_date;



    public double Msg_fromManagerId
    {
        get
        {
            return msg_fromManagerId;
        }

        set
        {
            msg_fromManagerId = value;
        }
    }

    public double Msg_toStudentId
    {
        get
        {
            return msg_toStudentId;
        }

        set
        {
            msg_toStudentId = value;
        }
    }

    public string Msg_subject
    {
        get
        {
            return msg_subject;
        }

        set
        {
            msg_subject = value;
        }
    }

    public string Msg_content
    {
        get
        {
            return msg_content;
        }

        set
        {
            msg_content = value;
        }
    }

    public bool Msg_hasRead
    {
        get
        {
            return msg_hasRead;
        }

        set
        {
            msg_hasRead = value;
        }
    }

    public string Msg_date
    {
        get
        {
            return msg_date;
        }

        set
        {
            msg_date = value;
        }
    }

    public double Msg_fromStudentId
    {
        get
        {
            return msg_fromStudentId;
        }

        set
        {
            msg_fromStudentId = value;
        }
    }

    public double Msg_toManagerId
    {
        get
        {
            return msg_toManagerId;
        }

        set
        {
            msg_toManagerId = value;
        }
    }

    public double Msg_toTeacherId
    {
        get
        {
            return msg_toTeacherId;
        }

        set
        {
            msg_toTeacherId = value;
        }
    }

    public double Msg_fromTeacherId
    {
        get
        {
            return msg_fromTeacherId;
        }

        set
        {
            msg_fromTeacherId = value;
        }
    }

    public int System_msg
    {
        get
        {
            return system_msg;
        }

        set
        {
            system_msg = value;
        }
    }


    //בנאי לטבלת ManagerMessages
    public Messages(double _msg_fromManagerId, double _msg_toStudentId, string _msg_subject, string _msg_content, bool _msg_hasRead, string _msg_date)
    {

        Msg_fromManagerId = _msg_fromManagerId;
        Msg_toStudentId = _msg_toStudentId;
        Msg_subject = _msg_subject;
        Msg_content = _msg_content;
        Msg_hasRead = _msg_hasRead;
        Msg_date = _msg_date;

    }

    //הכנסת הודעה חדשה לטבלת ManagerMessages
    public int InsertMessage()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertMessage(this);
        return numAffected;

    }

    //הכנסת הודעה חדשה לטבלת StudentMessages
    public int InsertMessageForStudent()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertMessageForStudent(this);
        return numAffected;

    }


    //הכנסת הודעה חדשה לטבלת TeacherMessages
    public int InsertMessageForTeacher()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertMessageForTeacher(this);
        return numAffected;

    }

    //הכנסת הודעה חדשה לטבלת ManagerMessagesToTeachers
    public int InsertMessageFromManagerToTeacher()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertMessageFromManagerToTeacher(this);
        return numAffected;

    }

    //פונקציה שמביאה את תוכן ההודעה עבור דואר נכנס לדף הודעות מנהלת
    public DataTable getMessageContent(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getMessageContent("studentDBConnectionString", "[StudentMessages]", msg_id);
        return dbs.dt;
    }

    //פונקציה שמביאה את תוכן ההודעה עבור דואר נכנס לדף הודעות תלמיד
    public DataTable getMessageContentForStudent(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getMessageContentForStudent("studentDBConnectionString", "[ManagerMessages]", msg_id);
        return dbs.dt;
    }



    //פונקציה שמביאה את תוכן ההודעה עבור דואר נכנס לדף הודעות מתגבר
    public DataTable getMessageContentForTeacher(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getMessageContentForTeacher("studentDBConnectionString", "[ManagerMessagesToTeachers]", msg_id);
        return dbs.dt;
    }



    // פונקציה שמעדכנת את ססטוס ההודעה לנקראה בטבלת הודעות סטודנט
    public int UpdateMessageStatus(int msg_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateMessageStatus(msg_id);
        return numAffected;
    }

    //  פונקציה שמעדכנת את ססטוס ההודעה לנקראה בטבלת הודעות מנהלת לתלמיד
    public int UpdateMessageStatusForStudent(int msg_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateMessageStatusForStudent(msg_id);
        return numAffected;
    }

    //  פונקציה שמעדכנת את ססטוס ההודעה לנקראה בטבלת הודעות מנהלת למתגבר
    public int UpdateMessageStatusForTeacher(int msg_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateMessageStatusForTeacher(msg_id);
        return numAffected;
    }


    // פונקציה שמעדכנת את ססטוס ההודעה לנקראה בטבלת הודעות מתגבר
    public int UpdateMessageStatusAtTeacherMessages(int msg_id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateMessageStatusAtTeacherMessages(msg_id);
        return numAffected;
    }


    //פונקציה שמביאה את תוכן ההודעה עבור דואר יוצא של המנהלת לתלמיד לדף הודעות מנהלת
    public DataTable getOutMessageContent(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getOutMessageContent("studentDBConnectionString", "[ManagerMessages]", msg_id);
        return dbs.dt;
    }

    //פונקציה שמביאה את תוכן ההודעה עבור דואר יוצא של המנהלת למתגבר לדף הודעות מנהלת
    public DataTable getOutMessageContentFromManagerMessagesToTeachers(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getOutMessageContentFromManagerMessagesToTeachers("studentDBConnectionString", "[ManagerMessagesToTeachers]", msg_id);
        return dbs.dt;
    }


    //פונקציה שמביאה את תוכן ההודעה עבור דואר יוצא לדף הודעות תלמיד
    public DataTable getOutMessageContentForStudent(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getOutMessageContentForStudent("studentDBConnectionString", "[StudentMessages]", msg_id);
        return dbs.dt;
    }

    //פונקציה שמביאה את תוכן ההודעה עבור דואר יוצא לדף הודעות מתגבר
    public DataTable getOutMessageContentForTeacher(int msg_id)
    {
        DBServices dbs = new DBServices();
        dbs = dbs.getOutMessageContentForTeacher("studentDBConnectionString", "[TeacherMessages]", msg_id);
        return dbs.dt;
    }


    //מחיקת הודעות נכנסות של המנהלת- מחיקה מטבלת הודעות תלמידים
    public int UpdateIsDeletedInMessageForManager(int id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedInMessageForManager(id);
        return numAffected;

    }

    //מחיקת הודעות נכנסות של המנהלת- מחיקה מטבלת הודעות מתגברים
    public int UpdateIsDeletedInMessageForManagerAtTeacherMessages(int id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedInMessageForManagerAtTeacherMessages(id);
        return numAffected;

    }
    //מחיקת הודעות יוצאות של המנהלת, מחיקה מטבלת הודעות מנהלת
    public int UpdateIsDeletedOutMessageForManager(int id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedOutMessageForManager(id);
        return numAffected;

    }

    // מחיקת הודעות יוצאות של המנהלת למתגברים, מחיקה מטבלת הודעות מנהלת למתגברים
    public int UpdateIsDeletedOutMessageForManagerToTeacher(int id)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedOutMessageForManagerToTeacher(id);
        return numAffected;

    }

    //מחיקת הודעות נכנסות של התלמיד- מחיקה מטבלת הודעות מנהלת
    public int UpdateIsDeletedInMessageForStudent(int id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedInMessageForStudent(id);
        return numAffected;

    }
    //מחיקת הודעות יוצאות של התלמיד, מחיקה מטבלת הודעות תלמיד
    public int UpdateIsDeletedOutMessageForStudent(int id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedOutMessageForStudent(id);
        return numAffected;

    }

    //מחיקת הודעות נכנסות של המתגבר- מחיקה מטבלת הודעות מנהלת למתגבר
    public int UpdateIsDeletedInMessageForTeacher(int id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedInMessageForTeacher(id);
        return numAffected;

    }

    //מחיקת הודעות יוצאות של המתגבר, מחיקה מטבלת הודעות מתגבר
    public int UpdateIsDeletedOutMessageForTeacher(int id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateIsDeletedOutMessageForTeacher(id);
        return numAffected;

    }

    //עדכון שדה סטטוס הודעה כנקראה עבור כל הרשומות בטבלת הודעות סטודנט
    public int UpdateHasReadForAllRecordsForManager()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateHasReadForAllRecordsForManager();
        return numAffected;
    }

    //עדכון שדה סטטוס הודעה כנקראה עבור כל הרשומות בטבלת הודעות מתגבר
    public int UpdateHasReadForAllRecordsForManagerFromTeachers()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateHasReadForAllRecordsForManagerFromTeachers();
        return numAffected;
    }

    // עדכון שדה סטטוס הודעה כנקראה עבור כל הרשומות בטבלת הודעות מנהלת לתלמידים
    public int UpdateHasReadForAllRecordsForStudent(double id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateHasReadForAllRecordsForStudent(id);
        return numAffected;
    }


    //עדכון שדה סטטוס הודעה כנקראה עבור כל הרשומות בטבלת הודעות מנהלת למתגברים
    public int UpdateHasReadForAllRecordsForTeacher(double id)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.UpdateHasReadForAllRecordsForTeacher(id);
        return numAffected;
    }

    //הכנסת הודעת מערכת חדשה לטבלת StudentMessages
    public int InsertMessageSystemStudent()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertMessageSystemStudentDB(this);
        return numAffected;


    }
}