using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMessagePanel : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}