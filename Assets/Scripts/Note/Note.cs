using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
  
    public void CreateNote()
    {
        Transform parent = transform.parent;
        GameObject go = Managers.Resource.Instantiate("Note", parent);
        go.GetComponent<Animator>().speed = Managers.Bpm.GetAnimSpeed();
    }

    public void Destroy()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
