
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCommand : MonoBehaviour
{
    public enum State
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    //　コマンド用UI
    [SerializeField]
    private GameObject commandUI = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //　コマンドUIの表示・非表示の切り替え
        if (Input.GetKeyDown(KeyCode.A))
        {
            //　コマンド
            if (!commandUI.activeSelf)
            {
                //　勇者をコマンド状態にする
            }
            else
            {
                ExitCommand();
            }
            //　コマンドUIのオン・オフ
            commandUI.SetActive(!commandUI.activeSelf);
        }
    }
    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}