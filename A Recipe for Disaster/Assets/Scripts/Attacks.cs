using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady;
    List<GameObject> order;
    List<GameObject> enemies;

    CombatantStats theseStats;
    CombatantStats targetStats;

    bool battleStarted;
    int indexNo;

    int dmg;
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
                indexNo = 0; // Reset to the first index position.
                battleStarted = true; // Battle initialized.
            }
            else if (enemies.Count == 0)
            {
                //end battle
                battleReady.ready = true;
                battleStarted = false;
                order.Clear();

                Debug.Log("Battle ends.");
            } else {
                if (indexNo < order.Count()) {
                    Fight(indexNo);
                } else {
                    indexNo = 0;
                }
            }
        }
    }

    void Fight(int i) {
        if (order.ElementAt<GameObject>(i).name == "PlayerCharacter")
        {
            PlayerAttacks();
        }
        else if (order.ElementAt<GameObject>(i).name == "Beet")
        {
            BeetAttacks();
        }
        else if (order.ElementAt<GameObject>(i).name == "Carrot")
        {
            CarrotAttacks();
        }
        else if (order.ElementAt<GameObject>(i).name == "Onion")
        {
            OnionAttacks();
        }
        else //*** Might not need this code since the indexNo is reset to 0 if not < .Count
        {
            Debug.Log("Error! No combatants match this indexNo.");
            indexNo++;
        } //***
    }

    void CalculateDamage (int raw, GameObject me, GameObject foe) {
        targetStats = foe.GetComponent<CombatantStats>();

        // Method 1 - Inspired by Pokemon Go.
        //int finDmg = Mathf.FloorToInt(0.5f * raw * (theseStats.power / targetStats.defense)) + 1; // Minimum hit is 1.

        // Method 2 - Abby's idea
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
        //;
        indexNo++;
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
