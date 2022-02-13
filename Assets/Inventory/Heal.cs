using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Heal", menuName = "Item/Heal")]
public class Heal : Item
{
    public int restore = 5;
    public CombatantStats targetStats;

    public void RestoreHealth()
    {
        targetStats = target.GetComponent<CombatantStats>();
        targetStats.HP += restore;
        if (targetStats.HP > targetStats.maxHP)
        {
            targetStats.HP = targetStats.maxHP;
        }
    }

    public void RestoreMana()
    {
        targetStats = target.GetComponent<CombatantStats>();
        targetStats.magic += restore;
    }
}
