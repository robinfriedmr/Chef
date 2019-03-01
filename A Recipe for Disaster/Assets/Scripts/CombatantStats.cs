using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int Level { get; set; }
    public int HP { get; set; }
    public int Defense { get; set; }
    public int Power { get; set; }
    public int Magic { get; set; }
    public int Speed { get; set; }
    public int Luck { get; set; }
    public int EXP { get; set; }
}

public class CombatantStats : MonoBehaviour
{
    public Stats stats;
    public int levelFromStatsClass;

    // This variable will need to be accessed by other scripts in order...
    //...to determine level and thus stat increases.

    void Start()
    {
        stats = this.GetComponent<Stats>();
        level = stats.Level;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExperienceGain(2);
            Debug.Log("Exp now " + exp);
        }
    }

    public void ExperienceGain(int gain)
    {
        exp += gain;

        if (exp % 10 == 0)
        {
            LevelUp();
            Debug.Log("Level up");
        }
    }

    public void LevelUp()
    {
        if (this.name == "PlayerCharacter")
        {
            HP += level + 1;
            defense++;
            power++;
            magic++;
            speed++;

            level++;
            Debug.Log("New level: " + level);
        }
    }
}