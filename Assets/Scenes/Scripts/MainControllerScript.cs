using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllerScript : MonoBehaviour
{
    // movement
    public float walkingSpeed;
    public float runningSpeed;
    public bool enableStamina;
    float stamina;
    public float maxStamina;
    public float staminaLoseMultiplier;
    public float staminaGainMultiplier;
    public int staminaSpeed;
    int staminaTimeout;
    bool staminaDisabled;

    // stamina bar
    UIBar staminaBar;
    UIBar healthBar;



    // HP
    public float maxHP;
    float HP;
    
    Rigidbody2D rb;

    // Start is called before the first frame update

    void Start()
    {    
        // movement
        walkingSpeed = 2.0f;
        runningSpeed = 5.0f;

        // stamina stuff
        enableStamina = true;
        maxStamina = 10.0f;
        stamina = maxStamina;
        staminaLoseMultiplier = 1.0f;
        staminaGainMultiplier = 1.0f;
        staminaSpeed = 1;
        staminaTimeout = staminaSpeed;
        staminaDisabled = false;
        staminaBar = GameObject.Find("StaminaBar").GetComponent<UIBar>();
        healthBar = GameObject.Find("HealthBar").GetComponent<UIBar>();
        
        // HP stuff
        maxHP = 10.0f;
        HP = maxHP;
        rb = GetComponent<Rigidbody2D>();
                
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate() 
    {
        move();
    }
    void move()
    {
        // movement variables
        float horizontal, vertical, speed;
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        // if pressing left and right at the same time
        if (right && left) 
        {
            horizontal = 0.0f;
        }
        else if(right) 
        {
            horizontal = 1.0f;
        }
        else if(left)
        {
            horizontal = -1.0f;
        }
        else
        {
            horizontal = 0.0f;
        }

        // if pressing up and down at the same time
        if(up && down) 
        {
            vertical = 0.0f;
        }
        if(up)
        {
            vertical = 1.0f;
        }
        else if(down)
        {
            vertical = -1.0f;
        }
        else
        {
            vertical = 0.0f;
        }

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
            staminaBar.setBar(stamina, maxStamina);
        }

        if(staminaDisabled) speed = walkingSpeed;
        Vector2 position = rb.position;
        position.x += Time.deltaTime * speed * horizontal;
        position.y += Time.deltaTime * speed * vertical;
        
        rb.MovePosition(position);
    }


    public void changeHealth(float value) {
        if(HP + value > 0.0f) {
            HP = Mathf.Clamp(HP + value, 0, maxHP);
            healthBar.setBar(HP, maxHP);
        } else {
            // player is dead
            HP = 0.0f;
            healthBar.setBar(HP, maxHP);
        }
    }
}
