using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBowlController : MonoBehaviour
{
    // Start is called before the first frame update
    float scale;
    public int candy = 40;
    public int candySubtract = 10;
    
    int quality;
    
    float timer;
    void Start()
    {
        quality = Random.Range(0, 5);
        scale = Random.Range(0.34f, 0.6f);
        GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void candyUpdate() {

    }
}
