using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public GameObject persistence;
    Vector3 overworldPos;

    public bool ready;

    public List<GameObject> combatants = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> allies;

    CombatantStats myStats;
    int mySpeed;

    CombatantStats partnerStats; //*** Later implementation.
    int partnerSpeed;

    GameObject enemyEncounter;
    CombatantStats enemyStats;
    int enemySpeed;

    public IOrderedEnumerable<GameObject> attackOrder;
    public List<GameObject> order;

    Vector3 battlingPlayer;
    Vector3 battlingEnemy;

    void Awake()
    {
        ready = true;
    }

    void Start () {
        battlingPlayer = new Vector3(-3f, 0.8f, 1f); // May need experimentation as sprites change.
        battlingEnemy = new Vector3(3f, 0f, 1f);

        myStats = GetComponent<CombatantStats>();
        mySpeed = myStats.speed;
        if (partnerStats != null) {
            partnerSpeed = partnerStats.speed;
        }

        Debug.Log(mySpeed + " is my speed.");
    }

	void OnCollisionEnter(Collision collision)
	{
        if (ready == true) {
            if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "Ally")
            {
                PrepareForBattle(collision);
            }
        }
    }

    void PrepareForBattle (Collision collision) {
        // Let the enemy array persist.
        DontDestroyOnLoad(persistence);

        // Keep the enemy (and player) GameObject when loading battle scene!
        enemyEncounter = collision.gameObject;  
        DontDestroyOnLoad(enemyEncounter); //***
        DontDestroyOnLoad(this.gameObject); //***

        // Identify enemy speed stat. 
        enemyStats = enemyEncounter.GetComponent<CombatantStats>();
        enemySpeed = enemyStats.speed;
        Debug.Log(enemySpeed + " is enemy speed.");

        // Add GameObjects to the combatants list, then order it.
        OrderTurns(); 

        // Create separate lists from "combatants" for enemies and allies.
        enemies = order.FindAll(combatant => combatant.tag.Equals("Enemy"));
        allies = order.Except<GameObject>(enemies).ToList<GameObject>();
        allies.RemoveAll(combatant => combatant.tag.Equals("Enemy"));

        // Save position of player.
        overworldPos = this.transform.position;

        // Move the player and enemy into position.
        BattlePosition(enemyEncounter);

        // Switch scenes on collision.
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "BattleScene")
        {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            //StartCoroutine(LoadBattleScene(enemyEncounter)); //***
        }

        // Change battle readiness. (Since we're moving to the BattleScene, ...
        ready = false; //...we don't need to be ready to enter it.)
    }

   /* IEnumerator LoadBattleScene(GameObject enemyEncounter)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = true; //????????

        Scene battleScene = SceneManager.GetSceneByName("BattleScene");
        if (battleScene.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(enemyEncounter, battleScene);
            SceneManager.MoveGameObjectToScene(this.gameObject, battleScene);

            SceneManager.SetActiveScene(battleScene);
        }


    } */

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
    
        attackOrder = from combatant in combatants
                  orderby combatant.GetComponent<CombatantStats>().speed descending
                  select combatant;
        order = attackOrder.ToList(); // IOrderedEnumerator --> List
        
        for (var i = 0; i < order.Count; i++) //***
        {
            Debug.Log("The combatant is " + order.ElementAt<GameObject>(i).name + 
                ", and its speed is " + order.ElementAt<GameObject>(i).GetComponent<CombatantStats>().speed);
        } //*** 
    }

    void BattlePosition (GameObject enemy) {
        // Place Player
        this.GetComponent<Transform>().position = battlingPlayer;
        
        //*** Yes we need this. Because the player totally rocketed off into space.
        Rigidbody myBody = this.GetComponent<Rigidbody>();
        myBody.constraints = RigidbodyConstraints.FreezeAll;
        //***

        // Place Enemy
        enemy.GetComponent<Transform>().position = battlingEnemy;
    }

    public void OverworldPosition () {
        this.transform.position = overworldPos;
    }
}
