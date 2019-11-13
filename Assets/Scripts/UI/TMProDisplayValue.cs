using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMProDisplayValue : MonoBehaviour
{
    
    public SODAVariable soda;
    public string beforeText = "";
    public string afterText = "";
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        text.text = beforeText + soda.ToString() + afterText;
    }

}
