using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    public BattleReady battleReady;
    public bool battleStarted;

    public WhoseTurn whoseTurn;
    public int indexNo; //Steal this from WhoseTurn script on player.

    // Pull these during intialization from BattleReady script.
    public List<GameObject> order;
    public List<GameObject> enemies;
    public List<GameObject> allies;

    CombatantStats theseStats; //Stats for GO this script is attached to (all combatants)
    CombatantStats targetStats; 

    int chooseAttack; //For random move choice of enemies.
    int chooseTarget; //For random target choice of enemies.

    int dmg; //Raw damage of a move, before calculation.
    GameObject me;
    GameObject target;


    void Start () {
        indexNo = whoseTurn.GetComponent<WhoseTurn>().indexNo; // Grab indexNo
        theseStats = this.GetComponent<CombatantStats>();
        me = this.gameObject;
        Debug.Log("my name is " + me.name);
    }
	
	void Update () {
        if (battleReady.ready == false)
        { // The battle has started!
            if (battleStarted == false)
            { // Battle not initialized.
                Debug.Log("Initializing.");

                // Pull in the lists from BattleReady.
                order = battleReady.attackOrder;
                enemies = battleReady.enemies;
                allies = battleReady.allies;

                whoseTurn.indexNo = 0; // Reset to the first index position.
                battleStarted = true; // Battle initialized.
            } else {
                Debug.Log("The battle has started/continues. whoseTurn.indexNo is " + whoseTurn.indexNo);
                Fight(whoseTurn.indexNo); //******************************ERROR***
            }
            
        }

        //indexNo = whoseTurn.indexNo;
        //Fight(indexNo);
    }

    void Fight(int i) {
        Debug.Log("Hello from the Fight() in Attacks.");
        if (order.ElementAt<GameObject>(i) == me) //******************************ERROR***
        {
            Debug.Log("The name of the element at indexNo is " + 
                order.ElementAt<GameObject>(i).name); //***

            if (me.name == "PlayerCharacter") {
                PlayerAttacks();
            } else if (me.name == "Beet") {
                BeetAttacks();
            } else if (me.name == "Carrot") {
                CarrotAttacks();
            } else if (me.name == "Onion") {
                OnionAttacks();
            }
        } /* else
        {
            Debug.Log("It's not my turn.");
        } */
    }

    void CalculateDamage (int raw, GameObject me, GameObject foe) {
        targetStats = foe.GetComponent<CombatantStats>();

        // Method A - Inspired by Pokemon Go.
        //int finDmg = Mathf.FloorToInt(0.5f * raw * (theseStats.power / targetStats.defense)) + 1; // Minimum hit is 1.

        // Method B - Abby's idea
        int finDmg = raw - targetStats.defense;
        finDmg = (finDmg < 0) ? 0 : finDmg;

        Debug.Log("Enemy is hit for " + finDmg); //***
        targetStats.HP -= finDmg; // Subtract damage from target's HP.
        Debug.Log("Enemy HP is " + targetStats.HP); //***

        if (targetStats.HP <= 0)
        {
            order.Remove(foe);
            enemies.Remove(foe);
            Destroy(foe);
            Debug.Log("Enemy defeated!");
        }
    }

    void PlayerAttacks () {
        if (theseStats.level >= 2) {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Debug.Log("Attack type: 2");
                if (theseStats.magic >= 3)
                {
                    dmg = theseStats.level + theseStats.power;
                    theseStats.magic -= 3;
                } else
                {
                    Debug.Log("Not enough magic left!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Attack type: 1");
            dmg = 3;
        }

        if (dmg != 0) {
            if (Input.GetKeyDown(KeyCode.Alpha8)) {
                target = enemies.ElementAt<GameObject>(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (enemies.ElementAt<GameObject>(1) != null) {
                    target = enemies.ElementAt<GameObject>(1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (enemies.ElementAt<GameObject>(2) != null)
                {
                    target = enemies.ElementAt<GameObject>(2);
                }
            }

            if (target != null) {
                Debug.Log("The target is " + target.name); //***
                CalculateDamage(dmg, me, target);
                dmg = 0;
                target = null;
                whoseTurn.indexNo++;
//                Debug.Log("After the attack: indexNo is " + indexNo); //***
            }
        }
    }

    void BeetAttacks () {
        Debug.Log("The beet attacks!");
        chooseAttack = Random.Range(1, 2); // choose an attack
        if (theseStats.level >= 3 && chooseAttack == 2) {
            Debug.Log("Attack type: 2");
            if (theseStats.magic >= 1) {
                dmg = theseStats.level + theseStats.power;
                theseStats.magic -= 1;
            } else {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        } else if (chooseAttack == 1) {
            Debug.Log("Attack type: 1");
            dmg = 4;
        } else {
            Debug.Log("Error. Choice 1 and 2 not chosen for some reason.");
        }

        chooseTarget = Random.Range(0, allies.Count() - 1); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            CalculateDamage(dmg, me, target);
            dmg = 0;
            target = null;
            whoseTurn.indexNo++;
        }
    }

    void CarrotAttacks()
    {
        Debug.Log("The carrot attacks!");
        //;
        indexNo++;
    }

    void OnionAttacks()
    { 
        Debug.Log("The onion attacks!");
        //;
        indexNo++;
    }
}