using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject Player;
    private Vector3 PlayerPos;
    int MoveSpeed = 10;
    float MinDist = 1.5f;
    public Animator myAnimator;
    public SpriteRenderer mySpriteRenderer;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(8, 10);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPos = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);

        if (Vector3.Distance(transform.position, PlayerPos) >= MinDist)
        {

         transform.position = Vector3.MoveTowards(transform.position, PlayerPos, MoveSpeed * Time.deltaTime);
         myAnimator.SetBool("Movement", true);

        }
        else if (Vector3.Distance(transform.position, PlayerPos) <= MinDist)
        {
            myAnimator.SetBool("Movement", false);
        }
        if (Input.GetKey(KeyCode.A))
        {

            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.flipX = true;
            }
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.flipX = false;
            }
        }
    }
}
