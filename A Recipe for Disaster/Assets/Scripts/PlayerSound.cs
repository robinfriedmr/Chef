using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip footStep;
    public AudioSource audioS;

	void Step ()
    {
        audioS.volume = Random.Range(0.8f, 1f); // experminet if desired
        audioS.pitch = Random.Range(0.8f, 1.1f); // experminet if desired
        audioS.PlayOneShot(footStep);
    }
}
