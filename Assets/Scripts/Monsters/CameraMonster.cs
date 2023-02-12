using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMonster : FieldObject
{

    MyField myField;        //MyField(스크립트)에 접근하기 편하도록 그냥 만듬

    Define.State nextBehavior = Define.State.SPAWN;                      //다음 상태


    [SerializeField] List<GameObject> horizontalMons = new List<GameObject>();       //가로 공격 몬스터용
    //[SerializeField] List<GameObject> verticalMons = new List<GameObject>();         //세로 공격 몬스터용
    [SerializeField] GameObject verticalMon;        //세로 공격 몬스터 프리팹
    //[SerializeField] GameObject horizontalMon;      //가로 공격 몬스터 프리팹
    [SerializeField] GameObject randomMon;          //랜덤 공격 몬스터 프리팹




    void Start()
    {
        myField = Managers.Field.GetMyField();
        Managers.Timing.BehaveAction -= BitBehave;      //몬스터의 비트 마다 실행할 BitBehave 구독
        Managers.Timing.BehaveAction += BitBehave;
    }

    protected override void BitBehave()
    {

        switch (nextBehavior)
        {

            case Define.State.SPAWN:

                //세로 공격 몬스터 스폰
                if (Managers.Game.CurrentVMons.Count < 2) //필드에 2개 이상 만들어지지 않음
                {
                    //SpawnVerticalMonster(verticalMon);
                }

                if (Managers.Game.CurrentHMons.Count < 1) //필드에 1개 이상 만들어지지 않음
                {
                    //SpawnHorizontalMonster();
                }

                if (Managers.Game.CurrentRMons.Count < 1) //필드에 1개 이상 만들어지지 않음
                {
                    SpawnRandomMonster(randomMon);
                }
                //SpawnVerticalMonster(verticalMon);
                //Debug.Log("spawn penguin");




                nextBehavior = Define.State.NOTSPAWN;

                break;


            case Define.State.NOTSPAWN:


                //딱히 하는 일 없음
                //Debug.Log("Not spawn");

                nextBehavior = Define.State.SPAWN;

                break;


        }
    }

    private void SpawnRandomMonster(GameObject prefab)
    {
        GameObject go = Instantiate<GameObject>(prefab);
        Debug.Log("생성된 랜덤 GameObject:" + go);
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

    private void SpawnVerticalMonster(GameObject prefab)        //펭귄 스폰
    {
        //몬스터 scene에 생성
        GameObject go = Instantiate<GameObject>(prefab);
        Debug.Log("생성된 GameObject:" + go);
        Managers.Game.CurrentVMons.Add(go);

        /*  //스폰 될 Imfo 랜덤배정
          int rand = Random.Range(1, myField.getWidth() - 1);
          Imfor imfor = myField.getGridArray(Define.FieldArray.TOP)[rand];

          //선택된 Imfo(클래스)가 갖고 있는 grid에 몬스터 위치 배정
          go.transform.position = imfor.grid.transform.position;*/
    }



    private void OnTriggerEnter2D(Collider2D collision)                 //Player의 공격으로 인해서 몬스터가 활성화된 grid에 닿을 경우
    {

    }





}
