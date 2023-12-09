using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitter : MonoBehaviour, IHittable
{
    public void TakeHit(int damage)
    {
        Debug.Log("!2");
    }
}
