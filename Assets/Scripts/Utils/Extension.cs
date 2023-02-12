using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension 
{
    //go.GetOrAddComponent<Component>(); �������� ����Ҽ� �ֵ��� ����
    public static T GetOrAddComponent<T>(this GameObject go) where T: UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

}
