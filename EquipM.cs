/*Name:		 				EquipM	
 *Description: 				
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EquipM
{
    public int id { get; set; }
    public int goId { get; set; }
    public string name { get; set; }
    public string tag { get; set; }
    public string layer { get; set; }
    public bool isStatic { get; set; }
    public bool isHoverOutline { get; set; }
    public bool isClickFlashing { get; set; }
    public string oriPos { get; set; }
    public string oriLocalPos { get; set; }


    private const string tbName = "equip";

    public static bool IsExist(U_MySQL mySql, GameObject go)
    {
        //return mySql.IsExist(DcConf.Ins.equipTable, "goId", go.IdStr());
        return mySql.IsExist("equip", "goId", go.IdStr());
    }

    //增
    public static void Add(U_MySQL mySql, GameObject go, bool isStatic, bool isHoverOutline, bool isClickFlashing)
    {
        Delect(mySql, go);
        mySql.InsertInfo
     (tbName,
     new string[] { "goId", "name", "tag", "layer", "isStatic", "isHoverOutline", "isClickFlashing", "oriPos", "oriLocalPos" },
     new string[] { go.GetInstanceID().ToString(),
                go.name,go.tag, LayerMask.LayerToName(go.layer),
                isStatic.ToTinyint(), isHoverOutline.ToTinyint(), isClickFlashing.ToTinyint(),
                go.transform.position.ToString(),
                go.transform.localPosition.ToString() });
    }
    //删
    public static void Delect(U_MySQL mySql, GameObject go)
    {
        mySql.Delect(tbName, "goId", go.IdStr());
    }

    //改
    public static void Update(U_MySQL mySql, GameObject go, string col, string value)
    {
        mySql.Change(tbName, col, value, "goId", go.IdStr());
    }
    //查
    public static DataRow FindDr(U_MySQL mySql, GameObject go)
    {
        DataTable dt = mySql.Select(tbName, "goId", go.IdStr());
        if (dt.Rows.Count > 0)
            return dt.Rows[0];
        else
            return null;
    }
    public static DataTable FindDt(U_MySQL mySql, GameObject go)
    {
        return mySql.Select(tbName, "goId", go.IdStr());
    }

    public static List<EquipM> FindObjS(U_MySQL mySql, string[] colS,string[] operationS,string[] valueS)
    {
        DataTable dt = mySql.Select(tbName, "goId", go.IdStr());
        return FindObjS(dt);
    }
    public static List<EquipM> FindObjS(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            return U_MySQL.ToModelS<EquipM>(dt);
        }
        else
            return null;
    }
    public static EquipM FindObj(U_MySQL mySql,string[] colS, string[] operationS, string[] valueS)
    {
        DataRow dr = FindDr(mySql, go);
        if (dr != null)
        {
            return U_MySQL.ToModel<EquipM>(dr);
        }
        else
            return null;
    }




    public static void DebugObj(U_MySQL mySql, GameObject go)
    {
        DataTable dt = mySql.Select(tbName, "goId", go.IdStr());
        if (dt.Rows.Count > 0)
        {
            DebugEquip(U_MySQL.ToModel<EquipM>(dt.Rows[0]));
        }
        else
        {
            Debug.Log("无此条记录");
        }
    }



    public static void DebugEquip(EquipM equip)
    {
        U_Debug.LogStr(
            equip.id + "   " + equip.goId + " " + equip.name + "  " + equip.tag + "  " + equip.layer
            + "  " + equip.isStatic + "  " + equip.isHoverOutline + "  " + equip.isClickFlashing
            + "  " + equip.oriPos + "  " + equip.oriLocalPos
            );
    }
}
