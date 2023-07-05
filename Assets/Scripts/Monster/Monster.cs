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

    public void Awake()
    {
        curHp = data.Monsters[(int)monsterName].maxHp;
    }

    private void Update()
    {

    }

    public void TakeHit(int damage)
    {
        curHp -= damage;
    }
}
