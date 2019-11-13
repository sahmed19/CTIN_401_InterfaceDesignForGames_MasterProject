using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderDisplayFloatSODA : MonoBehaviour
{
    public FloatReference value;
    public FloatReference min;
    public FloatReference max = new FloatReference(100);
    Slider slider;

    void Start() {
        slider = GetComponent<Slider>();
    }

    void Update() {
        slider.value = value.Value;
        slider.minValue = min.Value;
        slider.maxValue = max.Value;
    }   

}
