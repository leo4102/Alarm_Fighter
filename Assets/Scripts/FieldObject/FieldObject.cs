using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 이 클래스는 FieldObject이며, Player와 Monster에게 상속될 부모 클래스이다. (1.17 재윤 추가)
public abstract class FieldObject : MonoBehaviour       //MonsterVer2와 PlayerTest가 상속받는다
    //abstract클래스인 이유?
{
    // 상속 받을 수 있도록 protected로 접근 지정자 지정
    public int currentInd = 0, moveInd = 0;  // 현재 위치한 타일의 인덱스 
    protected int type = 0;                     // 이 FieldObject 클래스를 상속 받은 서브클래스의 타입 (type : 1 -> 플레이어 / type : 2 -> 몬스터)
    protected Field objectField;                // Scene에 생성된 Field를 담을 수 있는 Field형 변수
    protected List<GameObject> objectList;      // Field로 부터 받아온 타일의 리스트 -> 이 클래스를 상속 받은 클래스의 Start에서 초기화 해줌.
    
    // mayGo 함수 -> Player,Monster의 위치를 옮겨주며 direction을 매개변수로 받는다. -> Player와 Monster 모두 Direction을 넘겨주면 됨.
    protected void mayGo(Define.PlayerMove direction) 
    {
        try
        {
            // 플레이어가 다음에 위치할 타일의 인덱스
            moveInd = 0;
            if (direction == Define.PlayerMove.Up)
                moveInd = currentInd + objectField.getWidth();
            else if (direction == Define.PlayerMove.Down)
                moveInd = currentInd - objectField.getWidth();
            else if (direction == Define.PlayerMove.Left)
            {
                if ((currentInd % objectField.getWidth()) == 0)
                    return;
                moveInd = currentInd - 1;
            }
            else if (direction == Define.PlayerMove.Right)
            {
                if ((currentInd % objectField.getWidth()) == (objectField.getWidth() - 1))
                    return;
                moveInd = currentInd + 1;
            }
            // transform.position을 통해 위치를 옮겨줘야 함. -> 이때 objectList에 접근할 때 IndexOutOfRange가 나온다면 옮길 수 없는 상황이므로 예외 처리
            transform.position = objectList[moveInd].transform.position;
            currentInd = moveInd;
        }
        catch (ArgumentOutOfRangeException)  // 로그를 찍고, 움직이지 않으며 return
        {
            return;
        }
    }

    // 오버라이딩 해야되는 함수들
    protected virtual void BitBehave() {   }

    protected virtual void Attack() { }

    protected virtual void Hit() { }

    protected virtual void Die() { }        //추가?
}
