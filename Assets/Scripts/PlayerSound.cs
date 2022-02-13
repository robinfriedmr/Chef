using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip footStep;
    public AudioSource audioS;

    public AudioClip hurt;
    public AudioSource audioS2;

    void Step ()
    {
        audioS.volume = Random.Range(0.8f, 1f); // experiment if desired
        audioS.pitch = Random.Range(1.1f, 1.4f); // experiment if desired
        audioS.PlayOneShot(footStep);
    }

    void ChefHurt ()
    {
        audioS2.PlayOneShot(hurt);
    }
}
