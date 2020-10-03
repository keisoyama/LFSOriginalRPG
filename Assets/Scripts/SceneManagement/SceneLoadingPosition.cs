using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingPosition : MonoBehaviour
{
    [SerializeField]
    private SceneMovementData sceneMovementData = null;

    void Start()
    {

        // シーン遷移の種類に応じて初期位置のゲームオブジェクトの位置と角度に設定
        if (sceneMovementData.GetSceneType() == SceneMovementData.SceneType.StartGame)
        {
            var initialPosition = GameObject.Find("InitialPosition");
            if (initialPosition != null)
            {
                transform.position = initialPosition.transform.position;
                transform.rotation = initialPosition.transform.rotation;
            }
        }
        else if (sceneMovementData.GetSceneType() == SceneMovementData.SceneType.FirstVillage)
        {
            var initialPosition = GameObject.Find("InitialPosition");
            if (initialPosition != null)
            {
                transform.position = initialPosition.transform.position;
                transform.rotation = initialPosition.transform.rotation;
            }
        }
        else if (sceneMovementData.GetSceneType() == SceneMovementData.SceneType.FirstVillageToWorldMap)
        {
            var initialPosition = GameObject.Find("InitialPositionFirstVillageToWorldMap");
            if (initialPosition != null)
            {
                transform.position = initialPosition.transform.position;
                transform.rotation = initialPosition.transform.rotation;
            }
        }
        else if (sceneMovementData.GetSceneType() == SceneMovementData.SceneType.BattleToWorldMap)
        {
            transform.position = sceneMovementData.GetWorldMapPos();
            transform.rotation = sceneMovementData.GetWorldMapRot();
        }
    }
}