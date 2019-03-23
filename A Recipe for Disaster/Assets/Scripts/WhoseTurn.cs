using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhoseTurn : MonoBehaviour {

    BattleReady _br;
    Attacks _attacks;
    bool battleStarted;
    List<GameObject> order;
    public int indexNo;
    GameObject turnTaker;

    private void Start()
    {
        _attacks = GetComponent<Attacks>();
        indexNo = 0;
    }

    private void Update()
    {
        // Battle is on-going!
        if (battleStarted == true) // If battleStarted is false...
        {
            Debug.Log("The battle has started/continues. indexNo is " + indexNo + 
                ", order.Count() is " + order.Count);
            if (indexNo < order.Count) // Count()
            {
                Fight(indexNo);
            }
            else
            {
                Debug.Log("Resetting indexNo to 0.");
                indexNo = 0;
            }

            // Check for defeat
            if (_attacks.enemies.Count == 0 || _attacks.allies.Count == 0) 
            {
                //end battle
                _br.ready = true;
                battleStarted = false;
                order.Clear();
                Debug.Log("Battle ends.");
            }

        } else
        {
                //Initialization
            _br = GameObject.FindGameObjectWithTag("Ally").GetComponent<BattleReady>();
            Debug.Log("_br.order has a Count of " + _br.order.Count);
            order = _br.order;

            _attacks.enemies = _br.enemies;
            _attacks.allies = _br.allies;

            indexNo = 0;
            battleStarted = true;
        }   
    }

    void Fight(int i)
    {
        turnTaker = order.ElementAt<GameObject>(i);
        Debug.Log("Hello from the Fight() in WhoseTurn. " +
            "The name of the element at indexNo is " +
            turnTaker);

        if (turnTaker.tag == "Ally")
        {
            _attacks.AllyAttacks(turnTaker);
        } else if (turnTaker.tag == "Enemy"){
            _attacks.EnemyAttacks(turnTaker);
        }
    }
}
