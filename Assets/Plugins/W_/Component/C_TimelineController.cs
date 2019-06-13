/*Name:		 				W_TimelineController	
 *Description: 				
 *Author:       			李文博 
 *Date:         			2018-07-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class C_TimelineController : MonoBehaviour {

   protected  PlayableDirector director;

    [Header("动画播放速度")]
    [Range(0,10)]
    public float speed=1;
    protected virtual void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.extrapolationMode = DirectorWrapMode.None;

        PlayableAsset playableAsset = director.playableAsset;

        director.timeUpdateMode = DirectorUpdateMode.GameTime;

    }
    public bool playOnAwake;
    protected virtual void Start()
    {
        if(playOnAwake)
            U_Timeline.Play(director, speed);
    }

    public void Pause()
    {
        U_Timeline.Pause(director);
    }
    public void Resume()
    {
        U_Timeline.Resume(director);
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            U_Timeline.Play(director, speed);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            U_Timeline.Pause(director);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            U_Timeline.Resume(director);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            U_Timeline.Stop(director);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("开始倒放");
            U_Timeline.Rewind(director, 2);
        }



    }


}
