using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountManager : MonoBehaviour
{
    //　LoadSceneManager
    private LoadSceneManager sceneManager;
    [SerializeField]
    private float encountMinTime = 3f;
    //　敵と遭遇するランダム時間
    [SerializeField]
    private float encountMaxTime = 30f;
    //　経過時間
    [SerializeField]
    private float elapsedTime;
    //　目的の時間
    [SerializeField]
    private float destinationTime;
    //　ユニティちゃん
    private Transform YusyaObjct;
    //　ユニティちゃんスクリプト

    //　戦闘データ
    [SerializeField]
    private BattleData battleData = null;
    //　敵パーティーリスト
    [SerializeField]
    private EnemyPartyStatusList enemyPartyStatusList = null;


    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
        SetDestinationTime();
        YusyaObjct = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //　移動していない時は計測しない
        if (Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
            && Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
            )
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= destinationTime)
        {
            //　ワールドマップ上の勇者の位置に応じて遭遇する敵を決定したい
            if (Random.Range(0,1f)>0.25f)
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup1"));
            }
            else if (Random.Range(0, 1f) > 0.25f)
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup2"));
            }
            else if (Random.Range(0, 1f) > 0.25f)
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup3"));
            }
            else 
            {
                battleData.SetEnemyPartyStatus(enemyPartyStatusList.GetPartyMembersList().Find(enemyPartyStatus => enemyPartyStatus.GetPartyName() == "EnemyGroup4"));
            }
            sceneManager.GoToNextScene(SceneMovementData.SceneType.WorldMapToBattle);
            elapsedTime = 0f;
            SetDestinationTime();
        }
    }

    //　次に敵と遭遇する時間
    public void SetDestinationTime()
    {
        destinationTime = Random.Range(encountMinTime, encountMaxTime);
    }
}