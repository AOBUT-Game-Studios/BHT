using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllerScript : MonoBehaviour
{
    // movement
    public float walkingSpeed;
    public float runningSpeed;
    Rigidbody2D rb;

    // Start is called before the first frame update

    void Start()
    {    
        // movement
        walkingSpeed = 2.0f;
        runningSpeed = 5.0f;
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
        float horizontal, vertical;
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
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
        Vector2 position = rb.position;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Left Shift key was pressed");
        }
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            position.x += Time.deltaTime * runningSpeed * horizontal;
            position.y += Time.deltaTime * runningSpeed * vertical;
        } else {
            position.x += Time.deltaTime * walkingSpeed * horizontal;
            position.y += Time.deltaTime * walkingSpeed * vertical;
        }
        
        rb.MovePosition(position);
    }
}
