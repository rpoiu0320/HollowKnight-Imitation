using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    [SerializeField] PlayerInfo[] player;
    public PlayerInfo[] Player { get { return player; } }

    [Serializable]
    public class PlayerInfo
    {
        public Player player;
        public string name;
        public string description;

        public float mapHp;
        public float curHp;
        public float maxSoul;
        public float curSoul;
        public float attackDamage;
        public float howlingDamage;
        public float shotSoulDamage;
        public float diveDagame;
    }
}
