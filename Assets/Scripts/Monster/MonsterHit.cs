using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : MonoBehaviour, IHittable
{
    public void TakeHit()
    {
        Debug.Log("몬스터 체력 감소");
    }
}
