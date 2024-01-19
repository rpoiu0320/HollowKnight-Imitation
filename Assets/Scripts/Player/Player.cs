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
    [NonSerialized] public GroundCheck groundCheck;
    [NonSerialized] public Vector2 inputDir;
    [NonSerialized] public Vector2 lastStep;
    [NonSerialized] public bool actionLimite = false;
    [NonSerialized] public bool isGround = false;
    [NonSerialized] public bool lastSetpCheck = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
        groundCheck = GetComponent<GroundCheck>();
    }

    private void Start()
    {
        cameraController = GameObject.FindWithTag("CMcamera").GetComponent<CameraController>();
    }

    private void FixedUpdate()
    {
        PlayerGroundCheck();
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    #region GroundCheck
    /// <summary>
    /// Raycast를 사용해 Player 아래 Ground를 체크
    /// </summary>
    private void PlayerGroundCheck()
    {
        if (isGround = groundCheck.GroundLayerCheck())
        {
            animator.SetBool("IsGround", isGround = true);
            lastSetpCheck = true;
        }
        else
        {
            animator.SetBool("IsGround", isGround = false);

            if (lastSetpCheck)
            {
                lastStep = transform.position;
                lastSetpCheck = false;
            }
        }
    }
    #endregion
}
