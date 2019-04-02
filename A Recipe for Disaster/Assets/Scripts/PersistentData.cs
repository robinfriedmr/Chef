using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {

    GameObject[] enemyArray;

	void Start () {
		enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Reinstantiate () {
        foreach(GameObject enemy in enemyArray) {
            if (enemy != null) {
                enemy.SetActive(true);
                //Instantiate(enemy);
            }
        }
    }
	
    void BeforeSwitch () {
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyArray) {
            enemy.SetActive(false);
        }
    }
}
