using System.Collections.Generic;
using UnityEngine;

//spone?sponsor?

public class GameScene : BaseScene
{
    int monsterIndex = 0;           //���� ���� �ε���(����� ��)
    int maxMonsterNum;              //�ִ� ���� ��      //monsters(List).Count �� �ʱ�ȭ��

    [SerializeField]

    List<GameObject> monsters = new List<GameObject>();     //��� �ʱ�ȭ? Inspectorâ�� ���콺 �巡��


    [SerializeField]
    GameObject backGround;

    int GetMaxMonsterNum() { return maxMonsterNum; }
    void SetMaxMonsterNum() { maxMonsterNum = monsters.Count; }

    public override void Clear()
    {
        Managers.Timing.Clear();
        monsters.Clear();
        monsterIndex = 0;       //?????
    }

    protected override void Init()
    {
        base.Init();            //base�� �θ� Ŭ������ �ǹ�
        SetMaxMonsterNum();
        Managers.Game.SetMonsterCount(maxMonsterNum);
        SoundBgmPlay();         //BaseScene�� ���
        SponeMonster();
        SponeBackGround();
        SponeNoteBar();
        SponePlayer();
        SponeField();
    }
    public void Update()
    {
        Managers.Timing.UpdatePerBit();         //�� ��Ʈ���� ���� �ൿ ����Ʈ
        Managers.Game.CheckLeftMonster();       //��� ���Ͱ� ����ϴ��� Ȯ��
    }
    private void SponeMonster()                 //Ư�(monsterIndex) �ε��� ��° ���� ��
    {
        GameObject go = Managers.Resource.Instantiate(monsters[monsterIndex]) as GameObject;
    }
    private void SponeBackGround()              //
    {
        GameObject go = Instantiate(backGround) as GameObject;      //
        go = Instantiate<GameObject>(go) as GameObject;     //?????
    }
    private void SponeNoteBar()
    {
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/NoteBar/NoteBar");
        go = Instantiate(go) as GameObject;
    }
    private void SponePlayer()
    {
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Player/Player");
        go = Instantiate<GameObject>(go) as GameObject;     //
        Managers.Game.CurrentPlayer = go;
    }
    private void SponeField()
    {
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Fields/Field");
        go = Instantiate<GameObject>(go) as GameObject;
    }
    public void NextMonsterIndex()                          //��� ���� ��
    {
        if (monsterIndex < maxMonsterNum - 1)
        {
            monsterIndex++;
            SponeMonster();
        }
    }
}
