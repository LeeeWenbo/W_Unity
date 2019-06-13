/*Name:		 				C_Button	
 *Description: 				主要是为了点击效果，回调暂时不在里边
 *Author:       			李文博 
 *Date:         			2018-11-20
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class C_Button : MonoBehaviour
{
   ///"Button按下抬起，Switch按下不抬起，Toggle按下不抬起并且和其他的互斥，Slider滑块"。。。。
   /////注意，除了Slider意外，其他的都是用的Button，只是自带了显示效果而已，具体实现还是在回调函数上
    public enum ButtonType { Button, Switch, Toggle, Slider }
    public ButtonType buttonType = ButtonType.Button;
    Color oriImageColor;
    Color initPressedColor;
    Image image;
    Button but;
    public string Name
    {
        get
        {
            return transform.name;
        }
    }

    [Header("是否被按下了")]
    public bool isButtonPressed = false;

    [Header("Switch是否被按下了")]
    public bool isSwitchPressed = false;
    [Header("一般对应bool")]
    public bool switchToExternal;


    [Header("Toggle是否被按下了")]
    public bool isTogglePressed = false;

    [Header("☆把同组互斥的button弄到这里☆")]
    public List<C_Button> toggleGroup;
    [Header("一般对应枚举，-1是谁都没选，0是选了组里第一个button，类推")]
    public int toggleInt = -1;

    //正常button点击了之后回返颜色的时间
    public const float clickBackTime = 0.2f;
    protected virtual void Awake()
    {
        ObjInit();
    }

    protected virtual void ObjInit()
    {
        image = transform.GetComponent<Image>();
        but = transform.GetComponent<Button>();
        oriImageColor = image.color;
        initPressedColor = but.colors.pressedColor;
    }

    //主要是效果上的，回调方法没写在这里。
    public virtual void OnClick_Effect()
    {
        if (buttonType == ButtonType.Button)
        {
            Click_Button();
        }
        else if (buttonType == ButtonType.Switch)
        {
            Click_Switch();
        }
        else if (buttonType == ButtonType.Toggle)
        {
            Click_Toogle();
        }
        //daixiugao xg
        //else if (buttonType == ButtonType.Slider)
        //{
        //    Click_Slider();
        //}
    }


    protected virtual void Click_Button()
    {
        ToPressColor();
        StartCoroutine(IEToOriColor());
    }
    protected virtual void Click_Switch()
    {
        if (!isSwitchPressed)
        {
            ToPressColor();
            isSwitchPressed = true;
        }
        else
        {
            ToOriColor();
            isSwitchPressed = false;
        }
    }
    //要求是同一组里边只能有一个，谁是最后被点的，其他组的都自动变成false  
    protected virtual void Click_Toogle()
    {
        for (int i = 0; i < toggleGroup.Count; i++)
        //foreach(C_Button toggle in toggleGroup)
        {
            toggleGroup[i].isTogglePressed = false;
            toggleGroup[i].ToOriColor();
        }
        toggleInt = toggleGroup.IndexOf(this);
        isTogglePressed = true;
        ToPressColor();
    }
    protected virtual void Click_Slider()
    {
        StartCoroutine(IEToOriColor());
    }


    public virtual void ToPressColor()
    {
        image.color = initPressedColor;
    }
    public virtual void ToOriColor()
    {
        image.color = oriImageColor;
    }
    IEnumerator IEToOriColor(float waitTime = clickBackTime)
    {
        yield return new WaitForSeconds(waitTime);
        image.color = oriImageColor;
    }

}
