
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
    private float runSpeed ;
    [SerializeField]
    private Vector2 lastMove;

    private Animator animator;

    private bool isMoving;

    [SerializeField]
    private Rigidbody2D rigidbody2D;

    //それぞれ対応しているボタンが押されているかどうか
    private bool right;
    private bool left;
    private bool up;
    private bool down;

    //水平か垂直移動の判別
    private bool isHorizontalMove;
    private bool isVerticalMove;

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
        state = State.Normal;
        talk = GetComponent<Talk>();
    }

    private void Update()
    {
        if(state == State.Normal)
        {
            Animate();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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
        rigidbody2D.velocity = new Vector3(0, runSpeed, 0);
    }

    void GoDown()
    {
        rigidbody2D.velocity = new Vector3(0, -runSpeed, 0);
    }

    void GoRight()
    {
        rigidbody2D.velocity = new Vector3(runSpeed, 0, 0);
    }

    void GoLeft()
    {
        rigidbody2D.velocity = new Vector3(-runSpeed, 0, 0);
    }

    public void UpPushChange()
    {
        up = !up;
        isMoving = !isMoving;
        if (isMoving)
        {
            isVerticalMove = !isVerticalMove;
            animator.SetBool("isMoving", true);
            SetStateToAnimatorStartWalking();
        }
        else
        {
            isVerticalMove = !isVerticalMove;
            animator.SetBool("isMoving", false);
            Stop();
            SetStateToAnimatorStopWalking();
        }
    }

    public void DownPushChange()
    {
        down = !down;
        isMoving = !isMoving;
        if (isMoving)
        {
            isVerticalMove = !isVerticalMove;
            animator.SetBool("isMoving", true);
            SetStateToAnimatorStartWalking();
        }
        else
        {
            isVerticalMove = !isVerticalMove;
            animator.SetBool("isMoving", false);
            Stop();
            SetStateToAnimatorStopWalking();
        }
    }


    public void RightPushChange()
    {
        isHorizontalMove = !isHorizontalMove;
        right = !right;
        isMoving = !isMoving;
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
            SetStateToAnimatorStartWalking();
        }
        else
        {
            animator.SetBool("isMoving", false);
            Stop();
            SetStateToAnimatorStopWalking();
        }
    }

    public void LeftPushChange()
    {
        isHorizontalMove = !isHorizontalMove;
        left = !left;
        isMoving = !isMoving;
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
            SetStateToAnimatorStartWalking();
        }
        else
        {
            animator.SetBool("isMoving", false);
            Stop();
            SetStateToAnimatorStopWalking();
        }
    }

    void Stop()
    {
        rigidbody2D.velocity = Vector3.zero;
    }


    private void SetStateToAnimatorStartWalking()
    {
　　　　　if (up)
        {
            //animator.SetBool("isMovingUp",true);
        }
        else if (down)
        {
            //animator.SetBool("isMovingDown", true);
        }
        else if (right)
        {
            //animator.SetBool("isMovingRight", true);
        }
        else if (left)
        {
            //animator.SetBool("isMovingLeft", true);
        }
    }

    private void SetStateToAnimatorStopWalking()
    {
        //this.animator.speed = 0.0f;

        if (!up)
        {
            //animator.SetBool("isMovingUp", false);
        }
        else if (!down)
        {
            //animator.SetBool("isMovingDown", false);
        }
        else if (!right)
        {
            //animator.SetBool("isMovingRight", false);
        }
        else if (!left)
        {
           // animator.SetBool("isMovingLeft", false);
        }
    }

    public void Animate()
    {
        if (isMoving)
        {
            if (isHorizontalMove)
            {
                lastMove.x = rigidbody2D.velocity.x;
                lastMove.y = 0;
            }
            if (isVerticalMove)
            {
                lastMove.y = rigidbody2D.velocity.y;
                lastMove.x = 0;
            }
            animator.SetFloat("LastMove_X", lastMove.x);
            animator.SetFloat("LastMove_Y", lastMove.y);
        }
        else
        {
            animator.SetFloat("LastMove_X", 0);
            animator.SetFloat("LastMove_Y", 0);
        }
    }
    }