using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVer2 : FieldObject
{
    //idle ���� (1.25)
    Define.State nextBehavior = Define.State.MOVE;                  //���� ����
    //to do : MonsterMove or MoveDirection
    Define.PlayerMove nextDirection = Define.PlayerMove.Right;      //���� ������ ����
    MonsterPattern attackPattern = new LinePattern();               //������ ���� ����
    int maxHp = 1;
    int currentHp;
    
    private void Start()
    {
        currentHp = maxHp;

        type = 2;                                       //���� Ÿ��: 2
        objectField = Managers.Field.getField();        //BasicField(��ũ��Ʈ) ��ȯ
        objectList = objectField.getGridArray(type);    //monstergridArray(���� ������ grid) ��ȯ

        currentInd = objectList.Count / 2 - 1;          //���� �ʱ� ��ġ: monstergridArray �� �ε��� 2
        transform.position = objectList[currentInd].transform.position;
        
        Managers.Timing.BehaveAction -= BitBehave;      //������ ��Ʈ ���� ������ BitBehave ����
        Managers.Timing.BehaveAction += BitBehave;  
    }

    
    protected override void BitBehave()
    {
        Animator anim = GetComponent<Animator>();
        
        //���� currentInd�� �ٽ� ���� ������ ��������
        SpriteRenderer temp = objectList[currentInd].GetComponent<SpriteRenderer>();
        temp.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);        //Isolated Diamond(prefab)�� ��
        
        switch (nextBehavior)
        {
            // idle ����
            /*
            case Define.State.IDLE:
                anim.Play("Idle");
                updateIdle();
                break;*/
            case Define.State.ATTACKREADY:
                
                anim.Play("AttackReady");
                updateAtttackReady();       //AtttackReady �ܰ迡 �´� ��ȭ�� ��Ÿ������ ��

                temp = objectList[currentInd].GetComponent<SpriteRenderer>();
                temp.color = Color.magenta;
                
                break;
            case Define.State.ATTACK:
                anim.Play("Attack");
                updateAttack();

                temp = objectList[currentInd].GetComponent<SpriteRenderer>();
                temp.color = Color.magenta;
                
                break;
            case Define.State.MOVE:
                anim.Play("Move");
                updateMove();

                temp = objectList[currentInd].GetComponent<SpriteRenderer>();
                temp.color = Color.magenta;

                break;

        }
    }

    void ChaseCheck()       //���� ���� ���°� �������� �ϴ� Define.State.MOVE ��� ������ ���� ���� ��ȯ
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
        ChaseCheck();               //������ ���� ���� ����     
        mayGo(nextDirection);       //���� ������
        nextBehavior = Define.State.ATTACKREADY;
    }
    void updateAtttackReady()
    {
        AttackReady();              //������ ���� ����ȭ
        nextBehavior = Define.State.ATTACK;
    }
    void updateAttack()
    {
        Attack();                   //Damage���� collider Ȱ��ȭ + ����ȭ
        nextBehavior = Define.State.MOVE;
        //nextBehavior = Define.State.IDLE;
        //idle ����(1.25)
    }
    protected override void Attack()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //���Ͱ� ������ grid�� �ε����� pattern�� ��ȯ
        Managers.Field.Attack(pattern);                                 //Damage���� collider Ȱ��ȭ + ����ȭ
    }
    void AttackReady()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //���Ͱ� ������ grid�� �ε����� pattern�� ��ȯ
        Managers.Field.WarningAttack(pattern);                          //�ش� ���� ����ȭ
    }


    private void OnTriggerEnter2D(Collider2D collision)                 //Player�� �������� ���ؼ� ���Ͱ� Ȱ��ȭ�� grid�� ���� ���
    {
        currentHp -= 1;                                                 //���ʹ� �ѹ� ������ ����
        GetComponent<Animator>().Play("Hit");

        Debug.Log("Monster Hit");
        if (currentHp <= 0)
            Die();
    }
    
    void Die()
    {
        Debug.Log("MonsterDIe!");
        Managers.Game.MinusMonsterNum();
        Managers.Timing.BehaveAction -= BitBehave;                           //���� monsterIndex ��° ������ ��Ʈ ���� ������ BitBehave ���� ����
        GameScene gamescene = (GameScene)Managers.Scene.CurrentScene;        //GameScene(��ũ��Ʈ) ��ȯ
        gamescene.NextMonsterIndex();                                        //���� ���� ����
        Destroy(gameObject);
        Managers.Sound.Play("Die", Define.Sound.Effect, 2.0f);
    }
}
