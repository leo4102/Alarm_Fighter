using System;       //Action 때문에 필요
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager
{
    //(현재 화면에 활성화 된)생성되는 흰 note를 넣을 List
    public List<GameObject> noteList = new List<GameObject>();
    public Vector2[] timingRange = null;          //CheckingRange(스크립트)로 부터 가지고 온다

    public Action BehaveAction;     //TimingManager의 UpdatePerBit()에서 실행(바로 아래)
    double currentTime = 0;
    
  /*  public void Init()
    {
        timingRange = GameObject.FindObjectOfType<CheckingRange>().GetTimingRange();
        *//*if (timingRange == null)
            Debug.Log("timingRange가 NULL이다");*//*
    }*/

    //[SerializeField] Transform centerFlame=null;
    /* = GameObject.Find("CenterFlame").transform;*/  //CenterFlame의 위치


    //[SerializeField] 
    //RectTransform[] timingRect=null;
    /*={
    GameObject.Find("PerfectRec").GetComponent<RectTransform>(),
    GameObject.Find("CoolRec").GetComponent<RectTransform>(),
    GameObject.Find("GoodRec").GetComponent<RectTransform>(),
    GameObject.Find("BadRec").GetComponent<RectTransform>()
};*/ //색깔있는 이미지 박스
     //Vector2[] timingRange = null; //timingRect의 x범위

    //GameObject _root;
    /* public void Init()
     {
          timingRange = new Vector2[timingRect.Length]; //크기 4




         for (int i = 0; i < timingRect.Length; i++)
         {

             Util.FindChild(


             //timingRange[0]이 perfectRect의 범위 순
             timingRange[i] = new Vector2(timingRect[i].localPosition.x - timingRect[i].rect.width / 2,
                 timingRect[i].anchoredPosition.x + timingRect[i].rect.width / 2);
         }

         //_root = GameObject.Find("Note2");
     }*/

    /* void Start()
     {
         timingRange = new Vector2[timingRect.Length]; //크기 4

         for (int i = 0; i < timingRect.Length; i++)
         {
             //timingRange[0]이 perfectRect의 범위 순
             timingRange[i] = new Vector2(timingRect[i].localPosition.x - timingRect[i].rect.width / 2,
                 timingRect[i].anchoredPosition.x + timingRect[i].rect.width / 2);
         }
     }*/

    //사실상 몬스터 때문에 필요
    public void UpdatePerBit()      //GameScene의 Update()문에서 호출     //Note가 Bpm에 맞춰 생성되는 순간에 업데이트
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 60d / Managers.Bpm.BPM)
        {
            if (BehaveAction != null)
                BehaveAction.Invoke();
            //Debug.Log("work!");
            currentTime -= 60d / Managers.Bpm.BPM;
        }
    }
    
    //생성된 Note중 timingRange에 속하는 Note가 있는지 확인
    public bool CheckTiming()                   //PlayerTest Update()문에서 계속 호출됨     //사실상 Player 때문에 필요
    {
        for (int i = 0; i < noteList.Count; i++)//생성된 Note를 전부 확인
        {
            float notePosx = noteList[i].transform.localPosition.x;// Note한개의 x값

            for (int j = 0; j < timingRange.Length; j++)//4개의 timingRange와 접하는 확인
            {
                if ((timingRange[j].x <= notePosx) && (notePosx <= timingRange[j].y))
                {
                    //Note가 timingRange에 속하면 해당 Note 삭제
                    //Destroy(noteList[i]);
                    
                    //Managers.Bpm.Able = true;//삽입함        //------------------------------
                    noteList[i].GetComponent<Note>().HideNote();//Note 삭제 대신에 Note의 이미지만 비활성화//이유: BGM이 안 나옴
                    noteList.RemoveAt(i);
                    //Debug.Log("HIT" + j);
                    return true;
                }
            }
        }
        Debug.Log("Miss");//생성된 Note전부 timingRange에 속하지 않으면 Miss
        return false;
    }

    public void Clear()
    {
        noteList.Clear();       //noteList=null;로 하면 안됨
        timingRange = null;
        BehaveAction = null;
        currentTime = 0;
    }

    
}
