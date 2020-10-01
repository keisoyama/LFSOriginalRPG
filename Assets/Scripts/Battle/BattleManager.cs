using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //　戦闘データ
    [SerializeField]
    private BattleData battleData = null;
    //　キャラクターのベース位置
    [SerializeField]
    private Transform battleBasePosition;
    //　現在戦闘に参加しているキャラクター
    private List<GameObject> allCharacterList = new List<GameObject>();

    public enum CommandMode
    {
        SelectCommand,
        SelectDirectAttacker,
        SelectMagic,
        SelectMagicAttackTarget,
        SelectUseMagicOnAlliesTarget,
        SelectItem,
        SelectRecoveryItemTarget
    }

    //　現在戦闘に参加している全キャラクター
    private List<GameObject> allCharacterInBattleList = new List<GameObject>();
    //　現在戦闘に参加している味方キャラクター
    private List<GameObject> allyCharacterInBattleList = new List<GameObject>();
    //　現在戦闘に参加している敵キャラクター
    private List<GameObject> enemyCharacterInBattleList = new List<GameObject>();
    //　現在の攻撃の順番
    private int currentAttackOrder;
    //　現在攻撃をしようとしている人が選択中
    private bool isChoosing;
    //　戦闘が開始しているかどうか
    private bool isStartBattle;
    //　戦闘シーンの最初の攻撃が始まるまでの待機時間
    [SerializeField]
    private float firstWaitingTime = 3f;
    //　戦闘シーンのキャラ移行時の間の時間
    [SerializeField]
    private float timeToNextCharacter = 1f;
    //　待ち時間
    private float waitTime;
    //　戦闘シーンの最初の攻撃が始まるまでの経過時間
    private float elapsedTime;
    //　戦闘が終了したかどうか
    private bool battleIsOver;
    //　現在のコマンド
    private CommandMode currentCommand;

    // Start is called before the first frame update
    void Start()
    {
        //　キャラクターインスタンスの親
        Transform charactersParent = new GameObject("Characters").transform;
        //　キャラクターを配置するTransform
        Transform characterTransform;
        //　同じ名前の敵がいた場合の処理に使うリスト
        List<string> enemyNameList = new List<string>();

        GameObject ins;
        CharacterBattle characterBattleScript;
        string characterName;

        //　味方パーティーのプレハブをインスタンス化
        for (int i = 0; i < battleData.GetAllyPartyStatus().GetAllyGameObject().Count; i++)
        {
            characterTransform = battleBasePosition.Find("AllyPos" + i).transform;
            ins = Instantiate<GameObject>(battleData.GetAllyPartyStatus().GetAllyGameObject()[i], characterTransform.position, characterTransform.rotation, charactersParent);
            characterBattleScript = ins.GetComponent<CharacterBattle>();
            ins.name = characterBattleScript.GetCharacterStatus().GetCharacterName();
            if (characterBattleScript.GetCharacterStatus().GetHp() > 0)
            {
                allyCharacterInBattleList.Add(ins);
                allCharacterList.Add(ins);
            }
        }
        if (battleData.GetEnemyPartyStatus() == null)
        {
            Debug.LogError("敵パーティーデータが設定されていません。");
        }
        //　敵パーティーのプレハブをインスタンス化
        for (int i = 0; i < battleData.GetEnemyPartyStatus().GetEnemyGameObjectList().Count; i++)
        {
            characterTransform = battleBasePosition.Find("EnemyPos" + i).transform;
            ins = Instantiate<GameObject>(battleData.GetEnemyPartyStatus().GetEnemyGameObjectList()[i], characterTransform.position, characterTransform.rotation, charactersParent);
            //　既に同じ敵が存在したら文字を付加する
            characterName = ins.GetComponent<CharacterBattle>().GetCharacterStatus().GetCharacterName();
            if (!enemyNameList.Contains(characterName))
            {
                ins.name = characterName + 'A';
            }
            else
            {
                ins.name = characterName + (char)('A' + enemyNameList.Count(enemyName => enemyName == characterName));
            }
            enemyNameList.Add(characterName);
            enemyCharacterInBattleList.Add(ins);
            allCharacterList.Add(ins);
        }
        //　キャラクターリストをキャラクターの素早さの高い順に並べ替え
        allCharacterList = allCharacterList.OrderByDescending(character => character.GetComponent<CharacterBattle>().GetCharacterStatus().GetAgility()).ToList<GameObject>();
        //　現在の戦闘
        allCharacterInBattleList = allCharacterList.ToList<GameObject>();
        //　確認の為並べ替えたリストを表示
        foreach (var character in allCharacterInBattleList)
        {
            Debug.Log(character.GetComponent<CharacterBattle>().GetCharacterStatus().GetCharacterName() + " : " + character.GetComponent<CharacterBattle>().GetCharacterStatus().GetAgility());
        }
        //　戦闘前の待ち時間を設定
        waitTime = firstWaitingTime;
        //　ランダム値のシードの設定
        Random.InitState((int)Time.time);
    }

    // Update is called once per frame
    void Update()
    {

        //　戦闘が終了していたらこれ以降何もしない
        if (battleIsOver)
        {
            return;
        }

        //　戦闘開始
        if (isStartBattle)
        {
            //　現在のキャラクターの攻撃が終わっている
            if (!isChoosing)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime < waitTime)
                {
                    return;
                }
                elapsedTime = 0f;
                isChoosing = true;

                //　キャラクターの攻撃の選択に移る
                MakeAttackChoise(allCharacterInBattleList[currentAttackOrder]);
                //　次のキャラクターのターンにする
                currentAttackOrder++;
                //　全員攻撃が終わったら最初から
                if (currentAttackOrder >= allCharacterInBattleList.Count)
                {
                    currentAttackOrder = 0;
                }
            }
        }
        else
        {
            Debug.Log("経過時間： " + elapsedTime);
            //　戦闘前の待機
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                //　2回目以降はキャラ間の時間を設定
                waitTime = timeToNextCharacter;
                //　最初のキャラクターの待ち時間は0にする為にあらかじめ条件をクリアさせておく
                elapsedTime = timeToNextCharacter;
                isStartBattle = true;
            }
        }
    }

    //　キャラクターの攻撃の選択処理
    public void MakeAttackChoise(GameObject character)
    {
        BaseStatus characterStatus = character.GetComponent<CharacterBattle>().GetCharacterStatus();
        //　EnemyStatusにキャスト出来る場合は敵の攻撃処理
        if (characterStatus as EnemyStatus != null)
        {
            Debug.Log(character.gameObject.name + "の攻撃");
            EnemyAttack(character);
        }
        else
        {
            Debug.Log(characterStatus.GetCharacterName() + "の攻撃");
            AllyAttack(character);
        }
    }

    //　味方の攻撃
    public void AllyAttack(GameObject character)
    {
    }

    //　敵の攻撃処理
    public void EnemyAttack(GameObject character)
    {
        CharacterBattle characterBattleScript = character.GetComponent<CharacterBattle>();
        BaseStatus characterStatus = characterBattleScript.GetCharacterStatus();

        if (characterStatus.GetSkillList().Count <= 0)
        {
            return;
        }
        //　敵の行動アルゴリズム
        int randomValue = (int)(Random.value * characterStatus.GetSkillList().Count);
        var nowSkill = characterStatus.GetSkillList()[randomValue];

        //　テスト用（特定のスキルで確認）
        //nowSkill = characterStatus.GetSkillList()[0];

        if (nowSkill.GetSkillType() == Skill.Type.DirectAttack)
        {
            var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);
            //　攻撃相手のCharacterBattleScript
            characterBattleScript.ChooseAttackOptions(CharacterBattle.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], nowSkill);
            Debug.Log(character.name + "は" + nowSkill.GetKanjiName() + "を行った");
        }
        else if (nowSkill.GetSkillType() == Skill.Type.MagicAttack)
        {
            var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);
            if (characterBattleScript.GetMp() >= ((Magic)nowSkill).GetAmountToUseMagicPoints())
            {
                //　攻撃相手のCharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattle.BattleState.MagicAttack, allyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "は" + nowSkill.GetKanjiName() + "を行った");
            }
            else
            {
                Debug.Log("MPが足りない！");
                //　MPが足りない場合は直接攻撃を行う
                characterBattleScript.ChooseAttackOptions(CharacterBattle.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], characterStatus.GetSkillList().Find(skill => skill.GetSkillType() == Skill.Type.DirectAttack));
                Debug.Log(character.name + "は攻撃を行った");
            }
        }
        else if (nowSkill.GetSkillType() == Skill.Type.RecoveryMagic)
        {
            if (characterBattleScript.GetMp() >= ((Magic)nowSkill).GetAmountToUseMagicPoints())
            {
                var targetNum = (int)(Random.value * enemyCharacterInBattleList.Count);
                //　回復相手のCharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattle.BattleState.Healing, enemyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "は" + nowSkill.GetKanjiName() + "を行った");
            }
            else
            {
                Debug.Log("MPが足りない！");
                var targetNum = (int)(Random.value * allyCharacterInBattleList.Count);
                //　MPが足りない場合は直接攻撃を行う
                characterBattleScript.ChooseAttackOptions(CharacterBattle.BattleState.DirectAttack, allyCharacterInBattleList[targetNum], characterStatus.GetSkillList().Find(skill => skill.GetSkillType() == Skill.Type.DirectAttack));
                Debug.Log(character.name + "は攻撃を行った");
            }
        }
    }

    //　次のキャラクターに移行
    public void ChangeNextChara()
    {
        isChoosing = false;
    }

    public void DeleteAllCharacterInBattleList(GameObject deleteObj)
    {
        var deleteObjNum = allCharacterInBattleList.IndexOf(deleteObj);
        allCharacterInBattleList.Remove(deleteObj);
        if (deleteObjNum < currentAttackOrder)
        {
            currentAttackOrder--;
        }
        //　全員攻撃が終わったら最初から
        if (currentAttackOrder >= allCharacterInBattleList.Count)
        {
            currentAttackOrder = 0;
        }
    }

    public void DeleteEnemyCharacterInBattleList(GameObject deleteObj)
    {
        enemyCharacterInBattleList.Remove(deleteObj);
        if (enemyCharacterInBattleList.Count == 0)
        {
            Debug.Log("敵が全滅");
            battleIsOver = true;
        }
    }

    public void DeleteAllyCharacterInBattleList(GameObject deleteObj)
    {
        allyCharacterInBattleList.Remove(deleteObj);
        if (allyCharacterInBattleList.Count == 0)
        {
            Debug.Log("味方が全滅");
            battleIsOver = true;
        }
    }
}
