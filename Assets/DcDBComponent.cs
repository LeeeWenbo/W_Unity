/*Name:		 				DBComponent	
 *Description: 				数据库操作类
 *Author:       			lwb
 *Date:         			2019-06-12
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DcDBComponent : MonoBehaviour
{


    U_MySQL mySql;


    public bool isStatic;
    public bool isHoverOutline;
    public bool isClickFlashing;
    void OpenSql()
    {
        DcConf conf = DcConf.Ins;
        mySql = new U_MySQL(conf.sqlUrl, conf.sqlPort, conf.sqlUser, conf.sqlPassword, conf.sqlDb);
        mySql.OpenSql();
    }
    void CloseSql()
    {
        mySql.Close();
    }


    private void Start()
    {
        OpenSql();
        //DELECT_ALL();
    }
    #region Debug
    public void DebugThis()
    {
        OpenSql();
        EquipM.DebugObj(mySql, gameObject);
    }
    public void DebugSon()
    {
        OpenSql();
        foreach (Transform tran in transform)
        {
            EquipM.DebugObj(mySql, tran.gameObject);
        }
    }
    public void DebugChildren()
    {
        OpenSql();
        foreach (Transform tran in transform.GetComponentsInChildren<Transform>())
        {
            EquipM.DebugObj(mySql, tran.gameObject);
        }
    }

    #endregion

    #region Delect
    public void DelectThis()
    {
        OpenSql();
        EquipM.Delect(mySql, gameObject);
        Debug.Log("删除");
    }
    public void DelectSon()
    {
        OpenSql();
        foreach (Transform tran in transform)
        {
            EquipM.Delect(mySql, tran.gameObject);
        }
        Debug.Log("删除");
    }
    public void DelectChildren()
    {
        OpenSql();
        foreach (Transform tran in transform.GetComponentsInChildren<Transform>())
        {
            EquipM.Delect(mySql, tran.gameObject);
        }
        Debug.Log("删除");
    }
    #endregion

    #region Update
    public void AddThis()
    {
        OpenSql();
        EquipM.Add(mySql, gameObject, isStatic, isHoverOutline, isClickFlashing);
        //Debug.Log("更新");
        //CloseSql();
    }
    public void AddSon()
    {
        OpenSql();
        foreach (Transform tran in transform)
        {
            EquipM.Add(mySql, tran.gameObject, isStatic, isHoverOutline, isClickFlashing);
            Debug.Log("添加数据：" + mySql.GetTableRowStr(DcConf.Ins.equipTable));
        }
        Debug.Log("更新");
        CloseSql();
    }

    public void AddChildren()
    {
        OpenSql();
        foreach (Transform tran in transform.GetComponentsInChildren<Transform>())
        {
            EquipM.Add(mySql, tran.gameObject, isStatic, isHoverOutline, isClickFlashing);
        }
        Debug.Log("更新");
        CloseSql();
    }

    #endregion

    public GameObject tar;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            EquipM.Update(mySql, tar, "isStatic", true.ToTinyint());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            EquipM.Add(mySql, tar, isStatic, isHoverOutline, isClickFlashing);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EquipM.Delect(mySql, tar);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EquipM.DebugObj(mySql, tar);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //EquipM e = EquipM.FindObj(mySql,
            //    new string[] { "isStatic", "isHoverOutline", "isClickFlashing" },
            //    new string[] { "=", "=", "=" },
            //    new string[] { "1", "1", "1" });
            //    Debug.Log(e.name + "  " + e.oriLocalPos);




            List<EquipM> es = EquipM.FindObjS(mySql,
                new string[] { "isStatic", "isHoverOutline", "isClickFlashing" },
                new string[] { "=", "=", "=" },
                new string[] { "0", "1", "1" });

            foreach (EquipM e in es)
            {
                Debug.Log(e.name + "  " + e.oriLocalPos);
            }
        }
    }

    public void DELECT_ALL()
    {
        OpenSql();
        mySql.DELECT_ALL(DcConf.Ins.equipTable);
        CloseSql();
        Debug.Log("【表单已清除】");
    }
}
