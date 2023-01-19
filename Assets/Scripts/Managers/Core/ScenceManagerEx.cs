using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceManagerEx 
{
    public BaseScene CurrentScene
    {
        //FindObjectOfType: <C# script>�� component�� ���� GameObject�� ��ȯ
        get { return GameObject.FindObjectOfType<BaseScene>(); }
    }

    public void LoadScene(string scene)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(scene);  //SceneManager: ���� �ִ� Ŭ����
    }
}
