/*Name:		 				C_ScreenCapture	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-08-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_ScreenCapture : MonoBehaviour
{
    [Header("截图位置和名称,默认为Test")]
    public string saveName;
    [Header("截图保存位置，默认为Assets下")]
    public string saveFile;
    [Header("截图保存格式，默认为.jpg")]
    public string saveFormat;


    [Header("【截取整个Game视图】")]
    public bool byGameArea;
    bool quanpingFlag;
    public int muti = 1;


    [Header("【按区域截图】")]
    public bool byRect;
    bool quyuFlag;
    public Rect tarRect = new Rect(0, 0, 100, 100);

    [Header("【按UI截图】")]
    public bool byUI;
    bool uiFlag;
    public RectTransform tarRectTran;
    [Header("【按camera截图】")]
    public bool byCamera;
    bool cameraFlag;
    public Camera tarCamera;
    public Vector2 tarSize;


    void Init_FileName()
    {
        if (string.IsNullOrEmpty(saveFile))
        {
            saveFile = U_Path.assets;
        }
        if (string.IsNullOrEmpty(saveName))
        {
            saveName = "Test";
        }
        if (string.IsNullOrEmpty(saveFormat))
        {
            saveFormat = ".jpg";
        }
    }
    void Init_TarUI()
    {
        if (null == tarRectTran)
        {
            if (null != GetComponent<RectTransform>())
                tarRectTran = GetComponent<RectTransform>();
            else
                Debug.LogError("请拖入");
        }
    }
    void Init_TarCamera()
    {
        if (null == tarCamera)
        {
            if (null != GetComponent<Camera>())
                tarCamera = GetComponent<Camera>();
            else
            {
                tarCamera = Camera.main;
            }
        }
    }

    void SetCameraCaptureSize(int width=0,int height=0)
    {
        if (width == 0 || height == 0)
        {
            tarSize = U_Screen.GetPixelSize();
        }
        else
            tarSize = new Vector2(width,height);
    }
    public void Awake()
    {
        Init_FileName();
    }


    private void FixedUpdate()
    {
        U_Bool.BoolTrueMethord(byGameArea, ref quanpingFlag,
        () =>
        {
            string fileNmae = saveFile + saveName + U_ScreenCapture.count + saveFormat;
            U_ScreenCapture.Capture_Game(fileNmae, muti);
            byGameArea = false;
        });

        U_Bool.BoolTrueMethord(byRect, ref quyuFlag,
         () =>
         {
             string fileNmae = saveFile + saveName + U_ScreenCapture.count + saveFormat;
             StartCoroutine(U_ScreenCapture.Capture_Rect(fileNmae, tarRect));
             byRect = false;
         });
        U_Bool.BoolTrueMethord(byUI, ref uiFlag,
         () =>
         {
             Init_TarUI();
             string fileNmae = saveFile + saveName + U_ScreenCapture.count + saveFormat;
             StartCoroutine(U_ScreenCapture.Capture_RectTransform(fileNmae, tarRectTran));
             byUI = false;
         });
        U_Bool.BoolTrueMethord(byCamera, ref cameraFlag,
         () =>
         {
             Init_TarCamera();
             SetCameraCaptureSize();
             string fileNmae = saveFile + saveName + U_ScreenCapture.count + saveFormat;
             StartCoroutine(U_ScreenCapture.Capture_Camera(fileNmae, tarCamera,tarSize));
             byCamera = false;
         });


    }

    [ContextMenu("截图")]
    public void Jietu()
    {
        Debug.Log(Screen.currentResolution.height);
        StartCoroutine(U_ScreenCapture.Capture_Camera(U_Path.testPicture, Camera.main, U_Screen.GetPixelSize()));
    }


}