using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour, IHittable
{
    [SerializeField] public MonsterData data;
    [SerializeField] public new MonsterData.monsterName name;

    private int curHp;

    public Monster()
    {
        //curHp = data.Monsters[(int)name].maxHp;
        Debug.Log(curHp);
    }

    private void Start()
    {
        //curHp1 = data.Monsters[0].maxHp;
        //curHp2 = data.Monsters[1].maxHp;
        //curHp = data.Monsters[(int)name].maxHp;
        //curHp = data.Monsters[(int)MonsterData.monsterName.GruzMother].maxHp;
    }

    private void Update()
    {
        //Debug.Log(data.Monsters[(int)MonsterData.monsterName.GruzMother].maxHp);
        //Debug.Log(data.Monsters[(int)name].maxHp);
        //Debug.Log(curHp);
    }

    public void TakeHit(int damage)
    {
        Debug.Log("¸ÂÀ½");
     //   curHp -= damage;
    }
}
