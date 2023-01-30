using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideButton : MonoBehaviour
{

    bool slideBtnClicked = false;
    float speed = 100f;
    Vector3 startPos;
    public GameObject button;

    void Start()
    {
        startPos = transform.position;
    }

    private void OnMouseEnter()
    {
        
        
        Destroy(button);
    }

    void Update()
    {
        /* if (sildeBtnClicked)
         {
             transform.position = new Vector3(Input.mousePosition.x, transform.position.y, transform.position.z);
         }*/
     
        if (Input.GetMouseButton(0))
        {
            transform.position = new Vector3(Input.mousePosition.x, transform.position.y, transform.position.z);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            slideBtnClicked = false;
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos , Time.deltaTime * speed);
        }

        

    }
    public void OnClick()
    {
        slideBtnClicked = true;
    }
}
