using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject player;
    Transform pTrans;

    int moveSpeed = 10;
    public float MinDist;

    Animator myAnimator;
    Transform myTrans;
    int facing;

    // Use this for initialization
    void Start()
    {
        pTrans = player.GetComponent<Transform>();

        myAnimator = GetComponent<Animator>();
        myTrans = GetComponent<Transform>();
        facing = 3;

        Physics.IgnoreLayerCollision(8, 10);
    }

    // Update is called once per frame
    void LateUpdate() // Changed from Update to LateUpdate
    {
        Vector3 myPos = myTrans.position;
        Vector3 playerPos = pTrans.position;

        if (Vector3.Distance(myPos, playerPos) >= MinDist)
        {
         transform.position = Vector3.MoveTowards(myPos, playerPos, moveSpeed * Time.deltaTime);
         myAnimator.SetBool("walking", true);
        }
        else if (Vector3.Distance(myPos, playerPos) <= MinDist)
        {
            myAnimator.SetBool("walking", false);
        }

        if (myPos.x < playerPos.x)
        {
            facing = 0;
        } else if (myPos.x > playerPos.x)
        {
            facing = 2;
        } else
        {
            facing = 3;
        }
    }
}
