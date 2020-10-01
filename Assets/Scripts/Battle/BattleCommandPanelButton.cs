using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleCommandPanelButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //　ボタンを選択した時に表示する画像
    private Image selectedImage;
    //　選択した時の音のAudioSource
    private AudioSource audioSource;

    void Awake()
    {
        selectedImage = transform.Find("Image").GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //　アクティブになった時自身がEventSystemで選択されていたら
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            selectedImage.enabled = true;
            audioSource.Play();
        }
        else
        {
            selectedImage.enabled = false;
        }
    }

    //　ボタンが選択された時に実行
    public void OnSelect(BaseEventData eventData)
    {
        selectedImage.enabled = true;
        audioSource.Play();
    }
    //　ボタンが選択解除された時に実行
    public void OnDeselect(BaseEventData eventData)
    {
        selectedImage.enabled = false;
    }
}