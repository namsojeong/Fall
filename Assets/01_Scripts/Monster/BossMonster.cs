using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{

    private CharacterHP monsterHP;
    private FlashHit hitFlash;

    private void Awake()
    {
        hitFlash = GetComponent<FlashHit>();
        monsterHP = GetComponent<CharacterHP>();
    }

    private void Die()
    {
        
    }

    public void Hit()
    {
        hitFlash.DamageEffect();
    }
}
