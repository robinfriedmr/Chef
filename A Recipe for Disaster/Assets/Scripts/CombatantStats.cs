using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStats : MonoBehaviour {

    public int level;
    public int HP;
    public int defense;
    public int power;
    public int magic;
    public int speed;
    public int luck;

    // This variable will need to be accessed by other scripts in order...
    //...to determine level and thus stat increases.
    public int exp;

	void Start () {
        level = 1;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            ExperienceGain(2);
            Debug.Log("Exp now " + exp);
        }
	}

    public void ExperienceGain (int gain) {
        exp += gain;

        if (exp%10 == 0) {
            LevelUp();
            Debug.Log("Level up");
        }
    }

    public void LevelUp () {
        if (this.name == "PlayerCharacter") {
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
