using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    string background;

    [SerializeField]
    List<GameObject> monsters;

    [SerializeField]
    int monsterCount;

    [SerializeField]
    Field field;

    [SerializeField]
    AudioClip BGM;

    

    public override void Clear()
    {

    }

    protected override void Init()
    {
        base.Init();
        Managers.Game.SetMonsterCount(monsterCount);


    }

}
