using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private SpriteRenderer render;
    private ContactFilter2D contactFilter;
    private bool isGround;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Monster Enter");
        }
    }
}
