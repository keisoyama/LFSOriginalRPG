using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EnemyPartyStatus", menuName = "CreateEnemyPartyStatus")]
public class EnemyPartyStatus : ScriptableObject
{
    [SerializeField]
    private string partyName = null;
    [SerializeField]
    private List<GameObject> partyMembers = null;

    public string GetPartyName()
    {
        return partyName;
    }

    public List<GameObject> GetEnemyGameObjectList()
    {
        return partyMembers;
    }
}

