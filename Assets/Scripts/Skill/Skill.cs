using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skill : ScriptableObject
{
    public enum Type
    {
        DirectAttack,
        GetAway,
        Item,
        MagicAttack,
        RecoveryMagic,
        IncreaseAttackPowerMagic,
        IncreaseDefencePowerMagic
    }

    [SerializeField]
    private Type skillType = Type.DirectAttack;
    [SerializeField]
    private string kanjiName = "";
    [SerializeField]
    private string hiraganaName = "";
    [SerializeField]
    private string information = "";
    //　使用者のエフェクト
    [SerializeField]
    private GameObject skillUserEffect = null;
    //　魔法を受ける側のエフェクト
    [SerializeField]
    private GameObject skillReceivingSideEffect = null;

    //　スキルの種類を返す
    public Type GetSkillType()
    {
        return skillType;
    }
    //　スキルの名前を返す
    public string GetKanjiName()
    {
        return kanjiName;
    }
    //　スキルの平仮名の名前を返す
    public string GetHiraganaName()
    {
        return hiraganaName;
    }
    //　スキル情報を返す
    public string GetInformation()
    {
        return information;
    }
    //　使用者のエフェクトを返す
    public GameObject GetSkillUserEffect()
    {
        return skillUserEffect;
    }
    //　魔法を受ける側のエフェクトを返す
    public GameObject GetSkillReceivingSideEffect()
    {
        return skillReceivingSideEffect;
    }
}