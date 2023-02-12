using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public float speed = 2.0f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(Managers.Game.CurrentPlayer.transform.position.x,transform.position.y,transform.position.z),Time.deltaTime*speed);
    }
}
