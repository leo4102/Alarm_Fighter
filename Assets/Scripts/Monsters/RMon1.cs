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

    int[] currentGridInd = new int[2];   //���� ���Ͱ� ��ġ�ϰ� �ִ� gridInd
    int[] moveGridInd = new int[2];
    int[] towardPlayer = new int[2];    //RMons1-> MyPlayer �� ���ϴ� ���⺤��

    public float speed;

    int[] playerInd;   //MyPlayer�� ���� ��ġ �ε��� 

    int a = 0, b = 0;

    private void Start()
    {
        //currentHp = maxHp;    //----------------
        speed = 10f;
        playerInd = Managers.Game.CurrentPlayer.GetComponent<MyPlayer>().GetPlayerInd();

        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //��ü gridArray ��������

        //ó�� ���� ��ġ ����   
        int rand_x = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetWidth() - 1);
        int rand_y = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetHeight());
        transform.position = gridArray[rand_x][rand_y].grid.transform.position;

        //������ ���� Grid�ε��� �ʱ�ȭ
        currentGridInd[0] = rand_x;
        currentGridInd[1] = rand_y;

        moveGridInd[0] = rand_x;
        moveGridInd[1] = rand_y;

        //���� ��ġ Grid �� ����
        SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
        currentGridColor.color = Color.magenta;

        //��ȯ�Ǵ� �ִϸ��̼� ����(�߰�)

        //��Ʈ ���� ������ BitBehave ����
        Managers.Timing.BehaveAction -= AutoBitBehave;
        Managers.Timing.BehaveAction += AutoBitBehave;
    }

    public void AutoBitBehave()
    {
        

        if ((playerInd[0] - 3 < currentGridInd[0]) && (currentGridInd[0] < playerInd[0] + 3))     //MyPlayer�� +-1(��) ���� �ȿ� ������ RMons1 ������
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
                    Debug.Log("Die �� GameObject :" + gameObject);
                    Managers.Timing.BehaveAction -= AutoBitBehave;
                    Managers.Game.CurrentRMons.Remove(gameObject);

                    //�װ� �� currentGridInd �� ����
                    SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                    currentGridColor.color = new Color(255f, 255f, 255f, 1);
                    break;

            }
        }

    }

    //�������̵� �Ǿ�� �� �Լ�
    public void SelectNextDirection()         //MyPlayer�� ��ġ�� ����������� ���� ���� ����
    {
        towardPlayer[0] = playerInd[0] - currentGridInd[0];
        towardPlayer[1] = playerInd[1] - currentGridInd[1];

        if (towardPlayer[0] != 0 && towardPlayer[1] != 0)       //�� �Ǵ� �� ����
        {
            int rand = UnityEngine.Random.Range(0, 2);       //0(�� ����)�� 1(�� ����)�� �������� �ϳ� ����

            if (rand == 0) ChooseLeftOrRight();
            else if(rand ==1) ChooseUpOrDown();
        }
        else if (towardPlayer[0] != 0 && towardPlayer[1] == 0)      //�� ����
        {
            ChooseLeftOrRight();
        }
        else if (towardPlayer[0] == 0 && towardPlayer[1] != 0)      //�� ����
        {
            ChooseUpOrDown();
        }
        else 
        {
            Debug.Log("SelectNextDirection()���� ���� �߻�");
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


        SelectNextDirection();        //���� ������ ���� ����(nextDirection ����)

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

        //�����ϱ�� �ߴ� ���� �ٽ� ����ȭ
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

    
    private void Update()       //���� ��ü�� moveGridInd�� �̵��� ���
    {
        
        try
        {
            if (playerInd[0] == moveGridInd[0] && playerInd[1] == moveGridInd[1])    //Myplayer��ġ==������ ��ġ(�������� ����)
            {
                return;                
            }
            
            //����Ǳ� ���� ������ �ٽ� ���� ������
            SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
            currentGridColor.color = new Color(255f, 255f, 255f, 1);

            //��ü�� moveGridInd�� �̵�
            List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();

            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Walk");
            
            Vector3 movePoint = gridArray[moveGridInd[0]][moveGridInd[1]].grid.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, movePoint, Time.deltaTime * speed);

            //CurrnetGridInd�� moveGridInd�� �ֽ�ȭ
            currentGridInd[0] = moveGridInd[0];
            currentGridInd[1] = moveGridInd[1];
            
            //����� CurrentGridInd�� magenta������ ����
            currentGridColor = gridArray[currentGridInd[0]][currentGridInd[1]].grid.GetComponent<SpriteRenderer>();
            currentGridColor.color = Color.magenta;

        }
        catch (ArgumentOutOfRangeException)
        {
            moveGridInd[0] = currentGridInd[0];
            moveGridInd[1] = currentGridInd[1];
        }
    }

    IEnumerator ActiveDamageField(GameObject go)            //�ڷ�ƾ�� ������ ���� ����˴ϴ�.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage���� collider Ȱ��ȭ(���)
        yield return new WaitForFixedUpdate();              //yield ��ȯ ������ ������ �Ͻ� �����ǰ� ���� �����ӿ��� �ٽ� ���۵Ǵ� ����
        poly.enabled = false;
    }

    //maygo�� ������ Attack()�� ȣ��
    //maygo�� moveGridInd�� �ٲٸ� �ű�� RMons1�� �ٷ� �̵�
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


    public int[] GetCurrentInd()
    {
        return currentGridInd;
    }

   /* private void OnTriggerEnter2D(Collider2D collision)                 //Player�� �������� ���ؼ� ���Ͱ� Ȱ��ȭ�� grid�� ���� ���
    {
        currentHp -= 1;                                                 //���ʹ� �ѹ� ������ ����
        GetComponent<Animator>().Play("Hit");

        Debug.Log("Monster Hit");
        if (currentHp <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("MonsterDie!");
        //Managers.Game.MinusMonsterNum();
        Managers.Timing.BehaveAction -= AutoBitBehave;                           //���� monsterIndex ��° ������ ��Ʈ ���� ������ BitBehave ���� ����
        //GameScene gamescene = (GameScene)Managers.Scene.CurrentScene;        //GameScene(��ũ��Ʈ) ��ȯ
        //gamescene.NextMonsterIndex();                                        //���� ���� ����
        Destroy(gameObject);
        Managers.Sound.Play("Die", Define.Sound.Effect, 2.0f);
        Managers.Game.CurrentRMons.Remove(gameObject);
    }
*/



}
