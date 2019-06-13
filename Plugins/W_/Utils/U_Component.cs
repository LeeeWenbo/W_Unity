/*Name:		 				U_Component	
 *Description: 				组件工具类，多用于查组件，往列表里添加组件等
 *Author:       			李文博 
 *Date:         			2018-09-04
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using W_Enum;

public class U_Component : MonoBehaviour
{
    [ContextMenu("删除LightProbeProxyVolume")]
    public void RemoveLightProbeProxy()
    {
        LightProbeProxyVolume[] lightProbes = GetComponentsInChildren<LightProbeProxyVolume>();
        foreach (LightProbeProxyVolume a in lightProbes)
        {
            DestroyImmediate(a);
        }
    }


    //从VRTK抠的方法，查找所有的组件，即使是非激活
    public static T[] FindEvenInactiveComponents<T>() where T : Component
    {
        Scene activeScene = SceneManager.GetActiveScene();
        return Resources.FindObjectsOfTypeAll<T>()
                        .Where(@object => @object.gameObject.scene == activeScene)
                            .ToArray();
    }


    /// <summary>
    /////测试这个方法，当T=Renderer时，当父物体无Render时的状况，目前发现会少算一个，打印也显示他有
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <param name="isCotainSelf"></param>
    /// <returns></returns> 
    
        //[Obsolete("测试这个方法，当T=Renderer时，当父物体无Render时的状况，目前发现会少算一个，打印也显示他有")]
    public static List<T> GetChildrenComponents<T>(Transform trans, bool isCotainSelf = false)
    {
        T[] tArray = trans.GetComponentsInChildren<T>();

        List<T> tList = new List<T>(tArray);
        //不包含自己但是他却包含了自己，就不行
        if (!isCotainSelf && trans.GetComponent<T>() != null)
        {
            Debug.Log("是否包含自己"+ isCotainSelf);
            Debug.Log("      名字   "+trans.name+"      不包含renderer" + trans.GetComponent<T>() == null);
            tList.RemoveAt(0);
        }
        return tList;
    }
    public static List<T> GetChildrenComponents_New<T>(Transform trans, bool isCotainSelf = false)
    {
        T[] tArray = trans.GetComponentsInChildren<T>();

        List<T> tList = new List<T>(tArray);
        if (!isCotainSelf && trans.GetComponent<T>() != null)
        {
            Debug.Log("包含" + isCotainSelf);
            tList.RemoveAt(0);
        }
        return tList;
    }



    //删掉子物体中的所有相关组件
    public static void RemoveChildrenComponents<T>(Transform trans, bool isImmediate = false, bool isCotainSelf = false)
    {
        List<T> tList = GetChildrenComponents<T>(trans, isCotainSelf);
        if (isImmediate)
        {
            foreach (T t in tList)
            {
                GameObject.DestroyImmediate(t as UnityEngine.Object);
            }
        }
        else
        {
            foreach (T t in tList)
            {
                GameObject.Destroy(t as UnityEngine.Object);
            }
        }
    }

    //获取组件
    public static List<T> GetTComponent<T>(Transform tran, WE_TransformRange transformRange = WE_TransformRange.Self)
    {
        List<T> tC = new List<T>();
        switch (transformRange)
        {
            case WE_TransformRange.Self: tC.Add(tran.GetComponent<T>()); break;
            case WE_TransformRange.SelfSon: GetTC_FromTransforms<T>(U_Transform.GetSonS(tran, true)); break;
            case WE_TransformRange.SelfChildren: tC = GetTC_FromTransforms<T>(U_Transform.GetChildren(tran, true)); break;
            case WE_TransformRange.SelfBrother: tC = GetTC_FromTransforms<T>(U_Transform.GetBrotherS(tran, true)); break;
            case WE_TransformRange.Son: tC = GetTC_FromTransforms<T>(U_Transform.GetSonS(tran, false)); break;
            case WE_TransformRange.Children: GetTC_FromTransforms<T>(U_Transform.GetChildren(tran, false)); break;
            case WE_TransformRange.Brother: tC = GetTC_FromTransforms<T>(U_Transform.GetBrotherS(tran, false)); break;
        }
        return tC;
    }

    //从Transform列表中获取组件
    public static List<T> GetTC_FromTransforms<T>(List<Transform> trans)
    {
        List<T> ts = new List<T>();
        foreach (Transform tran in trans)
        {
            //T是不允许为空的，类型可以直接为空，而T是类型参数，当这个地方参数也为T时，需要这样判断。
            if (!tran.GetComponent<T>().ToString().Equals("null"))
            {
                ts.Add(tran.GetComponent<T>());
            }
        }
        return ts;
    }


    /// <summary>
    /// 添加组件不能写泛型，到时候复制本方法吧，把U_Component改成需要弄的组价名。
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static List<U_Component> AddComponentToList(List<Transform> trans)
    {
        List<U_Component> ts = new List<U_Component>();
        foreach (UnityEngine.Transform tran in trans)
        {
            if (null== tran.GetComponent<U_Component>())
                ts.Add( tran.gameObject.AddComponent<U_Component>());
            else
                ts.Add(tran.GetComponent<U_Component>());
        }
        return ts;
    }
}

