using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhoseTurn : MonoBehaviour {

    public BattleReady battleReady; //Ready to begin battle if true. If false, combat is ongoing.
    List<GameObject> order;
//    List<GameObject> enemies;
//    List<GameObject> allies;

    public bool battleStarted; //Initialized?
    public int indexNo; //What turn are we on? Can be modified from Attacks b/c it's public.

    Attacks _attacks;

    void Start () {
        battleReady = GetComponent<BattleReady>(); 
        battleStarted = false;
        //indexNo = 0; // Start it at 0 though.
        _attacks = GetComponent<Attacks>(); // Get player's Attacks to report back the indexNo.
    }

    void Update () {
        if (_attacks.battleStarted == true) {
            if (_attacks.enemies.Count == 0 || _attacks.allies.Count == 0) // is it ()?
            {
                //end battle
                battleReady.ready = true;
                battleStarted = false;
                order.Clear();
                Debug.Log("Battle ends.");
            }
            else
            { // Battle is on-going.
                if (order != null) {
                    Debug.Log("Hello from WhoseTurn; the indexNo is " + indexNo + ". ALSO, order is " + order);
                    if (indexNo < order.Count()) //***ARGUMENT NULL EXCEPTION. ARGUMENT CANNOT BE NULL.
                    {
                        Debug.Log("Hello from WhoseTurn; the indexNo is less than order.Count()!");
                    }
                    else
                    {
                        indexNo = 0;
                    }
                } else {
                    order = _attacks.order;
                }

            }
        }
    }


}
