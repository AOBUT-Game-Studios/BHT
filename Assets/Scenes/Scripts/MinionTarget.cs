using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionTarget : MonoBehaviour
{
    AIDestinationSetter destination;
    public Transform[] houses;
    public bool hired = false;
    int houseIndex, prevHouseIndex;
    bool changeTarget = false;
    
    // Start is called before the first frame update
    void Start()
    {
        destination = GetComponent<AIDestinationSetter>();  
        houseIndex = Random.Range(0, houses.Length);
        prevHouseIndex = houseIndex;

        
        destination.target = houses[houseIndex];
        if(hired)
        {
            //Fill
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bowl")
        {
            changeTarget = true;
            Debug.Log("Entered Candy Bowl");
            while(changeTarget)
            {
                houseIndex = Random.Range(0, houses.Length);
                if(houseIndex != prevHouseIndex)
                {
                    destination.target = houses[houseIndex];
                    prevHouseIndex = houseIndex;
                    changeTarget = false;
                }
            }
                
        }
    }
}
