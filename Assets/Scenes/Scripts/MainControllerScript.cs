using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllerScript : MonoBehaviour
{
    // movement
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 5.0f;
    public bool enableStamina = true;
    public float maxStamina = 10.0f;
    float stamina;
    public float staminaLoseMultiplier = 1.0f;
    public float staminaGainMultiplier = 1.0f;
    public int staminaSpeed = 1;
    int staminaTimeout;
    bool staminaDisabled = false;
    float horizontal, vertical;

    // stamina bar
    public GameObject staminaBarObject;
    public GameObject healthBarObject;
    UIBar staminaBar, healthBar;

    //Animation
    Animator animator;


    // prefabs
    public GameObject projectilePrefab;

    // HP
    public float maxHP = 10.0f;
    float HP;
    Vector2 lookDirection = new Vector2(0, 0);

    //Projectile
    public float throwForce = 200;

    Rigidbody2D rb;
    

    // Start is called before the first frame update

    void Start()
    {    
        stamina = maxStamina;
        staminaTimeout = staminaSpeed;
        
        
        // HP stuff
        HP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        /*
        staminaBar = staminaBarObject.GetComponent<UIBar>();
        healthBar = healthBarObject.GetComponent<UIBar>();
        staminaBar.setBar(1.0f, 10.0f);
          */      
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if(vertical == 0 && horizontal == 0)
        {
            animator.SetBool("Walking", false);
        } else {
            animator.SetBool("Walking", true);
        }
        animator.SetFloat("MoveX", horizontal);
        animator.SetFloat("MoveY", vertical);
        

        Vector2 move = new Vector2(horizontal, vertical);
          if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
    }
    public void flashlight(Vector2 mousePosition)
    {
        Vector2 position = mousePosition - rb.position;
        Quaternion angle = new Quaternion(0.0f, 0.0f, Mathf.Atan2(position.y, position.x), 1);

        GameObject.Find("Flashlight").GetComponent<Transform>().rotation = angle;
    }
    public void launchProjectile(Vector2 mousePos)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectileController projectile = projectileObject.GetComponent<ProjectileController>();
        if(projectile != null && projectileObject != null)
        {
            projectile.launch(mousePos - rb.position, throwForce);
        }
    }
    void FixedUpdate() 
    {
        move();
    }
    void move()
    {
        // movement variables
        float speed;

        // if shift key is pressed
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = runningSpeed;
        } else
        {
            speed = walkingSpeed;
        }

        // if stamina is enabled
        if(enableStamina)
        {
            // update once a second
            if(Time.time >= staminaTimeout)
            {
                staminaTimeout = Mathf.FloorToInt(Time.time) + staminaSpeed;

                // if running
                if(speed > walkingSpeed) 
                {
                    // if running out of stamina
                    if(stamina < staminaLoseMultiplier)
                    {
                        // forced to walk
                        speed = walkingSpeed;
                        stamina = Mathf.Clamp(stamina + staminaGainMultiplier, 0, maxStamina);
                        // Debug.Log("walking and gaining");
                        staminaDisabled = true;
                    }
                    else 
                    {
                        if(staminaDisabled && stamina < Mathf.Round(maxStamina * 0.4f))
                        {
                            speed = walkingSpeed;
                            stamina = Mathf.Clamp(stamina + staminaGainMultiplier, 0, maxStamina);
                            // Debug.Log("Stamina disabled");
                        }
                        else
                        {
                            // run and lose stamina
                            stamina = Mathf.Clamp(stamina - staminaLoseMultiplier, 0, maxStamina);
                            // Debug.Log("running and losing");
                            staminaDisabled = false;
                        }
                    }
                }
                else
                {
                    // walking voluntarily and gaining
                    // Debug.Log("walking voluntarily and gaining");
                    stamina = Mathf.Clamp(stamina + staminaGainMultiplier, 0, maxStamina);
                }
                // Debug.Log("Stamina: " + stamina + "/" + maxStamina + ". Speed: " + speed + "/" + runningSpeed);
            }
            // staminaBar.setBar(stamina, maxStamina);
        }

        if(staminaDisabled) speed = walkingSpeed;
        Vector2 position = rb.position;
        position.x += Time.deltaTime * speed * horizontal;
        position.y += Time.deltaTime * speed * vertical;
        
        rb.MovePosition(position);
    }


    public void changeHealth(float value) {
            HP = Mathf.Clamp(HP + value, 0.0f, maxHP);
            // healthBar.setBar(HP, maxHP);
            if(HP <= 0.0f)
            {
                // player is dead
            }
    }
}
