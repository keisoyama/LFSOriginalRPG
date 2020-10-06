using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Conversation", menuName = "CreateConversation")]
public class Conversation : ScriptableObject
{
    //　会話内容
    [SerializeField]
    [Multiline(100)]
    private string message = null;

    //　会話内容を返す
    public string GetConversationMessage()
    {
        return message;
    }
}