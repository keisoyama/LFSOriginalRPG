
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
        ItemPanelSelectCharacter,
        ItemPanel,
        UseItemPanel,
        UseItemSelectCharacterPanel,
        UseItemPanelToItemPanel,
        UseItemPanelToUseItemPanel,
        UseItemSelectCharacterPanelToUseItemPanel,
        NoItemPassed
    }

    private CommandMode currentCommand;
    //　ユニティちゃんコマンドスクリプト
    private YusyaCommand yusyaCommand;
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

    //　アイテム表示パネル
    private GameObject itemPanel;
    //　アイテムパネルボタンを表示する場所
    private GameObject content;
    //　アイテムを使う選択パネル
    private GameObject useItemPanel;
    //　アイテムを使う相手を誰にするか選択するパネル
    private GameObject useItemSelectCharacterPanel;
    //　情報表示パネル
    private GameObject itemInformationPanel;
    //　アイテム使用後の情報表示パネル
    private GameObject useItemInformationPanel;

    //　アイテムパネルのCanvas Group
    private CanvasGroup itemPanelCanvasGroup;
    //　アイテムを使う選択パネルのCanvasGroup
    private CanvasGroup useItemPanelCanvasGroup;
    //　アイテムを使うキャラクター選択パネルのCanvasGroup;
    private CanvasGroup useItemSelectCharacterPanelCanvasGroup;

    //　情報表示タイトルテキスト
    private Text informationTitleText;
    //　情報表示テキスト
    private Text informationText;

    //　キャラクターアイテムのボタンのプレハブ
    [SerializeField]
    private GameObject itemPanelButtonPrefab = null;
    //　アイテム使用時のボタンのプレハブ
    [SerializeField]
    private GameObject useItemPanelButtonPrefab = null;

    //　アイテムボタン一覧
    private List<GameObject> itemPanelButtonList = new List<GameObject>();

    private bool isCancelButtonPushed;

    [SerializeField]
    private AllyStatus yusyaStatus = null;

    void Awake()
    {
        //　コマンド画面を開く処理をしているUnityChanCommandScriptを取得
        yusyaCommand = GameObject.FindWithTag("Player").GetComponent<YusyaCommand>();
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

        itemPanel = transform.Find("ItemPanel").gameObject;
        content = itemPanel.transform.Find("Mask/Content").gameObject;
        useItemPanel = transform.Find("UseItemPanel").gameObject;
        useItemSelectCharacterPanel = transform.Find("UseItemSelectCharacterPanel").gameObject;
        itemInformationPanel = transform.Find("ItemInformationPanel").gameObject;
        useItemInformationPanel = transform.Find("UseItemInformationPanel").gameObject;

        itemPanelCanvasGroup = itemPanel.GetComponent<CanvasGroup>();
        useItemPanelCanvasGroup = useItemPanel.GetComponent<CanvasGroup>();
        useItemSelectCharacterPanelCanvasGroup = useItemSelectCharacterPanel.GetComponent<CanvasGroup>();

        //　情報表示用テキスト
        informationTitleText = itemInformationPanel.transform.Find("Title").GetComponent<Text>();
        informationText = itemInformationPanel.transform.Find("Information").GetComponent<Text>();
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

        itemPanel.SetActive(false);
        useItemPanel.SetActive(false);
        useItemSelectCharacterPanel.SetActive(false);
        itemInformationPanel.SetActive(false);
        useItemInformationPanel.SetActive(false);

        //　アイテムパネルボタンがあれば全て削除
        //for (int i = content.transform.childCount - 1; i >= 0; i--)
        //{
        //    Destroy(content.transform.GetChild(i).gameObject);
        //}

        //　アイテムを使うキャラクター選択ボタンがあれば全て削除
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }
        //　アイテムを使う相手のキャラクター選択ボタンがあれば全て削除
        for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
        }

        itemPanelButtonList.Clear();

        itemPanelCanvasGroup.interactable = false;
        useItemPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanelCanvasGroup.interactable = false;
    }
    private void Update()
    {

        //　キャンセルボタンを押した時の処理
        if (isCancelButtonPushed)
        {
            isCancelButtonPushed = false;

            //　コマンド選択画面時
            if (currentCommand == CommandMode.CommandPanel)
            {
                //baseCommand.ExitCommand();
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
                //　どのキャラクターのアイテムを表示するかの選択時
            }
            else if (currentCommand == CommandMode.ItemPanelSelectCharacter)
            {
                selectCharacterPanelCanvasGroup.interactable = false;
                selectCharacterPanel.SetActive(false);
                itemInformationPanel.SetActive(false);

                for (int i = selectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(selectCharacterPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.CommandPanel;
                //　アイテム一覧表示時
            }
            else if (currentCommand == CommandMode.ItemPanel)
            {
                itemPanelCanvasGroup.interactable = false;
                itemPanel.SetActive(false);
                itemInformationPanel.SetActive(false);
                //　リストをクリア
                itemPanelButtonList.Clear();
                ////　ItemPanelでCancelを押したらcontent以下のアイテムパネルボタンを全削除
                //for (int i = content.transform.childCount - 1; i >= 0; i--)
                //{
                //    Destroy(content.transform.GetChild(i).gameObject);
                //}

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                selectCharacterPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.ItemPanelSelectCharacter;
                //　アイテムを選択し、どう使うかを選択している時
            }
            else if (currentCommand == CommandMode.UseItemPanel)
            {
                useItemPanelCanvasGroup.interactable = false;
                useItemPanel.SetActive(false);
                //　UseItemPanelでCancelボタンを押したらUseItemPanelの子要素のボタンの全削除
                for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(useItemPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                itemPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.ItemPanel;
                //　アイテムを使用する相手のキャラクターを選択している時
            }
            else if (currentCommand == CommandMode.UseItemSelectCharacterPanel)
            {
                useItemSelectCharacterPanelCanvasGroup.interactable = false;
                useItemSelectCharacterPanel.SetActive(false);
                //　UseItemSelectCharacterPanelでCancelボタンを押したらアイテムを使用するキャラクターボタンの全削除
                for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
                }

                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                useItemPanelCanvasGroup.interactable = true;
                currentCommand = CommandMode.UseItemPanel;
            }

            //　アイテムを装備、装備を外す情報表示後の処理
            if (currentCommand == CommandMode.UseItemPanelToItemPanel)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentCommand = CommandMode.ItemPanel;
                    useItemInformationPanel.SetActive(false);
                    itemPanel.transform.SetAsLastSibling();
                    itemPanelCanvasGroup.interactable = true;

                    EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());

                }
                //　アイテムを使用する相手のキャラクター選択からアイテムをどうするかに移行する時
            }
            else if (currentCommand == CommandMode.UseItemSelectCharacterPanelToUseItemPanel)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentCommand = CommandMode.UseItemPanel;
                    useItemInformationPanel.SetActive(false);
                    useItemPanel.transform.SetAsLastSibling();
                    useItemPanelCanvasGroup.interactable = true;

                    EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                }
                //　アイテムを捨てるを選択した後の状態
            }
            else if (currentCommand == CommandMode.UseItemPanelToUseItemPanel)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentCommand = CommandMode.UseItemPanel;
                    useItemInformationPanel.SetActive(false);
                    useItemPanel.transform.SetAsLastSibling();
                    useItemPanelCanvasGroup.interactable = true;
                }
                //　アイテムを使用、渡す、捨てるを選択した後にそのアイテムの数が0になった時
            }
            else if (currentCommand == CommandMode.NoItemPassed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentCommand = CommandMode.ItemPanel;
                    useItemInformationPanel.SetActive(false);
                    useItemPanel.SetActive(false);
                    itemPanel.transform.SetAsLastSibling();
                    itemPanelCanvasGroup.interactable = true;

                    //　アイテムパネルボタンがあれば最初のアイテムパネルボタンを選択
                    if (content.transform.childCount != 0)
                    {
                        EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
                    }
                    else
                    {
                        //　アイテムパネルボタンがなければ（アイテムを持っていない）ItemSelectPanelに戻る
                        currentCommand = CommandMode.ItemPanelSelectCharacter;
                        itemPanelCanvasGroup.interactable = false;
                        itemPanel.SetActive(false);
                        selectCharacterPanelCanvasGroup.interactable = true;
                        selectCharacterPanel.SetActive(true);
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                    }
                }
            }
        }

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
if (EventSystem.current.currentSelectedGameObject == null) {
    if (currentCommand == CommandMode.CommandPanel) {
        EventSystem.current.SetSelectedGameObject(commandPanel.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.ItemPanel) {
        EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.ItemPanelSelectCharacter) {
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.StatusPanel) {
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.StatusPanelSelectCharacter) {
        EventSystem.current.SetSelectedGameObject(selectCharacterPanel.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.UseItemPanel) {
        EventSystem.current.SetSelectedGameObject(useItemPanel.transform.GetChild(0).gameObject);
    } else if (currentCommand == CommandMode.UseItemSelectCharacterPanel) {
        EventSystem.current.SetSelectedGameObject(useItemSelectCharacterPanel.transform.GetChild(0).gameObject);
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
        else if (command == "Item")
        {
            currentCommand = CommandMode.ItemPanelSelectCharacter;
            statusPanel.SetActive(false);
            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            GameObject characterButtonIns;

            
                characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, selectCharacterPanel.transform);
                characterButtonIns.GetComponentInChildren<Text>().text = "袋";
                characterButtonIns.GetComponent<Button>().onClick.AddListener(() => CreateItemPanelButton(yusyaStatus));
            
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

    //　キャラクターが持っているアイテムのボタン表示
    public void CreateItemPanelButton(AllyStatus allyStatus)
    {

        itemInformationPanel.SetActive(true);
        selectCharacterPanelCanvasGroup.interactable = false;

        //　アイテムパネルボタンを何個作成したかどうか
        int itemPanelButtonNum = 0;
        GameObject itemButtonIns;
        //　選択したキャラクターのアイテム数分アイテムパネルボタンを作成
        //　持っているアイテム分のボタンの作成とクリック時の実行メソッドの設定
        foreach (var item in allyStatus.GetItemDictionary().Keys)
        {
            itemButtonIns = content.transform.GetChild(itemPanelButtonNum).gameObject;
            itemButtonIns.SetActive(true);
            itemButtonIns.transform.Find("ItemName").GetComponent<Text>().text = item.GetKanjiName();
            itemButtonIns.GetComponent<Button>().onClick.RemoveAllListeners();
            itemButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItem(allyStatus, item));
            itemButtonIns.GetComponent<ItemPanelButton>().SetParam(item);

            //　アイテム数を表示
            itemButtonIns.transform.Find("Num").GetComponent<Text>().text = allyStatus.GetItemNum(item).ToString();

            //　装備している武器や防具には名前の前にEを表示し、そのTextを保持して置く
            if (allyStatus.GetEquipWeapon() == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
            }
            else if (allyStatus.GetEquipArmor() == item)
            {
                itemButtonIns.transform.Find("Equip").GetComponent<Text>().text = "E";
            }

            //　アイテムパネルボタン番号を更新
            itemPanelButtonNum++;
        }

        for (int i = itemPanelButtonNum; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(false);
        }

        //　アイテムパネルの表示と最初のアイテムの選択
        if (itemPanelButtonNum != 0)
        {
            //　SelectCharacerPanelで最後にどのゲームオブジェクトを選択していたか
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);
            currentCommand = CommandMode.ItemPanel;
            itemPanel.SetActive(true);
            itemPanel.transform.SetAsLastSibling();
            itemPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(content.transform.GetChild(0).gameObject);
        }
        else
        {
            informationTitleText.text = "";
            informationText.text = "アイテムを持っていません。";
            selectCharacterPanelCanvasGroup.interactable = true;
        }
    }

    //　アイテムをどうするかの選択
    public void SelectItem(AllyStatus allyStatus, Item item)
    {
        //　アイテムの種類に応じて出来る項目を変更する
        if (item.GetItemType() == Item.Type.Armor
            || item.GetItemType() == Item.Type.Weapon)
        {

            var itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            if (item == allyStatus.GetEquipWeapon() || item == allyStatus.GetEquipArmor())
            {
                itemMenuButtonIns.GetComponentInChildren<Text>().text = "装備を外す";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => RemoveEquip(allyStatus, item));
            }
            else
            {
                itemMenuButtonIns.GetComponentInChildren<Text>().text = "装備する";
                itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => Equip(allyStatus, item));
            }

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "捨てる";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(allyStatus, item));

        }
        else if (item.GetItemType() == Item.Type.ParalyzeRecovery
          || item.GetItemType() == Item.Type.PoisonRecovery
          || item.GetItemType() == Item.Type.SilentRecovery
          || item.GetItemType() == Item.Type.HPRecovery
          || item.GetItemType() == Item.Type.MPRecovery
          )
        {

            var itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "使う";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(allyStatus, item));

            itemMenuButtonIns = Instantiate<GameObject>(useItemPanelButtonPrefab, useItemPanel.transform);
            itemMenuButtonIns.GetComponentInChildren<Text>().text = "捨てる";
            itemMenuButtonIns.GetComponent<Button>().onClick.AddListener(() => ThrowAwayItem(allyStatus, item));

        }
        else if (item.GetItemType() == Item.Type.Valuables)
        {
            informationTitleText.text = item.GetKanjiName();
            informationText.text = item.GetInformation();
        }

        if (item.GetItemType() != Item.Type.Valuables)
        {
            useItemPanel.SetActive(true);
            itemPanelCanvasGroup.interactable = false;
            currentCommand = CommandMode.UseItemPanel;
            //　ItemPanelで最後にどれを選択していたか？
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            useItemPanel.transform.SetAsLastSibling();
            EventSystem.current.SetSelectedGameObject(useItemPanel.transform.GetChild(0).gameObject);
            useItemPanelCanvasGroup.interactable = true;
            Input.ResetInputAxes();
        }
    }

    //　アイテムを使用するキャラクターを選択する
    public void UseItem(AllyStatus allyStatus, Item item)
    {
        useItemPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(true);
        //　UseItemPanelでどれを最後に選択していたか
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject characterButtonIns;
        //　パーティメンバー分のボタンを作成
        foreach (var member in partyStatus.GetAllyStatus())
        {
            characterButtonIns = Instantiate<GameObject>(characterPanelButtonPrefab, useItemSelectCharacterPanel.transform);
            characterButtonIns.GetComponentInChildren<Text>().text = member.GetCharacterName();
            characterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItemToCharacter(allyStatus, member, item));
        }
        //　UseItemSelectCharacterPanelに移行する
        currentCommand = CommandMode.UseItemSelectCharacterPanel;
        useItemSelectCharacterPanel.transform.SetAsLastSibling();
        EventSystem.current.SetSelectedGameObject(useItemSelectCharacterPanel.transform.GetChild(0).gameObject);
        useItemSelectCharacterPanelCanvasGroup.interactable = true;
        Input.ResetInputAxes();
    }

    public void UseItemToCharacter(AllyStatus fromChara, AllyStatus toChara, Item item)
    {
        useItemInformationPanel.SetActive(true);
        useItemSelectCharacterPanelCanvasGroup.interactable = false;
        useItemSelectCharacterPanel.SetActive(false);

        if (item.GetItemType() == Item.Type.HPRecovery)
        {
            if (toChara.GetHp() == toChara.GetMaxHp())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は元気です。";
            }
            else
            {
                toChara.SetHp(toChara.GetHp() + item.GetAmount());
                //　アイテムを使用した旨を表示
                useItemInformationPanel.GetComponentInChildren<Text>().text = fromChara.GetCharacterName() + "は" + item.GetKanjiName() + "を" + toChara.GetCharacterName() + "に使用しました。\n" +
                    toChara.GetCharacterName() + "は" + item.GetAmount() + "回復しました。";
                //　持っているアイテム数を減らす
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.MPRecovery)
        {
            if (toChara.GetMp() == toChara.GetMaxMp())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "のMPは最大です。";
            }
            else
            {
                toChara.SetMp(toChara.GetMp() + item.GetAmount());
                //　アイテムを使用した旨を表示
                useItemInformationPanel.GetComponentInChildren<Text>().text = fromChara.GetCharacterName() + "は" + item.GetKanjiName() + "を" + toChara.GetCharacterName() + "に使用しました。\n" +
                    toChara.GetCharacterName() + "はMPを" + item.GetAmount() + "回復しました。";
                //　持っているアイテム数を減らす
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.PoisonRecovery)
        {
            if (!toChara.IsPoisonState())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は毒状態ではありません。";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は毒から回復しました。";
                toChara.SetPoisonState(false);
                //　持っているアイテム数を減らす
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.ParalyzeRecovery)
        {
            if (!toChara.IsParalyzeState())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は痺れ状態ではありません。";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は痺れから回復しました。";
                toChara.SetParalyze(false);
                //　持っているアイテム数を減らす
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }
        else if (item.GetItemType() == Item.Type.SilentRecovery)
        {
            if (!toChara.IsParalyzeState())
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は沈黙状態ではありません。";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = toChara.GetCharacterName() + "は沈黙から回復しました。";
                toChara.SetSilence(false);
                //　持っているアイテム数を減らす
                fromChara.SetItemNum(item, fromChara.GetItemNum(item) - 1);
            }
        }

        //　アイテムを使用したらアイテムを使用する相手のUseItemSelectCharacterPanelの子要素のボタンを全削除
        for (int i = useItemSelectCharacterPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemSelectCharacterPanel.transform.GetChild(i).gameObject);
        }
        //　itemPanleButtonListから該当するアイテムを探し数を更新する
        var itemButton = itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
        itemButton.transform.Find("Num").GetComponent<Text>().text = fromChara.GetItemNum(item).ToString();

        //　アイテム数が0だったらボタンとキャラクターステータスからアイテムを削除
        if (fromChara.GetItemNum(item) == 0)
        {
            //　アイテムが0になったら一気にItemPanelに戻す為、UseItemPanel内とUseItemSelectCharacterPanel内でのオブジェクト登録を削除
            selectedGameObjectStack.Pop();
            selectedGameObjectStack.Pop();
            //　itemPanelButtonListからアイテムパネルボタンを削除
            itemPanelButtonList.Remove(itemButton);
            //　アイテムパネルボタン自身の削除
            Destroy(itemButton);
            //　アイテムを渡したキャラクター自身のItemDictionaryからそのアイテムを削除
            fromChara.GetItemDictionary().Remove(item);
            //　ItemPanelに戻る為、UseItemPanel内に作ったボタンを全削除
            for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(useItemPanel.transform.GetChild(i).gameObject);
            }
            //　アイテム数が0になったのでCommandMode.NoItemPassedに変更
            currentCommand = CommandMode.NoItemPassed;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
        else
        {
            //　アイテム数が残っている場合はUseItemPanelでアイテムをどうするかの選択に戻る
            currentCommand = CommandMode.UseItemSelectCharacterPanelToUseItemPanel;
            useItemInformationPanel.transform.SetAsLastSibling();
            Input.ResetInputAxes();
        }
    }

    //　捨てる
    public void ThrowAwayItem(AllyStatus allyStatus, Item item)
    {
        //　アイテム数を減らす
        allyStatus.SetItemNum(item, allyStatus.GetItemNum(item) - 1);
        //　アイテム数が0になった時
        if (allyStatus.GetItemNum(item) == 0)
        {

            //　装備している武器を捨てる場合の処理
            if (item == allyStatus.GetEquipArmor())
            {
                var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
                equipArmorButton = null;
                allyStatus.SetEquipArmor(null);
            }
            else if (item == allyStatus.GetEquipWeapon())
            {
                var equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "";
                equipWeaponButton = null;
                allyStatus.SetEquipWeapon(null);
            }
        }
        //　ItemPanelの子要素のアイテムパネルボタンから該当するアイテムのボタンを探して数を更新する
        var itemButton = itemPanelButtonList.Find(obj => obj.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
        itemButton.transform.Find("Num").GetComponent<Text>().text = allyStatus.GetItemNum(item).ToString();
        useItemInformationPanel.GetComponentInChildren<Text>().text = item.GetKanjiName() + "を捨てました。";

        //　アイテム数が0だったらボタンとキャラクターステータスからアイテムを削除
        if (allyStatus.GetItemNum(item) == 0)
        {
            selectedGameObjectStack.Pop();
            itemPanelButtonList.Remove(itemButton);
            Destroy(itemButton);
            allyStatus.GetItemDictionary().Remove(item);

            currentCommand = CommandMode.NoItemPassed;
            useItemPanelCanvasGroup.interactable = false;
            useItemPanel.SetActive(false);
            useItemInformationPanel.transform.SetAsLastSibling();
            useItemInformationPanel.SetActive(true);
            //　ItemPanelに戻る為UseItemPanelの子要素のボタンを全削除
            for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(useItemPanel.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            useItemPanelCanvasGroup.interactable = false;
            useItemInformationPanel.transform.SetAsLastSibling();
            useItemInformationPanel.SetActive(true);
            currentCommand = CommandMode.UseItemPanelToUseItemPanel;
        }

        Input.ResetInputAxes();

    }

    //　装備する
    public void Equip(AllyStatus allyStatus, Item item)
    {
        //　キャラクター毎に装備出来る武器や鎧かどうかを調べ装備を切り替える
        if (allyStatus.GetCharacterName() == "ユニティちゃん")
        {
            if (item.GetItemType() == Item.Type.Armor)
            {
                var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "E";
                //　装備している鎧があればItemPanelでEquipのEを外す
                if (allyStatus.GetEquipArmor() != null)
                {
                    equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipArmor().GetKanjiName());
                    equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipArmor(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "は" + item.GetKanjiName() + "を装備しました。";
            }
            else if (item.GetItemType() == Item.Type.Weapon)
            {
                var equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
                equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "E";
                //　装備している武器があればItemPanelでEquipのEを外す
                if (allyStatus.GetEquipWeapon() != null)
                {
                    equipWeaponButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == allyStatus.GetEquipWeapon().GetKanjiName());
                    equipWeaponButton.transform.Find("Equip").GetComponent<Text>().text = "";
                }
                allyStatus.SetEquipWeapon(item);
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "は" + item.GetKanjiName() + "を装備しました。";
            }
            else
            {
                useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "は" + item.GetKanjiName() + "を装備出来ません。";
            }
        }
        //　装備を切り替えたらItemPanelに戻る
        useItemPanelCanvasGroup.interactable = false;
        useItemPanel.SetActive(false);
        itemPanelCanvasGroup.interactable = true;
        //　ItemPanelに戻るのでUseItemPanelの子要素に作ったボタンを全削除
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }

        useItemInformationPanel.transform.SetAsLastSibling();
        useItemInformationPanel.SetActive(true);

        currentCommand = CommandMode.UseItemPanelToItemPanel;

        Input.ResetInputAxes();

    }

    //　装備を外す
    public void RemoveEquip(AllyStatus allyStatus, Item item)
    {
        //　アイテムの種類に応じて装備を外す
        if (item.GetItemType() == Item.Type.Armor)
        {
            var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
            equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
            allyStatus.SetEquipArmor(null);
        }
        else if (item.GetItemType() == Item.Type.Weapon)
        {
            var equipArmorButton = itemPanelButtonList.Find(itemPanelButton => itemPanelButton.transform.Find("ItemName").GetComponent<Text>().text == item.GetKanjiName());
            equipArmorButton.transform.Find("Equip").GetComponent<Text>().text = "";
            allyStatus.SetEquipWeapon(null);
        }
        //　装備を外した旨を表示
        useItemInformationPanel.GetComponentInChildren<Text>().text = allyStatus.GetCharacterName() + "は" + item.GetKanjiName() + "を外しました。";
        //　装備を外したらItemPanelに戻る処理
        useItemPanelCanvasGroup.interactable = false;
        useItemPanel.SetActive(false);
        itemPanelCanvasGroup.interactable = true;
        //　ItemPanelに戻るのでUseItemPanelの子要素のボタンを全削除
        for (int i = useItemPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(useItemPanel.transform.GetChild(i).gameObject);
        }

        useItemInformationPanel.transform.SetAsLastSibling();
        useItemInformationPanel.SetActive(true);

        currentCommand = CommandMode.UseItemPanelToItemPanel;
        Input.ResetInputAxes();
    }

    public void CancelButtonPushed()
    {
        isCancelButtonPushed = true;
    }
}