using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 클래스는 PlayerText클래스이며 FieldObject를 상속받아 생성. FieldObject가 MonoBehaviour를 상속받고 있다. (1.17 재윤 추가)
// Issue (1.17) 처음 시작할 때 키보드 한번을 생략하고 시작함
public class PlayerTest : FieldObject
{
    
    // Start is called before the first frame update
    void Start()
    {
        // type을 초기화하고 objectField를 받아온 뒤, objectList에 PlayerField를 받아온다.
        type = 1;
        objectField = Managers.Field.getField();
        objectList = objectField.getGridArray(type);
        
        currentInd = objectList.Count / 2 - 1; // 이 초기화의 위치는 Field의 Width가 어떻든, 가운데에 오게할 수 있음 (1.18 재윤 추가)
        transform.position = objectList[currentInd].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        BitBehave();
    }

    protected override void BitBehave()
    {
        if (!Managers.Bpm.Able)
            return;
        if (Input.GetKeyDown(KeyCode.W))
            mayGo(Define.PlayerMove.Up);
        else if (Input.GetKeyDown(KeyCode.A))
            mayGo(Define.PlayerMove.Left);
        else if (Input.GetKeyDown(KeyCode.S))
            mayGo(Define.PlayerMove.Down);
        else if (Input.GetKeyDown(KeyCode.D))
            mayGo(Define.PlayerMove.Right);
        else if (Input.GetKeyDown(KeyCode.K))
            Attack();
    }

    protected override void Attack()
    {
        // Hierachy 상에서 Player 안의 애니메이션을 받아오겠다는 이야기
        Transform attack = transform.GetChild(0);
        attack.GetComponent<PlayerAttack>().Attacking();
    }

    protected override void Hit()
    {
        // hit 오버라이딩 하기
    }
}
