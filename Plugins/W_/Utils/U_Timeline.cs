/*Name:		 				U_Timeline	
 *Description: 				TimeLine工具类
 *Author:       			李文博 
 *Date:         			2018-08-01
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class U_Timeline 
{

    public static float timeScale=1;
    static bool isPlaying;
    /// <summary>
    /// 正放
    /// </summary>
    /// <param name="diretor"></param>
    public static void Play(PlayableDirector diretor,float timeScale=1)
    {
        Debug.Log("开始正放");
        Time.timeScale = timeScale;
        currentDirector = diretor;
        isPlaying = true;
        diretor.Play();
    }
    /// <summary>
    /// 倒放
    /// </summary>
    public static void Rewind(PlayableDirector diretor, float timeScale = 1)
    {
        Time.timeScale = timeScale;
        currentDirector = diretor;
        currentDirector.Play(); //控制time，必须得是在Play()状态。
        isRewinding = true;
    }
    static bool isRewinding;
    bool backFlag;
    private void Rewinding()
    {
        if (!isRewinding)
            return;
        if (!backFlag)
        {
            backFlag = true;
            ratio = currentDirector.duration;
        }
        
        currentDirector.time = ratio;
        ratio -= Time.deltaTime;
        if (ratio <= 0)
        {
            Debug.Log("结束倒放");
            isRewinding = false;
            backFlag = false;
            Stop(currentDirector);
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="diretor"></param>
    public static void Pause(PlayableDirector diretor)
    {
        diretor.Pause();
    }
    /// <summary>
    /// 继续播放
    /// </summary>
    /// <param name="diretor"></param>
    public static void Resume(PlayableDirector diretor)
    {
        diretor.Resume();
    }
    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="diretor"></param>
    public static void Stop(PlayableDirector diretor)
    {
        currentDirector = null;
        diretor.Stop();
    }
    public double ratio;
    public  static PlayableDirector currentDirector;

    void Playing()
    {
        if (isPlaying)
        {
            if (currentDirector.duration-currentDirector.time <= 0.1f)
            {
                isPlaying = false;
                currentDirector.time = currentDirector.duration;
                Debug.Log("结束正放");
            }
        }
    }
    
    private void Update()
    {
        Playing();
        Rewinding();
    }


}