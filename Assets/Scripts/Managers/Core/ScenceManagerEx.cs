using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceManagerEx 
{
    public BaseScene CurrentScene
    {
        //FindObjectOfType: <C# script>가 component로 붙은 GameObject를 반환
        get { return GameObject.FindObjectOfType<BaseScene>(); }
    }

    public void LoadScene(string scene)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(scene);  //SceneManager: 원래 있는 클래스
    }
}
