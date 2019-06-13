/*Name:		 				U_Input	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using UnityEngine;
using UnityEngine.SceneManagement;
public static class U_Scene
{
    //重新加载当前场景
    public static void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //加载某场景
    public static void Load(string str)
    {
        SceneManager.LoadScene(str);
    }
    //异步加载场景
    public static void LoadAsync(string str)
    {
        SceneManager.LoadSceneAsync(str);
    }


}
