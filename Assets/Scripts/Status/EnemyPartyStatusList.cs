using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatusList", menuName = "CreateEnemyPartyStatusList")]
public class EnemyPartyStatusList : ScriptableObject
{
    [SerializeField]
    private List<EnemyPartyStatus> partyMembersList = null;

    public List<EnemyPartyStatus> GetPartyMembersList()
    {
        return partyMembersList;
    }
}