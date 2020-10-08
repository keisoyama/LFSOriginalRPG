using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BaseVillager : MonoBehaviour
{

    public enum State
    {
        Wait,
        Walk,
        Talk
    }
    
    State state;

    //　会話内容保持スクリプト
    [SerializeField]
    private Conversation conversation = null;
    //　ユニティちゃんのTransform
    private Transform conversationPartnerTransform;

    //　村人の状態変更
    public void SetState(State state, Transform conversationPartnerTransform = null)
    {
        this.state = state;
    }

    //　Conversationスクリプトを返す
    public Conversation GetConversation()
    {
        return conversation;
    }

}
