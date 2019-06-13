/*Name:		 				JsonTransform	
 *Description: 				用于保存transform组件
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存物体ID，位置和旋转度，用这个。
public class W_JsonTransform
{
    public int instanceID;
    public string name;
    public string position;
    public string localPosition;
    public string euler;
    public string localEuler;
    public static W_JsonTransform TransformToJson(Transform trans)
    {
        W_JsonTransform json = new W_JsonTransform();
        json.instanceID = trans.GetInstanceID();
        json.name = trans.name;
        json.localPosition = trans.localPosition.ToString();
        json.position = trans.position.ToString();
        json.localEuler = trans.localEulerAngles.ToString();
        json.euler = trans.eulerAngles.ToString();
        return json;
    }
    public Vector3 GetPostion()
    {
        return U_Json.StringToVector3(this.position);
    }
    public Vector3 GetLocalPostion()
    {
        return U_Json.StringToVector3(this.localPosition);
    }
    public Vector3 GetEluer()
    {
        return U_Json.StringToVector3(this.euler);
    }
    public Vector3 GetLocalEuler()
    {
        return U_Json.StringToVector3(this.localEuler);
    }

    public override string ToString()
    {
        return "名字：" + name + "   ID：" + instanceID + 
            "     localPostion：" + localPosition + "   localEuler：" + localEuler;
    }
}