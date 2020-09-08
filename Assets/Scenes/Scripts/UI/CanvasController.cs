using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool autoEnableAlpha = true;
    void Start()
    {
        if(autoEnableAlpha)
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            cg.alpha = 1.0f; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
