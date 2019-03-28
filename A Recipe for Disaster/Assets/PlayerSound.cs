using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip footStep;

    public AudioSource audioS;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Footstep()
    {
        audioS.volume = Random.Range(0.8f, 1f);
        audioS.pitch = Random.Range(0.8f, 1.1f);
        audioS.PlayOneShot(footStep);
    }
}
