using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoseTurn : MonoBehaviour {

    public BattleReady battleReady; //Ready to begin battle if true. If false, combat is ongoing.
    List<GameObject> order;
    List<GameObject> enemies;
    List<GameObject> allies;

    bool battleStarted; //Initialized?
    public int indexNo; //What turn are we on?

    Attacks _attacks;

    void Start () {
        battleReady = GetComponent<BattleReady>();
        battleStarted = false;
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
            }
            else
            { // Battle is started and is on-going.
                if (indexNo < order.Count())
                {
                    Fight(indexNo);
                }
                else
                {
                    indexNo = 0;
                }
            }
        }
    }


}
