/*Name:		 				C_UICellItem	
 *Description: 				框的内容
 *Author:       			李文博 
 *Date:         			2019-05-
 *Copyright(C) 2019 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C_UICellItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    public static bool IsDragingItem;
    public string typeName;
    protected Transform oriParent;
    protected C_UICell tempCell;
    protected C_BaseMono baseMono;

    RectTransform itemRect;
    Image itemImage;
    Vector2 offsetPos;
    Vector2 oriSize;


    private void Start()
    {
        Init();
    }
    protected virtual void InitBaseMono()
    {
        Debug.LogError("请初始化BaseMono");
    }
    void Init()
    {
        InitBaseMono();
        oriParent = transform.parent;
        itemRect = transform.GetComponent<RectTransform>();
        itemImage = transform.GetComponent<Image>();
        oriSize = itemRect.rect.size;
    }
    protected virtual void StartDrag()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragingItem = true;

        offsetPos = eventData.position - (Vector2)transform.position;
        itemImage.raycastTarget = false;
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, oriSize.x);
        itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, oriSize.y);
        StartDrag();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - offsetPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragingItem = false;
        DetectDrop();
    }

    /// <summary>
    /// 检查落地
    /// </summary>
    /// <returns></returns>
    bool DetectDrop()
    {
        itemImage.raycastTarget = true;
        if (JudeFit())
        {
            MatchED();
            return true;
        }
        else
        {
            NotMactch(itemRect);
            return false;
        }
    }
    protected virtual void NotMactch(RectTransform itemRect)
    {
        //如果不匹配，就回到原位
        U_RectTransform.SetHefuqiYiyangda(itemRect);
    }
    protected virtual void MatchED()
    {
        transform.SetParent(baseMono.hovered.transform);
        U_RectTransform.SetHefuqiYiyangda(itemRect);
    }
    bool JudeFit()
    {

        if (!baseMono.onUI || !baseMono.hovered || !baseMono.hovered.GetComponent<C_UICell>())
            return false;
        else
            return JudeCellAndItem();
    }
    /// <summary>
    /// 通过这两个内容和格子的属性判断是否合适
    /// </summary>
    /// <returns></returns>
    protected virtual bool JudeCellAndItem()
    {
        tempCell = baseMono.hovered.GetComponent<C_UICell>();
        if (tempCell.typeName == typeName)
            return true;
        else
            return false;
    }

}
