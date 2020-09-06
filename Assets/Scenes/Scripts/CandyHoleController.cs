using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CandyHoleController : MonoBehaviour
{
    public int candy = 0;
    string candySTR;
    TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GameObject.Find("CandyCounter").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        candySTR = candy.ToString();
        textMeshProUGUI.SetText(candySTR);
    }

}
