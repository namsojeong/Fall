using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHP : MonoBehaviour
{
    private int curhp;
    public readonly int max_hp = 100;
    public bool IsDead => curhp <= 0;
    public float HP => curhp;

    private void Awake()
    {
        curhp = max_hp;
    }

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
