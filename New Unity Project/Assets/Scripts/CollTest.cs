using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollTest : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void OnTriggerEnter(Collider Other)
    {
        Destroy(gameObject);
    }

    
}
