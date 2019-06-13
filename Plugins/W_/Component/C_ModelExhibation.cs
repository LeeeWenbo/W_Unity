/*Name:		 				C_ExhibationModel	
 *Description: 				展示台,用txt保存信息，动画使用C_ObjectActiveAnimation配合,Model手动托的话，就不清除了。
 *Author:       			李文博 
 *Date:         			2018-11-2
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class C_ModelExhibation : MonoBehaviour
{
    #region File
    [Header("当前展示第几个模型，-1表示都不表示")]
    public int showIndex = 0;
    [Header("展览物")]
    public List<Transform> models = new List<Transform>();
    [Header("是否最后一个和第一个连起来")]
    public bool isLoop = true;
    [Header("是否需要切换动画")]
    public bool isHaveSwitchAnimation = true;
    protected Transform lastShowTran;
    [Header("先展示哪一个，0是第一个,负数不展示")]
    public int firstShow = 0;

    public bool isShowText;
    [Header("txt文件")]
    public TextAsset txt;
    [Header("txt名称，如果上为空的话，用这个，把它放到StreamingAssets里")]
    public string txtName;
    [Header("名字UI")]
    public Text nameText;
    [Header("介绍UI")]
    public Text introducionText;
    [Header("介绍是否首行缩进")]
    public bool isIntroductionIndent = true;
    [Header("有几类数据，比如名字+介绍，是2类")]
    public int typeCount = 2;
    List<string> names = new List<string>();
    List<string> introductions = new List<string>();

    AudioSource audioSource;
    public bool playSound = false;
    [Header("切换时声音")]
    public List<AudioClip> sounds = new List<AudioClip>();
    [Header("是否已经完成过一圈")]
    public bool isShowUpOneTime = false;
    [Header("是否当前是最后一个")]
    public bool isFinalOne = false;
    [Header("是否自动切换模型")]
    public bool automatic = false;
    [Header("是否根据声音长度自动切换模型")]
    public bool automaticBysounds = false;
    [Header("切换一个模型所需时间")]
    public float automaticTime = 10;
    float timer = 0;

    public event Action ShowUp_OneTime;
    public event Action ShowUp_ManyTime;
    #endregion

    #region Cycle
    protected virtual void Awake()
    {
        TxtReadInit();
        DisplayInit();
        DisplatAddC_OBJActive();
        SoundsInit();
        oriAutomatic = automatic;
        oriAutomaticBysounds = automaticBysounds;
    }
    protected virtual void OnEnable()
    {
        if (firstShow >= 0)
        {
            showIndex = firstShow;
            ShowByIndex(showIndex);
        }
    }
    protected virtual void Update()
    {
        AutoExhibate();
        AutoExhibate_BySounds(1);
    }
    #endregion

    #region Init

    public virtual void SoundsInit()
    {
        if (!playSound)
            return;
        if (sounds.Count != models.Count)
            return;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public virtual void DisplayInit()
    {
        if (models.Count != 0)
            return;
        for (int i = 0; i < transform.childCount; i++)
        {
            models.Add(transform.GetChild(i));
        }
    }
    public virtual void DisplatAddC_OBJActive()
    {
        if (!isHaveSwitchAnimation)
            return;
        foreach (Transform tran in models)
        {
            tran.gameObject.AddComponent<C_ObjectActiveAnimation>();
        }
    }
    public virtual void TxtReadInit()
    {
        if (!isShowText)
            return;
        List<List<string>> strListList;
        if (txt != null)
        {
             strListList = U_TextAssets.TxtToStrListList(txt, typeCount, U_TextAssets.En_ListSplitType.根据类别分, U_String.TextSplitString, U_String.En_RemoveLine.去掉空行);

        }
        else
        {
            strListList = U_TextAssets.TxtToStrListList(U_IO.ReadByFileToString(Application.streamingAssetsPath + @"\" + txtName), typeCount, U_TextAssets.En_ListSplitType.根据类别分);

        }
        names = strListList[0];
        introductions = strListList[1];
    }
    #endregion

    [Header("只要手动调一次，就相当于自动取消自动")]
    public bool shoudongWillInsteadAuto = false;
    [Header("点了回放取消自动播放，当跳到一个新的时，继续自动")]
    public bool shoudongWillInsteadAuto_Half = true;
    [Header("回放不再播放声音了（最接近第一个的不算）")]
    public bool dontPlaySoundWhenDisplayed=true;
    [Header("最大播放到了这里")]
    public float maxIndex;


    bool oriAutomatic;
    bool oriAutomaticBysounds;
    #region 切换模型

    public virtual void AutoJust()
    {
        if (shoudongWillInsteadAuto && (oriAutomatic || oriAutomaticBysounds))
        {
            automatic = false;
            automaticBysounds = false;
        }

    }
    public virtual void AutoJustHalf_Next()
    {
        AutoJust();
        if (shoudongWillInsteadAuto_Half && (oriAutomatic || oriAutomaticBysounds))
        {
            if (showIndex == maxIndex-1)
            {
                automatic = oriAutomatic;
                automaticBysounds = oriAutomaticBysounds;
            }
        }
    }
    public virtual void AutoJustHalf_Last()
    {
        AutoJust();
        if (shoudongWillInsteadAuto_Half && (oriAutomatic || oriAutomaticBysounds))
        {
            automatic = false;
            automaticBysounds = false;

        }
    }
    public virtual void NextModel()
    {
        AutoJustHalf_Next();


        if (showIndex == models.Count - 1)
        {
            if (isLoop)
                showIndex = -1;
            else
                return;
        }
        showIndex += 1;
        ShowByIndex(showIndex);
    }
    public virtual void LastModel()
    {
        AutoJustHalf_Last();
        if (showIndex == 0)
        {
            if (isLoop)
                showIndex = models.Count;
            else
                return;
        }
        showIndex -= 1;
        ShowByIndex(showIndex);
    }

    bool endFlag = false;
    public virtual void ShowByIndex(int index)
    {
        timerBySound = 0;
        timer = 0;
        HideObj(lastShowTran);
        lastShowTran = models[index];
        ShowObj(models[index]);
        UITextUpdate(index);
        if(playSound)
        SoundPlay(index);
        maxIndex = Mathf.Max(maxIndex, index);
        isFinalOne = (index == models.Count - 1);
        if (null != ShowUp_ManyTime && isFinalOne)
        {
            ShowUp_ManyTime();
        }
        if (null != ShowUp_OneTime && isFinalOne && !endFlag)
        {
            isShowUpOneTime = true;
            endFlag = true;
            ShowUp_OneTime();
        }
    }

    public virtual void AutoExhibate()
    {
        if (automatic)
        {
            timer += Time.deltaTime;
            if (timer >= automaticTime)
            {
                NextModel();
            }
        }
    }
    public float timerBySound;
    public virtual void AutoExhibate_BySounds(float interval = 1.5f)
    {
        if (automaticBysounds)
        {
            timerBySound += Time.deltaTime;
            automaticTime = sounds[showIndex].length + interval;
            if (timerBySound >= automaticTime)
            {
                NextModel();
            }
        }
    }

    public virtual void ShowObj(Transform tran, float duration = 0.5f)
    {
        if (null == tran)
            return;
        tran.gameObject.SetActive(true);
    }
    public virtual void HideObj(Transform tran)
    {
        if (null == tran)
            return;
        tran.gameObject.SetActive(false);
    }

    public virtual void StopSound()
    {
        if(GetComponent<AudioSource>()!=null)
        {
            GetComponent<AudioSource>().Stop();
        }
        playSound = false;
    }
    #endregion

    #region UI&Sound
    public virtual void UITextUpdate(int index)
    {
        if (nameText != null)
            nameText.text = names[index];

        if (introducionText != null)
        {
            if (isIntroductionIndent)
            {
                string introducion = U_String.UGUI_Text_Indent(introductions[index]);
                introducionText.text = introducion;
            }
            else
                introducionText.text = introductions[index];
        }

    }

    public virtual void SoundPlay(int index)
    {
        if (!playSound || audioSource == null)
            return;
        
        if (dontPlaySoundWhenDisplayed && index < maxIndex)
        {
            audioSource.clip = null;
            return;
        }
        audioSource.clip = sounds[index];
        audioSource.Play();
    }

    #endregion


}
