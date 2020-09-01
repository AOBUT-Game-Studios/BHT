using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionTarget : MonoBehaviour
{
    AIDestinationSetter destination;
    public GameObject house;
    // Start is called before the first frame update
    void Start()
    {
        destination = GetComponent<AIDestinationSetter>();
        destination.target = transform.Find(house.name);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
