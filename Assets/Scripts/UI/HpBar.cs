using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void updateValue(int value)
    {
        Debug.Log($"{value} : updateValue");
        slider.value = value;
    }
}
