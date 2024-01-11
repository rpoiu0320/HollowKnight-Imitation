using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        GameManager.Data.DecreaseCurSoul();
    }
}
