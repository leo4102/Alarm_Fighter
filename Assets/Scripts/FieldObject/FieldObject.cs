using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// �� Ŭ������ FieldObject�̸�, Player�� Monster���� ��ӵ� �θ� Ŭ�����̴�. (1.17 ���� �߰�)
public abstract class FieldObject : MonoBehaviour       //MonsterVer2�� PlayerTest�� ��ӹ޴´�
    //abstractŬ������ ����?
{
    // ��� ���� �� �ֵ��� protected�� ���� ������ ����
    public int currentInd = 0, moveInd = 0;  // ���� ��ġ�� Ÿ���� �ε��� 
    protected int type = 0;                     // �� FieldObject Ŭ������ ��� ���� ����Ŭ������ Ÿ�� (type : 1 -> �÷��̾� / type : 2 -> ����)
    protected Field objectField;                // Scene�� ������ Field�� ���� �� �ִ� Field�� ����
    protected List<GameObject> objectList;      // Field�� ���� �޾ƿ� Ÿ���� ����Ʈ -> �� Ŭ������ ��� ���� Ŭ������ Start���� �ʱ�ȭ ����.
    
    // mayGo �Լ� -> Player,Monster�� ��ġ�� �Ű��ָ� direction�� �Ű������� �޴´�. -> Player�� Monster ��� Direction�� �Ѱ��ָ� ��.
    protected void mayGo(Define.PlayerMove direction) 
    {
        try
        {
            // �÷��̾ ������ ��ġ�� Ÿ���� �ε���
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
            // transform.position�� ���� ��ġ�� �Ű���� ��. -> �̶� objectList�� ������ �� IndexOutOfRange�� ���´ٸ� �ű� �� ���� ��Ȳ�̹Ƿ� ���� ó��
            transform.position = objectList[moveInd].transform.position;
            currentInd = moveInd;
        }
        catch (ArgumentOutOfRangeException)  // �α׸� ���, �������� ������ return
        {
            return;
        }
    }

    // �������̵� �ؾߵǴ� �Լ���
    protected virtual void BitBehave() {   }

    protected virtual void Attack() { }

    protected virtual void Hit() { }

    protected virtual void Die() { }        //�߰�?
}
