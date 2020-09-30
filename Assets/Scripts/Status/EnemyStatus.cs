using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyStatus", menuName = "CreateEnemyStatus")]
public class EnemyStatus : BaseStatus
{

    //　倒した時に得られる経験値
    [SerializeField]
    private int gettingExperience = 10;
    //　倒した時に得られるお金
    [SerializeField]
    private int gettingMoney = 10;
    //　落とすアイテムと落とす確率（パーセンテージ表示）
    [SerializeField]
    private ItemDictionary dropItemDictionary = null;

    public int GetGettingExperience()
    {
        return gettingExperience;
    }

    public int GetGettingMoney()
    {
        return gettingMoney;
    }

    //　落とすアイテムのItemDictionaryを返す
    public ItemDictionary GetDropItemDictionary()
    {
        return dropItemDictionary;
    }

    //　アイテムを落とす確率を返す
    public int GetProbabilityOfDroppingItem(Item item)
    {
        return dropItemDictionary[item];
    }
}