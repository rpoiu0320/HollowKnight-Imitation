using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("플레이어 체력 감소");
    }
}
