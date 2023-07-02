using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TowerData;

[CreateAssetMenu (fileName = "PlayerData", menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    
        public Player player;
        public string name;
        public string description;

    
}
