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

        //hpBar = Util.FindChild<HpBar>(gameObject, null, true);    //gameObject �ڽĵ� �� HpBar(������Ʈ)�� ����ִ� �ڽ� ����� HpBar(������Ʈ) ��ȯ


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
        if (Input.GetKeyDown(KeyCode.W) ){ Debug.Log("W����"); } 
        if (Input.GetKeyDown(KeyCode.A)) { Debug.Log("A����"); }
        if (Input.GetKeyDown(KeyCode.S) ){ Debug.Log("S����"); }
        if (Input.GetKeyDown(KeyCode.D)) { Debug.Log("D����"); }
        
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
                Debug.Log("���Ϳ� �浹");
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

            //List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //����: ��ȣ�� ���� RoundField(��ũ��Ʈ)�����ϰ� ���� List�� ����
            //Debug.Log(gameObject.name);
            //Debug.Log(gameObject.transform.parent.name);

            //transform.parent.transform.position = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.localPosition;

            //Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.localPosition;
            //transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, movePoint, Time.deltaTime * speed);
            //currentGridInd[0] = moveGridInd[0];
            //currentGridInd[1] = moveGridInd[1];

        //}
        //catch (ArgumentOutOfRangeException)  // �α׸� ���, �������� ������ return
        //{
        //    return;
        //}
    }

    public int[] GetPlayerInd()
    {
        return currentGridInd;
    }
    
    /*private void OnTriggerEnter2D(Collider2D collision)                 //Player�� �������� ���ؼ� ���Ͱ� Ȱ��ȭ�� grid�� ���� ���
    {
        Hit();
    }


    protected void Hit()
    {
        Debug.Log("MyPlayer Hit!!");
        GetComponent<Animator>().SetTrigger("MyPlayerHit");       //Player�� Animator�� Idle ���� Hit������ transition�� ���µ�???
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
