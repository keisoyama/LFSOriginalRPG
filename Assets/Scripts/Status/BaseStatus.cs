using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class BaseStatus : ScriptableObject
{
    //　キャラクターの名前
    [SerializeField]
    private string characterName = "";
    //　毒状態かどうか
    [SerializeField]
    private bool isPoisonState = false;
    //　痺れ状態かどうか
    [SerializeField]
    private bool isParalyzeState = false;
    //　沈黙状態かどうか
    [SerializeField]
    private bool isSilentState = false;
    //　キャラクターのレベル
    [SerializeField]
    private int level = 1;
    //　最大HP
    [SerializeField]
    private int maxHp = 100;
    //　HP
    [SerializeField]
    private int hp = 100;
    //　最大MP
    [SerializeField]
    private int maxMp = 50;
    //　MP
    [SerializeField]
    private int mp = 50;
    //　素早さ
    [SerializeField]
    private int agility = 5;
    //　力
    [SerializeField]
    private int power = 10;
    //　打たれ強さ
    [SerializeField]
    private int strikingStrength = 10;
    //　魔法力
    [SerializeField]
    private int magicPower = 10;
    //　魔法防御
    [SerializeField]
    private int strikingMagic = 10;
    //物理、魔法耐性
    [SerializeField]
    private int phsicalResistance, magicalResistance;
    //属性軽減耐性
    [SerializeField]
    private int fireResistance, waterResistance, woodResistance, sunResistance, moonResistance;

    public void SetCharacterName(string characterName)
    {
        this.characterName = characterName;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public void SetPoisonState(bool poisonFlag)
    {
        isPoisonState = poisonFlag;
    }

    public bool IsPoisonState()
    {
        return isPoisonState;
    }

    public void SetParalyze(bool numbnessFlag)
    {
        isParalyzeState = numbnessFlag;
    }

    public bool IsParalyzeState()
    {
        return isParalyzeState;
    }

    public void SetSilence(bool silentFlag)
    {
        isSilentState = silentFlag;
    }

    public bool IsSilentState()
    {
        return isSilentState;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetMaxHp(int hp)
    {
        this.maxHp = hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void SetHp(int hp)
    {
        this.hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetMaxMp(int mp)
    {
        this.maxMp = mp;
    }

    public int GetMaxMp()
    {
        return maxMp;
    }

    public void SetMp(int mp)
    {
        this.mp = Mathf.Max(0, Mathf.Min(GetMaxMp(), mp));
    }

    public int GetMp()
    {
        return mp;
    }

    public void SetAgility(int agility)
    {
        this.agility = agility;
    }

    public int GetAgility()
    {
        return agility;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public int GetPower()
    {
        return power;
    }

    public void SetStrikingStrength(int strikingStrength)
    {
        this.strikingStrength = strikingStrength;
    }

    public int GetStrikingStrength()
    {
        return strikingStrength;
    }

    public void SetStrikingMagic(int strikingMagic)
    {
        this.strikingMagic = strikingMagic;
    }

    public int GetStrikingMagic()
    {
        return strikingMagic;
    }

    public void SetMagicPower(int magicPower)
    {
        this.magicPower = magicPower;
    }

    public int GetMagicPower()
    {
        return magicPower;
    }

    public void SetPhsicalResistance(int phsicalResistance)
    {
        this.phsicalResistance = phsicalResistance;
    }

    public int GetPhsicalResistance()
    {
        return phsicalResistance;
    }

    public void SetPhsicaResistance(int phsicalResistance)
    {
        this.phsicalResistance = phsicalResistance;
    }

    public int GetMagicalResistance()
    {
        return magicalResistance;
    }

    public void SetFireResistance(int fireResistance)
    {
        this.fireResistance = fireResistance;
    }

    public int GetFireResistance()
    {
        return fireResistance;
    }

    public void SetWaterResistance(int waterResistance)
    {
        this.waterResistance = waterResistance;
    }

    public int GetWaterResistance()
    {
        return waterResistance;
    }

    public void SetWoodResistance(int woodResistance)
    {
        this.woodResistance = woodResistance;
    }

    public int GetWoodResistance()
    {
        return woodResistance;
    }

    public void SetSunResistance(int sunResistance)
    {
        this.sunResistance = sunResistance;
    }

    public int GetSunResistance()
    {
        return sunResistance;
    }

    public void SetMoonResistance(int moonResistance)
    {
        this.moonResistance = moonResistance;
    }

    public int GetMoonResistance()
    {
        return moonResistance;
    }
}

