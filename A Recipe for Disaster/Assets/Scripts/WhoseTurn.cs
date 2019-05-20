using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhoseTurn : MonoBehaviour
{

    PersistentData _pd;
    BattleReady _br;
    Attacks _attacks;
    bool battleStarted;
    public List<GameObject> order;
    public int indexNo;
    GameObject turnTaker;

    IEnumerator endPause;
    public bool battleEnded;

    private void Start()
    {
        _attacks = GetComponent<Attacks>();
        indexNo = 0;
        battleEnded = false;
    }

    private void Update()
    {
        // Battle is on-going!
        if (battleStarted == true) // If battleStarted is false...
        {
            // Check for defeat first
            if (_attacks.enemies.Count == 0 || _attacks.allies.Count == 0)
            {
                EndBattle(null);
            }
            else if (indexNo < order.Count)
            {
                Fight(indexNo);
            }
            else
            {
                Debug.Log("Resetting indexNo to 0.");
                indexNo = 0;
            }

        }
        else
        {
            //Initialization
            _br = GameObject.FindGameObjectWithTag("Ally").GetComponent<BattleReady>();
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

        if (turnTaker.tag == "Ally")
        {
            _attacks.AllyAttacks(turnTaker);
        }
        else if (turnTaker.tag == "Enemy")
        {
            _attacks.EnemyAttacks(turnTaker);
        }
    }

    public IEnumerator EndPause(GameObject target)
    {
        battleEnded = false;
        yield return new WaitForSeconds(1.5f);

        // Clear animations from remaining combatants.
        foreach (GameObject combatant in order)
        {
            Animator animator = combatant.GetComponent<Animator>();
            animator.SetBool("attacking", false);
            animator.SetBool("hurt", false);

            CombatantStats stats = combatant.GetComponent<CombatantStats>();
            stats.dmgBuff = 1;
            stats.defDebuff = 0;
        }
        Destroy(GameObject.FindGameObjectWithTag("Move"));

        // Clear lists.
        order.Clear();
        _attacks.allies.Clear();
        _attacks.enemies.Clear();
        _br.combatants.Clear();

        battleEnded = true;

        // Reset readiness.
        _br.ready = true;
        battleStarted = false;

        Debug.Log("Battle ends.");

        // Kill the target.
        if (target != null)
        {
            if (target.tag == "Ally")
            {
                target.SetActive(false); // Don't destroy allies
            }
            else
            {
                Destroy(target);
            }
            Debug.Log(target.name + " defeated!");
        }

        // Reload overworld.
        _pd = FindObjectOfType<PersistentData>().GetComponent<PersistentData>();
        _pd.ReactivateEnemies();

        _br.RestoreOverworld();
        SceneManager.UnloadSceneAsync("ModeledBattleScene");

    }

    public void EndBattle(GameObject target)
    {
        endPause = EndPause(target);
        StartCoroutine(endPause);
    }
}
