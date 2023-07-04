using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IHittable
{
    public void TakeHit(int damage)
    {
        Debug.Log("플레이어 체력 감소");
    }
}
