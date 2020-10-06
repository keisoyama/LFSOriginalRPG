using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScope : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player"
            && col.GetComponent<BaseCommand>().GetState() != BaseCommand.State.Talk
            )
        {
            //　ユニティちゃんが近づいたら会話相手として自分のゲームオブジェクトを渡す
            col.GetComponent<Talk>().SetConversationPartner(transform.parent.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player"
            && col.GetComponent<BaseCommand>().GetState() != BaseCommand.State.Talk
            )
        {
            //　ユニティちゃんが遠ざかったら会話相手から外す
            col.GetComponent<Talk>().ResetConversationPartner(transform.parent.gameObject);
        }
    }
}