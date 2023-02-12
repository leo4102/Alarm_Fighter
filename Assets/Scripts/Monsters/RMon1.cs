using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RMon1 : MonoBehaviour
{
    //int maxHp = 1;//------------
    //int currentHp;//-----------------


    Define.State nextBehavior = Define.State.ATTACKREADY;
    Define.PlayerMove nextDirection;

    int[] currentGridInd = new int[2];   //현재 몬스터가 위치하고 있는 gridInd
    int[] moveGridInd = new int[2];
    int[] towardPlayer = new int[2];    //RMons1-> MyPlayer 로 향하는 방향벡터

    public float speed;

    int[] playerInd;   //MyPlayer의 현재 위치 인덱스 

    int a = 0, b = 0;

    private void Start()
    {
        //currentHp = maxHp;    //----------------
        speed = 10f;
        playerInd = Managers.Game.CurrentPlayer.GetComponent<MyPlayer>().GetPlayerInd();

        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //전체 gridArray 가져오기

        //처음 스폰 위치 결정   
        int rand_x = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetWidth() - 1);
        int rand_y = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetHeight());
        transform.position = gridArray[rand_x][rand_y].grid.transform.position;

        //몬스터의 현재 Grid인덱스 초기화
        currentGridInd[0] = rand_x;
        currentGridInd[1] = rand_y;

        moveGridInd[0] = rand_x;
        moveGridInd[1] = rand_y;

        //현재 위치 Grid 색 변경
        SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
        currentGridColor.color = Color.magenta;

        //소환되는 애니메이션 실행(추가)

        //비트 마다 실행할 BitBehave 구독
        Managers.Timing.BehaveAction -= AutoBitBehave;
        Managers.Timing.BehaveAction += AutoBitBehave;
    }

    public void AutoBitBehave()
    {
        

        if ((playerInd[0] - 3 < currentGridInd[0]) && (currentGridInd[0] < playerInd[0] + 3))     //MyPlayer의 +-1(열) 범위 안에 들어오면 RMons1 움직임
        {
            switch (nextBehavior)
            {
                case Define.State.ATTACKREADY:

                    AutoWarningAttack(nextDirection);            
                 
                    break;

                case Define.State.ATTACK:                        
                    AutoAttack(nextDirection);
                    
                    break;

                case Define.State.DIE:
                    Destroy(gameObject);
                    Debug.Log("Die 할 GameObject :" + gameObject);
                    Managers.Timing.BehaveAction -= AutoBitBehave;
                    Managers.Game.CurrentRMons.Remove(gameObject);

                    //죽고난 후 currentGridInd 색 복구
                    SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                    currentGridColor.color = new Color(255f, 255f, 255f, 1);
                    break;

            }
        }

    }

    //오버라이드 되어야 할 함수
    public void SelectNextDirection()         //MyPlayer와 위치가 가까워지도록 다음 방향 설정
    {
        towardPlayer[0] = playerInd[0] - currentGridInd[0];
        towardPlayer[1] = playerInd[1] - currentGridInd[1];

        if (towardPlayer[0] != 0 && towardPlayer[1] != 0)       //열 또는 행 변경
        {
            int rand = UnityEngine.Random.Range(0, 2);       //0(열 변경)과 1(행 변경)중 랜덤으로 하나 선택

            if (rand == 0) ChooseLeftOrRight();
            else if(rand ==1) ChooseUpOrDown();
        }
        else if (towardPlayer[0] != 0 && towardPlayer[1] == 0)      //열 변경
        {
            ChooseLeftOrRight();
        }
        else if (towardPlayer[0] == 0 && towardPlayer[1] != 0)      //행 변경
        {
            ChooseUpOrDown();
        }
        else 
        {
            Debug.Log("SelectNextDirection()에서 오류 발생");
        }

      
        
    }

    public void ChooseLeftOrRight()
    {
        if (Math.Sign(towardPlayer[0]) == -1)
        {
            nextDirection = Define.PlayerMove.Left;
            a = -1; b = 0;

        }
        else
        {
            nextDirection = Define.PlayerMove.Right;
            a = 1; b = 0;
        }
    }

    public void ChooseUpOrDown()
    {
        if (Math.Sign(towardPlayer[1]) == -1)
        {
            nextDirection = Define.PlayerMove.Up;
            a = 0; b = -1;
        }
        else
        {
            nextDirection = Define.PlayerMove.Down;
            a = 0; b = 1;
        }
    }



    
    public void AutoWarningAttack(Define.PlayerMove nextDirection)
    {
        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();


        SelectNextDirection();        //다음 움직일 방향 결정(nextDirection 설정)

        try
        {
            SpriteRenderer gridColor = gridArray[currentGridInd[0]+a][currentGridInd[1]+b].grid.GetComponent<SpriteRenderer>();
            gridColor.color = Color.red;
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("RMons1 AutoWarningAttack IndexOutofBoundArray");
            return;
        }
        
        nextBehavior = Define.State.ATTACK;
    }


    public void AutoAttack(Define.PlayerMove nextDirection)
    {
        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();

        //SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
        //currentGridColor.color = new Color(255f, 255f, 255f, 1);

        //공격하기로 했던 영역 다시 원색화
        //SpriteRenderer gridColor = gridArray[currentGridInd[0] + a][currentGridInd[1] + b].grid.GetComponent<SpriteRenderer>();
        //gridColor.color = Color.white;

        //Animator anim = GetComponent<Animator>();
        //anim.SetTrigger("Slide");
        //try
        //{
        mayGo(nextDirection);
        //}//if (currentGridInd[1] >= Managers.Field.GetHeight())
        //catch (ArgumentOutOfRangeException)
        //{
        //Destroy(gameObject);
        //Managers.Timing.BehaveAction -= AutoBitBehave;
        //}

        Debug.Log("moveGridInd[0]:" + moveGridInd[0]);
        Debug.Log("moveGridInd[1]:" + moveGridInd[1]);
            
        StartCoroutine("ActiveDamageField", gridArray[moveGridInd[0]][moveGridInd[1]].grid);

        //currentGridColor = gridArray[currentGridInd[0]][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
        //currentGridColor.color = Color.magenta;

        nextBehavior = Define.State.ATTACKREADY;
    }

    
    private void Update()       //실제 객체의 moveGridInd로 이동을 담당
    {
        
        try
        {
            if (playerInd[0] == moveGridInd[0] && playerInd[1] == moveGridInd[1])    //Myplayer위치==공격할 위치(움직이지 말것)
            {
                return;                
            }
            
            //변경되기 전의 영역을 다시 원래 색으로
            SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
            currentGridColor.color = new Color(255f, 255f, 255f, 1);

            //객체를 moveGridInd로 이동
            List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();

            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Walk");
            
            Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, Time.deltaTime * speed);

            //CurrnetGridInd를 moveGridInd로 최신화
            currentGridInd[0] = moveGridInd[0];
            currentGridInd[1] = moveGridInd[1];
            
            //변경된 CurrentGridInd를 magenta색으로 변경
            currentGridColor = gridArray[currentGridInd[0]][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
            currentGridColor.color = Color.magenta;

        }
        catch (ArgumentOutOfRangeException)
        {
            moveGridInd[0] = currentGridInd[0];
            moveGridInd[1] = currentGridInd[1];
        }
    }

    IEnumerator ActiveDamageField(GameObject go)            //코루틴이 다음과 같이 선언됩니다.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage영역 collider 활성화(잠깐)
        yield return new WaitForFixedUpdate();              //yield 반환 라인은 실행이 일시 중지되고 다음 프레임에서 다시 시작되는 지점
        poly.enabled = false;
    }

    //maygo는 무조건 Attack()서 호출
    //maygo서 moveGridInd를 바꾸면 거기로 RMons1이 바로 이동
    protected void mayGo(Define.PlayerMove direction)       
    {
        //try
        //{
        //List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();

        //SpriteRenderer moveGridColor = gridArray[moveGridInd[0]][moveGridInd[1]].grid.GetComponent<SpriteRenderer>();
        //moveGridColor.color = Color.magenta;

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


    public int[] GetCurrentInd()
    {
        return currentGridInd;
    }

   /* private void OnTriggerEnter2D(Collider2D collision)                 //Player의 공격으로 인해서 몬스터가 활성화된 grid에 닿을 경우
    {
        currentHp -= 1;                                                 //몬스터는 한방 맞으면 뒤짐
        GetComponent<Animator>().Play("Hit");

        Debug.Log("Monster Hit");
        if (currentHp <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("MonsterDie!");
        //Managers.Game.MinusMonsterNum();
        Managers.Timing.BehaveAction -= AutoBitBehave;                           //현재 monsterIndex 번째 몬스터의 비트 마다 실행할 BitBehave 구독 해제
        //GameScene gamescene = (GameScene)Managers.Scene.CurrentScene;        //GameScene(스크립트) 반환
        //gamescene.NextMonsterIndex();                                        //다음 몬스터 생성
        Destroy(gameObject);
        Managers.Sound.Play("Die", Define.Sound.Effect, 2.0f);
        Managers.Game.CurrentRMons.Remove(gameObject);
    }
*/



}
