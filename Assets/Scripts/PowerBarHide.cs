using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarHide : MonoBehaviour
{

    public CanvasGroup PowerBar;
    // Use this for initialization
    void Start()
    {
        ToggleCanvasGroupActive();

    }


    public void ToggleCanvasGroupActive()
    {
        // This will set the canvas group to active if it is inactive OR set it to inactive if it is active
        PowerBar.gameObject.SetActive(!PowerBar.gameObject.activeSelf);
    }
}
