using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBattle : MonoBehaviour {

    public float aniTime = 1.0f;
    Animator controller;

    public void Start()
    {
        controller = this.GetComponent<Animator>();
    }


    public IEnumerator Hurt()
    {
        controller.SetBool("hurt", true);
        yield return new WaitForSeconds(aniTime);
        controller.SetBool("hurt", false);
        Debug.Log("Hurt has run."); //***
    }

    public IEnumerator Attacking()
    {
        controller.SetBool("attacking", true);
        yield return new WaitForSeconds(aniTime);
        controller.SetBool("attacking", false);
        Debug.Log("Attacking has run."); //***
    }
}
