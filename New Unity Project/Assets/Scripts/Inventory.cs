using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject player;
    public GameObject inventoryPanel;
    public List<Item> list = new List<Item>();

    public static Inventory instance;


    void updatePanelSlots()
    {
        int index = 0;
        foreach(Transform child in inventoryPanel.transform)
        {
            inventorySlotController slot = child.GetComponent<inventorySlotController>();

            if (index < list.Count)
            {
                slot.item = list[index];
            }
            else
            {
                slot.item = null;
            }

            slot.updateInfo();
            index++;
        }
    }

    public void Add(Item item)
    {
        if (list.Count < 15)
        {
            list.Add(item);
        }
    }

    public void Remove(Item item)
    {
        list.Remove(item);
    }
    // Use this for initialization
	void Start ()
    {
        instance = this;
        updatePanelSlots();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
