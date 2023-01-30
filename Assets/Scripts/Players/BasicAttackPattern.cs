using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackPattern : PlayerPattern
{
    public override int[] calculatePattern(int currentInd)
    {
        int[] indexs = new int[2];
        for(int i=0; i< indexs.Length; i++)
        {
            currentInd += 3;
            indexs[i] = currentInd;
        }
        return indexs;
    }
}
