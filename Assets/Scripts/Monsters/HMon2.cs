using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMon2 : MonoBehaviour
{
    Define.State nextBehavior = Define.State.ATTACKREADY;
    Define.PlayerMove nextDirection = Define.PlayerMove.Left;

    public int[] currentGridInd = null;  //���� ���Ͱ� ��ġ�ϰ� �ִ� gridInd
    int[] moveGridInd = null;

    public float speed;
    
    private void Start()
    {
        speed = 10f;

        currentGridInd = new int[2];
        moveGridInd = new int[2];

        List<List<Imfor>> gridArray = Managers.Field.GetMyField().getAllGridArray();        //��ü gridArray ��������

        int rand = UnityEngine.Random.Range(1, Managers.Field.GetMyField().GetHeight());    //ó�� ���� ��ġ ����      

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

        //��ȯ�Ǵ� �ִϸ��̼� ����
        Managers.Timing.BehaveAction -= AutoBitBehave;      //VMon1�� ��Ʈ ���� ������ BitBehave ����
        Managers.Timing.BehaveAction += AutoBitBehave;
    }
    //Ư�� �׸��忡 ���� ���ڸ��� 

    //�Ʒ� �ݺ�
    //�Ʒ� grid�� ����ȭ�� �����Ұ����� �˸���
    //���� ���ڿ� �Ʒ��� �̵� �� ����

    //�� �̻� ������ grid �� �������� ���� ��� destroy


    public void AutoBitBehave()
    {
        switch (nextBehavior)
        {
            case Define.State.ATTACKREADY:

                AutoWarningAttack(nextDirection);            //�Ʒ� grid�� ����ȭ�� �����Ұ����� �˸���
                //nextBehavior = Define.State.ATTACK;

                //currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                //currentGridColor.color = Color.magenta;
                break;

            case Define.State.ATTACK:                        //���� ���ڿ� �Ʒ��� �̵� �� ����
                AutoAttack(nextDirection);
                //nextBehavior = Define.State.ATTACKREADY;

                //currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                //currentGridColor.color = Color.magenta;
                break;

            case Define.State.DIE:
                Destroy(gameObject);
                Debug.Log("Die �� GameObject :" + gameObject);
                Managers.Timing.BehaveAction -= AutoBitBehave;
                Managers.Game.CurrentHMons.Remove(gameObject);

                SpriteRenderer currentGridColor = Managers.Field.GetMyField().GetGrid(currentGridInd[0], currentGridInd[1]).GetComponent<SpriteRenderer>();
                currentGridColor.color = new Color(255f, 255f, 255f, 1);
                break;

        }
    }

    public void AutoWarningAttack(Define.PlayerMove nextDirection)
    {
        //�����غ� ������ �ִϸ��̼� ����
        //�Ʒ� grid ����ȭ
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

    IEnumerator ActiveDamageField(GameObject go)            //�ڷ�ƾ�� ������ ���� ����˴ϴ�.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage���� collider Ȱ��ȭ(���)
        yield return new WaitForFixedUpdate();              //yield ��ȯ ������ ������ �Ͻ� �����ǰ� ���� �����ӿ��� �ٽ� ���۵Ǵ� ����
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
}
