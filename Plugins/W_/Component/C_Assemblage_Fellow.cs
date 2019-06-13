/*Name:		 				C_Assemblage_Fellow	
 *Description: 				一步到位的算法，但是有分组，将跟随他运动的东西进行管理   【暂未对不运动的子物体进行考虑】，每次运行都会一个1ms的延迟
 *Author:       			李文博 
 *Date:         			2018-08-08
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class C_Assemblage_Fellow : MonoBehaviour
{
    #region Field
    [Header("该组件能实现分组运动，尚未完成，先保存初始位置，再保存目标位置")]
    public bool to;
    bool toFlag;
    public float toDuration = 1;

    public bool from;
    bool fromFlag;
    public float fromDuration = 1;
    [Header("在倒数第五位~第三位之间填入该标志字符，后两位代表运动批次")]
    public string mark = "w0_";

    List<Transform> children=new List<Transform>();
    List<string> childrenNames=new List<string>();
    public List<Transform> moveTrans = new List<Transform>();
    List<string> moveNames = new List<string>();
    List<int> orderList = new List<int>();
    public List<Vector3> oriPosList = new List<Vector3>();
    public List<Vector3> tarPosList = new List<Vector3>();
    List<FellowInstall> elements = new List<FellowInstall>();
    List<FellowInstall> toMoveList = new List<FellowInstall>();
    [Header("正在运动的组")]
    public List<Transform> toMoveTransfrom = new List<Transform>();
    int toMoveOrder = 0;
    int step;
    #endregion
    private void Awake()
    {
        Judeorder(mark);
        for (int i = 0; i < children.Count; i++)
        {
            elements.Add(new FellowInstall()
            {
                trans = moveTrans[i],
                hieName = moveNames[i],
                order = orderList[i],
                oriLocalPos = oriPosList[i],
                tarLocalPos = tarPosList[i]
            });
        }
    }

    #region Init
    [ContextMenu("清空数据")]
    public void DataReset()
    {
        U_List.ClearList<Transform>(children, moveTrans);
        U_List.ClearList<string>(childrenNames, moveNames);
        U_List.ClearList<Vector3>(oriPosList, tarPosList);
        U_List.ClearList<int>(orderList);
        U_List.ClearList<FellowInstall>(elements, toMoveList);
    }
    [ContextMenu("保存初始位置")]
    public void SaveOriPosition()
    {
        Judeorder(mark);
        SetOriPosition();
        for (int i = 0; i < moveTrans.Count; i++)
        {
            elements.Add(new FellowInstall()
            {
                trans = moveTrans[i],
                hieName = moveNames[i],
                order = orderList[i],
                oriLocalPos = oriPosList[i],
            });
            Debug.Log(elements[i]);
        }
    }
    [ContextMenu("保存目标位置")]
    public void SaveTarPosition()
    {
        Judeorder(mark);
        SetTarPosition();
        for (int i = 0; i < moveTrans.Count; i++)
        {
            elements[i].tarLocalPos = tarPosList[i];
            Debug.Log(elements[i]);
        }
    }

    [ContextMenu("返回初始位置")]
    public void ToOriPosition()
    {
        foreach (FellowInstall e in elements)
        {
            e.trans.localPosition = e.oriLocalPos;
        }
    }
    [ContextMenu("到达目标位置")]
    public void ToTarPosition()
    {
        foreach (FellowInstall e in elements)
        {
            e.trans.localPosition = e.tarLocalPos;
        }
    }


    void GetChildren()
    {
        children = U_Transform.GetChildren(transform);
    }

    void GetChildrenName()
    {
        childrenNames = U_Transform.GetChildrenName(transform);
    }
    void GetMoveTransAndNameList(string mark)
    {
        U_List.ClearList(children, moveTrans);
        U_List.ClearList(childrenNames, moveNames);
        GetChildren();
        GetChildrenName();

        foreach (Transform trans in children)
        {
            if (U_String.JudeLastContain(trans.name, mark, 5))
            {
                moveTrans.Add(trans);
                moveNames.Add(trans.name);
            }
        }
    }


    void Judeorder(string mark)
    {
        GetMoveTransAndNameList(mark);
        if (orderList.Count != 0)
            orderList.Clear();
        foreach (string str in moveNames)
        {
            orderList.Add(U_String.LastStringToInt(str, 2));
            if (U_String.LastStringToInt(str, 2) > step)
            {
                step = U_String.LastStringToInt(str, 2);
            }
        }
    }

    void SetOriPosition()
    {
        if (oriPosList.Count != 0)
            oriPosList.Clear();
        oriPosList =  U_Transform.GetChildrenLocalPostion(transform);
    }
    void SetTarPosition()
    {
        if (tarPosList.Count != 0)
            tarPosList.Clear();
        tarPosList = U_Transform.GetChildrenLocalPostion(transform);
    }
    #endregion


    void ReturnNextMoveList(int order)
    {
        toMoveList.Clear();
        foreach (FellowInstall i in elements)
        {
            if (i.order == order)
            {
                toMoveList.Add(i);
            }
        }
        ShowToMoveList();
    }



    //这个方法要是先一层层的调用，每一层都是使用想同的时间。小的的先动
    public void ToTar(float duration)
    {
        ReturnNextMoveList(toMoveOrder);
        for (int i = 0; i < toMoveList.Count; i++)
        {
            if (i == 0)
                toMoveList[i].trans.DOLocalMove(toMoveList[i].tarLocalPos, duration).OnComplete
                    (
                    () =>
                    {
                        Debug.Log("Tar了第" + toMoveOrder + "组");
                        toMoveOrder += 1;
                        if (toMoveOrder <= step)
                            ToTar(duration);
                        else
                        {
                            to = false;
                            Debug.Log("Tar完成");
                        }
                           
                    }
                    );
            else
                toMoveList[i].trans.DOLocalMove(toMoveList[i].tarLocalPos, duration);
        }
    }


    //相当于倒放
    public void ToOri(float duration)
    {
        ReturnNextMoveList(toMoveOrder - 1);
        for (int i = 0; i < toMoveList.Count; i++)
        {
            if (i == 0)
                toMoveList[i].trans.DOLocalMove(toMoveList[i].oriLocalPos, duration).OnComplete
                    (
                    () =>
                    {
                        toMoveOrder -= 1;
                        Debug.Log("Ori了第" + toMoveOrder + "组");
                        if (toMoveOrder > 0)
                            ToOri(duration);
                        else
                        {
                            from = false;
                            Debug.Log("Ori完成");
                        }
                    }
                    );
            else
                toMoveList[i].trans.DOLocalMove(toMoveList[i].oriLocalPos, duration);
        }
    }


    private void Update()
    {
        U_Bool.BoolTrueMethord
            (to, ref toFlag,() => ToTar(toDuration));

        U_Bool.BoolTrueMethord
            (from, ref fromFlag,() => ToOri(fromDuration));
    }


    void ShowToMoveList()
    {
        toMoveTransfrom.Clear();
        foreach (FellowInstall e in toMoveList)
        {
            toMoveTransfrom.Add(e.trans);
        }
    }

}

public class FellowInstall
{
    public Transform trans { get; set; }
    public string hieName { get; set; }
    public int order { get; set; }
    public Vector3 oriLocalPos { get; set; }
    public Vector3 tarLocalPos { get; set; }

    public override string ToString()
    {
        return trans.name + "     次序:" + order + "    原始位置：" + oriLocalPos + "    目标位置：" + tarLocalPos;
    }
}
