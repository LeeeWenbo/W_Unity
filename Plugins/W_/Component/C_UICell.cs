/*Name:		 				C_UICell	
 *Description: 				拖拽型的UI框子
 *Author:       			李文博 
 *Date:         			2019-05-29
 *Copyright(C) 2019 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class C_UICell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool showChild;
    Transform item;
    Image cellImage;
    public Color32 sweepColor = new Color32(255, 185, 0, 255);
    Color32 oriColor;
    public string typeName;

    public virtual void ItemSweep()
    {
        cellImage.color = sweepColor;
    }
    public virtual void ToOri()
    {
        cellImage.color = oriColor;
    }
    protected virtual void Init()
    {
        cellImage = GetComponent<Image>();
        oriColor = cellImage.color;
    }
    private void Awake()
    {
        Init();
    }

    //进入效果
    protected virtual void Combine(GameObject item)
    {
        GameObject go = Instantiate(item);
        go.transform.SetParent(transform);
        go.transform.position = Vector3.zero;
    }
    bool mouseIn;
    protected virtual bool IsDraging()
    {
        return C_UICellItem.IsDragingItem;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsDraging() && !mouseIn)
        {
            ItemSweep();
            mouseIn = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseIn)
        {
            ToOri();
            mouseIn = false;
        }
    }
}
