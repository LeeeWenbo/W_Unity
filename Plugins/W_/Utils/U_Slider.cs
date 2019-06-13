/*Name:		 				U_Slider	
 *Description: 				Slider的工具类
 *Author:       			李文博 
 *Date:         			2018-11-21
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class U_Slider
{


    //目标是，将0~1的 变化，转变成x-v%~x+v%的变化。
    //比如cslider初始值是0.618，我想让他从0~1是在90%~110%之间变动
	public static float SliderConvert(Slider slider,float precent)
    {
        float value=1;
        //slider.value从0到1，返回值从0.9~1.1
        value =(1-precent/100)+ slider.value*precent/100*2;   
        return value;
    }

}
