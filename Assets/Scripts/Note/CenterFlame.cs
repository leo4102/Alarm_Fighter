using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    //AudioSource myAudio;
    bool isPlaying = false;

    void Start()
    {
        //myAudio = GetComponent<AudioSource>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlaying)
        {
            if (collision.CompareTag("Note2"))
            {
                //myAudio.Play();
                //AudioManager.audioManager.PlayBGM("BGM0");//하드 코딩
                //isPlaying = true;
            }

        }
    }
       
        
    
}
