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
        WorldMapToBattle
    }
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
}