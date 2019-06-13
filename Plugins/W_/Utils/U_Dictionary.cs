/*Name:		 				U_Dictionary	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Dictionary : MonoBehaviour {

	public static void DebugDic<K,V>(Dictionary<K,V> dic)
    {
        foreach (KeyValuePair<K, V> kvp in dic)
        {
            Debug.Log( kvp.Key + "       " + kvp.Value);
        }
    }
    /// <summary>
    /// 添加元素
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Add<K, V>(Dictionary<K, V> dic,K key,V value)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
        }
    }
    

    public static void Example()
    {
        //一、创建泛型哈希表，然后加入元素
        Dictionary<string, string> oscar = new Dictionary<string, string>();
        oscar.Add("哈莉?贝瑞", "《死囚之舞》");
        oscar.Add("朱迪?丹奇", "《携手人生》");
        oscar.Add("尼科尔?基德曼", "《红磨坊》");
        oscar.Add("詹妮弗?康纳利", "《美丽心灵》");
        oscar.Add("蕾妮?齐维格", "《BJ单身日记》");

        //二、删除元素
        oscar.Remove("詹妮弗?康纳利");

        //三、假如不存在元素则加入元素
        if (!oscar.ContainsKey("茜茜?斯派克"))
        {
            oscar.Add("茜茜?斯派克", "《不伦之恋》");
        }

        //五、遍历集合
        Debug.Log("74届奥斯卡最佳女主角及其电影：");
        foreach (KeyValuePair<string, string> kvp in oscar)
        {
            Debug.Log("姓名：{0},电影：{1}"+kvp.Key+"       " + kvp.Value);
        }

        //六、得到哈希表中键的集合
        Dictionary<string, string>.KeyCollection keyColl = oscar.Keys;
        //遍历键的集合
        Debug.Log("最佳女主角：");
        foreach (string s in keyColl)
        {
            Debug.Log(s);
        }

        //七、得到哈希表值的集合
        Dictionary<string, string>.ValueCollection valueColl = oscar.Values;
        //遍历值的集合
        Debug.Log("最佳女主角电影：");
        foreach (string s in valueColl)
        {
            Debug.Log(s);
        }

        //八、使用TryGetValue方法获取指定键对应的值
        string slove = string.Empty;
        if (oscar.TryGetValue("朱迪?丹奇", out slove))
            Debug.Log("我最喜欢朱迪?丹奇的电影{0}"+"       "+ slove);
        else
            Debug.Log("没找到朱迪?丹奇的电影");

        //九、清空哈希表
        oscar.Clear();
    }




}
