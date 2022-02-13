using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentData : MonoBehaviour {

    public GameObject[] enemyArray;
    public List<GameObject> enemyList;
    public GameObject beet;
    public GameObject carrot;
    public GameObject onion;

    public GameObject environment;

	private void Start ()
	{
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemyList = enemyArray.ToList<GameObject>();
    }
	
    public void BeforeSwitch (GameObject except) {

        // Set overworld enemies as inactive
        foreach (GameObject enemy in enemyList) {
            if (enemy != null)
            {
                enemy.SetActive(false);
            } 
        }
        except.SetActive(true);

        // Set environment as inactive
        environment.SetActive(false);
    }

    public void ReactivateEnemies()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
    }
}
