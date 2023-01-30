using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderButton : MonoBehaviour, IDragHandler ,IEndDragHandler
{

    Vector3 startPos;
    float startLocal_x;
    public float speed = 15f;
   
    void Start()
    {
        startPos = transform.localPosition;
        startLocal_x =  Mathf.Abs(startPos.x);//37.5f
        Debug.Log(transform.parent.name);//SliderBar
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        /*GameObject parent = transform.parent.gameObject;
        Debug.Log(parent.name);
        GameObject sliderText = Util.FindChild(parent, "SliderText");
        //Debug.Log(sliderText.name);

        sliderText.SetActive(false);*/
        
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(Mathf.Clamp(position.x + eventData.delta.x, -startLocal_x, startLocal_x), position.y, position.z);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;
        //-startLocal_x < position.x &&
        if ( position.x < startLocal_x)
        {
            transform.localPosition = new Vector3(-startLocal_x, position.y, position.z);
            //transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, speed);
            return;
        }

        Managers.SceneManagerEx.LoadScene("Stage");
    }
}
