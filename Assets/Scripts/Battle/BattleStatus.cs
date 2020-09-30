using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatus : MonoBehaviour
{
    public enum Status
    {
        HP,
        MP,
    }
    //　パーティーステータス
    [SerializeField]
    private PartyStatus partyStatus = null;
    //　キャラクターステータス表示Transformリスト
    private Dictionary<BaseStatus, Transform> characterStatusDictionary = new Dictionary<BaseStatus, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //　最初の全データ表示
        DisplayStatus();
    }

    //　ステータスデータの表示
    public void DisplayStatus()
    {
        BaseStatus member;
        Transform characterPanelTransform;
        for (int i = 0; i < 4; i++)
        {
            characterPanelTransform = transform.Find("CharacterPanel" + i);
            if (i < partyStatus.GetAllyStatus().Count)
            {
                member = partyStatus.GetAllyStatus()[i];
                characterPanelTransform.Find("CharacterName").GetComponent<Text>().text = member.GetCharacterName();
                characterPanelTransform.Find("HPSlider").GetComponent<Slider>().value = (float)member.GetHp() / member.GetMaxHp();
                characterPanelTransform.Find("HPSlider/HPText").GetComponent<Text>().text = member.GetHp().ToString();
                characterPanelTransform.Find("MPSlider").GetComponent<Slider>().value = (float)member.GetMp() / member.GetMaxMp();
                characterPanelTransform.Find("MPSlider/MPText").GetComponent<Text>().text = member.GetMp().ToString();
                characterStatusDictionary.Add(member, characterPanelTransform);
            }
            else
            {
                characterPanelTransform.gameObject.SetActive(false);
            }
        }
    }

    //　データの更新
    public void UpdateStatus(BaseStatus characterStatus, Status status, int destinationValue)
    {
        if (status == Status.HP)
        {
            characterStatusDictionary[characterStatus].Find("HPSlider").GetComponent<Slider>().value = (float)destinationValue / characterStatus.GetMaxHp();
            characterStatusDictionary[characterStatus].Find("HPSlider/HPText").GetComponent<Text>().text = destinationValue.ToString();
        }
        else if (status == Status.MP)
        {
            characterStatusDictionary[characterStatus].Find("MPSlider").GetComponent<Slider>().value = (float)destinationValue / characterStatus.GetMaxMp();
            characterStatusDictionary[characterStatus].Find("MPSlider/MPText").GetComponent<Text>().text = destinationValue.ToString();
        }
    }
}
