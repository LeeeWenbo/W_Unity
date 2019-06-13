/*Name:		 				W_LoadSceneWithProgressBar	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class C_ProgressBar_LoadScene : C_ProgressBar {

    //public string nameOfSceneToLoad = "";
    AsyncOperation operation;
    public string sceneName;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (operation == null)
            return;
        if (operation.progress >= 0.9f && rate > 0.95f)
        {
            operation.allowSceneActivation = true;
        }
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(IAsynToScene());
        isRun = true;
    }
    IEnumerator IAsynToScene()
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false ;
        yield return operation;
    }

}
