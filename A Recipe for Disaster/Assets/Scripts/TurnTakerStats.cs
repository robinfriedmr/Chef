using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTakerStats : MonoBehaviour
{

    public CombatantStats myStats;
    public Text displayHealth;
    public Text displayMana;

    void Start()
    {
        Debug.Log("myStats is " + myStats);
        //UpdateDisplay();
    }

    void Update()
    {
        int _health = myStats.HP;
        int _magic = myStats.magic;

        displayHealth.text = _health.ToString();
        displayMana.text = _magic.ToString();
    }
}
