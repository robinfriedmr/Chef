using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyHide : MonoBehaviour
{

    public GameObject Inv; // Assign in inspector
    private bool isShowing;
    public Button Attack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Attack)
        {
            isShowing = !isShowing;
            Inv.SetActive(isShowing);
        }
    }
}