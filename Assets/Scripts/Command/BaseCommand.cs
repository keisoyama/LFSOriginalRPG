
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
    private float runSpeed = 5f;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    //それぞれ対応しているボタンが押されているかどうか
    private bool right;
    private bool left;
    private bool up;
    private bool down;

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
                        GoLeft();
                    }
        }
    }

    public void TalikingButtonPushed()
    {
        if (talk.GetConversationPartner() != null)
        {
            SetState(State.Talk);
        }
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
        rigidbody2D.AddForce(new Vector3(0, runSpeed, 0));
    }

    void GoDown()
    {
        rigidbody2D.AddForce(new Vector3(0, -runSpeed, 0));
    }

    void GoRight()
    {
        rigidbody2D.AddForce(new Vector3(runSpeed, 0, 0));
    }

    void GoLeft()
    {
        rigidbody2D.AddForce(new Vector3(-runSpeed, 0, 0));
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