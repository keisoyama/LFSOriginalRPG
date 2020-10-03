using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SceneMovementData", menuName = "CreateSceneMovementData")]
public class SceneMovementData : ScriptableObject
{
    public enum SceneType
    {
        StartGame,
        FirstVillage,
        FirstVillageToWorldMap,
        WorldMapToBattle,
        BattleToWorldMap,
    }

    //　ワールドマップ→戦闘シーンへ移行した時のワールドマップの位置情報
    private Vector3 worldMapPos;
    //　ワールドマップ→戦闘シーンへ移行した時のワールドマップの位置情報
    private Quaternion worldMapRot;

    [SerializeField]
    private SceneType sceneType;

    public void OnEnable()
    {
        sceneType = SceneType.StartGame;
    }

    public void SetSceneType(SceneType scene)
    {
        sceneType = scene;
    }

    public SceneType GetSceneType()
    {
        return sceneType;
    }

    public void SetWorldMapPos(Vector3 pos)
    {
        worldMapPos = pos;
    }

    public Vector3 GetWorldMapPos()
    {
        return worldMapPos;
    }

    public void SetWorldMapRot(Quaternion rot)
    {
        worldMapRot = rot;
    }

    public Quaternion GetWorldMapRot()
    {
        return worldMapRot;
    }
}