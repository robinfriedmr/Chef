using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour {

    Inventory inventory;
    GameObject player;
    public Item item;

    private void Start()
    {
        inventory = GameObject.Find("Inventory Canvas").GetComponent<Inventory>();
        player = GameObject.Find("PlayerCharacter");
        // Is there a better way to do this? ^
    }

    public void Update()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
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
	
    public void Use()
    {
        if (item)
        {
            Debug.Log("You clicked:" + item.itemName);

            if (item is Heal)
            {
                item.target = player;
                Heal healing = item as Heal;
                healing.RestoreHealth();
                Debug.Log("New HP is: " + healing.targetStats.HP);
            }

            inventory.Remove(item);
        }
    }
}
