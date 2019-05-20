//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySounds : MonoBehaviour
{

    public AudioClip OpenInventory;
    public AudioClip CloseInventory;
    public AudioSource audioSource;
    int counter = 2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (counter % 2 == 0)
            {
                audioSource.PlayOneShot(OpenInventory);
                counter++;
            }
            else
            {
                audioSource.PlayOneShot(CloseInventory);
                counter++;
            }
        }
    }
}
