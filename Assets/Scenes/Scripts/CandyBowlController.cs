using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBowlController : MonoBehaviour
{
    // Start is called before the first frame update
    float scale;
    public int candy = 40;
    public int candySubtract = 10;
    public int amount = 10;
    
    int quality;
    
    float timer;
    public bool collectable = true;
    CandyHoleController holeController;
    void Start()
    {
        quality = Random.Range(0, 5);
        scale = Random.Range(0.34f, 0.6f);
        GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
        holeController = GameObject.Find("CandyHole").GetComponent<CandyHoleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        } else if (timer < 0)
        {
            collectable = true;
        }
    }
    public void candyUpdate() {

    }
    //Timer For when player collects candy
    public void collectCandy()
    {
        collectable = false;
        timer = 10.0f;
        holeController.candy += amount;
    }
}
