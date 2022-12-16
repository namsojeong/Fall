using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    [SerializeField] Canvas bossUI;
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
        Debug.Log("Boss_Die");
        bossUI.gameObject.SetActive(false);
        StartCoroutine(GameClear());
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(1f);
        UI.Instance.ChangeScene(SceneState.GAMEOVER);
    }


    public void Hit()
    {
        monsterHP.Hit(10);
        if(monsterHP.IsDead)
        {
            Die();
        }
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
