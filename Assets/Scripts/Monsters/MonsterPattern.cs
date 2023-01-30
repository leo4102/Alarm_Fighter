using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterPattern
{
    public abstract int[] calculateIndex(int currentInd);

    protected int GetGridIndex(int index)
    {
        return index + 9;
    }

}
