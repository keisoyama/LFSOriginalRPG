using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdleCommandScript : MonoBehaviour
{
    //　パーティーステータス
    [SerializeField]
    private PartyStatus partyStatus = null;
    //　どれだけ操作をしていなければ表示するか
    [SerializeField]
    private float idleTime = 5f;
    //　どれだけ操作をしていないか
    private float elapsedTime;
    //　現在キャラクターステータスパネルを開いているかどうか
    private bool isOpenCharacterStatusPanel;
    //　キャラクター毎のステータスパネルプレハブ
    [SerializeField]
    private GameObject characterPanelPrefab = null;
    //　キャラクターステータスパネル
    private GameObject characterStatusPanel;
    //　SceneManager
    private LoadSceneManager sceneManager = null;

    private void Awake()
    {
        characterStatusPanel = transform.Find("CharacterStatusPanel").gameObject;
    }
    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //　何らかのキーが押された時
        if (Input.anyKeyDown
        || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0f)
        || !Mathf.Approximately(Input.GetAxis("Vertical"), 0f)
        )
        {

            elapsedTime = 0f;
            //　キャラクターステータスパネルの子要素があれば削除する
            for (int i = characterStatusPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(characterStatusPanel.transform.GetChild(i).gameObject);
            }
            characterStatusPanel.SetActive(false);
            isOpenCharacterStatusPanel = false;
            //　キャラクターステータスパネルを開いていない時だけ
        }
        else if (!isOpenCharacterStatusPanel)
        {
            elapsedTime += Time.deltaTime;
            //　経過時間が一定時間を越えたらキャラクターステータスパネルを表示
            if (elapsedTime >= idleTime)
            {
                GameObject characterPanel;
                //　パーティーメンバー分のステータスを作成
                foreach (var member in partyStatus.GetAllyStatus())
                {
                    characterPanel = Instantiate<GameObject>(characterPanelPrefab, characterStatusPanel.transform);
                    characterPanel.transform.Find("CharacterName").GetComponent<Text>().text = member.GetCharacterName();
                    characterPanel.transform.Find("HPSlider").GetComponent<Slider>().value = (float)member.GetHp() / member.GetMaxHp();
                    characterPanel.transform.Find("HPSlider/HPText").GetComponent<Text>().text = member.GetHp().ToString();
                    characterPanel.transform.Find("MPSlider").GetComponent<Slider>().value = (float)member.GetMp() / member.GetMaxMp();
                    characterPanel.transform.Find("MPSlider/MPText").GetComponent<Text>().text = member.GetMp().ToString();
                }
                characterStatusPanel.SetActive(true);
                elapsedTime = 0f;
                isOpenCharacterStatusPanel = true;
            }
        }
    }
}