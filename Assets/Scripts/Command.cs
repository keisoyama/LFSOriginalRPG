
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Command : MonoBehaviour
{

    public enum CommandMode
    {
        CommandPanel,
        StatusPanelSelectCharacter,
        StatusPanel,
    }

    private CommandMode currentCommand;
    //　ユニティちゃんコマンドスクリプト
    private BaseCommand baseCommand;
    //　最初に選択するButtonのTransform
    private GameObject firstSelectButton;

    //　コマンドパネル
    private GameObject commandPanel;
    //　ステータス表示パネル
    private GameObject statusPanel;
    //　キャラクター選択パネル
    private GameObject selectCharacterPanel;

    //　コマンドパネルのCanvasGroup
    private CanvasGroup commandPanelCanvasGroup;
    //　キャラクター選択パネルのCanvasGroup
    private CanvasGroup selectCharacterPanelCanvasGroup;

    //　キャラクター名
    private Text characterNameText;
    //　ステータスタイトルテキスト
    private Text statusTitleText;
    //　ステータスパラメータテキスト1
    private Text statusParam1Text;
    //　ステータスパラメータテキスト2
    private Text statusParam2Text;
    //　パーティーステータス
    [SerializeField]
    private PartyStatus partyStatus = null;

    //　キャラクター選択のボタンのプレハブ
    [SerializeField]
    private GameObject characterPanelButtonPrefab = null;

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    void Awake()
    {
        //　コマンド画面を開く処理をしているUnityChanCommandScriptを取得
        baseCommand = GameObject.FindWithTag("Player").GetComponent<BaseCommand>();
        //　現在のコマンドを初期化
        currentCommand = CommandMode.CommandPanel;
        //　階層を辿ってを取得
        firstSelectButton = transform.Find("CommandPanel/StatusButton").gameObject;
        //　パネル系
        commandPanel = transform.Find("CommandPanel").gameObject;
        statusPanel = transform.Find("StatusPanel").gameObject;
        selectCharacterPanel = transform.Find("SelectCharacterPanel").gameObject;
        //　CanvasGroup
        commandPanelCanvasGroup = commandPanel.GetComponent<CanvasGroup>();
        selectCharacterPanelCanvasGroup = selectCharacterPanel.GetComponent<CanvasGroup>();
        //　ステータス用テキスト
        characterNameText = statusPanel.transform.Find("CharacterNamePanel/Text").GetComponent<Text>();
        statusTitleText = statusPanel.transform.Find("StatusParamPanel/Title").GetComponent<Text>();
        statusParam1Text = statusPanel.transform.Find("StatusParamPanel/Param1").GetComponent<Text>();
        statusParam2Text = statusPanel.transform.Find("StatusParamPanel/Param2").GetComponent<Text>();
    }


    private void OnEnable()
    {
        //　現在のコマンドの初期化
        currentCommand = CommandMode.CommandPanel;
        //　コマンドメニュー表示時に他のパネルは非表示にする
        statusPanel.SetActive(false);
        selectCharacterPanel.SetActive(false);

        // キャラクター選択ボタンがあれば全て削除
        for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
        }

        selectedGameObjectStack.Clear();

        commandPanelCanvasGroup.interactable = true;
        selectCharacterPanelCanvasGroup.interactable = false;
        EventSystem.current.SetSelectedGameObject(firstSelectButton);
    }
    private void Update()
    {

        //　キャンセルボタンを押した時の処理
        if (Input.GetKeyDown(KeyCode.S))
        {
            //　コマンド選択画面時
            if (currentCommand == CommandMode.CommandPanel)
            {
                baseCommand.ExitCommand();
                gameObject.SetActive(false);
                //　ステータスキャラクター選択またはステータス表示時
            }
            else if (currentCommand == CommandMode.StatusPanelSelectCharacter
              || currentCommand == CommandMode.StatusPanel
              )
            {
                selectCharacterPanelCanvasGroup.interactable = false;
                selectCharacterPanel.SetActive(false);
                statusPanel.SetActive(false);
                //　キャラクター選択パネルの子要素のボタンを削除
                for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                }
                //　前のパネルで選択していたゲームオブジェクトを選択
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.CommandPanel;
            }
        }
    }

    //　選択したコマンドで処理分け
    public void SelectCommand(string command)
    {
        if (command == "Status")
        {
            currentCommand = CommandMode.StatusPanelSelectCharacter;
            //　UIのオン・オフや選択アイコンの設定
            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            GameObject characterButtonIns;

            //　パーティーメンバー分のボタンを作成
            foreach (var member in partyStatus.GetAllyStatus())
            {
                characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, selectCharacterPanel.transform);
                characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
                characterButtonIns.GetComponent<Button>().onClick.AddListener(() => ShowStatus(member));
            }
        }
        //　階層を一番最後に並べ替え
        selectCharacterPanel.transform.SetAsLastSibling();
        selectCharacterPanel.SetActive(true);
        selectCharacterPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);

    }


    //　キャラクターのステータス表示
    public void ShowStatus(AllyStatus allyStatus)
    {
        currentCommand = CommandMode.StatusPanel;
        statusPanel.SetActive(true);
        //　キャラクターの名前を表示
        characterNameText.text = allyStatus.GetCharacterName();

        //　タイトルの表示
        var text = "レベル\n";
        text += "HP\n";
        text += "MP\n";
        text += "経験値\n";
        text += "状態異常\n";
        text += "力\n";
        text += "素早さ\n";
        text += "打たれ強さ\n";
        text += "魔法力\n";
        text += "装備武器\n";
        text += "装備鎧\n";
        text += "攻撃力\n";
        text += "防御力\n";
        statusTitleText.text = text;

        //　HPとMPのDivision記号の表示
        text = "\n";
        text += allyStatus.GetHp() + "\n";
        text += allyStatus.GetMp() + "\n";
        statusParam1Text.text = text;

        //　ステータスパラメータの表示
        text = allyStatus.GetLevel() + "\n";
        text += allyStatus.GetMaxHp() + "\n";
        text += allyStatus.GetMaxMp() + "\n";
        text += allyStatus.GetEarnedExperience() + "\n";
        if (!allyStatus.IsPoisonState() && !allyStatus.IsParalyzeState() && !allyStatus.IsSilentState())
        {
            text += "正常";
        }
        else
        {
            if (allyStatus.IsPoisonState())
            {
                text += "毒";
                if (allyStatus.IsParalyzeState())
                {
                    text += "、痺れ";
                    if (allyStatus.IsSilentState())
                    {
                        text += "沈黙";
                    }
                }
            }
            else if (allyStatus.IsParalyzeState())
            {
                text += "痺れ";
                if (allyStatus.IsSilentState())
                {
                    text += "沈黙";
                }
            }
            else if (allyStatus.IsSilentState())
            {
                text += "沈黙";
            }
        }

        text += "\n";
        text += allyStatus.GetPower() + "\n";
        text += allyStatus.GetAgility() + "\n";
        text += allyStatus.GetStrikingStrength() + "\n";
        text += allyStatus.GetMagicPower() + "\n";
        text += allyStatus?.GetEquipWeapon()?.GetKanjiName() ?? "";
        text += "\n";
        text += allyStatus.GetEquipArmor()?.GetKanjiName() ?? "";
        text += "\n";
        text += allyStatus.GetPower() + (allyStatus.GetEquipWeapon()?.GetAmount() ?? 0) + "\n";
        text += allyStatus.GetStrikingStrength() + (allyStatus.GetEquipArmor()?.GetAmount() ?? 0) + "\n";
        statusParam2Text.text = text;
    }
}