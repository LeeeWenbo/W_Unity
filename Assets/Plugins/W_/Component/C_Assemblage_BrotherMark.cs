/*Name:		 				C_Assemblage_BrotherMark	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-09-30
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
public class C_Assemblage_BrotherMark : MonoBehaviour
{

    #region File
    public enum General_State { 单拆ing, 单装ing, 一键分步拆ing, 一键分步装ing, 一步拆ing, 一步装ing, 单拆到一半, 单装到一半, 拆完, 装完 }
    public enum Allow { 无限制, 只能单拆, 只能单装, 只能一键拆, 只能一键装, 只能一步到位拆, 只能一步到位装 }
    [Tooltip("标志")]
    public string mark = "as-";
    [Tooltip("标志位于倒数第几位")]
    public int markDigit = 5;



    public int groupCount { get; private set; }
    public int allStepCount { get; private set; }

    List<int> eachGroupCount = new List<int>();

    List<Transform> brother = new List<Transform>();
    //[Header("使用该脚本，不要更改层级关系，需要保证每组物体相对位置相同。")]
    //[Header("需装卸的物体")]
    List<Transform> oriTrans_Obj = new List<Transform>();
    //[Header("需装卸的步骤")]
    List<Transform> oriTrans_AllStep = new List<Transform>();
    //[Header("CAS组件(个数)")]
    List<C_AssembleOBJ> assembles_Obj = new List<C_AssembleOBJ>();
    //[Header("需装卸的目标")]
    List<Transform> tarTrans_AllStep = new List<Transform>();


    [Space(20)]
    [Tooltip("运动时间")]
    public float duration = 1;
    [Range(0, 3)]
    [Tooltip(@"跟进时间比。
爆炸（=0）
第n个完成的同时进行第n+1个（=1）
第n个未完成就进行第n+1个（0~1）
第n个完成等待一会才进行第n+1个（>1）")]
    public float fellowRate = 1;

    Allow allow = Allow.无限制;
    [Space(20)]
    [Tooltip("当前状态")]
    public General_State state = General_State.装完;
    [Tooltip(@"当前拆到哪一组了
-1是完好，
等于总数是分散")]
    public int groupIndex = -1;


    [HideInInspector]
    public bool chai_One = false;
    bool chaiFlag_One = false;
    [HideInInspector]
    public bool zhuang_One = false;
    bool zhuangFlag_One = false;
    [Multiline(3)]
    [HideInInspector]

    public bool chai_OneKeyAll = false;
    bool chaiFlag_OneKeyAll = false;
    [HideInInspector]

    public bool zhuang_OneKeyAll = false;
    bool zhuangFlag_OneKeyAll = false;

    [HideInInspector]

    public bool chai_OneStepAll = false;
    bool chaiFlag_OneStepAll = false;
    [HideInInspector]
    public bool zhuang_OneStepAll = false;
    bool zhuangFlag_OneStepAll = false;


    /// <summary>
    /// 下标是层级，元素是各层级的目标列表
    /// </summary>
    public List<List<Transform>> oriGroupList = new List<List<Transform>>();
    /// <summary>
    /// 下标是层级，元素是各层级的目标列表
    /// </summary>
    public List<List<Transform>> tarGroupList = new List<List<Transform>>();
    /// <summary>
    /// 下标是层级，元素是各层级的组件列表
    /// </summary>
    public List<List<C_AssembleOBJ>> casGroupList = new List<List<C_AssembleOBJ>>();

    [HideInInspector]
    public bool logStepOnComplete = false;
    [HideInInspector]
    public bool logGroupOnComplete = false;

    [HideInInspector]
    [SerializeField]
    public W_Transform_UnityEvent Chai_Step_Complete;
    [HideInInspector]
    [SerializeField]
    public W_Transform_UnityEvent Zhuang_Step_Complete;
    [HideInInspector]
    [SerializeField]
    public W_Int_UnityEvent Chai_Group_Complete;
    [HideInInspector]
    [SerializeField]
    public W_Int_UnityEvent Zhuang_Group_Complete;
    public List<Transform> movingTrans = new List<Transform>();
    #endregion

    #region 周期
    private void Start()
    {
        Init();
    }


    private void FixedUpdate()
    {
        Input_Bool();

    }
    #endregion

    #region 初始化

    [ContextMenu("初始化")]
    public void Init()
    {
        Base_Init();
        CAS_List_Init();
        Log_StepName_Complete_Init();
        Log_GroupIndex_Complete_Init();
    }

    //基础初始化，例如兄弟个数等等
    void Base_Init()
    {
        this.RemoveData();
        this.brother = U_Transform.GetBrotherS(transform, false);
        this.groupCount = brother.Count;
        this.oriTrans_Obj = U_Transform.GetAddedStrTrans(transform, brother[brother.Count - 1], mark, markDigit, true);
        this.assembles_Obj = AddComponentCAS(oriTrans_Obj);
    }
    void CAS_List_Init()
    {
        int allStep = 0;
        //共groupCount，group是他们的下标，正好从0开始
        for (int group = 0; group < groupCount; group++)
        {
            //这个地方是否消耗会很大，能否优化
            //获取第group组有多少个运动的物体(目标和本体)
            List<Transform> groupTarTrans = U_Transform.GetAddedStrTrans(transform, brother[group], mark, markDigit, false, group);
            List<Transform> groupOriTrans = U_Transform.GetAddedStrTrans(transform, brother[group], mark, markDigit, true, group);
            List<C_AssembleOBJ> tempASList = new List<C_AssembleOBJ>();
            oriTrans_AllStep.AddRange(groupOriTrans);
            tarTrans_AllStep.AddRange(groupTarTrans);
            eachGroupCount.Add(groupTarTrans.Count);
            //每一组中的step步。
            for (int step = 0; step < groupTarTrans.Count; step++)
            {
                //Debug.Log("allStep  "+allStep+"     ori  【"+oriTrans_AllStep[allStep].name+"】     tar  【" + tarTrans_AllStep[allStep].name + "】");
                C_AssembleOBJ tempCAS = oriTrans_AllStep[allStep].GetComponent<C_AssembleOBJ>();
                tempCAS.SetAssemble(tarTrans_AllStep[allStep], group, groupTarTrans.Count);
                tempASList.Add(tempCAS);
                allStep += 1;
            }
            tarGroupList.Add(groupTarTrans);
            oriGroupList.Add(groupOriTrans);
            casGroupList.Add(tempASList);
        }
        allStepCount = oriTrans_AllStep.Count;
    }

    [ContextMenu("清除数据")]
    public void RemoveData()
    {
        this.groupCount = 0;
        this.allStepCount = 0;
        U_List.ClearList(brother, oriTrans_Obj, oriTrans_AllStep, tarTrans_AllStep);
        U_List.ClearList(assembles_Obj);
        U_List.ClearList(eachGroupCount);
        U_List.ClearList(oriGroupList, tarGroupList);
        U_List.ClearList(casGroupList);

        U_Component.RemoveChildrenComponents<C_AssembleOBJ>(transform, true, true);
    }



    public static List<C_AssembleOBJ> AddComponentCAS(List<Transform> trans)
    {
        List<C_AssembleOBJ> ts = new List<C_AssembleOBJ>();
        foreach (Transform tran in trans)
        {
            if (null == tran.GetComponent<C_AssembleOBJ>())
                ts.Add(tran.gameObject.AddComponent<C_AssembleOBJ>());
            else
                ts.Add(tran.GetComponent<C_AssembleOBJ>());
        }
        return ts;
    }
    #endregion

    #region 拆装控制
    public IEnumerator C_One()
    {
        //这个地方可能重复了，否则有时候按超多次的时候，会出现数组越界，出现次数很少，但是有时会有,尚未知道原因。强制去越界。
        if (groupIndex >= groupCount) { groupIndex = groupCount; state = General_State.拆完; }

        switch (state)
        {
            case General_State.拆完: chai_One = false; chaiFlag_One = false; yield break;
            case General_State.装完: groupIndex = 0; break;
            case General_State.单装到一半: groupIndex += 1; break;
            case General_State.一键分步拆ing: chai_OneKeyAll = false; chaiFlag_OneKeyAll = false; break;
            case General_State.一键分步装ing: zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false; break;
        }
        state = General_State.单拆ing;
        allow = Allow.只能单拆;
        C_Group(groupIndex);
        SetNowTrans_Pre(groupIndex);
        yield return new WaitForSeconds(duration * fellowRate);

        if (allow == Allow.只能单拆)
        {
            if (null != Chai_Group_Complete) Chai_Group_Complete.Invoke(groupIndex);
            allow = Allow.无限制;
            groupIndex += 1;
            SetNowTrans_Aft();
        }
        if (groupIndex == groupCount) state = General_State.拆完; else state = General_State.单拆到一半;
        chai_One = false; chaiFlag_One = false;
        //xg
        OriGroupEffect(groupIndex);
    }
    public IEnumerator C_Order()
    {
        if (groupIndex >= groupCount) { groupIndex = groupCount; state = General_State.拆完; }

        switch (state)
        {
            case General_State.拆完: chai_OneKeyAll = false; chaiFlag_OneKeyAll = false; U_List.ClearList(movingTrans); yield break;
            case General_State.装完: groupIndex = 0; break;
            case General_State.单装到一半: groupIndex += 1; break;
        }
        state = General_State.一键分步拆ing;
        allow = Allow.只能一键拆;

        for (int i = groupIndex; i < groupCount; i++)
        {
            if (allow == Allow.只能一键拆)
            {
                C_Group(groupIndex);
                SetNowTrans_Pre(groupIndex);
            }
            yield return new WaitForSeconds(duration * fellowRate);
            if (allow == Allow.只能一键拆)
            {
                if (null != Chai_Group_Complete) Chai_Group_Complete.Invoke(groupIndex);
                groupIndex += 1;
                SetNowTrans_Aft();
            }
        }
        if (allow == Allow.只能一键拆)
        {
            state = General_State.拆完; allow = Allow.无限制;
            chai_OneKeyAll = false; chaiFlag_OneKeyAll = false;
        }
    }
    public IEnumerator C_All()
    {
        state = General_State.一步拆ing;
        allow = Allow.只能一步到位拆;

        foreach (C_AssembleOBJ assemble in assembles_Obj)
        {
            assemble.state = W_AssembleState.装ING;
            assemble.transform.DOLocalMove
                (assemble.DicGroupTarTrans[assemble.groups[assemble.groups.Count - 1]].localPosition, duration)
                .OnComplete(() => { assemble.state = W_AssembleState.拆ED; groupIndex = groupCount; state = General_State.拆完; });
            assemble.transform.DOLocalRotate
                (assemble.DicGroupTarTrans[assemble.groups[assemble.groups.Count - 1]].localEulerAngles, duration);
        }
        chai_One = false; chaiFlag_One = false; chai_OneKeyAll = false; chaiFlag_OneKeyAll = false;
        zhuang_One = false; zhuangFlag_One = false; zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false;
        yield return new WaitForSeconds(duration);
        SetNowTrans_Aft();
        chai_OneStepAll = false;
        chaiFlag_OneStepAll = false;
        allow = Allow.无限制;
    }

    public IEnumerator Z_One()
    {
        if (groupIndex <= -1) { groupIndex = -1; state = General_State.装完; }
        switch (state)
        {
            case General_State.拆完: groupIndex = groupCount - 1; break;
            case General_State.装完: zhuang_One = false; zhuangFlag_One = false; yield break;
            case General_State.单拆到一半: groupIndex -= 1; break;
            case General_State.一键分步拆ing: chai_OneKeyAll = false; chaiFlag_OneKeyAll = false; break;
            case General_State.一键分步装ing: zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false; break;
        }
        state = General_State.单装ing; allow = Allow.只能单装;

        Z_Group(groupIndex);
        SetNowTrans_Pre(groupIndex);
        yield return new WaitForSeconds(duration * fellowRate);

        if (allow == Allow.只能单装)
        {
            if (null != Zhuang_Group_Complete) Zhuang_Group_Complete.Invoke(groupIndex);
            allow = Allow.无限制;
            groupIndex -= 1;
            SetNowTrans_Aft();
        }
        if (groupIndex == -1) state = General_State.装完; else state = General_State.单装到一半;
        zhuang_One = false; zhuangFlag_One = false;
    }
    public IEnumerator Z_Order()
    {
        if (groupIndex <= -1) { groupIndex = -1; state = General_State.装完; }

        switch (state)
        {
            case General_State.拆完: groupIndex = groupCount - 1; break;
            case General_State.装完: zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false; yield break;
            case General_State.单拆到一半: groupIndex -= 1; break;
            case General_State.单装ing: groupIndex -= 1; break;
        }
        state = General_State.一键分步装ing; allow = Allow.只能一键装;

        for (int i = groupIndex; i > -1; i--)
        {
            if (allow == Allow.只能一键装)
            {
                Z_Group(groupIndex);
                SetNowTrans_Pre(groupIndex);
            }
            yield return new WaitForSeconds(duration * fellowRate);
            if (allow == Allow.只能一键装)
            {
                if (null != Zhuang_Group_Complete) Zhuang_Group_Complete.Invoke(groupIndex);
                groupIndex -= 1;
                SetNowTrans_Aft();
            }
        }
        //被这个无限制弄的不行
        if (allow == Allow.只能一键装)
        {
            state = General_State.装完; allow = Allow.无限制;
            zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false;
        }
    }
    public IEnumerator Z_All()
    {
        state = General_State.一步装ing;
        allow = Allow.只能一步到位装;

        foreach (C_AssembleOBJ assemble in assembles_Obj)
        {
            assemble.state = W_AssembleState.装ING;
            assemble.transform.DOLocalMove
                (assemble.oriPostion, duration)
                .OnComplete(() => { assemble.state = W_AssembleState.装ED; groupIndex = -1; state = General_State.装完; });
            assemble.transform.DOLocalRotate(assemble.oriEuler, duration);
        }
        chai_One = false; chaiFlag_One = false; chai_OneKeyAll = false; chaiFlag_OneKeyAll = false;
        zhuang_One = false; zhuangFlag_One = false; zhuang_OneKeyAll = false; zhuangFlag_OneKeyAll = false;
        yield return new WaitForSeconds(duration);
        SetNowTrans_Aft();
        zhuang_OneStepAll = false;
        zhuangFlag_OneStepAll = false;
        allow = Allow.无限制;
    }
    #endregion


    #region 运动
    void C_Group(int group)
    {
        List<Transform> oriS = oriGroupList[group];
        List<Transform> tarS = tarGroupList[group];
        List<C_AssembleOBJ> casS = casGroupList[group];

        for (int i = 0; i < oriS.Count; i++)
        {
            C_AssembleOBJ tempCas = casS[i];
            tempCas.state = W_AssembleState.拆ING;
            //Debug.Log("【组别】："+ group + "       原始物体："+ori[i].name + "       目标物体："+tar[i].name +"    字典物体    "+cas[i].DicGroupTarTrans[group].name);
            oriS[i].DOLocalMove(tarS[i].localPosition, duration)
                .OnComplete(() =>
                {
                    tempCas.state = W_AssembleState.拆ED;
                    if (null != Chai_Step_Complete)
                        Chai_Step_Complete.Invoke(tempCas.transform);
                });
            oriS[i].DOLocalRotate(tarS[i].localEulerAngles, duration);
        }
    }
    void Z_Group(int group)
    {
        //这一组的原始物体，目标物体和他们的组件
        List<Transform> oriS = oriGroupList[group];
        List<Transform> tarS = tarGroupList[group];
        List<C_AssembleOBJ> casS = casGroupList[group];

        for (int i = 0; i < oriS.Count; i++)
        {
            C_AssembleOBJ tempCas = casS[i];
            tempCas.state = W_AssembleState.装ING;
            //当前组是当前物体groups的第几组
            int asIndex = casS[i].groups.IndexOf(group);
            if (asIndex == 0)
            {
                //Debug.Log("【组别】：" + group+ "     当前物体的名字：" + oriS[i].name+ "      当先组别是当前物体的第几组" + asIndex+ "     目标物体是它本身，但它本身此刻早已改变位置：" + oriS[i].name);
                oriS[i].DOLocalMove(casS[i].oriPostion, duration)
                    .OnComplete(() =>
                    {
                        tempCas.state = W_AssembleState.装ED;
                        if (null != Zhuang_Step_Complete)
                            Zhuang_Step_Complete.Invoke(tempCas.transform);
                    });
                oriS[i].DOLocalRotate(casS[i].oriEuler, duration);
            }
            else
            {
                Transform tar = casS[i].DicGroupTarTrans[casS[i].groups[asIndex - 1]]; ;
                //Debug.Log("【组别】：" + group+ "     当前物体的名字：" + oriS[i].name+ "      当先组别是当前物体的第几组" + asIndex + "     目标物体：" + casS[i].DicGroupTarTrans[casS[i].groups[asIndex - 1]].name);
                oriS[i].DOLocalMove(tar.localPosition, duration).OnComplete(() => tempCas.state = W_AssembleState.装ED);
                oriS[i].DOLocalRotate(tar.localEulerAngles, duration);
            }
        }
    }

    void SetNowTrans_Pre(int groupIndex)
    {
        if (null == movingTrans)
            movingTrans = new List<Transform>();
        if (state == General_State.一步拆ing || state == General_State.一步装ing)
        {
            U_List.ClearList(movingTrans);
        }
        else
            movingTrans = U_List.CopyList(oriGroupList[groupIndex]);
    }
    void SetNowTrans_Aft()
    {
        if (null == movingTrans)
            movingTrans = new List<Transform>();
        U_List.ClearList(movingTrans);
    }

    void Input_Bool()
    {
        U_Bool.MutexBoolTwoMethord
         (
        ref chai_OneKeyAll, ref chaiFlag_OneKeyAll, () => StartCoroutine(C_Order()),
        ref zhuang_OneKeyAll, ref zhuangFlag_OneKeyAll, () => StartCoroutine(Z_Order())
         );
        U_Bool.MutexBoolTwoMethord
         (
         ref chai_One, ref chaiFlag_One, () => StartCoroutine(C_One()),
         ref zhuang_One, ref zhuangFlag_One, () => StartCoroutine(Z_One())
         );
        U_Bool.MutexBoolTwoMethord
         (
         ref chai_OneStepAll, ref chaiFlag_OneStepAll, () => StartCoroutine(C_All()),
         ref zhuang_OneStepAll, ref zhuangFlag_OneStepAll, () => StartCoroutine(Z_All())
         );
    }
    #endregion


    #region Event
    void Log_StepName_Complete_Init()
    {
        if (!logStepOnComplete)
            return;
        this.Chai_Step_Complete.AddListener(Chai_Step_Function);
        this.Zhuang_Step_Complete.AddListener(Zhuang_Step_Function);
    }
    void Log_GroupIndex_Complete_Init()
    {
        if (!logGroupOnComplete)
            return;
        this.Chai_Group_Complete.AddListener(Chai_Group_Function);
        this.Zhuang_Group_Complete.AddListener(Zhuang_Group_Function);
    }
    //哪个物体，什么事件
    void Chai_Group_Function(int i)
    {
        //if(trans== tarTrans)
        Debug.Log("拆完第" + i + "组");

    }
    void Zhuang_Group_Function(int i)
    {
        //if(trans== tarTrans)
        Debug.Log("装完第" + i + "组");

    }
    void Chai_Step_Function(Transform trans)
    {
        Debug.Log("拆完" + trans.name);

    }
    void Zhuang_Step_Function(Transform trans)
    {
        Debug.Log("装完" + trans.name);

    }
    #endregion


    /// <summary>
    /// 将要跑的组，给特效
    /// </summary>
    public void OriGroupEffect(int index)
    {
        if (index >= groupCount)
            return;
        List<Transform> tranS = oriGroupList[index];
        foreach(Transform tran in tranS)
        {

            tran.GetComponent<HighlightableObject>().FlashingOn();
        }
    }
}




[Serializable]
public class W_Int_UnityEvent : UnityEvent<int> { }

[Serializable]
public class W_Transform_UnityEvent : UnityEvent<Transform> { }