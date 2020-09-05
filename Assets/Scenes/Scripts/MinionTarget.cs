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
    public float speed = 2.0f;
    public string status = "roam";
    AIPath path;
    bool abducted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<AIPath>();
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
        if (hired)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.tag == "Bowl")
            {
                if(!abducted)
                {
                    Debug.Log("Entered Candy Bowl");    
                    // wait for 3 seconds
                    // take candy from bowl if hired
                    if(hired) Invoke("pickUpCandy", candyPickUpTime);
                    Invoke("changeTargets", candyPickUpTime);
                }
            } 
            // if at drop off zone
            else if(other.tag == "DropOffZone")
            {   
                if(hired && !abducted) 
                {
                    Invoke("dropOffCandy", candyDropOffTime);
                    Invoke("changeTargets", candyDropOffTime);
                }
            }
            else if(other.tag == "Enemy")
            {
                EnemyAI eAI = other.GetComponent<EnemyAI>();
                if(eAI.status != "flee" && eAI != null)
                {
                    destination.target = other.transform;
                    eAI.goToHostageZone();
                    path.maxSpeed = eAI.GetComponent<AIPath>().maxSpeed + 20.0f;
                }
            }
            else if(other.tag == "HostageZone")
            {
                status = "hostage";
                destination.target = other.gameObject.transform;
            }
        }
    }
    public void changeTargets() 
    {
        path.maxSpeed = speed;
        if(hired && candy >= maxCandy)
        {
            destination.target = GameObject.Find("CandyHole").transform;
        }
        else
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
    }
    void pickUpCandy()
    {
        CandyBowlController bowl = GameObject.Find("CandyBowl" + houseIndex).GetComponent<CandyBowlController>();
        bowl.candy -= Mathf.Clamp(bowl.candySubtract, 0, maxCandy);
        candy += Mathf.Clamp(bowl.candySubtract, 0, maxCandy);
    }
    void dropOffCandy()
    {
        CandyHoleController pile = GameObject.Find("CandyHole").GetComponent<CandyHoleController>();
        pile.candy += candy;
        candy = 0;

        // update UI
        /*
        TextMeshPro text = GameObject.Find("CandyPileCounter").GetComponent<TextMeshPro>();
        text.SetText(pile.candy.ToString());
        */
    }
}