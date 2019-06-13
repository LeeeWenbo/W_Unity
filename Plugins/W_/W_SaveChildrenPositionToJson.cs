/*Name:		 				W_SaveChildrenPositionToJson	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_SaveChildrenPositionToJson : MonoBehaviour {


    public string fileName="postion";
    [ContextMenu("写")]
    public void WriteEditor()
    {
        SavePosition();
    }
    [ContextMenu("读")]
    public void ReadEditor()
    {
        List<W_JsonTransform> jsonTransformList = U_Json.ReadAllToList<W_JsonTransform>(fileName);
        foreach(W_JsonTransform jsonTrans in jsonTransformList)
        Debug.Log(jsonTrans);
    }



    public List<Transform> children = new List<Transform>();

    void GetChildren()
    {
        children = U_Transform.GetChildren(transform);
    }

    void SavePosition()
    {
        GetChildren();
        foreach(Transform trans in children)
        {
            W_JsonTransform jsonTrans = W_JsonTransform.TransformToJson(trans);
            U_Json.Write<W_JsonTransform>(jsonTrans, fileName);
        }
    }

}
