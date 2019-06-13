/*Name:		 				C_TextPrint	
 *Description: 				Text组件逐字打印
 *Author:       			李文博 
 *Date:         			2018-11-
 *Copyright(C) 2018 by 		北京兆泰源信息技术有限公司*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_TextPrint : MonoBehaviour
{

    int i = 0;
    [Header("true打印")]
    public bool isNeedPrint = true;
    public float charsPerSeconds = 0.1f;
    private string content;
    private Text textTest;
    private float timer;
    private int currentPos;
    private bool isActive;

    public bool playSound=true;
    public AudioClip typingSound;
    AudioSource audioSource;


    public void ResetToOri()
    {
        currentPos = 0;
    }
    /// <summary>
    /// 重新打印至于要激活非激活该脚本即可
    /// </summary>
    void OnEnable()
    {
        textTest = GetComponent<Text>();
        typing = false;
        if (playSound)
        {
            if (!gameObject.GetComponent<AudioSource>())
                gameObject.AddComponent<AudioSource>();
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            if (typingSound != null)
            {
                audioSource.clip = typingSound;
            }
        }
        if (i == 0)
        {
            content = textTest.text;
            i++;
        }
        timer = charsPerSeconds;
        isActive = true;
    }
    private void OnDisable()
    {
        i = 0;
        currentPos = 0;
        content = "";
        textTest = null;
        isActive = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActive == true && isNeedPrint)
        {
            StartTyperEffect();
        }
    }
    public void TyperEffect()
    {
        isActive = true;
    }

    public event Action FinishEvent;
    /// <summary>
    /// 正在打字
    /// </summary>
    bool typing;
    private void StartTyperEffect()
    {
        timer += Time.deltaTime;
        if (timer > charsPerSeconds)
        {
            timer -= charsPerSeconds;
            currentPos++;

            textTest.text = content.Substring(0, currentPos);

            if (audioSource != null && !typing)
            {
                typing = true;
                audioSource.Play();
            }
            if (currentPos >= content.Length)
            {
                FinishTyperEffect();
                if(FinishEvent != null)
                FinishEvent();
            }
        }
    }
    private void FinishTyperEffect()
    {
        typing = false;
        isActive = false;
        timer = charsPerSeconds;
        currentPos = 0;
        textTest.text = content;
        audioSource.Stop();
    }

}
