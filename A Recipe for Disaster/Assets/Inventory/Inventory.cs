using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject player;
    public GameObject inventoryPanel;
    public List<Item> list = new List<Item>();

    public static Inventory instance;

    void Start()
    {
        instance = this;
        //UpdatePanelSlots();
    }

    void UpdatePanelSlots()
    {
        int index = 0;
        foreach(Transform child in inventoryPanel.transform)
        {
            InventorySlotController slot = child.GetComponent<InventorySlotController>();

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
            UpdatePanelSlots();
        }
    }

    public void Remove(Item item)
    {
        list.Remove(item);
        UpdatePanelSlots();
    }
}
