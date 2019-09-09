using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for Request
/// </summary>
public class Request
{
    private int req_id;
    private int req_decliend;
    private int req_actLes_id;
    private DateTime req_actLes_date;
    private double req_stu_id;
    private int req_status;
    private int req_is_permanent;
    private int req_type;
    private DateTime req_date;
    private string req_dateSTR;

    public Request()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public int Req_actLes_id
    {
        get
        {
            return req_actLes_id;
        }

        set
        {
            req_actLes_id = value;
        }
    }

    public DateTime Req_actLes_date
    {
        get
        {
            return req_actLes_date;
        }

        set
        {
            req_actLes_date = value;
        }
    }

    public double Req_stu_id
    {
        get
        {
            return req_stu_id;
        }

        set
        {
            req_stu_id = value;
        }
    }

    public int Req_status
    {
        get
        {
            return req_status;
        }

        set
        {
            req_status = value;
        }
    }

    public int Req_is_permanent
    {
        get
        {
            return req_is_permanent;
        }

        set
        {
            req_is_permanent = value;
        }
    }

    public int Req_id
    {
        get
        {
            return req_id;
        }

        set
        {
            req_id = value;
        }
    }

    public int Req_type
    {
        get
        {
            return req_type;
        }

        set
        {
            req_type = value;
        }
    }

    public DateTime Req_date
    {
        get
        {
            return req_date;
        }

        set
        {
            req_date = value;
        }
    }

    public string Req_dateSTR
    {
        get
        {
            return req_dateSTR;
        }

        set
        {
            req_dateSTR = value;
        }
    }

    public int Req_decliend
    {
        get
        {
            return req_decliend;
        }

        set
        {
            req_decliend = value;
        }
    }

    public Request(int actLesId, DateTime actLesDate, double stuId, int reqStatus, int reqIsPermanent)
    {

        Req_actLes_id = actLesId; ;
        Req_actLes_date = actLesDate;
        Req_stu_id = stuId;
        Req_status = reqStatus;
        Req_is_permanent = reqIsPermanent;



    }
    public Request(int actLesId, DateTime actLesDate, double stuId, int reqStatus, int reqIsPermanent, int reqType)
    {

        Req_actLes_id = actLesId; ;
        Req_actLes_date = actLesDate;
        Req_stu_id = stuId;
        Req_status = reqStatus;
        Req_is_permanent = reqIsPermanent;
        Req_type = reqType;


    }

    public Request(int actLesId, DateTime actLesDate, double stuId, int reqStatus, int reqIsPermanent, int reqType, DateTime req_date)
    {

        Req_actLes_id = actLesId; ;
        Req_actLes_date = actLesDate;
        Req_stu_id = stuId;
        Req_status = reqStatus;
        Req_is_permanent = reqIsPermanent;
        Req_type = reqType;
        Req_date = req_date;


    }

    public int updateSpecificRequest(int req_num, int status)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificRequest(req_num, status);
        return numAffected;

    }

    public int InsertRequest()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertRequest(this);
        return numAffected;

    }


    public DataTable ReadSpecificRequest(int req_num)
    {

        DBServices dbs = new DBServices();
        dbs = dbs.readSpecificRequestDB("studentDBConnectionString", "[Requests]", req_num);
        return dbs.dt;
    }





    public int updateReqDecliendField(int req_num, int req_decliend)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.updateReqDecliendFieldDB(req_num, req_decliend);
        return numAffected;

    }


    public int updateDecfullField(int req_num, int dec_full)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.updatedDecfullFieldDB(req_num, dec_full);
        return numAffected;

    }


    //מחיקת כל הבקשות להרשמה לתגבור, שהן עדיין ממתינות, עבור תלמיר שהופך ללא זכאי 
    public int deleteRequests(double stuID)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteRequestsDB(stuID);
        return numAffected;

    }




    //===============================request delete via FullCalendar==========================================

    public int deleteRequest(double stu_id, int les_id, DateTime act_les_date)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteRequest(stu_id, les_id, act_les_date);
        return numAffected;

    }

    public int deleteCancelldLessonRequests(int les_id, string act_les_date)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteCancelldLessonRequestsDB(les_id, act_les_date);
        return numAffected;

    }
    //===============================requestS delete 4 tigbur delete via FullCalendar==========================================
    public int deleteRequests(string id, string date)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteRequests(id, date);
        return numAffected;

    }

    //===============================request insert via FullCalendar==========================================
    public int InsertRequestCal(string id, string date, string stuId, string status, string perm, string sub_date, string type)
    {
        Request re = new Request();
        re.Req_actLes_id = Convert.ToInt16(id);
        re.Req_actLes_date = Convert.ToDateTime(date);
        re.Req_stu_id = Convert.ToDouble(stuId);
        re.Req_status = Convert.ToInt16(status);
        re.Req_is_permanent = Convert.ToInt16(perm);
        re.Req_dateSTR = sub_date;
        re.Req_type = Convert.ToInt16(type);
        DBServices dbs = new DBServices();
        //function to check if the request is already made
        int check = dbs.checkRequest(re);
        if (check > 0) return -1;
        int numAffected = dbs.InsertRequestCal(re);
        return numAffected;

    }
}

