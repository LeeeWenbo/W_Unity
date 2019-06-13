using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DcConf
{


    public string version;
    public string sqlUrl;
    public int sqlPort;
    public string sqlUser;
    public string sqlPassword;
    public string sqlDb;
    public string equipTable;


    private DcConf(){}
    private static DcConf _instance;
    public static DcConf Ins
    {
        get
        {
            if (_instance == null)
                _instance = GetBdzConfModel();
            return _instance;
        }
    }


    public static DcConf GetBdzConfModel(string pathName="")
    {
        if (string.IsNullOrEmpty(pathName))
        {
            pathName = Application.streamingAssetsPath + "/Conf/DcConf.json";
        }
        string json = U_IO.ReadByFileToString(pathName);
        try
        {
            var jsonData = JsonMapper.ToObject(json);
            DcConf model = new DcConf
            {
                version = jsonData["version"].ToString(),
                sqlUrl = jsonData["sqlUrl"].ToString(),
                sqlPort = Convert.ToInt32(jsonData["sqlPort"].ToString()),
                sqlUser = jsonData["sqlUser"].ToString(),
                sqlPassword = jsonData["sqlPassword"].ToString(),
                sqlDb = jsonData["sqlDb"].ToString(),
                equipTable = jsonData["equipTable"].ToString()
            };
            return model;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}
