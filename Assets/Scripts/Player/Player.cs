using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHittable
{
    public PlayerData data;

    public void TakeHit(int damage)
    {
        Debug.Log("�÷��̾� �ǰ�");
    }
}
