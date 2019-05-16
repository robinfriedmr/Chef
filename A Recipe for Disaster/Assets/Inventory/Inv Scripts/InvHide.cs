using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvHide : MonoBehaviour {
    public CanvasGroup Inv;
    bool hidden;

    // Use this for initialization
    void Start () {
        hidden = true;
        
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            hidden = !hidden;
        }
        if (hidden)
            Hide();
        if (!hidden)
            Show();

    }


    void Hide()
    {

        Inv.alpha = 0f; //this makes everything transparent
        Inv.blocksRaycasts = false; //this prevents the UI element to receive input events
    }
    void Show()
    {
        Inv.alpha = 1f;
        Inv.blocksRaycasts = true;
    }
}
