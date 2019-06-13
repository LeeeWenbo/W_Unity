/*Name:					W_AudioUtils		
 *Description: 			声音播放工具类，需要挂载一个物体上，可播放2D，3D
 *Author:       		李文博
 *Date:         		2018-06-21
 *Copyright(C) 2018 by 	北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource) )]
public class C_Audio : MonoBehaviour {

    void Start ()
    {
        audio2D = GetComponent<AudioSource>();
    }
    public static AudioSource audio2D;
    public static AudioSource audioBG;
    


    public void bjChange(bool flag)
    {
        if (flag)
        {
            if (!audio2D.isPlaying)
            {
                audio2D.Play();
            }
        }
        else
        {
            if (audio2D.isPlaying)
            {
                audio2D.Stop();
            }
        }
    }
    public static void W_PlayBG_Play(string pathAndName, float volume)
    {
        audioBG.spatialBlend = 0;
        audioBG.clip = Resources.Load<AudioClip>("Sounds/" + pathAndName);
        audioBG.volume = volume;
        if (!audioBG.isPlaying)
            audioBG.Play();
    }
    public static void W_PlayBG_Stop()
    {
        audioBG.Stop();
    }
    /// <summary>
    /// 使用本方法，需将文件拖到Resources/Sounds目录下
    /// </summary>
    /// <param name="pathAndName"></param>
    public static void W_Play2D_Play(string pathAndName, float volume)
    {
        audio2D.spatialBlend = 0;
        audio2D.clip = Resources.Load<AudioClip>("Sounds/" + pathAndName);
        audio2D.volume = volume;
        if (!audio2D.isPlaying)
            audio2D.Play();
    }
    public static void W_Play2D_Stop()
    {
        audio2D.Stop();
    }

    public static void W_Play3D_Play(string pathAndName, GameObject tarGameObject, float volume,bool isloop=true,bool isPlayOnAwake=true)
    {
        AudioSource audio3D;

        if (tarGameObject.GetComponent<AudioSource>() == null)
        {
            tarGameObject.AddComponent<AudioSource>();
        }
        audio3D = tarGameObject.GetComponent<AudioSource>();
        audio3D.clip = Resources.Load<AudioClip>("Sounds/" + pathAndName);
        audio3D.spatialBlend = 1;
        audio3D.volume = volume;
        if (isloop)
            audio3D.loop = true;
        else
            audio3D.loop = false;

        if (isPlayOnAwake)
            audio3D.playOnAwake = true;
        else
            audio3D.playOnAwake = false;

        if (!audio3D.isPlaying)
            audio3D.Play();
        else
            audio3D.Stop();
    }
   public static void W_Play3D_Stop(GameObject tarGameObject)
    {
        AudioSource audio3D;
        if (tarGameObject.GetComponent<AudioSource>() == null)
        {
            return;
        }
        audio3D = tarGameObject.GetComponent<AudioSource>();
            audio3D.Stop();
    }


}

public enum W_AudioClipType { wav,mp3}
