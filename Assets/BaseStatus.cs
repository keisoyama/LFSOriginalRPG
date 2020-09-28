using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus : MonoBehaviour
{
    //基礎ステータス
    protected int hitPoint;
    protected int magicPoint;
    protected int physicalAttackNumber;
    protected int physicalDefenceNumber;
    protected int magicalAttackNumber;
    protected int magicalDefenceNumber;
    protected int speedNumber;

    //各耐性
    protected int phsicalResistance;
    protected int magicalResistance;
    protected bool isVurnerableToFire;
    protected bool isVurnerableToWater;
    protected bool isVurnerableToWood;
    protected bool isVurnerableToSun;
    protected bool isVurnerableToMoon;
}
