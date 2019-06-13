/*Name:					W_ColorFlicker		
 *Description: 			颜色闪烁组件，挂给谁谁闪，分为once一下就恢复，常亮，闪烁亮
 *Author:       		李文博
 *Date:         		2018-06-27
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ColorFlashing : MonoBehaviour
{
    [Header("目标颜色")]
    public Color32 tarColor = new Color32(255, 218, 10, 255);
    private List<Color> oriColors = new List<Color>();
    Renderer[] renderers;
    List<Material> materials = new List<Material>();

    private bool flashingEndFlag;
    [Header("单次变色,OnceOn()")]
    public bool isOnceChange;
    [Header("长亮,ConstantSwitch()")]
    public bool isConstantChange;
    bool isConstantChanged;
    [Header("闪烁,FlickSwitch()")]
    public bool isFlashing;
    [Header("闪烁速度")]
    [Range(0, 5)]
    public float flashingSpeed = 1;

    //这个地方可以改成OnEnable，实现多次保存，写成协程是因为有可能使用了QuickOutline插件,故而放到每一帧最后执行。
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SaveOriColor();
    }

    private void FixedUpdate()
    {
        //单次触发变色
         OnceUpdate();

        //持续变色
        ConstantUpdate();


        //持续闪烁
         FlashingUpdate();
    }

    void OnceUpdate()
    {
        if (isOnceChange)
        {
            OnceOn();
        }
    }

    void ConstantUpdate()
    {
        if (isConstantChange)
            ConstantOn();
        else
            ConstantOff();
    }

    void FlashingUpdate()
    {
        if (isFlashing)
        {
            ColorFlashing();
            flashingEndFlag = true;
        }
        else if (!isFlashing && flashingEndFlag)
        {
            ToOriColor();
            flashingEndFlag = false;
        }
    }
    public void OnceOn()
    {
        if (isFlashing || isConstantChange)
            return;
        ToTarColor();
        StartCoroutine(OnceOnReset());
    }
    IEnumerator OnceOnReset()
    {
        yield return new WaitForEndOfFrame();
        isOnceChange = false;
        ToOriColor();
    }


    void ConstantOn()
    {
        if (isFlashing|| isConstantChanged)
            return;
        ConstantToTarColor();
    }

    void ConstantOff()
    {
        if (isFlashing || !isConstantChanged)
            return;
        ConstantToOriColor();
    }
    public void ConstantSwitch()
    {
        if (isConstantChange)
            isConstantChange = false;
        else
            isConstantChange = true;
    }





    void FlashingOn()
    {
        if (!isFlashing)
            isFlashing = true;
    }
    public void FlashingOff()
    {
        if (isFlashing)
            isFlashing = false;
    }

    public void FlashingSwitch()
    {
        if (isFlashing)
            isFlashing = false;
        else
            isFlashing = true;
    }


    void SaveOriColor()
    {
        renderers = transform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                materials.Add(renderers[i].materials[j]);
                oriColors.Add(renderers[i].materials[j].color);
            }
        }
    }

    void ToTarColor()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = tarColor;
        }
    }

    void ToOriColor()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = oriColors[i];
        }
    }
    void ConstantToTarColor()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = tarColor;
        }
        isConstantChanged = true;
    }

    void ConstantToOriColor()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = oriColors[i];
        }
        isConstantChanged = false;
    }

    void ColorFlashing()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].color = Color.Lerp(oriColors[i], tarColor, Mathf.PingPong(Time.time * flashingSpeed, 1));
        }
    }
}
