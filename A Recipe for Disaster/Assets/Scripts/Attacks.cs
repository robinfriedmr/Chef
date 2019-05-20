using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attacks : MonoBehaviour
{

    public PersistentData _pd;
    WhoseTurn _wt; //WhoseTurn needs indexNo++ after a turn.

    TurnTakerStats[] turnTakerStats;

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

    public AudioClip spfxClip;
    public AudioSource spfxSource;

    GameObject target; //Refers to the GameObject targeted by an attack
    public CombatantStats targetStats; //and this to the target's stats.

    bool moving;
    public IEnumerator hurt;
    public IEnumerator attacking;
    public IEnumerator healed;
    IEnumerator advance;

    public GameObject chefAttackMenu;
    int chefAttackChoice;
    public GameObject dGMoveMenu;
    int dGMoveChoice;
    bool moveSelected;
    Ray ray;
    RaycastHit hit;

    Vector3 moveOffset;
    public GameObject bP;
    public GameObject punch;
    public GameObject fB;
    public GameObject stinky;

    private void Start()
    {
        _wt = GetComponent<WhoseTurn>();
        moving = false;

        chefAttackMenu.SetActive(false);
        dGMoveMenu.SetActive(false);

        moveOffset = new Vector3(0.75f, 0, -0.1f);

        turnTakerStats = Object.FindObjectsOfType<TurnTakerStats>();
    }

    IEnumerator Advance()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(GameObject.FindGameObjectWithTag("Move"));
        moving = false;

        //HELLO
        /*
        foreach (TurnTakerStats stats in turnTakerStats)
        {
            stats.UpdateDisplay();
        }
        */

        _wt.indexNo++;
    }

    void TargetSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGO = hit.transform.gameObject;
                if (_wt.order.Contains(hitGO))
                {
                    target = hitGO;
                }
            }
        }
    }

    void CalculateDamage(int raw, CombatantStats attacker, CombatantStats targetStats)
    {
        moving = true;

        // Method A - Inspired by Pokemon Go.
        //int finDmg = attacker.dmgBuff * Mathf.FloorToInt(0.5f * raw * (attacker.power / (targetStats.defense - targetStats.defDebuff))) + 1; // Minimum hit is 1. Defense CANNOT be 0!

        // Method B - Abby's idea
        int finDmg = raw - targetStats.defense - targetStats.defDebuff;
        finDmg = (finDmg < 0) ? 0 : finDmg; // Minimum hit is 0.

        // Animate battle
        attacking = attacker.gameObject.GetComponent<AnimatedBattle>().Attacking();
        StartCoroutine(attacking);
        hurt = targetStats.gameObject.GetComponent<AnimatedBattle>().Hurt();
        if (hurt != null)
        {
            StartCoroutine(hurt);
        }

        // Do damage
        Debug.Log(targetStats.gameObject.name + " is hit for " + finDmg); //***
        targetStats.HP -= finDmg; // Subtract damage from target's HP.
        Debug.Log("HP is " + targetStats.HP); //***

        if (targetStats.HP <= 0)
        {
            GameObject target = targetStats.gameObject;
            _wt.EndBattle(target);
        }
    }

    void HealMove(int heal, CombatantStats target)
    {
        moving = true;

        healed = target.gameObject.GetComponent<AnimatedBattle>().Healed();
        StartCoroutine(healed);

        target.HP += heal;
        if (target.HP > target.maxHP)
        {
            target.HP = target.maxHP;
        }
        else
        {
            target.HP -= 0;
        }
        Debug.Log(target.gameObject.name + " is healed. New HP: " + target.HP); //***
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
        if (!moving)
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
    }

    public void AllyAttacks(GameObject ally)
    {
        if (!moving)
        {
            if (ally.name == "PlayerCharacter")
            {
                chefAttackMenu.SetActive(true);
                ChefAttacks(ally.GetComponent<CombatantStats>());
            }
            else
            {
                dGMoveMenu.SetActive(true);
                DeliveryMoves(ally.GetComponent<CombatantStats>());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _wt.EndBattle(null);
            }
        }
    }

    public void Smack()
    {
        dGMoveChoice = 1;
    }

    public void HealingMeal()
    {
        dGMoveChoice = 2;
    }

    public void OnTheGo()
    {
        dGMoveChoice = 3;
    }

    void DeliveryMoves(CombatantStats dG)
    {
        dGMoveMenu.SetActive(true);

        if (dGMoveChoice == 1 && moveSelected == false)
        {
            Debug.Log("Attack type: Smack");
            dmg = 3;
            heal = 0;
            spfx = null;
            moveSelected = true;
        }

        if (dGMoveChoice == 2 && moveSelected == false)
        {
            Debug.Log("Attack type: Healing Meal");
            if (dG.magic >= 2)
            {
                dmg = 0;
                heal = 5;
                spfx = null;

                dG.magic -= 2;
                moveSelected = true;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (dGMoveChoice == 3 && moveSelected == false)
        {
            Debug.Log("Attack type: On the Go");
            if (dG.magic >= 2)
            {
                dmg = 0;
                heal = 0;
                spfx = "2xD";

                dG.magic -= 2;
                moveSelected = true;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (dmg != 0 || heal != 0 || spfx != null)
        {
            TargetSelection();
        }

        if (target != null)
        {
            Debug.Log("The target is " + target.name); //***

            if (spfx != null)
            {
                spfxSource.PlayOneShot(spfxClip); // Buff: On the Go SFX plays
                ApplySpFX(spfx, target.GetComponent<CombatantStats>());
            }

            if (dmg != 0)
            {
                aquaSmackSource.PlayOneShot(aquaSmackClip); // Smack sound effect plays
                CalculateDamage(dmg, dG, target.GetComponent<CombatantStats>());
            }
            else if (heal != 0)
            {
                healingMealSource.PlayOneShot(healingMealClip); // Healing Meal SFX
                HealMove(heal, target.GetComponent<CombatantStats>());
                GameObject healMeal = Instantiate(bP, (dG.transform.position + moveOffset), Quaternion.identity);
                healMeal.GetComponent<Animator>().SetBool("openNow", true);
            }
            ResetAttacks();
            dGMoveMenu.SetActive(false);
        }
    }

    public void Punch()
    {
        chefAttackChoice = 1;
    }

    public void FlamingPunch()
    {
        chefAttackChoice = 2;
    }

    public void ChefAttacks(CombatantStats chef)
    {
        chefAttackMenu.SetActive(true);

        if (chefAttackChoice == 1 && moveSelected == false)
        {
            Debug.Log("Attack type: Punch");
            dmg = 5;

            moveSelected = true;
        }

        if (chefAttackChoice == 2 && moveSelected == false)
        {
            Debug.Log("Attack type: Flaming Punch");
            if (chef.magic >= 2)
            {
                dmg = 8;

                chef.magic -= 2;
                moveSelected = true;
            }
            else
            {
                Debug.Log("Not enough magic left!");
            }
        }

        if (dmg != 0)
        {
            TargetSelection();

            if (target != null)
            {
                Debug.Log("The target is " + target.name); //***

                if (dmg < 8)
                {
                    normalPunchSource.PlayOneShot(normalPunchClip); // The normal punch sound
                    GameObject flyingPunch = Instantiate(punch, (chef.transform.position + moveOffset), Quaternion.identity);
                    flyingPunch.GetComponent<Animator>().SetBool("impact", false);

                }
                else
                {
                    flamePunchSource.PlayOneShot(flamePunchClip); // Flame Punch sound effect plays  
                    GameObject flame = Instantiate(fB, (chef.transform.position + moveOffset), Quaternion.identity);
                }

                CalculateDamage(dmg, chef, target.GetComponent<CombatantStats>());
                chefAttackMenu.SetActive(false);
                ResetAttacks();
            }
        }
    }

    public void ResetAttacks()
    {
        dGMoveChoice = 0;
        chefAttackChoice = 0;
        moveSelected = false;
        dmg = 0;
        heal = 0;
        spfx = null;
        target = null;

        advance = Advance();
        StartCoroutine(advance);
    }

    void BeetAttacks(CombatantStats attacker)
    {
        Debug.Log("The beet attacks!");

        Debug.Log("Attack type: Bash");
        dmg = 4;

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

        chooseAttack = Random.Range(0, 2); // choose an attack

        if (chooseAttack == 0)
        {
            Debug.Log("Attack type: Drill");
            dmg = 3;
        }
        else if (chooseAttack == 1)
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
                chooseAttack = 0;
                Debug.Log("New choice is " + chooseAttack);
            }
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

    void OnionAttacks(CombatantStats attacker)
    {
        Debug.Log("The onion attacks!");

        chooseAttack = Random.Range(0, 2); // choose an attack
        if (chooseAttack == 0)
        {
            Debug.Log("Attack type: Smack");
            dmg = 3;
        }
        else if (chooseAttack == 1)
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
                chooseAttack = 0;
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
                GameObject stinkCloud = Instantiate(stinky, attacker.transform.position - moveOffset, Quaternion.identity);
            }
            CalculateDamage(dmg, attacker, targetStats);
            ResetAttacks();
        }
    }
}