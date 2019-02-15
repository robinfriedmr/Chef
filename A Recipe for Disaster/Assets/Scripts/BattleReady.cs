using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    GameObject enemyEncounter;
    public Scene battle;

    Vector3 battlingPlayer;
    Vector3 battlingEnemy;


	void Start () {
        battlingPlayer = new Vector3(-6, 2, 1);
        battlingEnemy = new Vector3(6, 2, 1);
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
        GetComponent<Transform>().position = battlingPlayer;

        Rigidbody enemyBody = GetComponent<Rigidbody>();
        enemyBody.velocity = new Vector3(0, 0, 0);
        enemyBody.rotation = new Quaternion(0, 0, 0, 0); // NOT WORKING*****

        // Stabilize Enemy
        enemy.gameObject.GetComponent<Transform>().position = battlingEnemy;

        // (no rigidbody!)
        //enemy.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
