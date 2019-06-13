/*Name:		 				C_FixButOnDeselected	
 *Description: 				想实现自动添加那个事件,带导航的点击松手之后，还是高亮状态
 *Author:       			李文博 
 *Date:         			2019-05-
 *Copyright(C) 2019 by 		华泰安信*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class C_FixButOnDeselected : MonoBehaviour
{
    private void Start()
    {
        AddOnDeselect();
    }
    void MyOnDec(BaseEventData data)
    {
        Button but = GetComponent<Button>();
        BaseEventData baseEventData = new BaseEventData(EventSystem.current);
        but.OnDeselect(baseEventData);
    }

    public void AddOnDeselect()
    {
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerExit;
        UnityAction<BaseEventData> callBack = new UnityAction<BaseEventData>(MyOnDec);
        myclick.callback.AddListener(callBack);
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(myclick);
    }
}
