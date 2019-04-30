using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleReady : MonoBehaviour {

    public PersistentData _pd;

    public bool ready;

    public List<GameObject> combatants = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> allies;

    public Rigidbody myBody;
    public Movement myMovement;
    public Animator myAnimator;

    public GameObject partner;
    public Follow partnerFollow;
    public Animator partnerAnimator;

    GameObject enemyEncounter;

    public IOrderedEnumerable<GameObject> attackOrder;
    public List<GameObject> order;

    public Vector3 battlingPlayer;
    public Vector3 battlingPartner;
    public Vector3 battlingEnemy;

    public Camera overworldCam;
    public Camera battleCam;
//    public AudioSource overworldMusic;
    public GameObject environment;
    Vector3 overworldPos;
    Vector3 monPos;
    public Vector3 standBack;

    void Awake()
    {
        ready = true;
        battleCam.enabled = false;
        overworldCam.enabled = true;
    }

    private void Start()
    {
        
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

        // Add GameObjects to the combatants list, then order it.
        OrderTurns(); 

        // Create separate lists from "combatants" for enemies and allies.
        enemies = order.FindAll(combatant => combatant.tag.Equals("Enemy"));
        allies = order.Except<GameObject>(enemies).ToList<GameObject>();
        allies.RemoveAll(combatant => combatant.tag.Equals("Enemy"));

        // Save position of player.
        overworldPos = this.transform.position;
        monPos = enemyEncounter.transform.position;

        // Move the player and enemy into position.
        BattlePosition(enemyEncounter);
        // Move camera, too.
        overworldCam.enabled = false;
        battleCam.enabled = true;

        // Disable overworld theme.
        environment.SetActive(false);

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
        partnerAnimator.SetBool("walking", false);
        partnerAnimator.SetInteger("facing", 3);
        partnerFollow.enabled = false;

        // Place Player, Partner, Enemy
        this.transform.position = battlingPlayer;
        partner.transform.position = battlingPartner;
        enemy.transform.position = battlingEnemy;

        myBody.constraints = RigidbodyConstraints.FreezeAll; //*** (The player totally rocketed off into space without this.)
        myAnimator.SetBool("walking", false);
        myAnimator.SetInteger("facing", 3);
        myMovement.enabled = false;
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

    public void RestoreOverworld () {
        // Place characters on map.
        this.transform.position = overworldPos + standBack;
        partner.transform.position = overworldPos + new Vector3(-.75f, 0f, -.75f);
        if (enemyEncounter != null) {
            enemyEncounter.transform.position = monPos;
        }

        // Restore movement.
        myMovement.enabled = true;
        partnerFollow.enabled = true;

        // Change cameras.
        battleCam.enabled = false;
        overworldCam.enabled = true;

        // Re-enable overworld environment, including sounds.
        environment.SetActive(true);
    }
}
