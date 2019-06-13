/*Name:		 				U_SerializableDictionary	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /// Usage:
 /// 
 /// [System.Serializable]
 /// class MyDictionary : SerializableDictionary<int, GameObject> {}
 /// public MyDictionary dic;
 ///
 [System.Serializable]
 public class U_SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // We save the keys and values in two lists because Unity does understand those.
    [SerializeField]
    private List<TKey> _keys = new List<TKey>();
    [SerializeField]
    private List<TValue> _values=new List<TValue>();

    // Before the serialization we fill these lists
    public void OnBeforeSerialize()
    {
        //官方例子有误，去掉     
    }

    // After the serialization we create the dictionary from the two lists
    public void OnAfterDeserialize()
    {
        this.Clear();
        int count = Mathf.Min(_keys.Count, _values.Count);
        for (int i = 0; i < count; ++i)
        {
            this.Add(_keys[i], _values[i]);
        }
    }
}