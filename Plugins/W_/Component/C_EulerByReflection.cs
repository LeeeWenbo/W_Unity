/*Name:		 				W_EulerByReflection	
 *Description: 				使用反射得到transform清零之前的数据。使用Set可以搞定。
 *Author:       			李文博 
 *Date:         			2018-06-20
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class C_EulerByReflection : MonoBehaviour
{
    Type transformType;
    MethodInfo SetLocalEulerAngles;
    MethodInfo GetLocalEulerAngles;
    PropertyInfo property_RotationOrder;
    int zEuler;
    
    private void Awake()
    {
        transformType = transform.GetType();
        SetLocalEulerAngles = transformType.GetMethod("SetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        property_RotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    Vector3 GetEuler()
    {
        object m_OldRotationOrder = property_RotationOrder.GetValue(transform, null);
        Vector3 value = (Vector3)GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        return value;
    }

    public Vector3 euler;
    void FixedUpdate()
    {
        euler = GetEuler();
        zEuler++;
        SetLocalEulerAngles.Invoke(transform, new object[] { new Vector3(0, 0, zEuler), 1 });
    }
}