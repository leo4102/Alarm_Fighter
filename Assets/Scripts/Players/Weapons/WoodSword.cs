using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSword : Weapon
{
    protected override void Init()
    {
        weapon = "Player/Weapons/WoodSword";
        Damage = 1;
        base.Init();
    }
    public override int[] CalculateAttackRange(int currentInd)
    {
        int[] pattern = new int[1];
        for (int i = 0; i < pattern.Length; i++)
        {
            currentInd += 3;
            pattern[i] = currentInd;

        }
        return pattern;
    }
}
