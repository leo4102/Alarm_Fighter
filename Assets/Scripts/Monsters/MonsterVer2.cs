using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVer2 : FieldObject
{
    //idle º¸·ù (1.25)
    Define.State nextBehavior = Define.State.MOVE;                  //´ÙÀ½ »óÅÂ
    //to do : MonsterMove or MoveDirection
    Define.PlayerMove nextDirection = Define.PlayerMove.Right;      //´ÙÀ½ ¿òÁ÷ÀÓ ¹æÇâ
    MonsterPattern attackPattern = new LinePattern();               //¸ó½ºÅÍÀÇ °ø°Ý ÆÐÅÏ
    int maxHp = 1;
    int currentHp;
    
    private void Start()
    {
        currentHp = maxHp;

        type = 2;                                       //¸ó½ºÅÍ Å¸ÀÔ: 2
        objectField = Managers.Field.getField();        //BasicField(½ºÅ©¸³Æ®) ¹ÝÈ¯
        objectList = objectField.getGridArray(type);    //monstergridArray(¸ó½ºÅÍ ¿µ¿ªÀÇ grid) ¹ÝÈ¯

        currentInd = objectList.Count / 2 - 1;          //¸ó½ºÅÍ ÃÊ±â À§Ä¡: monstergridArray ÀÇ ÀÎµ¦½º 2
        transform.position = objectList[currentInd].transform.position;
        
        Managers.Timing.BehaveAction -= BitBehave;      //¸ó½ºÅÍÀÇ ºñÆ® ¸¶´Ù ½ÇÇàÇÒ BitBehave ±¸µ¶
        Managers.Timing.BehaveAction += BitBehave;
    }

    protected override void BitBehave()
    {
        Animator anim = GetComponent<Animator>();
        
        //���� currentInd�� �ٽ� �� ����� ������
        SpriteRenderer temp = objectList[currentInd].GetComponent<SpriteRenderer>();
        temp.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);        //Isolated Diamond(prefab)�� ��
        
        switch (nextBehavior)
        {
            // idle º¸·ù
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

    void ChaseCheck()       //¸¸¾à ÇöÀç »óÅÂ°¡ ¿òÁ÷¿©¾ß ÇÏ´Â Define.State.MOVE ¶ó¸é ¿òÁ÷ÀÏ ·£´ý ¹æÇâ ¹ÝÈ¯
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
        ChaseCheck();               //¿òÁ÷ÀÏ ·£´ý ¹æÇâ °áÁ¤     
        mayGo(nextDirection);       //½ÇÁ¦ ¿òÁ÷ÀÓ
        nextBehavior = Define.State.ATTACKREADY;
    }
    void updateAtttackReady()
    {
        AttackReady();              //°ø°ÝÇÒ ¿µ¿ª »¡°­È­
        nextBehavior = Define.State.ATTACK;
    }
    void updateAttack()
    {
        Attack();                   //Damage¿µ¿ª collider È°¼ºÈ­ + Åõ¸íÈ­
        nextBehavior = Define.State.MOVE;
        //nextBehavior = Define.State.IDLE;
        //idle º¸·ù(1.25)
    }
    protected override void Attack()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //¸ó½ºÅÍ°¡ °ø°ÝÇÒ gridÀÇ ÀÎµ¦½º¸¦ pattern¿¡ ¹ÝÈ¯
        Managers.Field.Attack(pattern);                                 //Damage¿µ¿ª collider È°¼ºÈ­ + Åõ¸íÈ­
    }
    void AttackReady()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //¸ó½ºÅÍ°¡ °ø°ÝÇÒ gridÀÇ ÀÎµ¦½º¸¦ pattern¿¡ ¹ÝÈ¯
        Managers.Field.WarningAttack(pattern);                          //ÇØ´ç ¿µ¿ª »¡°­È­
    }


    private void OnTriggerEnter2D(Collider2D collision)                 //PlayerÀÇ °ø°ÝÀ¸·Î ÀÎÇØ¼­ ¸ó½ºÅÍ°¡ È°¼ºÈ­µÈ grid¿¡ ´êÀ» °æ¿ì
    {
        currentHp -= 1;                                                 //¸ó½ºÅÍ´Â ÇÑ¹æ ¸ÂÀ¸¸é µÚÁü
        GetComponent<Animator>().Play("Hit");

        Debug.Log("Monster Hit");
        if (currentHp <= 0)
            Die();
    }
    
    void Die()
    {
        Debug.Log("MonsterDIe!");
        Managers.Game.MinusMonsterNum();
        Managers.Timing.BehaveAction -= BitBehave;                           //ÇöÀç monsterIndex ¹øÂ° ¸ó½ºÅÍÀÇ ºñÆ® ¸¶´Ù ½ÇÇàÇÒ BitBehave ±¸µ¶ ÇØÁ¦
        GameScene gamescene = (GameScene)Managers.Scene.CurrentScene;        //GameScene(½ºÅ©¸³Æ®) ¹ÝÈ¯
        gamescene.NextMonsterIndex();                                        //´ÙÀ½ ¸ó½ºÅÍ »ý¼º
        Destroy(gameObject);
        Managers.Sound.Play("Die", Define.Sound.Effect, 2.0f);

    }
}
