using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IHittable
{
    public PlayerData data;
    public UnityEvent OnNoise;

    public void TakeHit(int damage)
    {
        Debug.Log("플레이어 피격");
        OnNoise?.Invoke();
    }
}
