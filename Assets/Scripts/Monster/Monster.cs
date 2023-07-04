using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IHittable
{
    [SerializeField] public MonsterData data;
    [SerializeField] public new MonsterData.monsterName name;
    //protected float curHp1;
    //protected float curHp2;
    protected float curHp;

    private void Start()
    {
        //curHp1 = data.Monsters[0].maxHp;
        //curHp2 = data.Monsters[1].maxHp;
        curHp = data.Monsters[(int)name].maxHp;
        //curHp = data.Monsters[(int)MonsterData.monsterName.GruzMother].maxHp;
    }

    public void TakeHit(int damage)
    {
        curHp -= damage;
    }
}
