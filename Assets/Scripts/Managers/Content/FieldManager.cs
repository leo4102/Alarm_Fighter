using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class FieldManager
{
    private MyField myField;
    public void SetMyField(MyField myField)
    {
        this.myField = myField;
    }

    public MyField GetMyField()
    {
        return this.myField;
    }

    public GameObject GetGrid(int latitude, int longtitude) { return myField.GetGrid(latitude, longtitude); }

    public int GetHeight() { return myField.GetHeight(); }
    public int GetWidth() { return myField.GetWidth(); }

    //------------------------------------------------------------------------------------------------------------
    private Field field;        //(ex)BasicField(��ũ��Ʈ)�� ���Ե� //Field�� Awake()������ �ʱ�ȭ

    public void setField(Field field) { this.field = field; }
    // getField() -> �� ���� Field���� field ��� ������ ������ �� �ֵ��� public���� ������ �Լ� �̴�. (1.17 ���� �߰�)
    public Field getField() { return this.field; }

    public void WarningAttack(int[] indexs)
    {
        field.WarningAttack(indexs);

    }
    public void Attack(int[] indexs)
    {
        field.Damage(indexs);
    }

    public void AttackedArea(int[] indexs)
    {
        field.AttackedArea(indexs);
    }



}
