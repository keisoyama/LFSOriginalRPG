using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "LevelUpData", menuName = "CreateLevelUpData")]
public class LevelUpData : ScriptableObject
{

    //　レベルアップに必要なトータル経験値
    [SerializeField]
    private LevelUpDictionary requiredExperience = null;
    //　MaxHpが上がる率
    [SerializeField]
    private float probabilityToIncreaseMaxHP = 100f;
    //　MaxMPが上がる率
    [SerializeField]
    private float probabliityToIncreaseMaxMP = 100f;
    [SerializeField]
    //　素早さが上がる率
    private float probabilityToIncreaseAgility = 100f;
    [SerializeField]
    //　力が上がる率
    private float probabilityToIncreasePower = 100f;
    [SerializeField]
    //　打たれ強さが上がる率
    private float probabilityToIncreaseStrikingStrength = 100f;
    [SerializeField]
    //　魔法力が上がる率
    private float probabilityToIncreaseMagicPower = 100f;

    //　MaxHPが上がった時の最低値
    [SerializeField]
    private int minHPRisingLimit = 10;
    //　MaxMPが上がった時の最低値
    [SerializeField]
    private int minMPRisingLimit = 10;
    //　素早さが上がった時の最低値
    [SerializeField]
    private int minAgilityRisingLimit = 1;
    //　力が上がった時の最低値
    [SerializeField]
    private int minPowerRisingLimit = 1;
    //　打たれ強さが上がった時の最低値
    [SerializeField]
    private int minStrikingStrengthRisingLimit = 1;
    //　魔法力が上がった時の最低値
    [SerializeField]
    private int minMagicPowerRisingLimit = 1;


    //　MaxHPが上がった時の最高値
    [SerializeField]
    private int maxHPRisingLimit = 50;
    //　MaxMPが上がった時の最高値
    [SerializeField]
    private int maxMPRisingLimit = 50;
    //　素早さが上がった時の最高値
    [SerializeField]
    private int maxAgilityRisingLimit = 2;
    //　力が上がった時の最高値
    [SerializeField]
    private int maxPowerRisingLimit = 2;
    //　打たれ強さが上がった時の最高値
    [SerializeField]
    private int maxStrikingStrengthRisingLimit = 2;
    //　魔法力が上がった時の最高値
    [SerializeField]
    private int maxMagicPowerRisingLimit = 2;

    //　このレベルに必要な経験値
    public int GetRequiredExperience(int level)
    {
        return requiredExperience.Keys.Contains(level) ? requiredExperience[level] : int.MaxValue;
    }

    public LevelUpDictionary GetLevelUpDictionary()
    {
        return requiredExperience;
    }

    public float GetProbabilityToIncreaseMaxHP()
    {
        return probabilityToIncreaseMaxHP;
    }
    public float GetProbabilityToIncreaseMaxMP()
    {
        return probabliityToIncreaseMaxMP;
    }
    public float GetProbabilityToIncreaseAgility()
    {
        return probabilityToIncreaseAgility;
    }
    public float GetProbabilityToIncreasePower()
    {
        return probabilityToIncreasePower;
    }
    public float GetProbabilityToIncreaseStrikingStrength()
    {
        return probabilityToIncreaseStrikingStrength;
    }
    public float GetProbabilityToIncreaseMagicPower()
    {
        return probabilityToIncreaseMagicPower;
    }

    public int GetMinHPRisingLimit()
    {
        return minHPRisingLimit;
    }
    public int GetMinMPRisingLimit()
    {
        return minMPRisingLimit;
    }
    public int GetMinAgilityRisingLimit()
    {
        return minAgilityRisingLimit;
    }
    public int GetMinPowerRisingLimit()
    {
        return minPowerRisingLimit;
    }
    public int GetMinStrikingStrengthRisingLimit()
    {
        return minStrikingStrengthRisingLimit;
    }
    public int GetMinMagicPowerRisingLimit()
    {
        return minMagicPowerRisingLimit;
    }


    public int GetMaxHPRisingLimit()
    {
        return maxHPRisingLimit;
    }
    public int GetMaxMPRisingLimit()
    {
        return maxMPRisingLimit;
    }
    public int GetMaxAgilityRisingLimit()
    {
        return maxAgilityRisingLimit;
    }
    public int GetMaxPowerRisingLimit()
    {
        return maxPowerRisingLimit;
    }
    public int GetMaxStrikingStrengthRisingLimit()
    {
        return maxStrikingStrengthRisingLimit;
    }
    public int GetMaxMagicPowerRisingLimit()
    {
        return maxMagicPowerRisingLimit;
    }
}