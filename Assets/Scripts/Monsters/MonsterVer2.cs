using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVer2 : FieldObject
{
    //idle 보류 (1.25)
    Define.State nextBehavior = Define.State.MOVE;                  //다음 상태
    //to do : MonsterMove or MoveDirection
    Define.PlayerMove nextDirection = Define.PlayerMove.Right;      //다음 움직임 방향
    MonsterPattern attackPattern = new LinePattern();               //몬스터의 공격 패턴
    int maxHp = 1;
    int currentHp;
    
    private void Start()
    {
        currentHp = maxHp;

        type = 2;                                       //몬스터 타입: 2
        objectField = Managers.Field.getField();        //BasicField(스크립트) 반환
        objectList = objectField.getGridArray(type);    //monstergridArray(몬스터 영역의 grid) 반환

        currentInd = objectList.Count / 2 - 1;          //몬스터 초기 위치: monstergridArray 의 인덱스 2
        transform.position = objectList[currentInd].transform.position;
        
        Managers.Timing.BehaveAction -= BitBehave;      //몬스터의 비트 마다 실행할 BitBehave 구독
        Managers.Timing.BehaveAction += BitBehave;  
    }

    
    protected override void BitBehave()
    {
        Animator anim = GetComponent<Animator>();
        
        //이전 currentInd를 다시 원래 색으로 돌려놓기
        SpriteRenderer temp = objectList[currentInd].GetComponent<SpriteRenderer>();
        temp.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);        //Isolated Diamond(prefab)의 색
        
        switch (nextBehavior)
        {
            // idle 보류
            /*
            case Define.State.IDLE:
                anim.Play("Idle");
                updateIdle();
                break;*/
            case Define.State.ATTACKREADY:
                
                anim.Play("AttackReady");
                updateAtttackReady();       //AtttackReady 단계에 맞는 변화가 나타나도록 함

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

    void ChaseCheck()       //만약 현재 상태가 움직여야 하는 Define.State.MOVE 라면 움직일 랜덤 방향 반환
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
        ChaseCheck();               //움직일 랜덤 방향 결정     
        mayGo(nextDirection);       //실제 움직임
        nextBehavior = Define.State.ATTACKREADY;
    }
    void updateAtttackReady()
    {
        AttackReady();              //공격할 영역 빨강화
        nextBehavior = Define.State.ATTACK;
    }
    void updateAttack()
    {
        Attack();                   //Damage영역 collider 활성화 + 투명화
        nextBehavior = Define.State.MOVE;
        //nextBehavior = Define.State.IDLE;
        //idle 보류(1.25)
    }
    protected override void Attack()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //몬스터가 공격할 grid의 인덱스를 pattern에 반환
        Managers.Field.Attack(pattern);                                 //Damage영역 collider 활성화 + 투명화
    }
    void AttackReady()
    {
        int[] pattern = attackPattern.calculateIndex(currentInd);       //몬스터가 공격할 grid의 인덱스를 pattern에 반환
        Managers.Field.WarningAttack(pattern);                          //해당 영역 빨강화
    }


    private void OnTriggerEnter2D(Collider2D collision)                 //Player의 공격으로 인해서 몬스터가 활성화된 grid에 닿을 경우
    {
        currentHp -= 1;                                                 //몬스터는 한방 맞으면 뒤짐
        GetComponent<Animator>().Play("Hit");

        Debug.Log("Monster Hit");
        if (currentHp <= 0)
            Die();
    }
    
    void Die()
    {
        Debug.Log("MonsterDIe!");
        Managers.Game.MinusMonsterNum();
        Managers.Timing.BehaveAction -= BitBehave;                           //현재 monsterIndex 번째 몬스터의 비트 마다 실행할 BitBehave 구독 해제
        GameScene gamescene = (GameScene)Managers.Scene.CurrentScene;        //GameScene(스크립트) 반환
        gamescene.NextMonsterIndex();                                        //다음 몬스터 생성
        Destroy(gameObject);
        Managers.Sound.Play("Die", Define.Sound.Effect, 2.0f);
    }
}
