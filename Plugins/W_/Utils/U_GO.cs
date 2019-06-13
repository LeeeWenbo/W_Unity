/*Name:		 				U_GameObject	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2019-05-
 *Copyright(C) 2019 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_GO : MonoBehaviour
{


    List<Transform> sonS = new List<Transform>();
    List<GameObject> sonGoS = new List<GameObject>();
    private void Awake()
    {
        sonS = transform.GetSonS(false);
        sonGoS = U_Transform.TranS_To_GameObjectS(sonS);
        StartCoroutine(ActiveOneByOne(sonGoS,1,0,true));
    }

    /// <summary>
    /// 激活/非激活某物体，第二个参数小于零取反，等于0隐藏，大于零激活
    /// </summary>
    public static void AcitveSwitch(GameObject obj, int i=-1)
    {
        if (i<0)
        {
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
            }
        }
        else if (i == 0)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }

    //public void ActiveOneByOne(List<GameObject> goS,int lifeTime)
    IEnumerator ActiveOneByOne(List<GameObject> goS, int lifeTime, int firstTime = 0,bool hideParentOnEnd=false)
    {
        yield return new WaitForSeconds(firstTime);
        goS[0].SetActive(true);
        for (int i = 1; i < goS.Count; i++)
        {
            yield return new WaitForSeconds(lifeTime);
            if (i - 1 >= 0)
            {
                goS[i - 1].SetActive(false);
            }
            goS[i].SetActive(true);

            Debug.Log(i);
        }
        yield return new WaitForSeconds(lifeTime);
        goS[goS.Count-1].SetActive(false);
        if (hideParentOnEnd)
            goS[0].transform.parent.gameObject.SetActive(false);
    }

    
}

public static class U_GO_Extren
{
    public static string IdStr(this GameObject go)
    {
        return go.GetInstanceID().ToString();
    }
    public static int Id(this GameObject go)
    {
        return go.GetInstanceID();
    }
}
