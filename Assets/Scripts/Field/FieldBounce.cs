using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBounce : MonoBehaviour
{
    void Start()
    {
        Managers.Timing.BehaveAction -= Bounce;      
        Managers.Timing.BehaveAction += Bounce;
        
    }

    public void Bounce()                         //매 비트마다 타일이 바운스
    {
       //GetComponent<Animator>().Play("Bounce");     //Bounce(AnimationClip) 재생
       GetComponent<Animator>().SetTrigger("Bounce");
        Debug.Log("Bounced");
    }
    
    
    
}
