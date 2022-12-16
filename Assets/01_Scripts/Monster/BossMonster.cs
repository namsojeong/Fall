using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    //[SerializeField] Canvas bossUI;
    private FlashHit hitFlash;

    private void Awake()
    {
        hitFlash = GetComponent<FlashHit>();
    }

    private void Start()
    {
        UI.Instance.SetCursor(true);
    }

    public void Hit()
    {
        Debug.Log("Hit2");
        ScoreManager.Instance.AddScore(10);
        hitFlash.DamageEffect();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Monster")
        {
        Debug.Log("Hit");
            Hit();
        }
    }
}
