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
    private Transform unityChanObjct;
    //　ユニティちゃんスクリプト

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
        SetDestinationTime();
        unityChanObjct = GameObject.FindWithTag("Player").transform;
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
            Debug.Log("遭遇");
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