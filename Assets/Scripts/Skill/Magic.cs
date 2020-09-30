using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Magic", menuName = "CreateMagic")]
public class Magic : Skill
{
    public enum MagicAttribute
    {
        Fire,
        Water,
        Wood,
        Sun,
        Moon,
        Other
        
    }

    //　魔法力
    [SerializeField]
    private int magicPower = 0;
    //　使うMP
    [SerializeField]
    private int amountToUseMagicPoints = 0;
    //　魔法の属性
    [SerializeField]
    private MagicAttribute magicAttribute = MagicAttribute.Other;

    //　魔法力を返す
    public int GetMagicPower()
    {
        return magicPower;
    }
    //　魔法の属性を返す
    public MagicAttribute GetMagicAttribute()
    {
        return magicAttribute;
    }

    public void SetAmountToUseMagicPoints(int point)
    {
        amountToUseMagicPoints = Mathf.Max(0, Mathf.Min(amountToUseMagicPoints, point));
    }

    public int GetAmountToUseMagicPoints()
    {
        return amountToUseMagicPoints;
    }
}