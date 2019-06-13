/*Name:		 				U_Json	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

public static class U_Json 
{

    //只能保存一个对象，并且覆盖，感觉原因是IO的问题
    public static void Write<T>(T t, string name, bool append = true,string path = "StreamingFile")
    {
        string filePath = Application.dataPath + "/" + path + "/" + name + ".json";
        if (!File.Exists(filePath))
        {
            Debug.Log("生成了文件    " + name);
        }
        StreamWriter sw = new StreamWriter(filePath, append);
        string content = JsonMapper.ToJson(t);
        sw.WriteLine(content);
        sw.Close();
        Debug.Log("写入了  " + content);
    }

    //读取，用自带的方法解析，这种方法要求被解析的类不能有自己写的构造函数，不能继承Monobehaviour
    public static T ReadAllToOne<T>(string name, string path = "StreamingFile")
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + path + "/" + name + ".json");
        string content = sr.ReadToEnd();
        T t = JsonMapper.ToObject<T>(content);
        return t;
    }
    public static T ReadByIndex<T>(string name, int index, string path = "StreamingFile")
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/" + path + "/" + name + ".json");
        string content = sr.ReadToEnd();
        string[] contentArry = content.Split('}');
        for (int i = 0;  i < contentArry.Length - 1; i++)
        {
            contentArry[i] += "}";
        }
        T t = JsonMapper.ToObject<T>(contentArry[index]);
        return t;
    }
    public static List<T> ReadAllToList<T>(string name, string path = "StreamingFile")
    {
        List<T> tList = new List<T>();
        StreamReader sr = new StreamReader(Application.dataPath + "/" + path + "/" + name + ".json");

        string content = sr.ReadToEnd();
        string[] contentArry = content.Split('}');
        for (int i = 0; i < contentArry.Length-1; i++)
        {
            contentArry[i] += "}";
            T jsonTrans = JsonMapper.ToObject<T>(contentArry[i]);
            tList.Add(jsonTrans);
        }

        return tList;
    }
    //读取，自己解析
    public static void ReadByJsonData(string name, string path = "StreamingFile")
    {
        //StreamReader sr = new StreamReader(Application.dataPath + "/" + path + "/" + name + ".json");
        //JsonData jsonData = JsonMapper.ToObject(sr);
    }

    public static string Vector3ToString(Vector3 vector3)
    {
        return vector3.ToString();
    }
    public static Vector3 StringToVector3(string name)
    {
        name = name.Replace("(", "").Replace(")", "");
        string[] s = name.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }


}


