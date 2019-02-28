using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady;
//    GameObject[] order;
    List<int> order;

    CombatantStats stats;

    void Start () {
        battleReady = GetComponent<BattleReady>();
        stats = this.GetComponent<CombatantStats>();	
    }
	
	void Update () {
        if (battleReady.ready == false) {
            Debug.Log("battleReady bool is " + battleReady.ready + ". We are in " + SceneManager.GetActiveScene().name + ". Engage!");
            order = battleReady.order;

            // Use the order to check against what gameObject.name or .tag this is, determine if this.gameObject
            //...is allowed to make its move.

            PlayerAttacks();
        }
    }

    void PlayerAttacks () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Attack type: 1");
            //;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Attack type: 2");
            //;
        }
    }

    void Attack1 () {

    }

    void Attack2 () {

    }
}
