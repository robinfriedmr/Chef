using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour
{

    public PersistentData _pd;
    WhoseTurn _wt; //WhoseTurn needs indexNo++ after a turn.

    public List<GameObject> enemies; //Public list populated by BattleReady.
    public List<GameObject> allies; //Public list populated by BattleReady.

    int chooseAttack; //For random move choice of enemies.
    int chooseTarget; //For random target choice of enemies.

    int dmg; //Raw damage of a move, before calculation.
    int heal; //Heal points.
    string spfx; //Special effect(s).
    int dmgBuff;

    // Sound effects
    public AudioClip flamePunchClip;
    public AudioSource flamePunchSource;

    public AudioClip normalPunchClip;
    public AudioSource normalPunchSource;

    public AudioClip aquaSmackClip;
    public AudioSource aquaSmackSource;

    public AudioClip healingMealClip;
    public AudioSource healingMealSource;

    GameObject target; //Refers to the GameObject targeted by an attack
    CombatantStats targetStats; //and this to the target's stats.

    private void Start()
    {
        _wt = GetComponent<WhoseTurn>();
    }

    void CalculateDamage(int raw, CombatantStats attacker, CombatantStats targetStats)
    {
        // Method A - Inspired by Pokemon Go.
        //int finDmg = attacker.dmgBuff * Mathf.FloorToInt(0.5f * raw * (attacker.power / (targetStats.defense - targetStats.defDebuff))) + 1; // Minimum hit is 1. Defense CANNOT be 0!

        // Method B - Abby's idea
        int finDmg = raw - targetStats.defense;
        finDmg = (finDmg < 0) ? 0 : finDmg; // Minimum hit is 0.

        Debug.Log(targetStats.gameObject.name + " is hit for " + finDmg); //***
        targetStats.HP -= finDmg; // Subtract damage from target's HP.
        Debug.Log("HP is " + targetStats.HP); //***

        if (targetStats.HP <= 0)
        {
            EndBattle();

            _pd = FindObjectOfType<PersistentData>().GetComponent<PersistentData>();
            _pd.enemyList.Remove(targetStats.gameObject);

            if (targetStats.gameObject.tag == "Ally") {
                targetStats.gameObject.SetActive(false); // Don't destroy allies
            } else {
                Destroy(targetStats.gameObject);
            }
            Debug.Log(targetStats.gameObject.name + " defeated!");
        }
    }

    void EndBattle() {


        //Remove target from list of combatants, list of allies/enemies
        _wt.order.Clear();
        enemies.Clear();
        allies.Clear();

        Debug.Log("Battle ends!");
    }

    void HealMove(int heal, CombatantStats target)
    {
        Debug.Log(targetStats.gameObject.name + " is healed for " + heal); //***
        target.HP += heal;

        if (target.HP > target.maxHP)
        {
            target.HP = target.maxHP;
        }
        else
        {
            target.HP -= 0;
        }
    }

    void ApplySpFX(string spfx, CombatantStats targetStats)
    {
        if (spfx == "2xD")
        {
            targetStats.dmgBuff = 2;
            Debug.Log("Target " + targetStats.name + "has 2* damage buff (" + targetStats.dmgBuff + ")");
        }
        if (spfx == "defDebuff")
        {
            targetStats.defDebuff = 1; //*** MAKE THIS A TEMPORARY DEBUFF! WITH 10% CHANCE OF HAPPENING.
        }
    }

    public void EnemyAttacks(GameObject enemy)
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
        }
        else
        {
            Debug.Log("Enemy name not understood.");
        }
    }

    public void AllyAttacks(GameObject ally)
    {
        if (ally.name == "PlayerCharacter")
        {
            ChefAttacks(ally.GetComponent<CombatantStats>());
        }
        else
        {
            DeliveryMoves(ally.GetComponent<CombatantStats>());
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            EndBattle();
        }
    }

    void DeliveryMoves(CombatantStats dG)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            aquaSmackSource.PlayOneShot(aquaSmackClip); // Smack sound effect plays
            Debug.Log("Attack type: Smack");
            dmg = 3;
            heal = 0;
            spfx = null;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Attack type: Healing Meal");
            if (dG.magic >= 2)
            {
                healingMealSource.PlayOneShot(healingMealClip); // Healing Meal SFX
                dmg = 0;
                heal = 5;
                spfx = null;

                dG.magic -= 2;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Attack type: On the Go");
            if (dG.magic >= 2)
            {
                //SFX HERE ***
                dmg = 0;
                heal = 0;
                spfx = "2xD";

                dG.magic -= 2;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (dmg != 0 || heal != 0 || spfx != null)
        {
            // Ally targets
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                target = allies.ElementAt<GameObject>(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                target = allies.ElementAt<GameObject>(1);
            }

            // Enemy targets
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                target = enemies.ElementAt<GameObject>(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (enemies.ElementAt<GameObject>(1) != null)
                {
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
        }

        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***

            if (spfx != null)
            {
                ApplySpFX(spfx, target.GetComponent<CombatantStats>());
            }

            if (dmg != 0)
            {
                CalculateDamage(dmg, dG, target.GetComponent<CombatantStats>());
            }
            else if (heal != 0)
            {
                HealMove(heal, target.GetComponent<CombatantStats>());
            }
            ResetAttacks();
        }
    }

    void ChefAttacks(CombatantStats chef)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            normalPunchSource.PlayOneShot(normalPunchClip); // The normal punch sound 
            Debug.Log("Attack type: Punch");
            dmg = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Attack type: Flaming Punch");
            if (chef.magic >= 2)
            {
                flamePunchSource.PlayOneShot(flamePunchClip); // Flame Punch sound effect plays
                dmg = 8;

                chef.magic -= 2;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (dmg != 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                target = enemies.ElementAt<GameObject>(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (enemies.ElementAt<GameObject>(1) != null)
                {
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

            if (target != null)
            {
                Debug.Log("The target is " + target.name); //***
                CalculateDamage(dmg, chef, target.GetComponent<CombatantStats>());
                ResetAttacks();
            }
        }
    }

    void ResetAttacks()
    {
        dmg = 0;
        heal = 0;
        spfx = null;
        target = null;
        _wt.indexNo++;
    }

    void BeetAttacks(CombatantStats attacker)
    {
        Debug.Log("The beet attacks!");

        if (chooseAttack == 1)
        {
            Debug.Log("Attack type: Bash");
            dmg = 4;
        }

        chooseTarget = Random.Range(0, allies.Count()); // Choose a target
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

        if (chooseAttack == 1)
        {
            Debug.Log("Attack type: Drill");
            dmg = 3;
        }
        else if (chooseAttack == 2)
        {
            Debug.Log("Attack type: Deadly Drill");
            if (attacker.magic >= 2)
            {
                dmg = 6;

                attacker.magic -= 2;
            }
            else
            {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        }
        Debug.Log("allies.Count() is " + allies.Count()); //******
        chooseTarget = Random.Range(0, allies.Count()); // Choose a target
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
        if (chooseAttack == 1)
        {
            Debug.Log("Attack type: Smack");
            dmg = 3;
        }
        else if (chooseAttack == 2)
        {
            Debug.Log("Attack type: Peel");
            if (attacker.magic >= 2)
            {
                dmg = 2 + attacker.power;

                float chance = Random.value;
                if (chance <= 0.1)
                {
                    spfx = "defDebuff";
                }

                attacker.magic -= 2;
                attacker.HP -= 1;
            }
            else
            {
                Debug.Log("Not enough magic left!");
                chooseAttack = 1;
                Debug.Log("New choice is " + chooseAttack);
            }
        }

        chooseTarget = Random.Range(0, allies.Count()); // Choose a target
        target = allies.ElementAt<GameObject>(chooseTarget);
        targetStats = target.GetComponent<CombatantStats>();
        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***
            if (spfx != null)
            {
                Debug.Log("Applying SpFX");
                ApplySpFX(spfx, targetStats);
            }
            CalculateDamage(dmg, attacker, targetStats);
            ResetAttacks();
        }
    }
}