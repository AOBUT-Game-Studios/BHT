using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBar : MonoBehaviour
{
    RectTransform rt;


    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void setBar(float value, float maxValue)
    {
        float posx = ((value / maxValue) * 90) - 90;
        rt.anchoredPosition = new Vector2(posx, 0.0f);
    }
}
