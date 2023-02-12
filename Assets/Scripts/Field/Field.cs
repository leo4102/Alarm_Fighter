using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Field : MonoBehaviour                         //BasicField�� �̸� ��ӹ޴´�
{
    List<GameObject> gridArray = new List<GameObject>();            //��ü ������ Diamond�� ����� �迭(field data)
    
    List<GameObject> playergridArray = new List<GameObject>();      //Player Diamond ���� �迭(field data)
    List<GameObject> monstergridArray = new List<GameObject>();     //Monster Diamond ���� �迭(field data)
    
    protected GameObject grid_All;  //��ü ������ Diamond�� ���� �θ� GameObject

    [SerializeField]
    GameObject gridPrefab;          //Isometric Diamond(Prefab)

    protected float height;         //����        //BasicField�� �ʱ�ȭ
    protected float width;          //����        //BasicField�� �ʱ�ȭ
    
    // getGridArray -> Ÿ�� (�÷��̾� or ����)�� ���� playergridArray �Ǵ� monstergridArray�� ��ȯ�Ѵ�. (1.17 ���� �߰�)
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


    // getHeight(), getWidth() -> ������� ��ü Ÿ���� width�� height�� ��ȯ�Ѵ�.  (1.17 ���� �߰�)
    public int getHeight() { return (int)height; }
    public int getWidth() { return (int)width; }


    private float scale_x;          //������ x��
    private float scale_y;          //������ y��
    private float location_x;       //grid�� ó�� x��ǥ 
    private float location_y;       //grid�� ó�� y��ǥ

    const double x_size = 0.5;      //Diamond ����
    const double y_size = 0.25;     //Diamond ����
    const double gap = 0.1;         //Diamond������ ���̰���

    public abstract void Setheight();
    public abstract void setWidth();
    
    public void Rotation(GameObject go)     //�⺻ ȸ��(����==�ð����)
    {
        go.transform.Rotate(0f, 0f, 30.0f);
    }
    protected virtual void prepabRotation(GameObject go, float theta)       //theta��ŭ ȸ�� �Լ�
    {
        go.transform.Rotate(0f, 0f, theta);
    }

    protected virtual void prepabMove(GameObject go)
    {
        go.transform.position = new Vector3(-1.49f, 0.54f, 0f);
    }
    private void gridInit()
    {
        scale_x = gridPrefab.transform.localScale.x;        //grid scale x��
        scale_y = gridPrefab.transform.localScale.y;        //grid scale y��
        location_x = gridPrefab.transform.localPosition.x;  //grid �ʱ� x��ǥ //-1.5f
        location_y = gridPrefab.transform.localPosition.y;  //grid �ʱ� y��ǥ //-2.5f
        location_x = -1.5f;
        location_y = -2.47737f;
    }
    protected virtual void createObject()
    {
        grid_All = new GameObject() { name = "grid_All" };
    }
    protected virtual void createField()                    //�ʵ� �����
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject go = new GameObject() { name = "Diamond Parent" };
                GameObject grid = Instantiate(gridPrefab) as GameObject;
                
                grid.transform.SetParent(go.transform);       //�ϳ��� ��ġ��
                go.transform.SetParent(grid_All.transform);       //�ϳ��� ��ġ��

                go.transform.position = new Vector3((float)(((x_size + gap) * scale_x * x + (x_size + gap) * scale_x * y) + location_x), 
                    (float)(((-gap - y_size) * scale_y * x + (gap + y_size) * scale_y * y) + location_y), 0f);

                gridArray.Add(grid);                                //������ ���� ����
                Debug.Log(grid);
            }
        }
    }
    protected virtual void seperatedGridArea()
    {
        for (int i = 0; i < gridArray.Count; i++)       //Basic Player Field 
        {
            // �����ϴ� �κ� ������ (������ 6�� �������� ����) (1.17 ���� ����)
            if (i < gridArray.Count - 6) playergridArray.Add(gridArray[i]);
            else monstergridArray.Add(gridArray[i]);
        }
    }

    public void WarningAttack(int[] indexs)     //���Ͱ� ������ ����:������ ǥ��    //indexs: ������ grid �ε���
    {
        for(int i=0;i<indexs.Length;i++)
        {
            SpriteRenderer temp = gridArray[indexs[i]].GetComponent<SpriteRenderer>();
            temp.color = Color.red;
        }
    }

    public void AttackedArea(int[] indexs)      //�÷��̾�� �����ҿ���:������ ǥ��    
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
        rend.color = Color.red;                             //AttackedArea(������ ����) ��� ����ȭ
        yield return new WaitForSeconds(0.3f);              //������ (float)�� ��ŭ ���
        rend.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);
    }

    public void Damage(int[] indexs)                        //Damage���� collider Ȱ��ȭ + ����ȭ
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            GameObject temp = gridArray[indexs[i]];
            StartCoroutine("ActiveDamageField", temp);      //�ڷ�ƾ�� �����Ϸ��� StartCoroutine �Լ��� ����ؾ� �մϴ�.
            
            SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
            sr.color = new Color(87 / 255f, 87 / 255f, 87 / 255f, 1);       //���� Isolated Diamond(prefab)������ ���ƿ�
            //sr.color = new Color(1, 1, 1, 0);                             //���(����),Damage���� �����ϰ�
        }
    }

    //yield ������ �����ϴ� ��� �Լ��� �ڷ�ƾ���� �νĵǸ� IEnumerator ��ȯ Ÿ���� ��������� ������ �ʿ䰡 �����ϴ�.
    //yield return null�� ���� ����ϴ� �Լ��� Update()�� ������ �׶� yield return null������ ���� �κ��� ����˴ϴ�.
    //���� FixedUpdate() �� ����ɶ����� ��ٸ��� �˴ϴ�.
    //�� FixedUpdate()�� Update()�� �޸� ������ �ð� ����(0.02��)�� ȣ��Ǵ� Update() �Լ���� �����Ͻø� �˴ϴ�.
    // FixedUpdate()�� ������ �׶� yield return new WaitForFixedUpdate()������ ���� �κ��� ����˴ϴ�.
    IEnumerator ActiveDamageField(GameObject go)            //�ڷ�ƾ�� ������ ���� ����˴ϴ�.
    {
        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        poly.enabled = true;                                //Damage���� collider Ȱ��ȭ(���)
        yield return new WaitForFixedUpdate();              //yield ��ȯ ������ ������ �Ͻ� �����ǰ� ���� �����ӿ��� �ٽ� ���۵Ǵ� ����
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
        Managers.Field.setField(this);      //(ex)this == BasicField(��ũ��Ʈ)
        Field temp = Managers.Field.getField();     //���ʿ�?
    }
}
