using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    //　戦闘中のキャラクターの状態
    public enum BattleState
    {
        Idle,
        DirectAttack,
        MagicAttack,
        Healing,
        UseHPRecoveryItem,
        UseMPRecoveryItem,
        UseParalyzeRecoveryItem,
        UsePoisonRecoveryItem,
        UseSilenceRecoveryItem,
        IncreaseAttackPowerMagic,
        IncreaseDefencePowerMagic,
        Damage,
        Dead,
    }

    private BattleManager battleManager;
    private BattleStatus battleStatusScript;
    [SerializeField]
    private BaseStatus baseStatus = null;
    private BattleState battleState;

    //　元のステータスからコピー

    //　HP
    private int hp = 0;
    //　MP
    private int mp = 0;

    //　補助の素早さ
    private int auxiliaryAgility = 0;
    //　補助の力
    private int auxiliaryPower = 0;
    //　補助の打たれ強さ
    private int auxiliaryStrikingStrength = 0;
    //　痺れ状態か
    private bool isNumbness;
    //　毒状態か
    private bool isPoison;
    //沈黙状態か
    private bool isSilent;

    //　今選択したスキル
    private Skill currentSkill;
    //　今のターゲット
    private GameObject currentTarget;
    //　今使用したアイテム
    private Item currentItem;
    //　ターゲットのCharacterBattleScript
    private CharacterBattle



        targetCharacterBattleScript;
    //　ターゲットのCharacterStatus
    private BaseStatus targetBaseStatus;
    //　キャラクターが死んでいるかどうか
    private bool isDead;

    //　攻撃力アップしているかどうか
    private bool isIncreasePower;
    //　攻撃力アップしているポイント
    private int increasePowerPoint;
    //　攻撃力アップしているターン
    private int numOfTurnsIncreasePower = 3;
    //　攻撃力アップしてからのターン
    private int numOfTurnsSinceIncreasePower = 0;
    //　防御力アップしているかどうか
    private bool isIncreaseStrikingStrength;
    //　防御力アップしているポイント
    private int increaseStrikingStrengthPoint;
    //　防御力アップしているターン
    private int numOfTurnsIncreaseStrikingStrength = 3;
    //　防御力アップしてからのターン
    private int numOfTurnsSinceIncreaseStrikingStrength = 0;

    private void Start()
    {
        //　元データから設定
        hp = baseStatus.GetHp();
        mp = baseStatus.GetMp();
        isNumbness = baseStatus.IsParalyzeState();
        isPoison = baseStatus.IsPoisonState();
        isSilent = baseStatus.IsSilentState();

        //　状態の設定
        battleState = BattleState.Idle;
        //　コンポーネントの取得
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        battleStatusScript = GameObject.Find("BattleUI/StatusPanel").GetComponent<BattleStatus>();
        //　既に死んでいる場合は倒れている状態にする
        if (baseStatus.GetHp() <= 0)
        {
            isDead = true;
        }
    }

    void Update()
    {
        //　既に死んでいたら何もしない
        if (isDead)
        {
            return;
        }

        //　自分のターンでなければ何もしない
        if (battleState == BattleState.Idle)
        {
            return;
        }
        //　選択したアニメーションによって処理を分ける
        if (battleState == BattleState.DirectAttack)
        {
            ShowEffectOnTheTarget();
            DirectAttack();
            //　自分のターンが来たので上がったパラメータのチェック
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.MagicAttack)
        {
            ShowEffectOnTheTarget();
            MagicAttack();
            //　自分のターンが来たので上がったパラメータのチェック
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.Healing)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //　自分のターンが来たので上がったパラメータのチェック
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        else if (battleState == BattleState.IncreaseAttackPowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //　自身の攻撃力をアップした場合はターン数をカウントしない
            if (currentTarget == this.gameObject)
            {
                CheckIncreaseStrikingStrength();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseStrikingStrength();
            }
        }
        else if (battleState == BattleState.IncreaseDefencePowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //　自身の防御力をアップした場合はターン数をカウントしない
            if (currentTarget == this.gameObject)
            {
                CheckIncreaseAttackPower();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseStrikingStrength();
            }
        }
        else if (battleState == BattleState.UseHPRecoveryItem
          || battleState == BattleState.UseMPRecoveryItem
          || battleState == BattleState.UseParalyzeRecoveryItem
          || battleState == BattleState.UsePoisonRecoveryItem
          )
        {
            UseItem();
            //　自分のターンが来たので上がったパラメータのチェック
            CheckIncreaseAttackPower();
            CheckIncreaseStrikingStrength();
        }
        //　ターゲットのリセット
        currentTarget = null;
        currentSkill = null;
        currentItem = null;
        targetCharacterBattleScript = null;
        targetBaseStatus = null;
        battleState = BattleState.Idle;
        //　自身の選択が終了したら次のキャラクターにする
        battleManager.ChangeNextChara();
    }

    //　選択肢から選んだモードを実行
    public void ChooseAttackOptions(BattleState selectOption, GameObject target, Skill skill = null, Item item = null)
    {

        //　スキルやターゲットの情報をセット
        currentTarget = target;
        currentSkill = skill;
        targetCharacterBattleScript = target.GetComponent<CharacterBattle>();
        targetBaseStatus = targetCharacterBattleScript.GetCharacterStatus();

        //　選択したキャラクターの状態を設定
        battleState = selectOption;

        if (selectOption == BattleState.DirectAttack)
        {
            battleManager.ShowMessage(gameObject.name + "は" + currentTarget.name + "に" + currentSkill.GetKanjiName() + "を行った。");
        }
        else if (selectOption == BattleState.MagicAttack
          || selectOption == BattleState.Healing
          || selectOption == BattleState.IncreaseAttackPowerMagic
          || selectOption == BattleState.IncreaseDefencePowerMagic
          )
        {
            //　魔法使用者のMPを減らす
            SetMp(GetMp() - ((Magic)skill).GetAmountToUseMagicPoints());
            battleManager.ShowMessage(gameObject.name + "は" + currentTarget.name + "に" + currentSkill.GetKanjiName() + "を使った。");
            //　使用者が味方キャラクターであればStatusPanelの更新
            if (GetCharacterStatus() as AllyStatus != null)
            {
                battleStatusScript.UpdateStatus(GetCharacterStatus(), BattleStatus.Status.MP, GetMp());
            }
            //Instantiate(((Magic)skill).GetSkillUserEffect(), transform.position, ((Magic)skill).GetSkillUserEffect().transform.rotation);
        }
        else if (selectOption == BattleState.UseHPRecoveryItem
          || selectOption == BattleState.UseMPRecoveryItem
          || selectOption == BattleState.UseParalyzeRecoveryItem
          || selectOption == BattleState.UsePoisonRecoveryItem
          )
        {
            currentItem = item;
            battleManager.ShowMessage(gameObject.name + "は" + currentTarget.name + "に" + item.GetKanjiName() + "を使った。");
        }
    }

    //　ターゲットエフェクトの表示
    public void ShowEffectOnTheTarget()
    {
        //Instantiate<GameObject>(currentSkill.GetSkillReceivingSideEffect(), currentTarget.transform.position, currentSkill.GetSkillReceivingSideEffect().transform.rotation);
    }

    //物理攻撃のダメージ処理
    public void DirectAttack()
    {
        var damage = 0;

        //　攻撃相手のStatus
        if (targetBaseStatus as AllyStatus != null)
        {
            var castedTargetStatus = (AllyStatus)targetCharacterBattleScript.GetCharacterStatus();
            //　攻撃相手の通常の防御力＋相手のキャラの補助値
            var targetDefencePower = castedTargetStatus.GetStrikingStrength() + (castedTargetStatus.GetEquipArmor()?.GetAmount() ?? 0);
            damage = Mathf.Max(0, (baseStatus.GetPower() * auxiliaryPower) - targetDefencePower * targetCharacterBattleScript.GetAuxiliaryStrikingStrength());
            battleManager.ShowMessage(currentTarget.name + "は" + damage + "のダメージを受けた。");
            //　相手のステータスのHPをセット
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
            //　ステータスUIを更新
            battleStatusScript.UpdateStatus(targetBaseStatus, BattleStatus.Status.HP, targetCharacterBattleScript.GetHp());
        }
        else if (targetBaseStatus as EnemyStatus != null)
        {
            var castedTargetStatus = (EnemyStatus)targetCharacterBattleScript.GetCharacterStatus();
            //　攻撃相手の通常の防御力＋相手のキャラの補助値
            var targetDefencePower = castedTargetStatus.GetStrikingStrength() * targetCharacterBattleScript.GetAuxiliaryStrikingStrength();
            damage = Mathf.Max(0, (baseStatus.GetPower() + (((AllyStatus)baseStatus).GetEquipWeapon()?.GetAmount() ?? 0) * auxiliaryPower) - targetDefencePower);
            battleManager.ShowMessage(currentTarget.name + "は" + damage + "のダメージを受けた。");
            //　敵のステータスのHPをセット
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
        }
        else
        {
            Debug.LogError("直接攻撃でターゲットが設定されていない");
        }

    }

    //魔法攻撃のダメージ処理
    public void MagicAttack()
    {
        var damage = 0;

        //　攻撃相手のStatus
        if (targetBaseStatus as AllyStatus != null)
        {
            var castedTargetStatus = (AllyStatus)targetCharacterBattleScript.GetCharacterStatus();
            damage = Mathf.Max(0, ((Magic)currentSkill).GetMagicPower() );
            battleManager.ShowMessage(currentTarget.name + "は" + damage + "のダメージを受けた。");
            ////　相手のステータスのHPをセット
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
            //　ステータスUIを更新
            battleStatusScript.UpdateStatus(targetBaseStatus, BattleStatus.Status.HP, targetCharacterBattleScript.GetHp());
        }
        else if (targetBaseStatus as EnemyStatus != null)
        {
            var castedTargetStatus = (EnemyStatus)targetCharacterBattleScript.GetCharacterStatus();
            damage = Mathf.Max(0, ((Magic)currentSkill).GetMagicPower());
            battleManager.ShowMessage(currentTarget.name + "は" + damage + "のダメージを受けた。");
            //　相手のステータスのHPをセット
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() - damage);
        }
        else
        {
            Debug.LogError("魔法攻撃でターゲットが設定されていない");
        }

        Debug.Log(gameObject.name + "は" + currentTarget.name + "に" + currentSkill.GetKanjiName() + "をして" + damage + "を与えた。");

    }

    public void UseMagic()
    {

        var magicType = ((Magic)currentSkill).GetSkillType();
        //かいふく魔法
        if (magicType == Skill.Type.RecoveryMagic)
        {
            var recoveryPoint = ((Magic)currentSkill).GetMagicPower();
            if (targetBaseStatus as AllyStatus != null)
            {
                targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() + recoveryPoint);
                battleStatusScript.UpdateStatus(targetBaseStatus, BattleStatus.Status.HP, targetCharacterBattleScript.GetHp());
            }
            else
            {
                targetCharacterBattleScript.SetHp(GetHp() + recoveryPoint);
            }
            battleManager.ShowMessage(currentTarget.name + "を" + recoveryPoint + "回復した。");
        }
        //攻撃アップ魔法
        else if (magicType == Skill.Type.IncreaseAttackPowerMagic)
        {
            increasePowerPoint = ((Magic)currentSkill).GetMagicPower() ;
            targetCharacterBattleScript.SetAuxiliaryPower(targetCharacterBattleScript.GetAuxiliaryPower() * increasePowerPoint);
            targetCharacterBattleScript.SetIsIncreasePower(true);
            battleManager.ShowMessage(currentTarget.name + "の力を増やした。");
        }
        //防御アップ魔法
        else if (magicType == Skill.Type.IncreaseDefencePowerMagic)
        {
            increaseStrikingStrengthPoint = ((Magic)currentSkill).GetMagicPower();
            targetCharacterBattleScript.SetAuxiliaryStrikingStrength(targetCharacterBattleScript.GetAuxiliaryStrikingStrength() * increaseStrikingStrengthPoint);
            targetCharacterBattleScript.SetIsIncreaseStrikingStrength(true);
            battleManager.ShowMessage(currentTarget.name + "の防御を増やした。");
        }
    }

    public void UseItem()
    {
        //　キャラクターのアイテム数を減らす
        ((AllyStatus)baseStatus).SetItemNum(currentItem, ((AllyStatus)baseStatus).GetItemNum(currentItem) - 1);

        if (currentItem.GetItemType() == Item.Type.HPRecovery)
        {
            //　回復力
            var recoveryPoint = currentItem.GetAmount();
            targetCharacterBattleScript.SetHp(targetCharacterBattleScript.GetHp() + recoveryPoint);
            battleStatusScript.UpdateStatus(targetBaseStatus, BattleStatus.Status.HP, targetCharacterBattleScript.GetHp());
            battleManager.ShowMessage(currentTarget.name + "のHPを" + recoveryPoint + "回復した。");
        }
        else if (currentItem.GetItemType() == Item.Type.MPRecovery)
        {
            //　回復力
            var recoveryPoint = currentItem.GetAmount();
            targetCharacterBattleScript.SetMp(targetCharacterBattleScript.GetMp() + recoveryPoint);
            battleStatusScript.UpdateStatus(targetBaseStatus, BattleStatus.Status.MP, targetCharacterBattleScript.GetMp());
            battleManager.ShowMessage(currentTarget.name + "のMPを" + recoveryPoint + "回復した。");
        }
        else if (currentItem.GetItemType() == Item.Type.ParalyzeRecovery)
        {
            targetBaseStatus.SetParalyze(false);
            battleManager.ShowMessage(currentTarget.name + "の麻痺を消した。");
        }
        else if (currentItem.GetItemType() == Item.Type.PoisonRecovery)
        {
            targetBaseStatus.SetPoisonState(false);
            battleManager.ShowMessage(currentTarget.name + "の毒を消した。");
        }
        else if (currentItem.GetItemType() == Item.Type.SilentRecovery)
        {
            targetBaseStatus.SetSilence(false);
            battleManager.ShowMessage(currentTarget.name + "の沈黙を消した。");
        }

        //　アイテム数が0になったらItemDictionaryからそのアイテムを削除
        if (((AllyStatus)baseStatus).GetItemNum(currentItem) == 0)
        {
            ((AllyStatus)baseStatus).GetItemDictionary().Remove(currentItem);
        }
    }

    //　死んだときに実行する処理
    public void Dead()
    {
        battleManager.ShowMessage(gameObject.name + "は倒れた。");
        battleManager.DeleteAllCharacterInBattleList(this.gameObject);
        if (GetCharacterStatus() as AllyStatus != null)
        {
            battleStatusScript.UpdateStatus(GetCharacterStatus(), BattleStatus.Status.HP, GetHp());
            battleManager.DeleteAllyCharacterInBattleList(this.gameObject);
        }
        else if (GetCharacterStatus() as EnemyStatus != null)
        {
            battleManager.DeleteEnemyCharacterInBattleList(this.gameObject);
            //ここに倒された時にgameobjectをfalseにしたい
            battleManager.VanishEnemyObject(((EnemyStatus)GetCharacterStatus()).GetEnemyNumber());
        }
        isDead = true;
    }

    public void CheckIncreaseAttackPower()
    {
        //　自分のターンが来た時に何らかの効果魔法を使ってたらターン数を増やす
        if (IsIncreasePower())
        {
            numOfTurnsSinceIncreasePower++;
            if (numOfTurnsSinceIncreasePower >= numOfTurnsIncreasePower)
            {
                numOfTurnsSinceIncreasePower = 0;
                SetAuxiliaryPower(GetAuxiliaryPower() / increasePowerPoint);
                SetIsIncreasePower(false);
                battleManager.ShowMessage(gameObject.name + "の攻撃力アップの効果が消えた");
            }
        }
    }

    public void CheckIncreaseStrikingStrength()
    {
        if (IsIncreaseStrikingStrength())
        {
            numOfTurnsSinceIncreaseStrikingStrength++;
            if (numOfTurnsSinceIncreaseStrikingStrength >= numOfTurnsIncreaseStrikingStrength)
            {
                numOfTurnsSinceIncreaseStrikingStrength = 0;
                SetAuxiliaryStrikingStrength(GetAuxiliaryStrikingStrength() / increaseStrikingStrengthPoint);
                SetIsIncreaseStrikingStrength(false);
                battleManager.ShowMessage(gameObject.name + "の防御力アップの効果が消えた");
            }
        }
    }

    public BaseStatus GetCharacterStatus()
    {
        return baseStatus;
    }

    public void SetHp(int hp)
    {
        this.hp = Mathf.Max(0, Mathf.Min(baseStatus.GetMaxHp(), hp));

        if (this.hp <= 0)
        {
            Dead();
        }
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetMp(int mp)
    {
        this.mp = Mathf.Max(0, Mathf.Min(baseStatus.GetMaxMp(), mp));
    }

    public int GetMp()
    {
        return mp;
    }

    public int GetAuxiliaryAgility()
    {
        return auxiliaryAgility;
    }

    public int GetAuxiliaryPower()
    {
        return auxiliaryPower;
    }

    public int GetAuxiliaryStrikingStrength()
    {
        return auxiliaryStrikingStrength;
    }

    //　補正の素早さを設定
    public void SetAuxiliaryAgility(int value)
    {
        auxiliaryAgility = value;
    }

    //　補正の力を設定
    public void SetAuxiliaryPower(int value)
    {
        auxiliaryPower = value;
    }

    //　補正の打たれ強さを設定
    public void SetAuxiliaryStrikingStrength(int value)
    {
        auxiliaryStrikingStrength = value;
    }

    public bool IsNumbness()
    {
        return isNumbness;
    }

    public bool IsPoison()
    {
        return isPoison;
    }

    public bool IsSilence()
    {
        return isSilent;
    }

    public void SetNumbness(bool isNumbness)
    {
        this.isNumbness = isNumbness;
    }

    public void SetPoison(bool isPoison)
    {
        this.isPoison = isPoison;
    }

    public bool IsIncreasePower()
    {
        return isIncreasePower;
    }

    public void SetIsIncreasePower(bool isIncreasePower)
    {
        this.isIncreasePower = isIncreasePower;
    }

    public bool IsIncreaseStrikingStrength()
    {
        return isIncreaseStrikingStrength;
    }

    public void SetIsIncreaseStrikingStrength(bool isIncreaseStrikingStrength)
    {
        this.isIncreaseStrikingStrength = isIncreaseStrikingStrength;
    }

    public void SetBattleState(BattleState state)
    {
        this.battleState = state;
    }
}