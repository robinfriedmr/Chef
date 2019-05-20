using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaSound : MonoBehaviour {

    public AudioClip aquaStep;
    public AudioSource audioS;

    public AudioClip hurt;
    public AudioSource audioS2;

    void Step()
    {
        audioS.volume = Random.Range(0.8f, 1f); // experiment if desired
        audioS.pitch = Random.Range(1.1f, 1.4f); // experiment if desired
        audioS.PlayOneShot(aquaStep);
    }

    void AquaHurt()
    {
        audioS2.volume = Random.Range(0.8f, 1f); // experiment if desired
        audioS2.pitch = Random.Range(1.1f, 1.4f); // experiment if desired
        audioS2.PlayOneShot(hurt);
    }
}
