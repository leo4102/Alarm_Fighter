using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Imfor
{
    public GameObject grid; // one prefab
    public float ratio = 1.0f; // used by player scale
}

public class MyField : MonoBehaviour
{
    protected GameObject Grid_All;

    [SerializeField]
    int height;
    [SerializeField]
    int width;

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }

    List<List<Imfor>> gridArray = new List<List<Imfor>>();// 2 demention list

    List<Imfor> leftSpawnArea = new List<Imfor>();      //죄측 스폰 영역
    List<Imfor> rightSpawnArea = new List<Imfor>();     //우측 스폰 영역
    List<Imfor> topSpawnArea = new List<Imfor>();       //상단 스폰 영역

    


    void Awake()
    {
        Init();
    }

    public GameObject GetGrid(int latitude, int longitude)
    {
        if ((latitude > width || longitude > height || latitude < 0 || longitude < 0)) { Debug.Log("out of index"); return null; }
        return gridArray[latitude][longitude].grid;
    }

    private void FindObject()
    {
        Grid_All = gameObject;
    }
    private void Init()
    {
        FindObject();

        for (int i = 0; i < width; i++)
            gridArray.Add(new List<Imfor>());

        int index = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Imfor imfor = new Imfor();
                imfor.grid = transform.GetChild(index).gameObject;



                if ((0 < i && i < width - 1) && j == 0)      //상단 영역 grid topSpawnArea에 삽입
                {
                    topSpawnArea.Add(imfor);
                }
                if (i == 0 && (0 < j && j < height))        //좌측 영역 grid leftSpawnArea에 삽입
                {
                    leftSpawnArea.Add(imfor);
                }
                if (i == (width - 1) && (0 < j && j < height))     //우측 영역 gird rightSpawnArea에 삽입
                {
                    rightSpawnArea.Add(imfor);
                }

                gridArray[i].Add(imfor);
                index++;
            }
        }

        Managers.Field.SetMyField(this);


        //Debug.Log(gridArray.Count);     //21->7
        //Debug.Log(gridArray[0].Count);  //3
        //Debug.Log(leftSpawnArea.Count);       //2
        //Debug.Log(rightSpawnArea.Count);      //2
        //Debug.Log(topSpawnArea.Count);        //5



    }
   

    public List<List<Imfor>> getAllGridArray()
    {
        return gridArray;
    }

  
    public List<Imfor> getGridArray(Define.FieldArray fieldArray)
    {
        if (fieldArray == Define.FieldArray.LEFT)              // player 
        {
            return leftSpawnArea;
        }
        else if (fieldArray == Define.FieldArray.RIGHT)         // monster
        {
            return rightSpawnArea;
        }
        else if (fieldArray == Define.FieldArray.TOP)
        {
            return topSpawnArea;
        }
        /* else if (fieldArray == Define.FieldArray.ALL_ONE)
         {
             return gridArray1D;
         }
        */
        else
            return null;
    }


}

