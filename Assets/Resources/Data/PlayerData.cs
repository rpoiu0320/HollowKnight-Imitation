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

        public int mapHp;
        public float curHp;
        public int maxSoul;
        public float curSoul;
        public int attackDamage;
        public int howlingDamage;
        public int shotSoulDamage;
        public int diveDagame;
    }
}
