using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHP : MonoBehaviour
{
    private int curhp;
    private readonly int max_hp;
    private bool IsDead => curhp <= 0;

    public void Hit(int damage)
    {
        curhp -= damage;
    }

    public void SetHP(int hp)
    {
        curhp = hp;
    }

    public void ReviveHP()
    {
        curhp = max_hp;
    }
}
