using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    GameObject enemyEncounter;
    public Scene battle;

    Vector3 battlingPlayer;
    Vector3 battlingEnemy;
    Quaternion noRotation;

	void Start () {
        battlingPlayer = new Vector3(-3, 2, 1);
        battlingEnemy = new Vector3(3, 2, 1);
        noRotation = new Quaternion(0, 0, 0, 0);
    }
	
	void Update () {
		
	}

	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log("Collision detected.");

        // Keep these when loading battle scene!
        enemyEncounter = collision.gameObject;
        DontDestroyOnLoad(enemyEncounter);
        DontDestroyOnLoad(this.gameObject);

        // But move the player and enemy into position.
        Reposition(collision);

        // Switch scenes on collision
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "BattleScene") {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
        Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
    }

    void Reposition (Collision enemy) {
        // Stabilize Player
        this.GetComponent<Transform>().position = battlingPlayer;

        Rigidbody myBody = this.GetComponent<Rigidbody>();
        myBody.constraints = RigidbodyConstraints.FreezeAll;
        //myBody.velocity = Vector3.zero;

        // Stabilize Enemy
        enemy.gameObject.GetComponent<Transform>().position = battlingEnemy;

        // (no rigidbody!)
        //enemy.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
