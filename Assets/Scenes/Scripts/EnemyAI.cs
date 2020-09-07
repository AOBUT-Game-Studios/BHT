using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public float roamingSpeed = 1.9f;
    public float chasingSpeed = 5.1f;
    public float rageSpeed = 7.0f;
    public float fleeSpeed = 5.1f;
    public float maxHealth = 20.0f;
    public string status = "roam";
    public float roamInterval = 5.0f;
    float roamMax;
    float roamTime;
    GameObject[] hostageZones;
    float health;
    
    // pathfinding stuff
    AIDestinationSetter ds;
    AIPath path;
    Rigidbody2D rb;



    

    void Start()
    {
        hostageZones = new GameObject[4] {
        GameObject.Find("HostageZoneNorth"),
        GameObject.Find("HostageZoneSouth"),
        GameObject.Find("HostageZoneEast"),
        GameObject.Find("HostageZoneWest")
        };
        // Get Components
        ds = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        



        roamMax = roamInterval;
        // start roaming
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        // check if looking in direction of a minion
        RaycastHit2D hitLeft = Physics2D.Raycast(rb.position + Vector2.up * 1.45f + Vector2.right * 0.35f, new Vector2(-1, 0), 4f);
        RaycastHit2D hitRight = Physics2D.Raycast(rb.position + Vector2.up * 1.45f + Vector2.right * 0.35f,  new Vector2(1, 0), 4f);
        RaycastHit2D hitUp = Physics2D.Raycast(rb.position + Vector2.up * 1.45f + Vector2.right * 0.35f,  new Vector2(0, 1), 4f);
        RaycastHit2D hitDown = Physics2D.Raycast(rb.position + Vector2.up * 1.45f + Vector2.right * 0.35f,  new Vector2(0, -1), 7f);


        // put all raycast hits into an array
        RaycastHit2D[] hit = {hitLeft, hitRight, hitUp, hitDown};

        // check for all hits in every direction
        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                if(hit[i].collider.gameObject.tag == "Minion")
                {
                    // go after minion if there isn't a minion captured already
                    if(status != "flee") 
                    {
                        chase(hit[i].collider.gameObject);
                        // Debug.Log("Going after: " + hit[i].collider.gameObject.name);
                        break;
                    }
                }
                else if(hit[i].collider.gameObject.tag == "MainCharacter")
                {
                    if(status != "flee")
                    {
                        // Debug.Log("Going after: MainCharacter");
                        chaseMainCharacter();
                        break;
                    }
                }
            }
        }
            roamTime = Time.time;
            if(roamTime >= roamMax)
            {
                roamMax = Time.time + roamInterval;
                if(status == "roam") roam();
            }
    }
    void roam()
    {
        status = "roam";
        // set speed
        path.maxSpeed = roamingSpeed;

        // roam around random houses
        ds.target = GameObject.Find("CandyBowl" + Random.Range(0, GameObject.Find("Houses").transform.childCount)).transform;
    }
    void chase(GameObject gameObject)
    {
        status = "chase";
        path.maxSpeed = chasingSpeed;
        ds.target = gameObject.transform;

    }
    void chaseMainCharacter()
    {
        status = "chase";
        // if one in a hundred chance run really fast
        if(Random.Range(1, 100) == 1)
        {
            path.maxSpeed = rageSpeed;
        } else {
            path.maxSpeed = chasingSpeed;
        }
        ds.target = GameObject.Find("MainCharacter").transform;
    }
    public void goToHostageZone()
    {
        status = "flee";

        // go to one of the hostage zones
        ds.target = hostageZones[Random.Range(0, hostageZones.Length)].transform;
        path.maxSpeed = fleeSpeed;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "HostageZone")
        {
            // go back to roaming
            Invoke("roam", 5.0f);
        } else if(other.tag == "Projectile")
        {
            changeHealth(-5.0f);
        }
        else if(other.tag == "MainCharacter")
        {
            MainControllerScript controller = other.gameObject.GetComponent<MainControllerScript>();
            if(controller != null)
            {
                controller.changeHealth(-5.0f);
            }
        }
    }
    void changeHealth(float value)
    {
        health = Mathf.Clamp(health + value, 0.0f, maxHealth);
        if(health <= 0.0f)
        {
            // death animation?
            Destroy(gameObject);
        }
    }
}
