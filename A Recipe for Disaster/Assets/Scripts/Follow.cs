﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject player;

    int moveSpeed = 10;
    public float minDist;

    Animator myAnimator;
    public float allowance;
    int facing;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        facing = 3;

        Physics.IgnoreLayerCollision(8, 10);
    }

    void LateUpdate() // Changed from Update to LateUpdate
    {
        Vector3 myPos = transform.position;
        Vector3 playerPos = player.transform.position;

        // MOVING -- Currently jittery due to rapid alternation between walking and not.
        if (Vector3.Distance(myPos, playerPos) > minDist) // + allowance)
        {
         transform.position = Vector3.MoveTowards(myPos, playerPos, moveSpeed * Time.deltaTime);
         myAnimator.SetBool("walking", true);
        }
        else if (Vector3.Distance(myPos, playerPos) <= minDist)
        {
            myAnimator.SetBool("walking", false);
        }

        // FACING
        if (myPos.x < playerPos.x - allowance)
        {
            myAnimator.SetInteger("facing", 0);
        } else if (myPos.x > playerPos.x + allowance)
        {
            myAnimator.SetInteger("facing", 2);
        } else if (Mathf.Abs(myPos.x - playerPos.x) < allowance)
        {
            if (myPos.z - minDist + allowance > playerPos.z)
            {
                myAnimator.SetInteger("facing", 3);
            } else if (myPos.z + minDist - allowance < playerPos.z)
            {
                myAnimator.SetInteger("facing", 1);
            }
        }
    }
}
