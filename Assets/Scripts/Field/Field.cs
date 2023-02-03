using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Field : MonoBehaviour                         //BasicField가 이를 상속받는다
{
    List<GameObject> gridArray = new List<GameObject>();            //전체 생성된 Diamond가 저장될 배열(field data)
    
    List<GameObject> playergridArray = new List<GameObject>();      //Player Diamond 저장 배열(field data)
    List<GameObject> monstergridArray = new List<GameObject>();     //Monster Diamond 정장 배열(field data)
    
    protected GameObject grid_All;  //전체 생성된 Diamond를 담을 부모 GameObject

    [SerializeField]
    GameObject gridPrefab;          //Isometric Diamond(Prefab)

    protected float height;         //세로        //BasicField서 초기화
    protected float width;          //가로        //BasicField서 초기화
    
    // getGridArray -> 타입 (플레이어 or 몬스터)에 따라 playergridArray 또는 monstergridArray를 반환한다. (1.17 재윤 추가)
    public List<GameObject> getGridArray(int type)
    {
        if (type == 1)              // player 
        {
            return playergridArray;
        }
        else if (type == 2)         // monster
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


    // getHeight(), getWidth() -> 만들어진 전체 타일의 width와 height를 반환한다.  (1.17 재윤 추가)
    public int getHeight() { return (int)height; }
    public int getWidth() { return (int)width; }


    private float scale_x;          //스케일 x축
    private float scale_y;          //스케일 y축
    private float location_x;       //grid의 처음 x좌표 
    private float location_y;       //grid의 처음 y좌표

    const double x_size = 0.5;      //Diamond 장축
    const double y_size = 0.25;     //Diamond 단축
    const double gap = 0.1;         //Diamond끼리의 사이간격

    public abstract void Setheight();
    public abstract void setWidth();
    
    public void Rotation(GameObject go)     //기본 회전(음수==시계방향)
    {
        go.transform.Rotate(0f, 0f, 30.0f);
    }
    protected virtual void prepabRotation(GameObject go, float theta)       //theta만큼 회전 함수
    {
        go.transform.Rotate(0f, 0f, theta);
    }

    protected virtual void prepabMove(GameObject go)
    {
        go.transform.position = new Vector3(-1.49f, 0.54f, 0f);
    }
    private void gridInit()
    {
        scale_x = gridPrefab.transform.localScale.x;        //grid scale x축
        scale_y = gridPrefab.transform.localScale.y;        //grid scale y축
        location_x = gridPrefab.transform.localPosition.x;  //grid 초기 x좌표 //-1.5f
        location_y = gridPrefab.transform.localPosition.y;  //grid 초기 y좌표 //-2.5f
        location_x = -1.5f;
        location_y = -2.47737f;
    }
    protected virtual void createObject()
    {
        grid_All = new GameObject() { name = "grid_All" };
    }
    protected virtual void createField()                    //필드 만들기
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject go = new GameObject() { name = "Diamond Parent" };
                GameObject grid = Instantiate(gridPrefab) as GameObject;
                
                grid.transform.SetParent(go.transform);       //하나로 뭉치기
                go.transform.SetParent(grid_All.transform);       //하나로 뭉치기

                go.transform.position = new Vector3((float)(((x_size + gap) * scale_x * x + (x_size + gap) * scale_x * y) + location_x), 
                    (float)(((-gap - y_size) * scale_y * x + (gap + y_size) * scale_y * y) + location_y), 0f);

                gridArray.Add(grid);                                //데이터 정보 저장
                Debug.Log(grid);
            }
        }
    }
    protected virtual void seperatedGridArea()
    {
        for (int i = 0; i < gridArray.Count; i++)       //Basic Player Field 
        {
            // 분할하는 부분 나누기 (원래는 6을 기준으로 나뉨) (1.17 재윤 수정)
            if (i < gridArray.Count - 6) playergridArray.Add(gridArray[i]);
            else monstergridArray.Add(gridArray[i]);
        }
    }

    public void WarningAttack(int[] indexs)     //몬스터가 공격할 영역:빨간색 표시    //indexs: 선택할 grid 인덱스
    {
        for(int i=0;i<indexs.Length;i++)
        {
            SpriteRenderer temp = gridArray[indexs[i]].GetComponent<SpriteRenderer>();
            temp.color = Color.red;
        }
    }

    public void AttackedArea(int[] indexs)
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            GameObject grid = gridArray[indexs[i]];
            StartCoroutine("TempChangeColor", grid);
        }
    }

    IEnumerator TempChangeColor(GameObject go)           
    {
        SpriteRenderer rend = go.GetComponent<SpriteRenderer>();
        rend.color = Color.red;                             //AttackedArea(공격한 영역) 잠깐 빨강화
        yield return new WaitForSeconds(0.3f);              //지정된 (float)초 만큼 대기
        rend.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);
    }

    public void Damage(int[] indexs)                        //Damage영역 collider 활성화 + 투명화
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            GameObject temp = gridArray[indexs[i]];
            StartCoroutine("ActiveDamageField", temp);      //코루틴을 실행하려면 StartCoroutine 함수를 사용해야 합니다.
            
            SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
            sr.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);       //원래 Isolated Diamond(prefab)색으로 돌아옴
            //sr.color = new Color(1, 1, 1, 0);                             //흰색(투명),Damage영역 투명하게
        }
    }

    //yield 구문을 포함하는 모든 함수는 코루틴으로 인식되며 IEnumerator 반환 타입은 명시적으로 선언할 필요가 없습니다.
    //yield return null은 자주 사용하는 함수인 Update()가 끝나면 그때 yield return null구문의 밑의 부분이 실행됩니다.
    //다음 FixedUpdate() 가 실행될때까지 기다리게 됩니다.
    //이 FixedUpdate()는 Update()와 달리 일정한 시간 단위(0.02초)로 호출되는 Update() 함수라고 생각하시면 됩니다.
    // FixedUpdate()가 끝나면 그때 yield return new WaitForFixedUpdate()구문의 밑의 부분이 실행됩니다.
    IEnumerator ActiveDamageField(GameObject go)            //코루틴이 다음과 같이 선언됩니다.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage영역 collider 활성화(잠깐)
        yield return new WaitForFixedUpdate();              //yield 반환 라인은 실행이 일시 중지되고 다음 프레임에서 다시 시작되는 지점
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
        Managers.Field.setField(this);      //(ex)this == BasicField(스크립트)
        Field temp = Managers.Field.getField();     //불필요?
    }
}
