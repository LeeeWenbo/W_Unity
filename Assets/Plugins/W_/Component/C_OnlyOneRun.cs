/*Name:		 				C_OnlyOneRun	
 *Description: 				挂载该组件，同一程序只能运行一个
 *Author:       			李文博 
 *Date:         			2018-08-24
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_OnlyOneRun : MonoBehaviour {
    private void Awake()
    {
        OnlyOneCanRun();
    }

    void OnlyOneCanRun()
    {
        string appID = Application.productName;
        bool isFirst = U_Mutex.Instance().IsFirst(appID);
        if(false==isFirst)
        {
            Application.Quit();
        }
    }
}
