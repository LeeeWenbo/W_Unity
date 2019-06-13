/*Name:		 				U_IO	
 *Description: 				文件读取和写入
 *Author:       			李文博 
 *Date:         			2018-08-15
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class U_IO
{


    //这个直接填入目录和名字就好，比如Assets下的一个test.txt，直接"C:\\Users\\zty_固态\\Desktop\\AssetBundle\\Assets\\test.txt"，字符串用\\代替\
    public static byte[] ReadByFileToByte(string pathAndName)
    {
        byte[] bytes= File.ReadAllBytes(pathAndName);
        return bytes;
    }
    public static string ReadByFileToString(string pathAndName)
    {
        string str = File.ReadAllText(pathAndName);
        return str;
    }
    public static void WriteTo(string content, string name, bool append = true,string path= "StreamingFile")
    {
        string filePath = Application.dataPath + "/"+ path + "/" + name+".json";
        if (!File.Exists(filePath))
        {
            Debug.Log("生成了文件    "+name);
        }

        StreamWriter sw = new StreamWriter(filePath, append);
        sw.WriteLine(content);
        sw.Close();
        Debug.Log("写入了  " + content);

    }

    public static void ReadFrom(string name,string path="StreamingFile")
    {

        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        if (File.Exists(filePath))
        {
            //创建一个StreamReader，用来读取流
            StreamReader sr = new StreamReader(filePath);
            //将读取到的流赋值给jsonStr
            string jsonStr = sr.ReadToEnd();
            Debug.Log(jsonStr);
            //关闭
            sr.Close();

        }
        else
        {
            Debug.Log("文件不存在");
        }
    }

    //检查并创建目录
    public static void CheckAndSetDirectory(string path,bool isInAssets=true)
    {
        if(isInAssets)
        {
            path = "Assets/" + path;
            if (!Directory.Exists( path))
            {
                Directory.CreateDirectory(path);
            }
        }
        else
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
 


    



}
