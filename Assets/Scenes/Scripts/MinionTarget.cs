using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionTarget : MonoBehaviour
{
    AIDestinationSetter destination;
    public Transform[] houses;
    public bool hired = false;
    int houseIndex;
    // Start is called before the first frame update
    void Start()
    {
        destination = GetComponent<AIDestinationSetter>();  
        houseIndex = Random.Range(0, houses.Length);

        
        destination.target = houses[houseIndex];
        if(hired)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
