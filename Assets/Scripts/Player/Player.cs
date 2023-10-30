using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum UpDown { None, Up, Down }

    //protected SoulUI soulUi;  DataManager를 통해 연동
    //protected HpUI hpUi;
    [NonSerialized] public Animator animator;
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public SpriteRenderer render;
    [NonSerialized] public GameObject attackPoint;
    [NonSerialized] public Vector2 inputDir;
    [NonSerialized] public LayerMask groundLayer;
    [NonSerialized] public UpDown upDown;
    [NonSerialized] public bool actionLimite;
    [NonSerialized] public bool isGround;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        actionLimite = false;
        isGround = false;
        upDown = UpDown.None;
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }
}
