using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageWeapon : MonoBehaviour
{
    public void toWoodSword()
    {
        Destroy(GetComponent<Weapon>());

        gameObject.AddComponent<WoodSword>();
    }

    public void toDiaSword()
    {
        Destroy(GetComponent<Weapon>());

        gameObject.AddComponent<Sword>();

    }
}
