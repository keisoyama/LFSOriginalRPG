using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    //　戦闘終了後のBGM
    [SerializeField]
    private AudioClip BGMAfterTheBattle;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = BGMAfterTheBattle;
        audioSource.Play();
    }
}