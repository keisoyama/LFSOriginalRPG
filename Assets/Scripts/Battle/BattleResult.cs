using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleResult : MonoBehaviour
{

    //　結果を表示してからワールドマップに戻れるようになるまでの時間
    [SerializeField]
    private float timeToDisplay = 3f;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private PartyStatus partyStatus;
    //　戦闘結果表示をしているかどうか
    private bool isDisplayResult;
    //　結果を表示し戦闘から抜け出せるかどうか
    private bool isFinishResult;
    //　戦闘に勝利したかどうか
    private bool won;
    //　逃げたかどうか
    private bool ranAway;
    //　戦闘結果テキストのスクロール値
    [SerializeField]
    private float scrollValue = 50f;
    //　MusicManager
    [SerializeField]
    private MusicManager musicManager;

    void Update()
    {
        //　結果表示前は何もしない
        if (!isDisplayResult)
        {
            return;
        }

        //　結果表示後は結果表示テキストをスクロールして見れるようにする
        if (Input.GetAxis("Vertical") != 0f)
        {
            resultText.transform.localPosition += new Vector3(0f, -Input.GetAxis("Vertical") * scrollValue, 0f);
        }
        //　戦闘を抜け出すまでの待機時間を越えていない
        if (!isFinishResult)
        {
            return;
        }
        //　タップしたらワールドマップに戻る
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (won || ranAway)
            {
                GameObject.Find("SceneManager").GetComponent<LoadSceneManager>().GoToNextScene(SceneMovementData.SceneType.BattleToWorldMap);
            }
            else
            {
                GameObject.Find("SceneManager").GetComponent<LoadSceneManager>().GoToNextScene(SceneMovementData.SceneType.FirstVillage);
            }
        }
    }

    //　勝利時の初期処理
    public void InitialProcessingOfVictoryResult(List<GameObject> allCharacterList, List<GameObject> allyCharacterInBattleList)
    {
        StartCoroutine(DisplayVictoryResult(allCharacterList, allyCharacterInBattleList));
    }

    //　勝利時の結果
    public IEnumerator DisplayVictoryResult(List<GameObject> allCharacterList, List<GameObject> allyCharacterInBattleList)
    {
        yield return new WaitForSeconds(timeToDisplay);
        won = true;
        resultPanel.SetActive(true);
        //　戦闘で獲得した経験値
        var earnedExperience = 0;
        //　戦闘で獲得したお金
        var earnedMoney = 0;
        //　戦闘で獲得したアイテムとその個数
        Dictionary<Item, int> getItemDictionary = new Dictionary<Item, int>();
        //　Floatのランダム値
        float randomFloat;
        //　アイテム取得確率
        float probability;
        //　キャラクターステータス
        BaseStatus BaseStatus;
        //　敵のアイテムディクショナリー
        ItemDictionary enemyItemDictionary;

        foreach (var character in allCharacterList)
        {
            BaseStatus = character.GetComponent<CharacterBattle>().GetCharacterStatus();
            if (BaseStatus as EnemyStatus != null)
            {
                earnedExperience += ((EnemyStatus)BaseStatus).GetGettingExperience();
                earnedMoney += ((EnemyStatus)BaseStatus).GetGettingMoney();
                enemyItemDictionary = ((EnemyStatus)BaseStatus).GetDropItemDictionary();
                //　敵が持っているアイテムの種類の数だけ繰り返し
                foreach (var item in enemyItemDictionary.Keys)
                {
                    //　0～100の間のランダム値を取得
                    randomFloat = Random.Range(0f, 100f);
                    //　アイテムの取得確率を取得
                    probability = enemyItemDictionary[item];
                    //　ランダム値がアイテム取得確率以下の値であればアイテム取得
                    if (randomFloat <= probability)
                    {
                        if (getItemDictionary.ContainsKey(item))
                        {
                            getItemDictionary[item]++;
                        }
                        else
                        {
                            getItemDictionary.Add(item, 1);
                        }
                    }
                }
            }
        }
        resultText.text = earnedExperience + "の経験値を獲得した。\n";
        resultText.text += earnedMoney + "のお金を獲得した。\n";

        //　パーティーステータスにお金を反映する
        partyStatus.SetMoney(partyStatus.GetMoney() + earnedMoney);

        //　intのランダム値
        int randomInt;
        AllyStatus allyStatus;

        //　取得したアイテムを味方パーティーに分配する
        foreach (var item in getItemDictionary.Keys)
        {
            //　パーティーメンバーの誰にアイテムを渡すか決定
            randomInt = Random.Range(0, allyCharacterInBattleList.Count);
            allyStatus = (AllyStatus)allyCharacterInBattleList[randomInt].GetComponent<CharacterBattle>().GetCharacterStatus();
            //　キャラクターが既にアイテムを持っている時
            if (allyStatus.GetItemDictionary().ContainsKey(item))
            {
                allyStatus.SetItemNum(item, allyStatus.GetItemNum(item) + getItemDictionary[item]);
            }
            else
            {
                allyStatus.SetItemDictionary(item, getItemDictionary[item]);
            }
            resultText.text += allyStatus.GetCharacterName() + "は" + item.GetKanjiName() + "を" + getItemDictionary[item] + "個手に入れた。\n";
            resultText.text += "\n";
        }

        //　上がったレベル
        var levelUpCount = 0;
        //　上がったHP
        var raisedHp = 0;
        //　上がったMP
        var raisedMp = 0;
        //　上がった素早さ
        var raisedAgility = 0;
        //　上がった力
        var raisedPower = 0;
        //　上がった打たれ強さ
        var raisedStrikingStrength = 0;
        //　上がった魔法力
        var raisedMagicPower = 0;
        //　LevelUpData
        LevelUpData levelUpData;

        //　レベルアップ等の計算
        foreach (var characterObj in allyCharacterInBattleList)
        {
            var character = (AllyStatus)characterObj.GetComponent<CharacterBattle>().GetCharacterStatus();
            //　変数を初期化
            levelUpCount = 0;
            raisedHp = 0;
            raisedMp = 0;
            raisedAgility = 0;
            raisedPower = 0;
            raisedStrikingStrength = 0;
            raisedMagicPower = 0;
            levelUpData = character.GetLevelUpData();

            //　キャラクターに経験値を反映
            character.SetEarnedExperience(character.GetEarnedExperience() + earnedExperience);

            //　そのキャラクターの経験値で何レベルアップしたかどうか
            for (int i = 1; i < levelUpData.GetLevelUpDictionary().Count; i++)
            {
                //　レベルアップに必要な経験値を満たしていたら
                if (character.GetEarnedExperience() >= levelUpData.GetRequiredExperience(character.GetLevel() + i))
                {
                    levelUpCount++;
                }
                else
                {
                    break;
                }
            }
            //　レベルを反映
            character.SetLevel(character.GetLevel() + levelUpCount);

            //　レベルアップ分のステータスアップを計算し反映する
            for (int i = 0; i < levelUpCount; i++)
            {
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMaxHP())
                {
                    raisedHp += Random.Range(levelUpData.GetMinHPRisingLimit(), levelUpData.GetMaxHPRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMaxMP())
                {
                    raisedMp += Random.Range(levelUpData.GetMinMPRisingLimit(), levelUpData.GetMaxMPRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseAgility())
                {
                    raisedAgility += Random.Range(levelUpData.GetMinAgilityRisingLimit(), levelUpData.GetMaxAgilityRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreasePower())
                {
                    raisedPower += Random.Range(levelUpData.GetMinPowerRisingLimit(), levelUpData.GetMaxPowerRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseStrikingStrength())
                {
                    raisedStrikingStrength += Random.Range(levelUpData.GetMinStrikingStrengthRisingLimit(), levelUpData.GetMaxStrikingStrengthRisingLimit());
                }
                if (Random.Range(0f, 100f) <= levelUpData.GetProbabilityToIncreaseMagicPower())
                {
                    raisedMagicPower += Random.Range(levelUpData.GetMinMagicPowerRisingLimit(), levelUpData.GetMaxMagicPowerRisingLimit());
                }
            }
            if (levelUpCount > 0)
            {
                resultText.text += character.GetCharacterName() + "は" + levelUpCount + "レベル上がってLv" + character.GetLevel() + "になった。\n";
                if (raisedHp > 0)
                {
                    resultText.text += "最大HPが" + raisedHp + "上がった。\n";
                    character.SetMaxHp(character.GetMaxHp() + raisedHp);
                }
                if (raisedMp > 0)
                {
                    resultText.text += "最大MPが" + raisedMp + "上がった。\n";
                    character.SetMaxMp(character.GetMaxMp() + raisedMp);
                }
                if (raisedAgility > 0)
                {
                    resultText.text += "素早さが" + raisedAgility + "上がった。\n";
                    character.SetAgility(character.GetAgility() + raisedAgility);
                }
                if (raisedPower > 0)
                {
                    resultText.text += "力が" + raisedPower + "上がった。\n";
                    character.SetPower(character.GetPower() + raisedPower);
                }
                if (raisedStrikingStrength > 0)
                {
                    resultText.text += "打たれ強さが" + raisedStrikingStrength + "上がった。\n";
                    character.SetStrikingStrength(character.GetStrikingStrength() + raisedStrikingStrength);
                }
                if (raisedMagicPower > 0)
                {
                    resultText.text += "魔法力が" + raisedMagicPower + "上がった。\n";
                    character.SetMagicPower(character.GetMagicPower() + raisedMagicPower);
                }
                resultText.text += "\n";
            }
        }
        //　結果を計算し終わった
        isDisplayResult = true;

        //　戦闘終了のBGMに変更する
        musicManager.ChangeBGM();

        //　結果後に数秒待機
        yield return new WaitForSeconds(timeToDisplay);
        //　戦闘から抜け出す
        resultPanel.transform.Find("FinishText").gameObject.SetActive(true);
        isFinishResult = true;
    }

    //　敗戦時の初期処理
    public void InitialProcessingOfDefeatResult()
    {
        StartCoroutine(DisplayDefeatResult());
    }

    //　敗戦時の表示
    public IEnumerator DisplayDefeatResult()
    {
        yield return new WaitForSeconds(timeToDisplay);
        resultPanel.SetActive(true);
        resultText.text = "ユニティちゃん達は全滅した。";
        isDisplayResult = true;
        yield return new WaitForSeconds(timeToDisplay);
        var finishText = resultPanel.transform.Find("FinishText");
        finishText.GetComponent<Text>().text = "最初の街へ";
        finishText.gameObject.SetActive(true);

        //　味方が全滅したのでユニティちゃんのHPだけ少し回復しておく
        var unityChanStatus = partyStatus.GetAllyStatus().Find(character => character.GetCharacterName() == "ユニティちゃん");
        if (unityChanStatus != null)
        {
            unityChanStatus.SetHp(1);
        }

        isFinishResult = true;
    }

    //　逃げた時の初期処理
    public void InitialProcessingOfRanAwayResult()
    {
        StartCoroutine(DisplayRanAwayResult());
    }

    //　逃げた時の表示
    public IEnumerator DisplayRanAwayResult()
    {
        yield return new WaitForSeconds(timeToDisplay);
        ranAway = true;
        resultPanel.SetActive(true);
        resultText.text = "ユニティちゃん達は逃げ出した。";
        isDisplayResult = true;
        yield return new WaitForSeconds(timeToDisplay);
        var finishText = resultPanel.transform.Find("FinishText");
        finishText.GetComponent<Text>().text = "ワールドマップへ";
        finishText.gameObject.SetActive(true);
        isFinishResult = true;
    }
}