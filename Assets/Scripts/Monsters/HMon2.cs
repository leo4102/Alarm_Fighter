using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMon2 : MonoBehaviour
{
    Define.State nextBehavior = Define.State.ATTACKREADY;
    Define.PlayerMove nextDirection = Define.PlayerMove.Left;

    public int[] currentGridInd = null;  //현재 몬스터가 위치하고 있는 gridInd
    int[] moveGridInd = null;

    public float speed;
    
    private void Start()
    {
        speed = 10f;

        currentGridInd = new int[2];
        moveGridInd = new int[2];

        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //전체 gridArray 가져오기

        int rand = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetHeight());    //처음 스폰 위치 결정      

        //Debug.Log(gameObject.name);
        //GameObject spawnGrid = topSpawnArea[rand].grid;
        Debug.Log("rand : " + rand);

        transform.position = Managers.Field.GetMyField().getAllGridArray()[Managers.Field.GetMyField().GetWidth() - 1][rand].grid.transform.position;
        //transform.parent.transform.position = spawnGrid.transform.position;
        currentGridInd[0] = Managers.Field.GetMyField().GetWidth()-1;
        currentGridInd[1] = rand;

        moveGridInd[0] = Managers.Field.GetMyField().GetWidth()-1;
        moveGridInd[1] = rand;

        SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
        currentGridColor.color = Color.magenta;

        //=======================================================
        /*    Debug.Log("1:"+Managers.Field.getMyField().getWidth());     //7
            Debug.Log("1:" + Managers.Field.getMyField().getWidth()/2); //3

            Debug.Log("1:" + Managers.Field.getMyField().getHeight());  //3
            Debug.Log("1:" + Managers.Field.getMyField().getHeight() / 2);  //1

            Debug.Log("2"+currentGridInd[1]);
            Debug.Log("2"+currentGridInd[0]);

            currentGridInd[0] = 3;
            currentGridInd[1] = 1;

            currentGridInd[0] = Managers.Field.getMyField().getWidth() / 2 ;        //7/2 == 3
            currentGridInd[1] = Managers.Field.getMyField().getHeight() / 2 ;       //3/2 == 1*/

        //소환되는 애니메이션 실행
        Managers.Timing.BehaveAction -= AutoBitBehave;      //VMon1의 비트 마다 실행할 BitBehave 구독
        Managers.Timing.BehaveAction += AutoBitBehave;
    }
    //특정 그리드에 스폰 되자마자 

    //아래 반복
    //아래 grid는 빨강화로 공격할것임을 알린다
    //다음 박자에 아래로 이동 밑 공격

    //더 이상 내려갈 grid 가 존재하지 않을 경우 destroy


    public void AutoBitBehave()
    {
        switch (nextBehavior)
        {
            case Define.State.ATTACKREADY:

                AutoWarningAttack(nextDirection);            //아래 grid는 빨강화로 공격할것임을 알린다
                //nextBehavior = Define.State.ATTACK;

                //currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                //currentGridColor.color = Color.magenta;
                break;

            case Define.State.ATTACK:                        //다음 박자에 아래로 이동 밑 공격
                AutoAttack(nextDirection);
                //nextBehavior = Define.State.ATTACKREADY;

                //currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                //currentGridColor.color = Color.magenta;
                break;

            case Define.State.DIE:
                Destroy(gameObject);
                Debug.Log("Die 할 GameObject :" + gameObject);
                Managers.Timing.BehaveAction -= AutoBitBehave;
                Managers.Game.CurrentHMons.Remove(gameObject);

                SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                currentGridColor.color = new Color(255f, 255f, 255f, 1);
                break;

        }
    }

    public void AutoWarningAttack(Define.PlayerMove nextDirection)
    {
        //공격준비 상태의 애니메이션 실행
        //아래 grid 빨강화
        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();
        try
        {
            SpriteRenderer gridColor = gridArray[currentGridInd[0] - 1][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
            gridColor.color = Color.red;
        }
        catch (ArgumentOutOfRangeException)
        {
            //Debug.Log("abcde");
            nextBehavior = Define.State.DIE;
            return;
        }
        nextBehavior = Define.State.ATTACK;
    }


    public void AutoAttack(Define.PlayerMove nextDirection)
    {
        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();

        //SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
        //currentGridColor.color = new Color(255f, 255f, 255f, 1);



        //Animator anim = GetComponent<Animator>();
        //anim.SetTrigger("Slide");
        //try
        //{
        mayGo(Define.PlayerMove.Left);
        //}//if (currentGridInd[1] >= Managers.Field.GetHeight())
        //catch (ArgumentOutOfRangeException)
        //{
        //Destroy(gameObject);
        //Managers.Timing.BehaveAction -= AutoBitBehave;
        //}

        StartCoroutine("ActiveDamageField", gridArray[currentGridInd[0]][currentGridInd[1]].grid);

        //currentGridColor = gridArray[currentGridInd[0]][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
        //currentGridColor.color = Color.magenta;

        nextBehavior = Define.State.ATTACKREADY;
    }

    IEnumerator ActiveDamageField(GameObject go)            //코루틴이 다음과 같이 선언됩니다.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage영역 collider 활성화(잠깐)
        yield return new WaitForFixedUpdate();              //yield 반환 라인은 실행이 일시 중지되고 다음 프레임에서 다시 시작되는 지점
        poly.enabled = false;
    }


    private void Update()
    {
        try
        {
            SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
            currentGridColor.color = new Color(255f, 255f, 255f, 1);

            List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();
            Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, Time.deltaTime * speed);

            currentGridInd[0] = moveGridInd[0];
            currentGridInd[1] = moveGridInd[1];

            currentGridColor = gridArray[currentGridInd[0]][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
            currentGridColor.color = Color.magenta;

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
}
