/*Name:		 				DZ_Audio	
 *Description: 				临时场景声音控制
 *Author:       			李文博 
 *Date:         			2018-12-21
 *Copyright(C) 2018 by 		智网易联*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_CommonSoundEffect : MonoBehaviour
{
    #region Files
    AudioSource source;
    public AudioClip hover;
    public AudioClip hoverDropdown;
    public AudioClip clickUI;
    public AudioClip openDoor;
    public AudioClip wear;
    public AudioClip warining;
    public AudioClip walk;
    public AudioClip run;

    #endregion
    private void Awake()
    {
        if (GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();
        source = GetComponent<AudioSource>();
    }




    private void W_ClickUI(GameObject eGo)
    {
        //如果是Drop就改声音
         if (eGo.GetComponent<Dropdown>())
        {
            HoverDropdown();
        }
        else if (eGo.GetComponent<Button>()|| eGo.GetComponent<Toggle>() || eGo.GetComponent<Slider>())
        {
            ClickUI();
        }
    }

    /// <summary>
    /// 滑过UI
    /// </summary>
    /// <param name="e"></param>
    private void W_EnterUI()
    {

    }


    protected void Play(bool loop = false,float value=1f)
    {
        source.loop = loop;
        source.volume = value;
        source.Play();
    }
    public void Warning()
    {
        source.clip = warining;
        Play();
    }
    public void HoverUI(float value=0.3f)
    {
        source.clip = hover;
        Play(false, value);
    }
    public void ClickUI(float value = 0.5f)
    {
        source.clip = clickUI;
        Play(false, value);
    }
    public void HoverDropdown()
    {
        source.clip = hoverDropdown;
        Play();
    }
    public void Wear()
    {
        source.clip = wear;
        Play();
    }
    public void OpenDoor()
    {
        source.clip = openDoor;
        Play(false,0.5f);
    }
    public void Walk()
    {
        source.clip = walk;
        Play(true,0.6f);
    }
    public void Run()
    {
        source.clip = run;
        Play(true);
    }
    public void Stop()
    {
        source.clip = null;
        source.Stop();
    }




}
