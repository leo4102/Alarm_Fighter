using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMonster : FieldObject
{

    MyField myField;        //MyField(��ũ��Ʈ)�� �����ϱ� ���ϵ��� �׳� ����

    Define.State nextBehavior = Define.State.SPAWN;                      //���� ����


    [SerializeField] List<GameObject> horizontalMons = new List<GameObject>();       //���� ���� ���Ϳ�
    //[SerializeField] List<GameObject> verticalMons = new List<GameObject>();         //���� ���� ���Ϳ�
    [SerializeField] GameObject verticalMon;        //���� ���� ���� ������
    //[SerializeField] GameObject horizontalMon;      //���� ���� ���� ������
    [SerializeField] GameObject randomMon;          //���� ���� ���� ������




    void Start()
    {
        myField = Managers.Field.GetMyField();
        Managers.Timing.BehaveAction -= BitBehave;      //������ ��Ʈ ���� ������ BitBehave ����
        Managers.Timing.BehaveAction += BitBehave;
    }

    protected override void BitBehave()
    {

        switch (nextBehavior)
        {

            case Define.State.SPAWN:

                //���� ���� ���� ����
                if (Managers.Game.CurrentVMons.Count < 2) //�ʵ忡 2�� �̻� ��������� ����
                {
                    //SpawnVerticalMonster(verticalMon);
                }

                if (Managers.Game.CurrentHMons.Count < 1) //�ʵ忡 1�� �̻� ��������� ����
                {
                    //SpawnHorizontalMonster();
                }

                if (Managers.Game.CurrentRMons.Count < 1) //�ʵ忡 1�� �̻� ��������� ����
                {
                    SpawnRandomMonster(randomMon);
                }
                //SpawnVerticalMonster(verticalMon);
                //Debug.Log("spawn penguin");




                nextBehavior = Define.State.NOTSPAWN;

                break;


            case Define.State.NOTSPAWN:


                //���� �ϴ� �� ����
                //Debug.Log("Not spawn");

                nextBehavior = Define.State.SPAWN;

                break;


        }
    }

    private void SpawnRandomMonster(GameObject prefab)
    {
        GameObject go = Instantiate<GameObject>(prefab);
        Debug.Log("������ ���� GameObject:" + go);
        Managers.Game.CurrentRMons.Add(go);
    }

    private void SpawnHorizontalMonster()
    {
        int rand = UnityEngine.Random.Range(0, horizontalMons.Count);
        GameObject go = Instantiate<GameObject>(horizontalMons[rand]);
        Managers.Game.CurrentHMons.Add(go);
    }


    
    /*private void SpawnHorizontalMonster(GameObject prefab)
    {
        GameObject go = Instantiate<GameObject>(prefab);
        Managers.Game.CurrentHMons.Add(go);
    }*/

    private void SpawnVerticalMonster(GameObject prefab)        //��� ����
    {
        //���� scene�� ����
        GameObject go = Instantiate<GameObject>(prefab);
        Debug.Log("������ GameObject:" + go);
        Managers.Game.CurrentVMons.Add(go);

        /*  //���� �� Imfo ��������
          int rand = Random.Range(1, myField.getWidth() - 1);
          Imfor imfor = myField.getGridArray(Define.FieldArray.TOP)[rand];

          //���õ� Imfo(Ŭ����)�� ���� �ִ� grid�� ���� ��ġ ����
          go.transform.position = imfor.grid.transform.position;*/
    }



    private void OnTriggerEnter2D(Collider2D collision)                 //Player�� �������� ���ؼ� ���Ͱ� Ȱ��ȭ�� grid�� ���� ���
    {

    }





}
