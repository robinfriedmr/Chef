using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady;
    List<GameObject> order;

    CombatantStats theseStats;

    bool battleStarted;
    int indexNo;

    void Start () {
        battleReady = GetComponent<BattleReady>();
        battleStarted = false;
        theseStats = this.GetComponent<CombatantStats>();	
    }
	
	void Update () {
        if (battleReady.ready == false) { // The battle has started!
            if (battleStarted == false) { // Battle not initialized.
                order = battleReady.attackOrder; // Pull in the list from BattleReady and call it "order".
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

    void PlayerAttacks () {
        if (theseStats.level >= 2) {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Attack type: 2");
                indexNo++;
                //;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Attack type: 1");
            indexNo++;
            //;
        }
    }

    void BeetAttacks () {
        Debug.Log("The beet attacks!");
        indexNo++;
    }

    void CarrotAttacks()
    { // *** Generic name for testing.
        Debug.Log("The carrot attacks!");
        indexNo++;
    }

    void OnionAttacks()
    { // *** Generic name for testing.
        Debug.Log("The onion attacks!");
        indexNo++;
    }
}
