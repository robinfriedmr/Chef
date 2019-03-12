using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public bool ready;

    public List<GameObject> combatants = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> allies;

    CombatantStats myStats;
    int mySpeed;

    public CombatantStats partnerStats;
    int partnerSpeed;

    GameObject enemyEncounter;
    CombatantStats enemyStats;
    int enemySpeed;

    public IOrderedEnumerable<GameObject> order;
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
        if (ready == true) {
            if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "Player")
            {
                PrepareForBattle(collision);
            }
        }
    }

    void PrepareForBattle (Collision collision) {
        // Keep the enemy GameObject when loading battle scene!
        enemyEncounter = collision.gameObject;
        DontDestroyOnLoad(enemyEncounter);
        DontDestroyOnLoad(this.gameObject);

        // Identify enemy speed stat. 
        enemyStats = enemyEncounter.GetComponent<CombatantStats>();
        enemySpeed = enemyStats.speed;
        Debug.Log(enemySpeed + " is enemy speed.");

        // Add GameObjects to the combatants list, then order it.
        OrderTurns(); 

        // Create separate lists from "combatants" for enemies and allies.
        enemies = attackOrder.FindAll(combatant => combatant.tag.Equals("Enemy"));
        allies = attackOrder;
        allies.RemoveAll(combatant => combatant.tag.Equals("Enemy"));

        // Move the player and enemy into position.
        Reposition(enemyEncounter);

        // Switch scenes on collision.
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "BattleScene")
        {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }

        // Change battle readiness. (Since we're moving to the BattleScene, ...
        ready = false; //...we don't need to be ready to enter it.)
    }

    void OrderTurns() {
        //Add GameObjects to combatants list.
        combatants.Add(this.gameObject);
        if (partnerStats != null) {
            Debug.Log("partnerStats is not null; adding its gameObject to list.");
            combatants.Add(partnerStats.gameObject);
        } else {
            //Debug.Log("There is no partnerStats value; no partner gameObject added to list.");
        }
        combatants.Add(enemyEncounter);
    
        order = from combatant in combatants
                  orderby combatant.GetComponent<CombatantStats>().speed descending
                  select combatant;
        attackOrder = order.ToList(); // IOrderedEnumerator --> List
        
        for (var i = 0; i < attackOrder.Count(); i++) //***
        {
            Debug.Log("The combatant is " + attackOrder[i] + ", and its speed is " + attackOrder[i].GetComponent<CombatantStats>().speed);
        } //*** 
    }

    void Reposition (GameObject enemy) {
        // Place and stabilize Player
        this.GetComponent<Transform>().position = battlingPlayer;
        Rigidbody myBody = this.GetComponent<Rigidbody>();
        myBody.constraints = RigidbodyConstraints.FreezeAll;

        // Place Enemy
        enemy.GetComponent<Transform>().position = battlingEnemy;
    }
}
