using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum MonsterMove
    {
        Left,
        Right,
        Stop,
    }
    public enum PlayerMove
    {
        Up,
        Down,
        Left,
        Right,
        Stop,
    }
    public enum State
    {
        IDLE,
        ATTACKREADY,
        ATTACK,     //����
        HIT,        //����
        MOVE,
        DIE,
        SPAWN,
        NOTSPAWN,
    }

    public enum FieldArray
    {
        LEFT,
        RIGHT,
        TOP,
        All_ONE,
    }



    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
}
