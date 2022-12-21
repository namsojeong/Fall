using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    FlashHit flashHit;

    private void Awake()
    {
        flashHit = GetComponent<FlashHit>();
    }

    public void Hit()
    {
        flashHit.DamageEffect();
        ScoreManager.Instance.AddScore(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Monster"))
        {
            Hit();
        }
    }
}
