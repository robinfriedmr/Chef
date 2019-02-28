using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public bool ready;
//    public List<GameObject> order = new List<GameObject>();
    public List<int> order = new List<int>();

    CombatantStats myStats;
    int mySpeed;

    public CombatantStats partnerStats;
    int partnerSpeed;

    GameObject enemyEncounter;
    CombatantStats enemyStats;
    int enemySpeed;

    Vector3 battlingPlayer;
    Vector3 battlingEnemy;

    private void Awake()
    {
        ready = true;
    }

    void Start () {
        battlingPlayer = new Vector3(-3f, 0.8f, 1f); // May need experimentation as sprites change.
        battlingEnemy = new Vector3(3, 2, 1);

        myStats = GetComponent<CombatantStats>();
        mySpeed = myStats.speed;
        if (partnerStats != null) {
            partnerSpeed = partnerStats.speed;
        }

        Debug.Log(mySpeed + " is my speed.");
    }
	
	void Update () {
		
	}

	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log("Collision detected.");
        if (ready == true) {
            if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "Player")
            {
                // Identify enemy speed stat. 
                enemyStats = collision.gameObject.GetComponent<CombatantStats>();
                enemySpeed = enemyStats.speed;
                Debug.Log(enemySpeed + " is the enemy's speed.");

                // Keep these when loading battle scene!
                enemyEncounter = collision.gameObject;
                DontDestroyOnLoad(enemyEncounter);
                DontDestroyOnLoad(this.gameObject);

                // Compute turn-order.
                OrderTurns();

                // Move the player and enemy into position.
                Reposition(collision);

                // Switch scenes on collision, change battle readiness
                Scene currentScene = SceneManager.GetActiveScene();
                string sceneName = currentScene.name;
                if (sceneName != "BattleScene")
                {
                    SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
                }
                Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);

                ready = false;
            }
        }
    }

    void OrderTurns () {
        // The partner (delivery girl) is always faster than the player (chef).

        order.Add(mySpeed);
        order.Add(enemySpeed);
        if (partnerStats != null)
        {
            Debug.Log("adding partnerSpeed because partnerStats is " + partnerStats);
            order.Add(partnerSpeed);
        } else {
            Debug.Log("partnerStats is " + partnerStats + "; trim excess.");
            order.TrimExcess();
            Debug.Log("capactiy of order is " + order.Capacity);
        }

        order.Sort();
        order.Reverse();

        foreach(int speed in order)
        { Debug.Log(speed); }
        

        //Adds these as GameObjects.
        /*
        order.Add(this.gameObject);
        order.Add(enemyEncounter);
        if (partnerStats.gameObject != null)
        {
            order.Add(partnerStats.gameObject);
        }
        */

        /*
        if (player >= enemy) {
            order[0] = partnerStats.gameObject; // The gameObject referenced for partnerStats.
            order[1] = this.gameObject; // The chef/player character.
            order[2] = enemyEncounter; // The enemy collided with/referenced for enemyStats.
        } else if (player < enemy) {
            if (partner < enemy) {
                order[0] = enemyEncounter;
                order[1] = partnerStats.gameObject;
                order[2] = this.gameObject;
            } else if (partner >= enemy) {
                order[0] = partnerStats.gameObject;
                order[1] = enemyEncounter;
                order[2] = this.gameObject;
            }
        } */
    }

    void Reposition (Collision enemy) {
        // Stabilize Player
        this.GetComponent<Transform>().position = battlingPlayer;
        Rigidbody myBody = this.GetComponent<Rigidbody>();
        myBody.constraints = RigidbodyConstraints.FreezeAll;

        // Stabilize Enemy
        enemy.gameObject.GetComponent<Transform>().position = battlingEnemy;

    }
}
