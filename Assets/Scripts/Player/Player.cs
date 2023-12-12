using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [NonSerialized] public Animator animator;
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public Collider2D col;
    [NonSerialized] public SpriteRenderer render;
    [NonSerialized] public GameObject attackPoint;
    [NonSerialized] public CameraController cameraController;
    [NonSerialized] public Vector2 inputDir;
    [NonSerialized] public LayerMask groundLayer;
    [NonSerialized] public bool actionLimite = false;
    [NonSerialized] public bool isGround = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        cameraController = GameObject.FindWithTag("CMcamera").GetComponent<CameraController>();
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }
}
