using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCommonAttackPointX : MonoBehaviour
{
    private PlayerMover playerMover;
    private float originPositionX;

    private void Awake()
    {
        playerMover = GetComponentInParent<PlayerMover>();
    }

    private void Start()
    {
        originPositionX = transform.position.x;
    }

    private void Update()
    {
        ChangePositionX();
    }

    private void ChangePositionX()
    {
        if (playerMover.InputDir().x > 0)
        {
            transform.Translate(new Vector3(originPositionX, transform.position.y));
        }
        else if (playerMover.InputDir().x < 0)
        {
            transform.Translate(new Vector3(-originPositionX, transform.position.y));
        }
    }
}
