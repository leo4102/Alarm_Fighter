using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected string weapon;
    public int Damage { get; protected set;}
    //to do : Effect



    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        ArmWeapon();
    }

    public virtual int[] CalculateAttackRange(int currentInd)
    {
        return new int[1] { currentInd };
    }

    public virtual void ArmWeapon()
    {
        if (weapon == null)
            return;
        GameObject go = Util.FindChild(gameObject, "Hand");
        Managers.Resource.Instantiate(weapon, Util.FindChild(gameObject, "Hand").transform);
    }
}
