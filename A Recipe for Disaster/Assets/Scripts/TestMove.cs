using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMove : MonoBehaviour {

    Rigidbody _rb;
    public float speed;

	// Use this for initialization
	void Start () {
        _rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "BattleScene")
        {
            float moveV = Input.GetAxis("Vertical");
            float moveH = Input.GetAxis("Horizontal");
            Vector3 move3 = new Vector3(moveH, 0, moveV) * speed;
            _rb.AddForce(move3, ForceMode.Impulse);
        }

        // Kenny - Start 
        // Footsteps are played when arrow keys are pressed
        if (_rb.velocity.magnitude > 1f && GetComponent<AudioSource>().isPlaying == false
            && (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")))
        {
            GetComponent<AudioSource>().volume = Random.Range(0.8f, 1f); // Random volume for each step
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f); // Random pitch for each step 
            GetComponent<AudioSource>().Play();
        }
        // End
    }
}
