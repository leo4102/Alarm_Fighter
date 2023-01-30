using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo //Pool에 생성하고자 하는 오브젝트 정보를 담음
{
    public GameObject notePrefab;   //생성할 Prefab
    public int count;   //생성할 Prefab의 개수
    public Transform poolParent;    //Prefab이 생성 시 부모로 들어갈 오브젝트
}


public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfo = null;
    public static ObjectPool objectPool; //어디서든 접근 도록 하기 위해서

    //한개의 Queue가 한개의 Pool의미
    public Queue<GameObject> noteQueue = new Queue<GameObject>();   //실질적인 하나의 Pool

    void Start()
    {
        objectPool = this;
        noteQueue = CreatePool(objectInfo[0]);
        //noteQueue2 = CreatePool(objectInfo[1]);   //생성하고자 하는 Pool이 또 생길 때 사용
    }

    Queue<GameObject> CreatePool(ObjectInfo objectInfo) //Pool 생성
    {
        Queue<GameObject> pool = new Queue<GameObject>();

        for (int i = 0; i < objectInfo.count; i++)
        {
            //Prefab Scene에 생성
            GameObject obj = Instantiate(objectInfo.notePrefab, transform.position, Quaternion.identity);//일단 scene 아무곳에 생성
            obj.SetActive(false);//생성하자마자 비활성화
            //obj가 들어갈 부모 오브젝트 설정
            if (objectInfo.poolParent != null)
            {
                obj.transform.SetParent(objectInfo.poolParent);
            }
            else
            {
                obj.transform.SetParent (this.transform);
            }
            pool.Enqueue(obj); //생성된 obj를 Pool에 넣음
        }

        return pool; //새로 만든 Pool 반환
            
    }

}


