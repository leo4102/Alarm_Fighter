using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicField : Field         //Field(GameObject)에 삽입됨(GameScene(스크립트)가 실행되면 생성)
{
    public override void Setheight()    //Field의 Setheight()를 오버라이딩
    {
        height = 5;
    }
   
    public override void setWidth()
    {
        width = 3;
    }

}
