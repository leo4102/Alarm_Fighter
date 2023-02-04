using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    int MonsterCount;           //���� ����        //��� �ʱ�ȭ? GameSCene ��ũ��Ʈ Init()��
    public GameObject CurrentPlayer { get; set; }
    public void GameOver()      //���ΰ� ���
    {
        Managers.Clear();
        Managers.Sound.Clear();     //���ʿ�?
        Managers.Scene.LoadScene("GameOver");

        //Managers.Sound.Play("GameClear", Define.Sound.Bgm);     //GameClear�� ��µ�?�׸��� ��ο� Asset>Resources ���ʿ�?

    }
    public void StageClear()       //���� ���
    {
        Managers.Clear();
        Managers.Scene.LoadScene("StageClear");

        Managers.Sound.Clear();     //���ʿ�?
        //Managers.Sound.Play("GameClear", Define.Sound.Bgm);

    }
    public void SetMonsterCount(int num)        
    {
        MonsterCount = num;
    }
    public void MinusMonsterNum()
    {
        MonsterCount--;
    }
    public int GetCurrentMonsterNum()
    {
        return MonsterCount;
    }
    public void CheckLeftMonster()      //��� ���Ͱ� ��ٸ� �������� Ŭ����  //GameScene Update()������ ȣ��
    {
        if (MonsterCount <= 0)
            StageClear();
    }
  
}

