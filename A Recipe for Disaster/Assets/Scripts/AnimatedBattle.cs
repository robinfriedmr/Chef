using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBattle : MonoBehaviour {

    public float aniTime = 1.0f;
    Animator controller;
    SpriteRenderer _sr;

    public void Start()
    {
        controller = this.GetComponent<Animator>();
        _sr = this.GetComponent<SpriteRenderer>();
    }


    public IEnumerator Hurt()
    {
        controller.SetBool("hurt", true);
        _sr.color = new Vector4(1, 0.7f, 0.7f, 1);
        yield return new WaitForSeconds(aniTime);
        _sr.color = new Vector4(1, 1, 1, 1);

        controller.SetBool("hurt", false);
        Debug.Log("Hurt has run."); //***
    }

    public IEnumerator Healed()
    {
        _sr.color = new Vector4(0.7f, 1f, 0.7f, 1f);
        yield return new WaitForSeconds(aniTime);
        _sr.color = new Vector4(1, 1, 1, 1);
    }

    public IEnumerator Attacking()
    {
        controller.SetBool("attacking", true);
        yield return new WaitForSeconds(aniTime);
        controller.SetBool("attacking", false);
        Debug.Log("Attacking has run."); //***
    }
}
