using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour       //초기 비활성화 되어있는 "Attack"(GameObject)에 붙어있음
{
    
    public void Attacking()
    {
        gameObject.SetActive(true);     //Attack(GameObject) 활성화
        Animator anim = GetComponent<Animator>();
        anim.Play("Attack");

    }

    void Update()
    {
        
    }
    public void End()
    {
        gameObject.SetActive(false);
    }
}
