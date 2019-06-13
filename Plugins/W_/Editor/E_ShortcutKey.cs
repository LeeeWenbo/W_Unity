/*Name:		 				E_ShortcutKey	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using UnityEngine;
//引入unity编辑器命名空间
using UnityEditor;
using System.Collections;


public class E_ShortcutKey : ScriptableObject
{
    //%对应Ctrl,#对应Shift &对应Alt f对应F
    public const string KeyName = "W_/DisableSelectGameObect %q";

    //根据当前有没有选中物体来判断可否用快捷键
    [MenuItem(KeyName, true)]
    static bool ValidateSelectEnableDisable()
    {
        GameObject[] go = GetSelectedGameObjects() as GameObject[];

        if (go == null || go.Length == 0)
            return false;
        return true;
    }

    [MenuItem(KeyName)]
    static void SeletEnable()
    {
        bool enable = false;
        GameObject[] gos = GetSelectedGameObjects() as GameObject[];

        foreach (GameObject go in gos)
        {
            enable = !go.activeInHierarchy;
            EnableGameObject(go, enable);
        }
    }

    //获得选中的物体
    static GameObject[] GetSelectedGameObjects()
    {
        return Selection.gameObjects;
    }

    //激活或关闭当前选中物体
    public static void EnableGameObject(GameObject parent, bool enable)
    {
        parent.gameObject.SetActive(enable);
    }
}