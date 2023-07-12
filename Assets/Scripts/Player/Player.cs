using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private PlayerData.PlayerInfo playerInfo;
    private int curHp;
    private int curSoul;
    private int attackDamage;
    private int howlingDamage;
    private int shotSoulDamage;
    private int diveDagame;

    private void Awake()
    {
        playerInfo = GameManager.Resource.Instantiate<PlayerData>("Data/PlayerData", Vector3.zero, transform).Player[0];
        curHp = playerInfo.curHp;
        curSoul = playerInfo.curSoul;
        attackDamage = playerInfo.attackDamage;
        howlingDamage = playerInfo.howlingDamage;
        shotSoulDamage = playerInfo.shotSoulDamage;
        diveDagame = playerInfo.diveDagame;
    }

    public int CurHp
    {
        get { return curHp; }
        set { curHp += value; }
    }

    public int CurSoul
    {
        get { return curSoul; }
        set { curSoul += value; }
    }

    public int AttackDamage
    {
        get { return attackDamage; }
    }

    public int HowlingDamage
    {
        get { return howlingDamage; }
    }

    public int ShotSoulDamage
    {
        get { return shotSoulDamage; }
    }

    public int DiveDagame
    {
        get { return diveDagame; }
    }
}
