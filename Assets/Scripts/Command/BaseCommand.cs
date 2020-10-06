﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCommand : MonoBehaviour
{
    //　キャラクターの速度
    private Vector3 velocity;
    //　キャラクターの走るスピード
    [SerializeField]
    private float runSpeed = 4f;

    bool right;
    bool left;
    bool up;
    bool down;

    public enum State
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    //　ユニティちゃんの状態
    private State state;
    //　ユニティちゃん会話処理スクリプト
    private Talk talk;


    //　コマンド用UI
    [SerializeField]
    private GameObject commandUI = null;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;
        talk = GetComponent<Talk>();
    }


    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal)
        {
                    if (up)
                    {
                        GoUp();
                    }
                    else if (down)
                    {
                        GoDown();
                    }
                    else if (right)
                    {
                        GoRight();
                    }
                    else if (left)
                    {
                        GoRight();
                    }
        }
        else if (state == State.Talk)
        {
            
        }

        

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

    public void TalikingButtonPushed()
    {
        if (talk.GetConversationPartner() != null)
        {
            SetState(State.Talk);
        }
    }

    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    //　状態変更と初期設定
    public void SetState(State state)
    {
        this.state = state;

        if (state == State.Talk)
        {
            talk.StartTalking();
        }
    }

    public State GetState()
    {
        return state;
    }

    void GoUp()
    {
        transform.position += new Vector3(0, 0, runSpeed);
    }

    void GoDown()
    {
        transform.position += new Vector3(0, 0, -runSpeed);
    }

    void GoRight()
    {
        transform.position += new Vector3(runSpeed, 0, 0);
    }

    void GoLeft()
    {
        transform.position += new Vector3(-runSpeed, 0, 0);
    }

    public void RightPushChange()
    {
        //      右ボタンを押している間
        right = !right;
    }

    public void LeftPushChange()
    {
        //      右ボタンを押している間
        left = !left;
    }

    public void UpPushChange()
    {
        //      右ボタンを押している間
        up = !up;
    }

    public void DownPushChange()
    {
        //      右ボタンを押している間
        down = !down;
    }
}