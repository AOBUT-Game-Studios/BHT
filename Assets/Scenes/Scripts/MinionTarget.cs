using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class MinionTarget : MonoBehaviour
{
    AIDestinationSetter destination;
    public Transform[] houses;
    public bool hired = true;
    int houseIndex, prevHouseIndex;
    public int maxCandy = 20;
    int candy = 0;
    public float candyPickUpTime = 3.0f;
    public float candyDropOffTime = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {

        houses = new Transform[GameObject.Find("Houses").transform.childCount];
        for(int i = 0; i < houses.Length; i++)
        {
            houses[i] = GameObject.Find("CandyBowl" + i).transform;
        }




        destination = GetComponent<AIDestinationSetter>();  
        houseIndex = Random.Range(0, houses.Length);
        prevHouseIndex = houseIndex;

        
        destination.target = houses[houseIndex];
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bowl")
        {
            Debug.Log("Entered Candy Bowl");    
            // wait for 3 seconds
            // take candy from bowl if hired
            if(hired) Invoke("pickUpCandy", candyPickUpTime);
            Invoke("changeTargets", candyPickUpTime);
        } 
        // if at drop off zone
        else if(other.tag == "DropOffZone")
        {   
            if(hired) 
            {
                Invoke("dropOffCandy", candyDropOffTime);
                Invoke("changeTargets", candyDropOffTime);
            }
        }
    }
    void changeTargets() 
    {
        if(hired && candy >= maxCandy)
        {
            destination.target = GameObject.Find("CandyPile").transform;
        }
        else
        {
            changeHouses();
        }
    }
    void changeHouses()
    {
        bool changeTarget = true;
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
    void pickUpCandy()
    {
        CandyBowlController bowl = GameObject.Find("CandyBowl" + houseIndex).GetComponent<CandyBowlController>();
        bowl.candy -= Mathf.Clamp(bowl.candySubtract, 0, maxCandy);
        candy += Mathf.Clamp(bowl.candySubtract, 0, maxCandy);
    }
    void dropOffCandy()
    {
        CandyPileController pile = GameObject.Find("CandyPile").GetComponent<CandyPileController>();
        pile.candy += candy;
        candy = 0;

        // update UI
        /*
        TextMeshPro text = GameObject.Find("CandyPileCounter").GetComponent<TextMeshPro>();
        text.SetText(pile.candy.ToString());
        */
    }
}