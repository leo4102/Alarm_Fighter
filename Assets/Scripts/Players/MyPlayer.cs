using System;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    /*int maxHp = 100;
    int currentHp;

    //HpBar hpBar;

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            //hpBar.updateValue(currentHp);
        }
    }*/
    //============================================
    int[] currentGridInd = null;
    int[] moveGridInd = null;

    public float speed;
    void Start()
    {
        //currentHp = maxHp;    //---------------------------------------

        //hpBar = Util.FindChild<HpBar>(gameObject, null, true);    //gameObject 자식들 중 HpBar(컴포넌트)를 들고있는 자식 존재시 HpBar(컴포넌트) 반환


        currentGridInd = new int[2];
        moveGridInd = new int[2];
        speed = 10f;

        currentGridInd[0] = Managers.Field.GetWidth() / 2;
        currentGridInd[1] = Managers.Field.GetHeight() / 2;

        moveGridInd[0] = Managers.Field.GetWidth() / 2;
        moveGridInd[1] = Managers.Field.GetHeight() / 2;

        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();
        transform.position = gridArray[currentGridInd[0]][currentGridInd[1]].grid.transform.position;

    }


    void Update()
    {
        Debug.Log("MyPlayer Update");
        if (Input.GetKeyDown(KeyCode.W) ){ Debug.Log("W눌림"); } 
        if (Input.GetKeyDown(KeyCode.A)) { Debug.Log("A눌림"); }
        if (Input.GetKeyDown(KeyCode.S) ){ Debug.Log("S눌림"); }
        if (Input.GetKeyDown(KeyCode.D)) { Debug.Log("D눌림"); }
        
        if (Input.GetKeyDown(KeyCode.W) && Managers.Timing.CheckTiming()) { mayGo(Define.PlayerMove.Up); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.A) && Managers.Timing.CheckTiming()) { mayGo(Define.PlayerMove.Left); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.S) && Managers.Timing.CheckTiming()) { mayGo(Define.PlayerMove.Down); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.D) && Managers.Timing.CheckTiming()) { mayGo(Define.PlayerMove.Right); Managers.Sound.Play("Click"); }
        //else if (Input.GetKeyDown(KeyCode.K) && Managers.Timing.CheckTiming()) { Attack(); Managers.Sound.Play("KnifeAttack1"); }

        /*foreach (GameObject rMon in Managers.Game.CurrentRMons)
        {
            int[] currentInd=rMon.GetComponent<RMon1>().GetCurrentInd();
            if (moveGridInd[0] == currentInd[0] && moveGridInd[1] == currentInd[1])
            {
                Debug.Log("몬스터와 충돌");
            }
        }*/
        try
        {
            List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();
            Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, Time.deltaTime * speed);

            currentGridInd[0] = moveGridInd[0];
            currentGridInd[1] = moveGridInd[1];

        }
        catch (ArgumentOutOfRangeException) 
        {
            moveGridInd[0] = currentGridInd[0];
            moveGridInd[1] = currentGridInd[1];
        }
    }

    protected void mayGo(Define.PlayerMove direction)
    {
        //try
        //{

        Debug.Log("MyPlayer mayGo Activated");
        
        if (direction == Define.PlayerMove.Up)
            {
                moveGridInd[0] = currentGridInd[0];
                moveGridInd[1] = currentGridInd[1] - 1;
            }
            else if (direction == Define.PlayerMove.Down)
            {
                moveGridInd[0] = currentGridInd[0];
                moveGridInd[1] = currentGridInd[1] + 1;
            }
            else if (direction == Define.PlayerMove.Left)
            {
                moveGridInd[0] = currentGridInd[0] - 1;
                moveGridInd[1] = currentGridInd[1];
            }
            else if (direction == Define.PlayerMove.Right)
            {
                moveGridInd[0] = currentGridInd[0] + 1;
                moveGridInd[1] = currentGridInd[1];
            }

            //List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //우측: 선호가 만든 RoundField(스크립트)접근하고 이중 List에 접근
            //Debug.Log(gameObject.name);
            //Debug.Log(gameObject.transform.parent.name);

            //transform.parent.transform.position = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.localPosition;

            //Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.localPosition;
            //transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, movePoint, Time.deltaTime * speed);
            //currentGridInd[0] = moveGridInd[0];
            //currentGridInd[1] = moveGridInd[1];

        //}
        //catch (ArgumentOutOfRangeException)  // 로그를 찍고, 움직이지 않으며 return
        //{
        //    return;
        //}
    }

    public int[] GetPlayerInd()
    {
        return currentGridInd;
    }
    
    /*private void OnTriggerEnter2D(Collider2D collision)                 //Player의 공격으로 인해서 몬스터가 활성화된 grid에 닿을 경우
    {
        Hit();
    }


    protected void Hit()
    {
        Debug.Log("MyPlayer Hit!!");
        GetComponent<Animator>().SetTrigger("MyPlayerHit");       //Player의 Animator의 Idle 에서 Hit으로의 transition이 없는데???
        CurrentHp -= 1;
        Managers.Sound.Play("Hit");
        if (CurrentHp <= 0)
            Die();

    }
    void Die()
    {
        Debug.Log("Player Die!!");
        Destroy(gameObject);
        
        //Managers.Game.GameOver();
        Managers.Sound.Play("Die");

    }*/
 
}
