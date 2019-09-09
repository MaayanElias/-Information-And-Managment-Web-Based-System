using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for teachersRequests
/// </summary>
public class teachersRequests
{

    private int lesId;
    private DateTime lesDate;
    private double teaId;
    private int reqStatus;
    private DateTime reqDate;
    private string reqReason;

    public teachersRequests()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int LesId
    {
        get
        {
            return lesId;
        }

        set
        {
            lesId = value;
        }
    }

    public DateTime LesDate
    {
        get
        {
            return lesDate;
        }

        set
        {
            lesDate = value;
        }
    }

    public double TeaId
    {
        get
        {
            return teaId;
        }

        set
        {
            teaId = value;
        }
    }

    public int ReqStatus
    {
        get
        {
            return reqStatus;
        }

        set
        {
            reqStatus = value;
        }
    }

    public DateTime ReqDate
    {
        get
        {
            return reqDate;
        }

        set
        {
            reqDate = value;
        }
    }

    public string ReqReason
    {
        get
        {
            return reqReason;
        }

        set
        {
            reqReason = value;
        }
    }



    public teachersRequests(int les_id, DateTime les_date, double tea_id, int req_Status, DateTime req_date, string req_reason)
    {
        LesId = les_id;
        LesDate = les_date;
        TeaId = tea_id;
        ReqStatus = req_Status;
        ReqDate = req_date;
        ReqReason = req_reason;
    }

    public int InsertTeacherRequest()
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.InsertTeacherRequestDB(this);
        return numAffected;

    }

    public int deleteTeacherRequest(double teaReq_teaId, int teaReq__lessId, DateTime teaReq__lessDate)
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.deleteTeacherRequestDB(teaReq_teaId, teaReq__lessId, teaReq__lessDate);
        return numAffected;

    }

    public int updateSpecificTeacherRequest(int req_num, int status)
    {

        DBServices dbs = new DBServices();
        int numAffected = dbs.updateSpecificTeacherRequestDB(req_num, status);
        return numAffected;

    }

    public int CheckRequestCAL()
    {
        DBServices dbs = new DBServices();
        int numAffected = dbs.CheckTeacherRequestCAL(this);
        return numAffected;

    }
}