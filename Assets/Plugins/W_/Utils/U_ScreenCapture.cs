/*Name:		 				U_ScreenCapture	
 *Description: 				Unity截屏
 *Author:       			李文博 
 *Date:         			2018-08-30
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U_ScreenCapture
{
    public static int count=0;

    /// <summary>
    /// 截取整个Game界面
    /// </summary>
    /// <param name="fileName">保存路径+名称+后缀名</param>
    /// <param name="definition">像素放大倍数</param>
    public static void Capture_Game(string fileName, int definition = 1)
    {
        //全屏截图,图片保存路径，提高分辨率系数
        ScreenCapture.CaptureScreenshot(fileName, definition);
        count += 1;
    }



    /// <summary>
    /// 协程，根据一个Rect(屏幕区域)来截取指定范围的屏幕  
    /// </summary>
    /// <param name="mFileName"></param>
    /// <param name="mRect"></param>
    /// <returns></returns>
    public static IEnumerator Capture_Rect(string mFileName, Rect mRect)
    {
        //等待渲染线程结束  
        yield return new WaitForEndOfFrame();
        //初始化Texture2D  
        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据  
        mTexture.ReadPixels(mRect, 0, 0);
        mTexture.Apply();
        //将图片信息编码为字节信息  
        byte[] bytes = mTexture.EncodeToPNG();
        //保存  
        System.IO.File.WriteAllBytes(mFileName, bytes);
        count += 1;
    }
    /// <summary>
    /// 协程，根据一个UI来截取指定范围的屏幕  
    /// </summary>
    /// <param name="mFileName">地址+名字+后缀</param>
    /// <param name="tran">截取的UI工具</param>
    /// <returns></returns>
    public static IEnumerator Capture_RectTransform(string mFileName, RectTransform tran)
    {
        Rect rect = U_RectTransform.GetReal_Rect(tran);
        //等待渲染线程结束  
        yield return new WaitForEndOfFrame();
        //初始化Texture2D  
        Texture2D mTexture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据  
        mTexture.ReadPixels(rect, 0, 0);
        mTexture.Apply();
        //将图片信息编码为字节信息  
        byte[] bytes = mTexture.EncodeToPNG();
        //保存  
        System.IO.File.WriteAllBytes(mFileName, bytes);
        count += 1;
    }





    /// <summary>
    /// 截取某一个Camera到某一大小的图里。
    /// </summary>
    /// <param name="mFileName">地址+名字+后缀</param>
    /// <param name="camera">被保存的摄像机</param>
    /// <param name="size">一般为屏幕大小</param>
    /// <returns></returns>
    public static IEnumerator Capture_Camera(string mFileName, Camera camera, Vector2 size)
    {
        yield return new WaitForEndOfFrame();
        //初始化RenderTexture  
        RenderTexture mRender = new RenderTexture((int)size.x, (int)size.y, 0);
        //设置相机的渲染目标  
        camera.targetTexture = mRender;
        //开始渲染  
        camera.Render();
        //激活渲染贴图读取信息  
        RenderTexture.active = mRender;
        Texture2D mTexture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        //读取屏幕像素信息并存储为纹理数据  
        Rect mRect = new Rect(0,0, (int)size.x, (int)size.y);
        mTexture.ReadPixels(mRect, 0, 0);
        mTexture.Apply();
        //释放相机，销毁渲染贴图  
        camera.targetTexture = null;
        RenderTexture.active = null;
        //GameObject.Destroy(mRender);
        byte[] bytes = mTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(mFileName, bytes);
        count += 1;
    }
}
