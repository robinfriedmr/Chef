using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {

    Rigidbody _rb;


	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float move = Input.GetAxis("Vertical");
        Vector3 move3 = new Vector3(0, 0, move);
        _rb.AddForce(move3, ForceMode.Impulse);
    }
}
