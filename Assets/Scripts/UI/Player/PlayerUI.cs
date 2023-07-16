using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    protected Player player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
