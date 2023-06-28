using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSoul : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private Vector2 lookDir;
    private SpriteRenderer render;
    private ContactFilter2D contactFilter;
    private Collider2D[] colliderArray;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        lookDir = playerMover.LookDir();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
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

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Monster Enter");
        }

        
    }

    private void OnTriggerExit2D(Collider2D MapBoundary)
    {
        if (MapBoundary.gameObject.layer == LayerMask.NameToLayer("MapBoundary"))
        {
            Debug.Log("MapBoundary Enter");
            GameManager.Pool.Release(gameObject);
        }
    }
}
