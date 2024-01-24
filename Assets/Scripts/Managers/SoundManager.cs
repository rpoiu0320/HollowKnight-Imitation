using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    private GameObject bgmObject;

    public enum Sound { BGM, SFX, Size}

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        gameObject.AddComponent<AudioSource>();
        
        GameObject bgmObject = new GameObject();
        bgmObject.gameObject.name = "BGM";
        bgmObject.transform.parent = transform;
    }
}
