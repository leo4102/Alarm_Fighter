using System;       //Action ������ �ʿ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager
{
    //(���� ȭ�鿡 Ȱ��ȭ ��)�����Ǵ� �� note�� ���� List
    public List<GameObject> noteList = new List<GameObject>();
    public Vector2[] timingRange = null;          //CheckingRange(��ũ��Ʈ)�� ���� ������ �´�

    public Action BehaveAction;     //TimingManager�� UpdatePerBit()���� ����(�ٷ� �Ʒ�)
    double currentTime = 0;
    
  /*  public void Init()
    {
        timingRange = GameObject.FindObjectOfType<CheckingRange>().GetTimingRange();
        *//*if (timingRange == null)
            Debug.Log("timingRange�� NULL�̴�");*//*
    }*/

    //[SerializeField] Transform centerFlame=null;
    /* = GameObject.Find("CenterFlame").transform;*/  //CenterFlame�� ��ġ


    //[SerializeField] 
    //RectTransform[] timingRect=null;
    /*={
    GameObject.Find("PerfectRec").GetComponent<RectTransform>(),
    GameObject.Find("CoolRec").GetComponent<RectTransform>(),
    GameObject.Find("GoodRec").GetComponent<RectTransform>(),
    GameObject.Find("BadRec").GetComponent<RectTransform>()
};*/ //�����ִ� �̹��� �ڽ�
     //Vector2[] timingRange = null; //timingRect�� x����

    //GameObject _root;
    /* public void Init()
     {
          timingRange = new Vector2[timingRect.Length]; //ũ�� 4




         for (int i = 0; i < timingRect.Length; i++)
         {

             Util.FindChild(


             //timingRange[0]�� perfectRect�� ���� ��
             timingRange[i] = new Vector2(timingRect[i].localPosition.x - timingRect[i].rect.width / 2,
                 timingRect[i].anchoredPosition.x + timingRect[i].rect.width / 2);
         }

         //_root = GameObject.Find("Note2");
     }*/

    /* void Start()
     {
         timingRange = new Vector2[timingRect.Length]; //ũ�� 4

         for (int i = 0; i < timingRect.Length; i++)
         {
             //timingRange[0]�� perfectRect�� ���� ��
             timingRange[i] = new Vector2(timingRect[i].localPosition.x - timingRect[i].rect.width / 2,
                 timingRect[i].anchoredPosition.x + timingRect[i].rect.width / 2);
         }
     }*/

    //��ǻ� ���� ������ �ʿ�
    public void UpdatePerBit()      //GameScene�� Update()������ ȣ��     //Note�� Bpm�� ���� �����Ǵ� ������ ������Ʈ
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
    
    //������ Note�� timingRange�� ���ϴ� Note�� �ִ��� Ȯ��
    public bool CheckTiming()                   //PlayerTest Update()������ ��� ȣ���     //��ǻ� Player ������ �ʿ�
    {
        for (int i = 0; i < noteList.Count; i++)//������ Note�� ���� Ȯ��
        {
            float notePosx = noteList[i].transform.localPosition.x;// Note�Ѱ��� x��

            for (int j = 0; j < timingRange.Length; j++)//4���� timingRange�� ���ϴ� Ȯ��
            {
                if ((timingRange[j].x <= notePosx) && (notePosx <= timingRange[j].y))
                {
                    //Note�� timingRange�� ���ϸ� �ش� Note ����
                    //Destroy(noteList[i]);
                    
                    //Managers.Bpm.Able = true;//������        //------------------------------
                    noteList[i].GetComponent<Note>().HideNote();//Note ���� ��ſ� Note�� �̹����� ��Ȱ��ȭ//����: BGM�� �� ����
                    noteList.RemoveAt(i);
                    //Debug.Log("HIT" + j);
                    return true;
                }
            }
        }
        Debug.Log("Miss");//������ Note���� timingRange�� ������ ������ Miss
        return false;
    }

    public void Clear()
    {
        noteList.Clear();       //noteList=null;�� �ϸ� �ȵ�
        timingRange = null;
        BehaveAction = null;
        currentTime = 0;
    }

    
}
