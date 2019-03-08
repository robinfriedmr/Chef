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
            if (battleStarted == false) { // Battle not initialized.
                order = battleReady.attackOrder; // Pull in the list from BattleReady and call it "order".

                enemies = order.FindAll(combatant => combatant.name.Equals("Enemy"));

                foreach (GameObject enemy in enemies) { //***
                    Debug.Log(enemy.name);
                } //***

                indexNo = 0; // Reset to the first index position.
                Debug.Log("Starting at indexNo = " + indexNo);
                battleStarted = true; // Battle initialized.
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
        else
        {
            Debug.Log("Error! No named combatants match.");
            indexNo++;
        }
    }

    void CalculateDamage (int dmg, GameObject me, GameObject foe) {
        
    }

    void PlayerAttacks () {
        if (theseStats.level >= 2) {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Debug.Log("Attack type: 2");
                dmg = theseStats.level + theseStats.power;
//                indexNo++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log("Attack type: 1");
            dmg = 3;
//            indexNo++;
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
