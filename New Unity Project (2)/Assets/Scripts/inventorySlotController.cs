using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventorySlotController : MonoBehaviour {

    public Item item;

    public void Update()
    {
        updateInfo();
    }

    public void updateInfo()
    {
        Text displayText = transform.Find("Text").GetComponent<Text>();
        Image displayImage = transform.Find("Image").GetComponent<Image>();

        if (item)
        {
            displayText.text = item.itemName;
            displayImage.sprite = item.icon;
        }
        else
        {
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
        }
    }
	
    public void use()
    {
        if (item)
        {
            Debug.Log("You clicked:" + item.itemName);
        }
    }
}
