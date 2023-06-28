using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSoul : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private Vector2 lookDir;
    private SpriteRenderer render;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        lookDir = playerMover.LookDir();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (lookDir.x > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }
        else if (lookDir.x < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == GameObject.FindWithTag("Monster"))
        {
            Debug.Log("Monster Hit");
        }
    }
}
