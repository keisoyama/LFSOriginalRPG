using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BattlePartyStatus", menuName = "CreateBattlePartyStatus")]
public class BattlePartyStatus : ScriptableObject
{
    [SerializeField]
    private List<GameObject> partyMembers = null;

    public List<GameObject> GetAllyGameObject()
    {
        return partyMembers;
    }
}