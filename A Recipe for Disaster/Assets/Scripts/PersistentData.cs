using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour {

    public List<GameObject> enemyList;
    public GameObject beet;
    public GameObject carrot;
    public GameObject onion;

    public GameObject environment;

	private void Awake()
	{
        // Instantiate enemies in places specified
        enemyList.Add(Instantiate(carrot, new Vector3(2.15f, 1f, 2.66f), Quaternion.identity) as GameObject);
        enemyList.Add(Instantiate(beet, new Vector3(-2.15f, 1.12f, 1.63f), Quaternion.identity) as GameObject);
        enemyList.Add(Instantiate(onion, new Vector3(-2.84f, 0.78f, -4.11f), Quaternion.identity) as GameObject);

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

    public void Reactivate()
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
