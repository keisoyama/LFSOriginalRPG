using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerMain : MonoBehaviour
{

    public enum BattleMode
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
    //　戦闘が終了したかどうか
    private bool battleIsOver;
    //　現在のコマンド
    private BattleMode currentCommand;

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
