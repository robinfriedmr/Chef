using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    public WhoseTurn whoseTurn;
    public int indexNo; //WhoseTurn needs indexNo++ after a turn.

    public List<GameObject> enemies;
    public List<GameObject> allies;

    int chooseAttack; //For random move choice of enemies.
    int chooseTarget; //For random target choice of enemies.

    int dmg; //Raw damage of a move, before calculation.
    GameObject target; //Refers to the GameObject targeted by an attack (except in CalcDam)

    void CalculateDamage (int raw, CombatantStats attacker, CombatantStats target) {
        // Method A - Inspired by Pokemon Go.
        //int finDmg = Mathf.FloorToInt(0.5f * raw * (attacker.power / target.defense)) + 1; // Minimum hit is 1.

        // Method B - Abby's idea
        int finDmg = raw - target.defense;
        finDmg = (finDmg < 0) ? 0 : finDmg;

        Debug.Log("Enemy is hit for " + finDmg); //***
        target.HP -= finDmg; // Subtract damage from target's HP.
        Debug.Log("Enemy HP is " + target.HP); //***

        if (target.HP <= 0)
        {
            //Remove target from list of combatants, list of allies/enemies
            Destroy(target);
            Debug.Log("Enemy defeated!");
        }
    }

    public void EnemyAttacks (GameObject enemy)
    {
        if (enemy.name == "Beet")
        {
            BeetAttacks(enemy.GetComponent<CombatantStats>());
        }
        else if (enemy.name == "Carrot")
        {
            CarrotAttacks(enemy.GetComponent<CombatantStats>());
        }
        else if (enemy.name == "Onion")
        {
            OnionAttacks(enemy.GetComponent<CombatantStats>());
        } else
        {
            Debug.Log("Enemy name not understood.");
        }
    }

    public void AllyAttacks(GameObject ally) {
        if (ally.name == "PlayerCharacter")
        {
            ChefAttacks(ally.GetComponent<CombatantStats>());
        } else
        {
            DeliveryMoves(ally.GetComponent<CombatantStats>());
        }
    }

    void DeliveryMoves(CombatantStats dG)
    {
        Debug.Log("Delivery girl moves!");
        //for Partner
    }

    void ChefAttacks (CombatantStats chef) {
        Debug.Log("Chef attacks!");

        if (chef.level >= 2) {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Debug.Log("Attack type: 2");
                if (chef.magic >= 3)
                {
                    dmg = chef.level + chef.power;
                    chef.magic -= 3;
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
                CalculateDamage(dmg, chef, target.GetComponent<CombatantStats>());
                dmg = 0;
                target = null;
                whoseTurn.indexNo++;
//                Debug.Log("After the attack: indexNo is " + indexNo); //***
            }
        }
    }

    void BeetAttacks (CombatantStats attacker) {
        Debug.Log("The beet attacks!");

        chooseAttack = Random.Range(1, 2); // choose an attack
        if (attacker.level >= 3 && chooseAttack == 2) {
            Debug.Log("Attack type: 2");
            if (attacker.magic >= 1) {
                dmg = attacker.level + attacker.power;
                attacker.magic -= 1;
            } else {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        } else if (chooseAttack == 1) {
            Debug.Log("Attack type: 1");
            dmg = 4;
        } else {
            Debug.Log("Error. Choice 1 or 2 not chosen for some reason.");
        }

        chooseTarget = Random.Range(0, allies.Count() - 1); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            CalculateDamage(dmg, attacker, target.GetComponent<CombatantStats>());
            dmg = 0;
            target = null;
            whoseTurn.indexNo++;
        }
    }

    void CarrotAttacks(CombatantStats attacker)
    {
        Debug.Log("The carrot attacks!");
        //;
        indexNo++;
    }

    void OnionAttacks(CombatantStats attacker)
    { 
        Debug.Log("The onion attacks!");
        //;
        indexNo++;
    }
}