using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAnimation : MonoBehaviour
{
    float horizontal =  GameObject.FindGameObjectWithTag("Minion").transform.position.x, 
    vertical =  GameObject.FindGameObjectWithTag("Minion").transform.position.y, 
    prevHorizontal, prevVertical, deltaHorizontal, deltaVertical;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        prevHorizontal = horizontal;
        prevVertical = vertical;
        horizontal =  GameObject.FindGameObjectWithTag("Minion").transform.position.x;
        vertical =  GameObject.FindGameObjectWithTag("Minion").transform.position.y;
        deltaVertical = vertical - prevVertical;
        deltaHorizontal = horizontal - prevHorizontal;
        if(deltaVertical == 0 && deltaHorizontal == 0)
        {
            animator.SetBool("Walking", false);
        } else {
            animator.SetBool("Walking", true);
        }
        animator.SetFloat("MoveX", deltaHorizontal);
        animator.SetFloat("MoveY", deltaVertical);
    }

    void FixedUpdate()
    {

    }
}
