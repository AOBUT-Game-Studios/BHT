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
    public float fleeSpeed = 1.0f;
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
    Animator animator;

    // minion
    
    public GameObject alert;
    Transform mainCharacter;
    public AudioClip deathNoise;
    GameObject abductedMinion;



    

    void Start()
    {
        hostageZones = new GameObject[4] {
        GameObject.Find("HostageZoneNorth"),
        GameObject.Find("HostageZoneSouth"),
        GameObject.Find("HostageZoneEast"),
        GameObject.Find("HostageZoneWest")
        };
        // Get Components
        ds = gameObject.GetComponent<AIDestinationSetter>();
        path = gameObject.GetComponent<AIPath>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.transform.GetChild(0).GetComponent<Animator>();

        health = maxHealth;
        mainCharacter = GameObject.Find("MainCharacter").transform;


        roamMax = roamInterval;
        // start roaming
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(rb.position + Vector2.up * 1.5f + Vector2.right * -2.0f,  new Vector2(-1, 0) * 6f);
        Debug.DrawRay(rb.position + Vector2.up * 1.5f + Vector2.right * 2.0f,  new Vector2(1, 0) * 6f);
        Debug.DrawRay(rb.position + Vector2.up * -0.8f,  new Vector2(0, -1) * 6f);
        Debug.DrawRay(rb.position + Vector2.up * 2.45f,  new Vector2(0, 1) * 6f);
        // check if looking in direction of a minion
        RaycastHit2D hitLeft = Physics2D.Raycast(rb.position + Vector2.up * 1.5f + Vector2.right * -2.0f, new Vector2(-1, 0), 6f);
        RaycastHit2D hitRight = Physics2D.Raycast(rb.position + Vector2.up * 1.5f + Vector2.right * 2.0f,  new Vector2(1, 0), 6f);
        RaycastHit2D hitDown = Physics2D.Raycast(rb.position + Vector2.up * -0.8f,  new Vector2(0, -1), 6f);
        RaycastHit2D hitUp = Physics2D.Raycast(rb.position + Vector2.up * 2.45f,  new Vector2(0, 1), 6f);


        // put all raycast hits into an array
        RaycastHit2D[] hit = {hitLeft, hitRight, hitUp, hitDown};

        // check for all hits in every direction
        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                if(hit[i].collider.gameObject.tag == "Minion")
                {
                    // Debug.Log("Going after minion");
                    // go after minion if there isn't a minion captured already
                    if(status == "roam") 
                    {
                        chase(hit[i].collider.gameObject);
                        Debug.Log("Going after: " + hit[i].collider.gameObject.name);
                        break;
                    }
                }
                else if(hit[i].collider.gameObject.tag == "MainCharacter")
                {
                    if(status == "roam")
                    {
                        // Debug.Log("Going after: MainCharacter");
                        // chaseMainCharacter();
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
        animator.SetBool("IsAbducting", false);
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
        // if one in ten chances run really fast
        if(Random.Range(1, 10) == 1)
        {
            path.maxSpeed = rageSpeed;
        } else {
            path.maxSpeed = chasingSpeed;
        }
        ds.target = GameObject.Find("MainCharacter").transform;
    }
    public void goToHostageZone(GameObject minion)
    {
        abductedMinion = minion;
        status = "flee";
        if(minion.GetComponent<MinionTarget>().hired) {
            GameObject alertObject = Instantiate(alert, mainCharacter.position, mainCharacter.rotation, mainCharacter);
            alertObject.GetComponent<Alert>().CreateAlert(this.transform);
        }
        animator.SetBool("IsAbducting", true);
        // go to one of the hostage zones
        ds.target = hostageZones[Random.Range(0, hostageZones.Length)].transform;
        path.maxSpeed = fleeSpeed;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "HostageZone")
        {
            // go back to roaming
            if(status == "flee")
            {
                Destroy(gameObject);
            }
            Invoke("roam", 5.0f);
        }
        else if(other.tag == "Projectile")
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
            GameObject.Find("MainCharacter").GetComponent<MainControllerScript>().playClip(deathNoise);
            if(abductedMinion != null)
            {
                // important to activate the minon FIRST before anything else
                abductedMinion.SetActive(true);
                abductedMinion.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
    }
}
