using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
//用来制作小地图，根据物体的位置生成图片，在UI上动
public class C_LittleMap : MonoBehaviour
{
    #region File
    public enum EN_MapType { 地图不动人物动, 地图动人物在中心点 }
    public EN_MapType mapType = EN_MapType.地图不动人物动;

    public Camera cam;
    public RectTransform mapBG;
    Rect bgRect;
    protected float bg_width;
    protected float bg_height;
    protected float bg_left;
    protected float bg_bottom;
    public Transform player;
    public RectTransform playerImage;
    public Transform target;
    public RectTransform targetImage;
    public float camSectionHeight = 650;
    bool mapIsShow = false;

    #endregion
    #region Cycle
    protected virtual void Awake()
    {
        mapIsShow = transform.GetChild(0).gameObject.activeInHierarchy;
        Init();
    }
    protected virtual void Update()
    {
        CamFellow();
        ImagePosition();
    }
    #endregion

    #region Init

    public virtual void Init()
    {
        bgRect = mapBG.rect;
        bg_width = bgRect.width;
        bg_left = bgRect.xMin;
        bg_height = bgRect.height;
        bg_bottom = bgRect.yMin;
    }

    #endregion

    #region Update

    public virtual void MapSwitch()
    {
        mapIsShow = !mapIsShow;
        transform.GetChild(0).gameObject.SetActive(mapIsShow);
    }
    public virtual void CamFellow()
    {
        if (mapType == EN_MapType.地图不动人物动)
            return;
    }
    public virtual void ImagePosition()
    {
        PostionToMapPosition(player, playerImage);
        PostionToMapPosition(target, targetImage);
    }
    //将物体运动方向转化为地图上的箭头指示
    public virtual void ImageEuler(Vector3 moveDirection)
    {
        Vector2 xzDir = new Vector2(moveDirection.x, moveDirection.z);
        Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward, moveDirection).eulerAngles;
        playerImage.localEulerAngles = new Vector3(0, 0, 90 + U_Math.Vector2ToEulerY(xzDir));
    }
    #endregion

    #region Calculate
    public virtual Vector2 GetTrueRatio(Transform tran)
    {
        Vector4 camCorner = U_Camera.GetCorners_Vector4(cam, camSectionHeight);
        float xRatio = U_Math.GetRatio(tran.position.x, camCorner.z, camCorner.w);
        float zRatio = U_Math.GetRatio(tran.position.z, camCorner.y, camCorner.x);
        return new Vector2(xRatio, zRatio);
    }
    public virtual Vector2 GetIconPositionByRatio(Vector2 ratio)
    {
        float x = ratio.x * bg_width + bg_left;
        float y = ratio.y * bg_height + bg_bottom;
        return new Vector2(x, y);
    }
    public virtual void PostionToMapPosition(Transform tran, RectTransform rectTran)
    {
        Vector4 camCorner = U_Camera.GetCorners_Vector4(cam, camSectionHeight);
        float xRatio = U_Math.GetRatio(tran.position.x, camCorner.z, camCorner.w);
        float zRatio = U_Math.GetRatio(tran.position.z, camCorner.y, camCorner.x);
        float x = xRatio * bg_width + bg_left;
        float y = zRatio * bg_height + bg_bottom;
        rectTran.anchoredPosition = new Vector2(x, y);
    }

    public static Vector3 GetLookAtEuler(Transform originalObj, Vector3 targetPoint)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = targetPoint - originalObj.position;
        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);
        //把四元数值转换成角度
        Vector3 resultEuler = lookAtRot.eulerAngles;

        return resultEuler;
    }
    #endregion
}