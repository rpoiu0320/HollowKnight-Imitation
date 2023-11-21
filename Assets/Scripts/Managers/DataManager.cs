using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private PlayerData.PlayerInfo playerInfo;
    private int maxHp;
    private int curHp;
    private int maxSoul;
    private int curSoul;
    private int attackDamage;
    private int howlingDamage;
    private int shotSoulDamage;
    private int diveDagame;

    private void Awake()
    {
        playerInfo = GameManager.Resource.Instantiate<PlayerData>("Data/PlayerData", Vector3.zero, transform).Player[0];
        maxHp = playerInfo.maxHp;
        curHp = playerInfo.curHp;
        maxSoul = playerInfo.maxSoul;
        curSoul = playerInfo.curSoul;
        attackDamage = playerInfo.attackDamage;
        howlingDamage = playerInfo.howlingDamage;
        shotSoulDamage = playerInfo.shotSoulDamage;
        diveDagame = playerInfo.diveDagame;
    }

    private void Update()
    {
        Debug.Log(maxHp + "DataManager maxHp");
        Debug.Log(curHp + "DataManager curHp");
    }

    //public PlayerData.PlayerInfo PlayerInfo
    //{
    //    get { return playerInfo; }  
    //}

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public int CurHp
    {
        get { return curHp; }
    }

    public int MaxSoul
    {
        get { return maxSoul; }
        set { maxHp = value; }
    }

    public int CurSoul
    {
        get { return curSoul; }
    }

    public void IncreaseCurHp()
    {
        if (curHp < maxHp)
            curHp++;
    }

    public void DecreaseCurHp()
    {
        if (curHp > 0)
        curHp--;
    }

    public void IncreaseCurSoul()
    {
        if (curSoul < maxSoul)
            curSoul++;
    }

    public void DecreaseCurSoul()
    {
        if (curSoul > 0)
            curSoul -= 3;
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
