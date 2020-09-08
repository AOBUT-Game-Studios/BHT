using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    float horizontal, vertical, prevHorizontal, prevVertical, deltaHorizontal, deltaVertical;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        horizontal =  this.transform.position.x;
        vertical =  this.transform.position.y;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        prevHorizontal = horizontal;
        prevVertical = vertical;
        horizontal =  this.transform.position.x;
        vertical =  this.transform.position.y;
        deltaVertical = vertical - prevVertical;
        deltaHorizontal = horizontal - prevHorizontal;

        if(deltaHorizontal > 0)
        {
            animator.SetFloat("MoveX", 0.5f);
        } else if (deltaHorizontal < 0)
        {
            animator.SetFloat("MoveX", -0.5f);
        }
        
    }
}
