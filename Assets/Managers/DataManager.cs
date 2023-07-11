using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private PlayerData playerData;
    private int curHp;
    private int curSoul;
    private int attackDamage;
    private int howlingDamage;
    private int shotSoulDamage;
    private int diveDagame;

    private void Awake()
    {
        playerData = GameManager.Resource.Instantiate<PlayerData>("Data/PlayerData", transform.position, transform);
    }

    public DataManager()
    {
        curHp = playerData.Player[0].curHp;
        curSoul = playerData.Player[0].curSoul;
        attackDamage = playerData.Player[0].attackDamage;
        howlingDamage = playerData.Player[0].howlingDamage;
        shotSoulDamage = playerData.Player[0].shotSoulDamage;
        diveDagame = playerData.Player[0].diveDagame;
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
