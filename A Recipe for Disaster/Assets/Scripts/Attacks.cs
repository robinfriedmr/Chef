using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady; //Ready to begin battle if true. If false, combat is ongoing.
    List<GameObject> order;
    List<GameObject> enemies;
    List<GameObject> allies;

    CombatantStats theseStats; //Stats for GO this script is attached to (all combatants)
    CombatantStats targetStats; 

    bool battleStarted; //Initialized?
    int indexNo; //What turn are we on?

    int chooseAttack; //For random move choice of enemies.
    int chooseTarget; //For random target choice of enemies.

    int dmg; //Raw damage of a move, before calculation.
    GameObject me;
    GameObject target;


    void Start () {
        battleReady = GetComponent<BattleReady>();
        battleStarted = false;
        theseStats = this.GetComponent<CombatantStats>();
        me = this.gameObject;
    }
	
	void Update () {
        if (battleReady.ready == false) { // The battle has started!
            if (battleStarted == false)
            { // Battle not initialized.
                order = battleReady.attackOrder; // Pull in the list from BattleReady and call it "order".
                enemies = order.FindAll(combatant => combatant.tag.Equals("Enemy")); // For targeting enemies, 
                                                                                     // make a list of enemies from "order" list.
                allies = order;
                allies.RemoveAll(combatant => combatant.tag.Equals("Enemy"));

                foreach (GameObject combatant in allies)
                {
                    Debug.Log("Ally: " + combatant.name);
                }

                indexNo = 0; // Reset to the first index position.
                battleStarted = true; // Battle initialized.
            }
            else if (enemies.Count == 0 || allies.Count == 0)
            {
                //end battle
                battleReady.ready = true;
                battleStarted = false;
                order.Clear();

                Debug.Log("Battle ends.");
            } else { // Battle is started and is on-going.
                if (indexNo < order.Count()) {
                    Fight(indexNo);
                } else {
                    indexNo = 0;
                }
            }
        }
    }

    void Fight(int i) {    
        if (order.ElementAt<GameObject>(i) == me)
        {
            if (me.name == "PlayerCharacter")
            {
                PlayerAttacks();
            }
            if (me.name == "Beet")
            {
                BeetAttacks();
            }
            if (me.name == "Carrot")
            {
                CarrotAttacks();
            }
            if (me.name == "Onion")
            {
                OnionAttacks();
            }
        } else
        {
            Debug.Log("It's not my turn.");
        }
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
                indexNo++;
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
            indexNo++;
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
