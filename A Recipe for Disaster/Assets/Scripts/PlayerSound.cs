using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip footStep;
    public AudioSource audioS;

	void Step ()
    {
        audioS.PlayOneShot(footStep);
    }
}
