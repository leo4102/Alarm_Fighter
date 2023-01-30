using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVer2 : FieldObject
{
    //idle 보류 (1.25)
    Define.State nextBehavior = Define.State.MOVE;
    //to do : MonsterMove or MoveDirection
    Define.PlayerMove nextDirection = Define.PlayerMove.Right;
    MonsterPattern attackPattern = new LinePattern();
    int maxHp = 3;
    int currentHp;

    HpBar hpBar;

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            hpBar.updateValue(currentHp);
        }
    }
    private void Start()
    {

        hpBar = Util.FindChild<HpBar>(gameObject, null, true);
        CurrentHp = maxHp;

        type = 2;
        objectField = Managers.Field.getField();
        objectList = objectField.getGridArray(type);

        currentInd = objectList.Count / 2 - 1;
        transform.position = objectList[currentInd].transform.position;

        Managers.Timing.BehaveAction -= BitBehave;
        Managers.Timing.BehaveAction += BitBehave;
    }

    protected override void BitBehave()
    {
        Animator anim = GetComponent<Animator>();
        switch(nextBehavior)
        {
            // idle 보류
            /*
            case Define.State.IDLE:
                anim.Play("Idle");
                updateIdle();
                break;*/
            case Define.State.ATTACKREADY:
                anim.Play("AttackReady");
                updateAtttackReady();
                break;
            case Define.State.ATTACK:
                anim.Play("Attack");
                updateAttack();
                break;
            case Define.State.MOVE:
                anim.Play("Move");
                updateMove();
                break;

        }
    }

    void ChaseCheck()
    {
        //to do : left or right or Stop Check
        int rand = Random.Range(0, 2);
        switch(rand)
        {
            case 0:
                nextDirection = Define.PlayerMove.Right;
                break;
            case 1:
                nextDirection = Define.PlayerMove.Left;
                break;
            case 2:
                nextDirection = Define.PlayerMove.Stop;
                break;
        }    
    }


    void updateIdle()
    {
        nextBehavior = Define.State.MOVE;
    }
    void updateMove()
    {
        ChaseCheck();
        mayGo(nextDirection);
        nextBehavior = Define.State.ATTACKREADY;
    }
    void updateAtttackReady()
    {
        AttackReady();
        nextBehavior = Define.State.ATTACK;
    }
    void updateAttack()
    {
        Attack();
        nextBehavior = Define.State.MOVE;
        //nextBehavior = Define.State.IDLE;
        //idle 보류(1.25)
    }
    protected override void Attack()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);
        Managers.Field.Attack(pattern);
    }
    void AttackReady()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);
        Managers.Field.WarningAttack(pattern);

    }

    protected override void Hit()
    {
        CurrentHp -= 1;
        GetComponent<Animator>().SetTrigger("isHit");

        Debug.Log("Monster Hit");
        if (CurrentHp <= 0)
            Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit();
    }
    void Die()
    {
        Debug.Log("MonsterDIe!");
    }
}
