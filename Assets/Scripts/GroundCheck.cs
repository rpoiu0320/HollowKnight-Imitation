using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float layerLength;
    [NonSerialized] private LayerMask groundLayer;

    private void Awake()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    public bool GroundLayerCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, layerLength, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * layerLength, Color.red);

        if (hit.collider != null)
            return true;
        else
            return false;
    }
}
