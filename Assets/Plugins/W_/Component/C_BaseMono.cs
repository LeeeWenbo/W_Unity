/*Name:		 				C_BaseMono	
 *Description: 				需要Button什么的是有导航的，配套那个Fix_But
 *Author:       			李文博 
 *Date:         			2019-05-14
 *Copyright(C) 2019 by 		智网易联*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C_BaseMono : MonoBehaviour
{
    [Header("悬停监视的画布")]
    public GameObject canvas;

    [HideInInspector]
    public GameObject hovered;
    [HideInInspector]
    public GameObject lastHovered;
    [Header("点击在")]
    [HideInInspector]
    public GameObject clicked;
    [HideInInspector]
    public GameObject lastClicked;

    PointerEventData pointerEventData;
    GraphicRaycaster graphicRaycaster;
    RaycastHit objHit;
    Ray objRay;
    public bool onUI;
    [Header("是否包含声音")]
    public bool withAudio;
    #region Cycle
    protected virtual void Awake()
    {
        pointerEventData = new PointerEventData(EventSystem.current);
        SetCanvas(canvas);
    }

    protected virtual void Update()
    {
        Ray();
        Hover();
        Click();
    }
    #endregion


    #region public
    public void SetCanvas(GameObject canvas)
    {
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }
    #endregion


    #region Main Private
    void Ray()
    {
        pointerEventData.position = Input.mousePosition;
        objRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void Hover()
    {
        List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, uiRaycastResults);
        if (uiRaycastResults.Count != 0)
        {
            hovered = uiRaycastResults[0].gameObject;
            Hover_UI(hovered);
        }

        else if (Physics.Raycast(objRay, out objHit, 100f))
        {
            hovered = objHit.collider.gameObject;
            Hover_OBJ(objHit);
        }
        else
        {
            hovered = null;
        }

        //每一次不一样，都会执行一次
        if (lastHovered != hovered)
        {
            if (hovered != null && hovered.GetComponent<RectTransform>() != null)
                Enter_Obj(hovered);

            if (lastHovered != null)
                Exit_UI(lastHovered);
        }

        lastHovered = hovered;
    }


    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null)
            {
                clicked = EventSystem.current.currentSelectedGameObject;
                Click_UI(clicked);
                onUI = true;
            }
            else
            {
                if (Physics.Raycast(objRay, out objHit, 10f))
                {
                    clicked = objHit.collider.gameObject;
                    Click_OBJ(objHit);
                }
                else
                {
                    clicked = null;
                }
            }
            lastClicked = clicked;
        }
        if (Input.GetMouseButtonUp(0))
        {
            onUI = false;
        }
        if (Input.GetMouseButton(1))
        {

        }
    }
    #endregion


    #region virtual

    protected virtual void Enter_UI(GameObject go) { }
    protected virtual void Enter_Obj(GameObject go) { }

    protected virtual void Exit_UI(GameObject go) { }
    protected virtual void Exit_OBJ(GameObject go) { }


    protected virtual void Hover_OBJ(RaycastHit hit)
    {
        GameObject go = hit.collider.gameObject;
        Hover_OBJ(go);
    }
    protected virtual void Hover_OBJ(GameObject go) { }
    protected virtual void Hover_UI(GameObject go) { }




    #region Click
    protected virtual void Click_OBJ(RaycastHit hit)
    {
        GameObject go = hit.collider.gameObject;
        Click_OBJ(go);
    }
    protected virtual void Click_OBJ(GameObject go){ }
    protected virtual void Click_UI(GameObject go){}
    #endregion

    #endregion
}
