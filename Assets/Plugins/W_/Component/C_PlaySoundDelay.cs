using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class C_PlaySoundDelay : MonoBehaviour
{

    AudioSource audioSource;
    [Header("几秒后播放，注意，需要将AudioScource的PlayOnAwake取消")]
    [Range(1, 30)]
    public float firstDelayTime = 3;

    public AudioClip[] clipS;
    public int playIndex=0;
    public float interval=1;
    public float sumTime;
    void Awake()
    {
        Init();
        PlaySound(firstDelayTime);
        StartCoroutine(PlayNext(interval));
    }

    protected virtual void Init()
    {
        if (!GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        SumTime();

    }
    protected virtual void SumTime()
    {
        sumTime = firstDelayTime + clipS.Length * interval;
        foreach(AudioClip clip in clipS)
        {
            sumTime += clip.length;
        }
    }
    public bool havePlayCompleted = false;

    IEnumerator PlayNext(float interval = 1.5f)
    {
        float waitTime = clipS[playIndex].length;
        if (playIndex == 0)
            waitTime += firstDelayTime;
        yield return new WaitForSeconds(waitTime);

        if (playIndex + 1 < clipS.Length)
        {
            playIndex += 1;
            PlaySound(interval);
            StartCoroutine(PlayNext(interval));
        }
        else
        {
            havePlayCompleted = true;
            if (AllPlayComplete != null)
                AllPlayComplete();
        }

    }

    public event Action AllPlayComplete;

    public  void PlaySound(float delayed)
    {
        audioSource.clip = clipS[playIndex];
        audioSource.PlayDelayed(delayed);
    }
    public void StopAllSound()
    {
        audioSource.Stop();
    }
}
