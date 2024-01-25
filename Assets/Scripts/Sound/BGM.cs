using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private void Start()
    {
        GameManager.Sound.BGMChange(bgm);
    }
}
