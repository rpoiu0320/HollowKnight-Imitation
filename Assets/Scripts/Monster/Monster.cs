using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour, IHittable
{
    [SerializeField] public MonsterData data;
    [SerializeField] public MonsterData.MonsterName monsterName;

    [NonSerialized] public int curHp;
    [NonSerialized] public bool alive = true;

    public void Awake()
    {
        curHp = data.Monsters[(int)monsterName].maxHp;
    }

    public void TakeHit(int damage)
    {
        curHp -= damage;
        Debug.Log(curHp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IHittable hittable = collision.gameObject.GetComponent<IHittable>();
            hittable?.TakeHit(0);
        }
    }
}
