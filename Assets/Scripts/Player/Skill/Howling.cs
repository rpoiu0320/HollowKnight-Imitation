using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howling : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private SpriteRenderer render;
    private ContactFilter2D contactFilter;
    private float direction;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        direction = playerMover.LastDirX();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (direction > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }
        else if (direction < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Monster Enter");
        }
    }
}
