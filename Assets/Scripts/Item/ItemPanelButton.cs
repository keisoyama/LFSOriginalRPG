using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class ItemPanelButton : MonoBehaviour, ISelectHandler
{

    private Item item;
    //　アイテムタイトル表示テキスト
    private Text itemTitleText;
    //　アイテム情報表示テキスト
    private Text itemInformationText;

    private void Awake()
    {
        itemTitleText = transform.root.Find("ItemInformationPanel/Title").GetComponent<Text>();
        itemInformationText = transform.root.Find("ItemInformationPanel/Information").GetComponent<Text>();
    }

    //　ボタンが選択された時に実行
    public void OnSelect(BaseEventData eventData)
    {
        ShowItemInformation();
    }

    //　アイテム情報の表示
    public void ShowItemInformation()
    {
        itemTitleText.text = item.GetKanjiName();
        itemInformationText.text = item.GetInformation();
    }
    //　データをセットする
    public void SetParam(Item item)
    {
        this.item = item;
    }
}
