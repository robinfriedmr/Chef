using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public PersistentData _pd;

    Vector3 overworldPos;

    public bool ready;

    public List<GameObject> combatants = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> allies;

    CombatantStats myStats;
    int mySpeed;

    public GameObject partner;
    CombatantStats partnerStats;
    int partnerSpeed;

    GameObject enemyEncounter;
    CombatantStats enemyStats;
    int enemySpeed;

    public IOrderedEnumerable<GameObject> attackOrder;
    public List<GameObject> order;

    Vector3 battlingPlayer;
    Vector3 battlingPartner;
    Vector3 battlingEnemy;

    public Camera overworldCam;
    public Camera battleCam;
    public AudioSource overworldMusic;

    void Awake()
    {
        ready = true;
        battleCam.enabled = false;
        overworldCam.enabled = true;
    }

    void Start () {
        // Positions may need experimentation as sprites change.
        battlingPlayer = new Vector3(-3f, 2f, 1f); 
        battlingPartner = new Vector3(-4.5f, 2f, 0f);
        battlingEnemy = new Vector3(3f, -2f, 1f);

        myStats = GetComponent<CombatantStats>();
        mySpeed = myStats.speed;
        if (partner != null) {
            partnerStats = partner.GetComponent<CombatantStats>();
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
        enemyEncounter = collision.gameObject;  

        // Set everyone (except the enemyEncounter) as inactive via PersistentData script.
        _pd.BeforeSwitch(enemyEncounter);

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
        // Move camera, too.
        overworldCam.enabled = false;
        battleCam.enabled = true;

        // Disable overworld theme.
        overworldMusic.enabled = false;

        // Switch scenes on collision.
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "BattleScene")
        {
            StartCoroutine(LoadBattleScene(enemyEncounter)); //***
        }

        // Change battle readiness. (Since we're moving to the BattleScene, ...
        ready = false; //...we don't need to be ready to enter it.)
    }

    void OrderTurns() {
        //Add GameObjects to combatants list.
        combatants.Add(this.gameObject);
        if (partner != null) {
            Debug.Log("partner is not null; adding gameObject to list.");
            combatants.Add(partner);
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
        // Place Player & Partner
        //this.GetComponent<Transform>().position = battlingPlayer;
        this.transform.position = battlingPlayer;

        partner.GetComponent<Transform>().position = battlingPartner;
        // Place Enemy
        enemy.GetComponent<Transform>().position = battlingEnemy;

        //*** Yes we need this. Because the player totally rocketed off into space.
        Rigidbody myBody = this.GetComponent<Rigidbody>();
        myBody.constraints = RigidbodyConstraints.FreezeAll;
        //***
    }

    IEnumerator LoadBattleScene(GameObject enemyException)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ModeledBattleScene", LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OverworldPosition () {
        this.transform.position = overworldPos;
        partner.transform.position = overworldPos;

        battleCam.enabled = false;
        overworldCam.enabled = true;

        // Re-enable overworld theme.
        overworldMusic.enabled = true;
    }
}
