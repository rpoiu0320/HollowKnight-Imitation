using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource bgmAudio;

    public enum Sound { BGM, SFX, Size }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        GameObject bgmObject = new GameObject();
        bgmObject.gameObject.name = "BGM";
        bgmObject.transform.parent = transform;
        bgmObject.AddComponent<AudioSource>();
        bgmAudio = bgmObject.GetComponent<AudioSource>();
    }

    public void BGMChange(AudioClip nextBGM)
    {
        if (bgmAudio.clip != nextBGM)
        {
            bgmAudio.Stop();
            bgmAudio.clip = nextBGM;
            bgmAudio.Play();
        }
    }
}
