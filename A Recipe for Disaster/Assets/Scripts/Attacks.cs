using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour {

    public PersistentData _pd;
    WhoseTurn _wt; //WhoseTurn needs indexNo++ after a turn.

    public List<GameObject> enemies; //Public list populated by BattleReady.
    public List<GameObject> allies; //Public list populated by BattleReady.

    int chooseAttack; //For random move choice of enemies.
    int chooseTarget; //For random target choice of enemies.

    int dmg; //Raw damage of a move, before calculation.
    string sfx; //Special effect(s). Not yet implemented.
    GameObject target; //Refers to the GameObject targeted by an attack
    CombatantStats targetStats; //and this to the target's stats.

    private void Start()
    {
        _wt = GetComponent<WhoseTurn>();

    }

    void CalculateDamage (int raw, CombatantStats attacker, CombatantStats targetStats) {
        // Method A - Inspired by Pokemon Go.
        int finDmg = Mathf.FloorToInt(0.5f * raw * (attacker.power / targetStats.defense)) + 1; // Minimum hit is 1.

        // Method B - Abby's idea
        //int finDmg = raw - targetStats.defense;
        //finDmg = (finDmg < 0) ? 0 : finDmg;

        Debug.Log(targetStats.gameObject.name + " is hit for " + finDmg); //***
        targetStats.HP -= finDmg; // Subtract damage from target's HP.
        Debug.Log("HP is " + targetStats.HP); //***

        if (targetStats.HP <= 0)
        {
            //Remove target from list of combatants, list of allies/enemies
            _wt.order.Remove(targetStats.gameObject);

            if (targetStats.gameObject.tag == "Enemy")
            {
                enemies.Remove(targetStats.gameObject);
            } else if (targetStats.gameObject.tag == "Ally") 
            {
                allies.Remove(targetStats.gameObject);
            } else
            {
                Debug.Log("Error trying to remove " + targetStats.gameObject.name + " from its list! Check tag: " + targetStats.gameObject.tag);
            }

            _pd = FindObjectOfType<PersistentData>().GetComponent<PersistentData>();
            _pd.enemyList.Remove(targetStats.gameObject);

            Destroy(targetStats.gameObject);
            Debug.Log(targetStats.gameObject.name + " defeated!");
        }
    }

    void ApplySFX (string sfx, CombatantStats targetStats)
    {
        if (sfx == "defense debuff")
        {
            targetStats.defense -= 1; //*** MAKE THIS A TEMPORARY DEBUFF! WITH 10% CHANCE OF HAPPENING.
        }
    }

    public void EnemyAttacks (GameObject enemy)
    {
        if (enemy.name.Contains("Beet"))
        {
            BeetAttacks(enemy.GetComponent<CombatantStats>());
        }
        else if (enemy.name.Contains("Carrot"))
        {
            CarrotAttacks(enemy.GetComponent<CombatantStats>());
        }
        else if (enemy.name.Contains("Onion"))
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
        //Debug.Log("Chef attacks!");

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
                ResetAttacks();
            }
        }
    }

    void ResetAttacks ()
    {
        dmg = 0;
        sfx = null;
        target = null;
        _wt.indexNo++;
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
        } /* else {
            Debug.Log("Error. Choice 1 or 2 not chosen for some reason.");
        } */

        chooseTarget = Random.Range(0, allies.Count() - 1); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            CalculateDamage(dmg, attacker, target.GetComponent<CombatantStats>());
            ResetAttacks();
        }
    }

    void CarrotAttacks(CombatantStats attacker)
    {
        Debug.Log("The carrot attacks!");

        chooseAttack = Random.Range(1, 2); // choose an attack
        if (attacker.level >= 4 && chooseAttack == 2)
        {
            Debug.Log("Attack type: 2");
            if (attacker.magic >= 2)
            {
                dmg = attacker.level + attacker.power;

                attacker.magic -= 2;
            }
            else
            {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        }
        else if (chooseAttack == 1)
        {
            Debug.Log("Attack type: 1");
            dmg = 3;
        } /* else {
            Debug.Log("Error. Choice 1 or 2 not chosen for some reason.");
        } */

        chooseTarget = Random.Range(0, allies.Count() - 1); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            CalculateDamage(dmg, attacker, target.GetComponent<CombatantStats>());
            ResetAttacks();
        }
    }

    void OnionAttacks(CombatantStats attacker)
    { 
        Debug.Log("The onion attacks!");

        chooseAttack = Random.Range(1, 2); // choose an attack
        if (attacker.level >= 2 && chooseAttack == 2)
        {
            Debug.Log("Attack type: 2");
            if (attacker.magic >= 2)
            {
                dmg = attacker.level + attacker.power;
                sfx = "defense debuff";

                attacker.magic -= 2;
                float recoil = (attacker.level + attacker.power) / 2;
                attacker.HP -= Mathf.FloorToInt(recoil);
            }
            else
            {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        }
        else if (chooseAttack == 1)
        {
            Debug.Log("Attack type: 1");
            dmg = 3;
        } /* else {
            Debug.Log("Error. Choice 1 or 2 not chosen for some reason.");
        } */

        chooseTarget = Random.Range(0, allies.Count() - 1); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        targetStats = target.GetComponent<CombatantStats>();
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            CalculateDamage(dmg, attacker, targetStats);
            if (sfx != null)
            {
                ApplySFX(sfx, target.GetComponent<CombatantStats>());
            }
            ResetAttacks();
        }
    }
}