using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScript : MonoBehaviour {

    GameObject[] enemyArray;

	void Start () {
		enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }


	
}
