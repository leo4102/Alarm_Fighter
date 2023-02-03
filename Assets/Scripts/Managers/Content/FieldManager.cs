using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager
{
    private Field field;        //(ex)BasicField(스크립트)가 삽입됨 //Field의 Awake()문에서 초기화
    
    public void setField(Field field) { this.field = field; }
    // getField() -> 이 안의 Field형인 field 멤버 변수에 접근할 수 있도록 public으로 선언한 함수 이다. (1.17 재윤 추가)
    public Field getField() { return this.field; }

    public void WarningAttack(int[] indexs)
    {
        field.WarningAttack(indexs);
        
    }
    public void Attack(int[] indexs)
    {
        field.Damage(indexs);
    }

    public void AttackedArea(int [] indexs)
    {
        field.AttackedArea(indexs);
    }

    

}
