using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour
{
    List<GameObject> gridArray = new List<GameObject>();//전체 field data
    
    List<GameObject> playergridArray = new List<GameObject>();//Player field data
    List<GameObject> monstergridArray = new List<GameObject>();//Monster field data
    // getGridArray -> 타입 (플레이어 or 몬스터)에 따라 playergridArray 또는 monstergridArray를 반환한다. (1.17 재윤 추가)
    public List<GameObject> getGridArray(int type)
    {
        if (type == 1) // player 
        {
            return playergridArray;
        }
        else if (type == 2) // monster
        {
            return monstergridArray;
        }
        else if(type ==3)
        {
            return gridArray;
        }
        else
        {
            return null;
        }
    }

    protected GameObject grid_All;

    [SerializeField]
    GameObject gridPrefab;//Diamond

    protected float height;//높이
    protected float width;//길이

    // getHeight(), getWidth() -> 만들어진 전체 타일의 width와 height를 반환한다.  (1.17 재윤 추가)
    public int getHeight() { return (int)height; }
    public int getWidth() { return (int)width; }


    private float scale_x;//스케일 x축
    private float scale_y;//스케일 y축
    private float location_x;//grid의 처음 x좌표 
    private float location_y;//grid의 처음 y좌표

    const double x_size = 0.5;//Diamond 장축
    const double y_size = 0.25;//Diamond 단축
    const double gap = 0.1;//간격

    public abstract void Setheight();
    public abstract void setWidth();
    public void Rotation(GameObject go)//기본 회전
    {
        go.transform.Rotate(0f, 0f, 30.0f);
    }
    protected virtual void prepabRotation(GameObject go, float theta)//theta만큼 회전 함수
    {
        go.transform.Rotate(0f, 0f, theta);
    }

    protected virtual void prepabMove(GameObject go)
    {
        go.transform.position = new Vector3(-1.49f, 0.54f, 0f);
    }
    private void gridInit()
    {
        scale_x = gridPrefab.transform.localScale.x;//grid scale x축
        scale_y = gridPrefab.transform.localScale.y;//grid scale y축
        location_x = gridPrefab.transform.localPosition.x;//grid 초기 x좌표 
        location_y = gridPrefab.transform.localPosition.y;//grid 초기 y좌표
    }
    protected virtual void createObject()
    {
        grid_All = new GameObject() { name = "grid_All" };

    }
    protected virtual void createField()//필드 만들기
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject grid = Instantiate(gridPrefab) as GameObject;
                grid.transform.position = new Vector3((float)(((x_size + gap) * scale_x * x + (x_size + gap) * scale_x * y) + location_x), (float)(((-gap - y_size) * scale_y * x + (gap + y_size) * scale_y * y) + location_y), 0f);
                grid.transform.SetParent(grid_All.transform);//하나로 뭉치기
                gridArray.Add(grid);//데이터 정보 저장
                Debug.Log(grid);
            }
        }
    }
    protected virtual void seperatedGridArea()
    {
        for (int i = 0; i < gridArray.Count; i++)//Basic Player Field 
        {
            // 분할하는 부분 나누기 (원래는 6을 기준으로 나뉨) (1.17 재윤 수정)
            if (i < gridArray.Count - 6) playergridArray.Add(gridArray[i]);
            else monstergridArray.Add(gridArray[i]);

        }

    }

    public void WarningAttack(int[] indexs)
    {
        for(int i=0;i<indexs.Length;i++)
        {
            SpriteRenderer temp = gridArray[indexs[i]].GetComponent<SpriteRenderer>();
            temp.color = Color.red;
        }
    }
    public void Damage(int[] indexs)
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            GameObject temp = gridArray[indexs[i]];
            SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
            StartCoroutine("ActiveDamageField", temp);
            sr.color = new Color(1, 1, 1, 0);
        }
    }
    IEnumerator ActiveDamageField(GameObject go)
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;
        yield return new WaitForFixedUpdate();
        poly.enabled = false;

    }
    void Awake()
    {
        setWidth();
        Setheight();
        gridInit();
        createObject();
        createField();
        Rotation(grid_All);
        prepabMove(grid_All);
        seperatedGridArea();
        Managers.Field.setField(this);
        Field temp = Managers.Field.getField();
    }
}
