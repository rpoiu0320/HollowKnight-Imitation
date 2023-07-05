using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flying : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask reversMask;

    private Monster monster;
    private Collider2D col;
    private SpriteRenderer render;
    private Vector2 moveDir;
    [SerializeField] private bool isFly;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
        //groundMask = LayerMask.NameToLayer("Ground"); // layerMask 이상하게 들어감
    }

    private void Start()
    {
        moveDir = new Vector2 (-1, -1);
        isFly = true;
    }

    private void Update()
    {
        if (isFly)
            Move();
    }

    private void FixedUpdate()
    {
        //HorizonCheck();
        //VerticalCheck();
    }

    private void Move()
    {
        if (moveDir.x > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
        else if (moveDir.x < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }

        if (moveDir.y > 0)
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        }
        else if (moveDir.y < 0)
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
    }

    //private void VerticalCheck()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, moveDir.y), 3f, reversMask);
    //    Debug.DrawRay(transform.position, new Vector2(0, moveDir.y) * 3f, Color.red);
        
    //    if (hit.collider != null && hit.collider.gameObject != gameObject)
    //    {
    //        moveDir.y = -moveDir.y;
    //    }
    //}

    //private void HorizonCheck()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(moveDir.x, 0), 3f, reversMask);
    //    Debug.DrawRay(transform.position, new Vector2(moveDir.x, 0) * 3f, Color.red);

    //    if (hit.collider != null && hit.collider.gameObject != gameObject)
    //    {
    //        moveDir.x = -moveDir.x;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y != 0)
            moveDir = new Vector2(moveDir.x, collision.contacts[0].normal.y);
        else if (collision.contacts[0].normal.x != 0)
            moveDir = new Vector2(collision.contacts[0].normal.x, moveDir.y);

        //HorizonCheck();
        //VerticalCheck();
    }

    public bool IsFly(bool isFly)
    {
        this.isFly = isFly;

        return this.isFly;
    }

    // 콜라이더에 부딪히면 반대 방향으로, 천장이나 땅에 부딛히면 반대로
}
