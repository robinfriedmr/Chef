using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject player;

    public float minDist;
    public float moveSpeed = 10f;
    public float smoothDamp;

    Animator myAnimator;
    public float allowance;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(8, 10);
    }

    void LateUpdate() // Changed from Update to LateUpdate
    {
        Vector3 myPos = transform.position;
        Vector3 playerPos = player.transform.position;

        if (Vector3.Distance(myPos, playerPos) > minDist)
        {
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(myPos, playerPos, //from MoveTowards...
             ref velocity, smoothDamp, moveSpeed); //...third parameter was: (moveSpeed * Time.deltaTime)
            myAnimator.SetBool("walking", true);
        }
        else if (Vector3.Distance(myPos, playerPos) <= minDist)
        {
            myAnimator.SetBool("walking", false);
        }

        // FACING
        if (myPos.x < playerPos.x - allowance)
        {
            myAnimator.SetInteger("facing", 2);
        } else if (myPos.x > playerPos.x + allowance)
        {
            myAnimator.SetInteger("facing", 0);
        } else if (Mathf.Abs(myPos.x - playerPos.x) < allowance)
        {
            if (myPos.z - minDist + allowance > playerPos.z)
            {
                myAnimator.SetInteger("facing", 1);
            } else if (myPos.z + minDist - allowance < playerPos.z)
            {
                myAnimator.SetInteger("facing", 3);
            }
        }
    }
}
