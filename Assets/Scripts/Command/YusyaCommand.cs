using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YusyaCommand : MonoBehaviour
{
    private LoadSceneManager sceneManager;
    //　コマンド用UI
    [SerializeField]
    private GameObject commandUI = null;
    private BaseCommand baseCommand;
    bool isMenuButtonPushed;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<LoadSceneManager>();
        baseCommand = GetComponent<BaseCommand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsTransition()
            || baseCommand.GetState() == BaseCommand.State.Talk
            )
        {
            return;
        }
        //　コマンドUIの表示・非表示の切り替え
        if (isMenuButtonPushed)
        {
            isMenuButtonPushed = false;
            //　コマンド
            if (!commandUI.activeSelf)
            {
                //　ユニティちゃんをコマンド状態にする
                baseCommand.SetState(BaseCommand.State.Command);
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
        baseCommand.SetState(BaseCommand.State.Normal);
    }

    public void MenuButtonPushed()
    {
        isMenuButtonPushed = true;
    }
}