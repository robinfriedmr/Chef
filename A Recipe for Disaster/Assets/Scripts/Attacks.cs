using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady;
    List<GameObject> order;

    List<CombatantStats> _combatantStats;

    bool battleStarted;
    int indexNo;

    void Start () {
        battleReady = GetComponent<BattleReady>();
        battleStarted = false;
        //stats = this.GetComponent<CombatantStats>();	
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
        else if (order.ElementAt<GameObject>(i).tag == "Enemy")
        {
            EnemyAttacks();
        }
        else
        {
            Debug.Log("Error! No element available.");
            indexNo = 0;
        }
    }

    void PlayerAttacks () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Attack type: 1");
            indexNo++;
            //;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Attack type: 2");
            indexNo++;
            //;
        }
    }

    void EnemyAttacks () { // *** Generic name for testing.
        Debug.Log("The enemy attacks!");
        indexNo++;
    }

    void BeetAttacks () {

    }
}
