
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

    private Animator animator;

    private bool isMoving;

    private Dictionary<string, float> Movingforward;

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
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        Dictionary<string, float> Movingforward = new Dictionary<string, float>()
        {
            {"x",0 },
            {"y",0 }
        };
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

            if (!isMoving)
            {
                this.animator.speed = 0.0f;
                return;
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

    public void UpPushChange()
    {
        up = !up;
        isMoving = !isMoving;
        if (isMoving)
        {
            SetStateToAnimatorStartWalking();
        }
        else
        {
            SetStateToAnimatorStopWalking();
        }

    }

    public void DownPushChange()
    {
        down = !down;
        isMoving = !isMoving;
        if (isMoving)
        {
            SetStateToAnimatorStartWalking();
        }
        else
        {
            SetStateToAnimatorStopWalking();
        }
    }


    public void RightPushChange()
    {
        right = !right;
        isMoving = !isMoving;
        if (isMoving)
        {
            SetStateToAnimatorStartWalking();
        }
        else
        {
            SetStateToAnimatorStopWalking();
        }
    }

    public void LeftPushChange()
    {
        left = !left;
        isMoving = !isMoving;
        if (isMoving)
        {
            SetStateToAnimatorStartWalking();
        }
        else
        {
            SetStateToAnimatorStopWalking();
        }
    }


    private void SetStateToAnimatorStartWalking()
    {
　　　　　if (up)
        {
            animator.SetBool("isMovingUp",true);
        }
        else if (down)
        {
            animator.SetBool("isMovingDown", true);
        }
        else if (right)
        {
            animator.SetBool("isMovingRight", true);
        }
        else if (left)
        {
            animator.SetBool("isMovingLeft", true);
        }
    }

    private void SetStateToAnimatorStopWalking()
    {

        if (up)
        {
            animator.SetBool("isMovingUp", false);
        }
        else if (down)
        {
            animator.SetBool("isMovingDown", false);
        }
        else if (right)
        {
            animator.SetBool("isMovingRight", false);
        }
        else if (left)
        {
            animator.SetBool("isMovingLeft", false);
        }
    }


}