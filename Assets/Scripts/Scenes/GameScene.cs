using System.Collections.Generic;
using UnityEngine;

//spone?sponsor?

public class GameScene : BaseScene
{
    int monsterIndex = 0;           //현재 몬스터 인덱스(죽으면 증가)
    int maxMonsterNum;              //최대 몬스터 수      //monsters(List).Count 로 초기화됨

    [SerializeField]
    List<GameObject> monsters = new List<GameObject>();     //어디서 초기화? Inspector창서 마우스 드래그

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
        base.Init();            //base는 부모 클래스를 의미
        SetMaxMonsterNum();
        Managers.Game.SetMonsterCount(maxMonsterNum);
        SoundBgmPlay();         //BaseScene에 존재
        SponeMonster();
        SponeBackGround();
        SponeNoteBar();
        SponePlayer();        
        SponeField();
    }
    public void Update()
    {
        Managers.Timing.UpdatePerBit();         //매 비트마다 몬스터 행동 업데이트
        Managers.Game.CheckLeftMonster();       //남은 몬스터가 존재하는지 확인
    }
    private void SponeMonster()                 //특정(monsterIndex) 인덱스 번째 몬스터 생성
    {
        GameObject go = Instantiate(monsters[monsterIndex]) as GameObject;
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
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Player/MyPlayer");
        go = Instantiate<GameObject>(go) as GameObject;     //
        Managers.Game.CurrentPlayer = go;
    }
    private void SponeField()
    {
        GameObject go = Managers.Resource.Load<GameObject>("Prefabs/Fields/MyField");
        go = Instantiate<GameObject>(go) as GameObject;
    }
    public void NextMonsterIndex()                          //다음 몬스터 생성
    {
        if (monsterIndex < maxMonsterNum - 1)
        {
            monsterIndex++;
            SponeMonster();
        }
    }
}
