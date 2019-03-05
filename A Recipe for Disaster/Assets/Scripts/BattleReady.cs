using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public bool ready;
    public List<GameObject> combatants = new List<GameObject>();

    CombatantStats myStats;
    int mySpeed;

    public CombatantStats partnerStats;
    int partnerSpeed;

    GameObject enemyEncounter;
    CombatantStats enemyStats;
    int enemySpeed;

    public IOrderedEnumerable<GameObject> newOrder;
    public List<GameObject> attackOrder;

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

    void OrderTurns() {
        // The partner (delivery girl) is always faster than the player (chef).

        //Adds these as GameObjects.
        combatants.Add(this.gameObject);
        if (partnerStats != null)
        {
            Debug.Log("partnerStats is not null; adding its gameObject to list.");
            combatants.Add(partnerStats.gameObject);
        } else
        {
            Debug.Log("There is no partnerStats value; no partner gameObject added to list.");
        }
        combatants.Add(enemyEncounter);
    
        newOrder = from combatant in combatants
                  orderby combatant.GetComponent<CombatantStats>().speed descending
                  select combatant;
        attackOrder = newOrder.ToList();
        
        for (var i = 0; i < attackOrder.Count(); i++) // ****
        {
            Debug.Log("The combatant is " + attackOrder[i] + ", and its speed is " + attackOrder[i].GetComponent<CombatantStats>().speed);
        } // **** 
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
