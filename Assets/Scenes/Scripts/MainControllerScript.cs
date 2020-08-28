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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
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
