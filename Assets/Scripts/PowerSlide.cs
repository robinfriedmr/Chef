using UnityEngine;
using System.Collections;

public class PowerSlide : MonoBehaviour
{
    bool upOrDown = true; //whether we are moving up or down
    bool stopped = false;    //whether we have stopped or not
    public float upSpeed;    //our upwards movement speed
    public float downSpeed;    //our downwards movement speed
    public float maxHeight;    //the max height at which we will change direction
    public float minHeight;    //the min height at which we will change direction
    public float MinMaxDamageHeight;    //the minimum height we must be at to win
    public float MaxMaxDamageHeight;
    public float MaxMedDamageHeight;//the maximum height we must be at to win
    public float MinMedDamageHeight;
    public float MinMinDamageHeight;
    public float MaxMinDamageHeight;
    public CanvasGroup PowerBar;
    public Transform transform;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {    //if space is hit
            stopped = true;    //then stop
            Stopped();
            StartCoroutine(Finish());
        }
        if (!stopped)
        { //if we haven't stopped
            MoveUpDown(); //move line
        }
    }


    void MoveUpDown()
    {
        if (upOrDown)
        {    //if we are moving up
            transform.Translate(Vector3.up * upSpeed);    //move up
            if (transform.position.y > maxHeight)
            {    //if we are at the max height
                upOrDown = false;    //switch to moving down
            }
        }
        else
        {    //if we are moving down
            transform.Translate(Vector3.up * -downSpeed);    //move down
            if (transform.position.y < minHeight)
            {    //if we are at the min height
                upOrDown = true; //switch to moving up
            }
        }
    }

    IEnumerator Finish()
    {
        yield return new WaitForSeconds(1);

        //Game object will turn off
        ToggleCanvasGroupActive();
        stopped = false;

    }
    public void ToggleCanvasGroupActive()
    {
        // This will set the canvas group to active if it is inactive OR set it to inactive if it is active
        PowerBar.gameObject.SetActive(!PowerBar.gameObject.activeSelf);
    }


    void Stopped()    //when we have stopped
    {
        if (transform.position.y > MinMaxDamageHeight && transform.position.y < MaxMaxDamageHeight)
        {
            Debug.Log("Max Damage!!!");
            //if line is between win min and max height we win or lose
        }
   
        if (transform.position.y > MinMedDamageHeight && transform.position.y < MaxMedDamageHeight)
        {
            Debug.Log("MED DAMAGE1!!");
            StartCoroutine(Finish());
            //if line is between win min and max height we win or lose
        }
        if (transform.position.y > MinMinDamageHeight && transform.position.y < MaxMinDamageHeight)
        {
            Debug.Log("MIN DAMAGE!!!");
            StartCoroutine(Finish());
            //if line is between win min and max height we win or lose
        }
    }
}