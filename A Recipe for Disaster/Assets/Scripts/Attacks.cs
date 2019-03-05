using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    BattleReady battleReady;
//    GameObject[] order;
    List<GameObject> order;

    CombatantStats stats;

    void Start () {
        battleReady = GetComponent<BattleReady>();
        stats = this.GetComponent<CombatantStats>();	
    }
	
	void Update () {
        if (battleReady.ready == false) {
            // Pull in the IOrderedEnumerable from BattleReady and call it "order".
            order = battleReady.attackOrder;

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
