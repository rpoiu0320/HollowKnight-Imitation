using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howling : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private ContactFilter2D contactFilter;
    private bool limiteMove;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        playerMover.LimitMove(limiteMove = true);
    }

    private void OnDisable()
    {
        playerMover.LimitMove(limiteMove = false);
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
