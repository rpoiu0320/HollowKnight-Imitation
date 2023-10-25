using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField] MonsterInfo[] monsters;
    public MonsterInfo[] Monsters { get { return monsters; } }

    public enum MonsterName { None, GruzMother, Gruzzer, HuskHornhead };

    [Serializable]
    public class MonsterInfo
    {
        public Monster monster;
        public string name;
        public string description;
        
        public int maxHp;
        public int haveGeo;
    }
}
