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

    private void Start()
    {
        UI.Instance.SetCursor(true);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Hit();
        }
    }

    public void Hit()
    {
        monsterHP.Hit(10);
        if(monsterHP.IsDead)
        {
            Die();
        }
        Debug.Log(monsterHP.HP);
        hitFlash.DamageEffect();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Monster")
        {
            Hit();
        }
    }
}
