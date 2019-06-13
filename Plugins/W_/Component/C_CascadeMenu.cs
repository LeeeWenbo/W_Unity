/*Name:		 				C_Menu	
 *Description: 				级联菜单。目前就是用来做点一个非激活一整个
 *Author:       			lwb
 *Date:         			2019-06-
 *Copyright(C) 2019 by 		company@zhiwyl.com*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class C_CascadeMenu : MonoBehaviour
{

    Button but;
    [Header("如果他是用来激活某物体的button，拖入目标物体，即可实现")]
    public GameObject acitveOBJ;
    [Header("级别，同级别同上司的，隐藏")]
    public int level=1;
    [Header("上级菜单")]
    public C_CascadeMenu higherMenu;


    [Header("是否可以被同级的隐藏，不勾选的话，会隐藏")]
    public bool isMetux=true;
    [Header("是否隐藏其他同级菜单")]
    public bool isMetuxOther=true;
    [HideInInspector]
    public C_CascadeManager manager;

    private void Awake()
    {
        but = GetComponent<Button>();
        if (acitveOBJ != null)
        {
            but.onClick.AddListener(()=> { ActiveLowerObj(); });
        }

        manager = U_RectTransform.GetCanvas(transform).GetComponent<C_CascadeManager>();

    }


    private void ActiveLowerObj()
    {
        manager.ActiveButton(this);
    }
}
