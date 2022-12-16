using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHP : MonoBehaviour
{

    public Image hpImage;
    public Text hpText;
    public float slideSpeed;

    public bool IsDead => curhp <= 0;
    public float HP => curhp;

    private float curhp;
    private readonly float max_hp = 100;

    private void Awake()
    {
        curhp = max_hp;
    }

    private void Update()
    {
        HPSlide();
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


    private void HPSlide()
    {
        hpText.text = String.Format($"{curhp}%");
        hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, curhp / max_hp, Time.deltaTime * slideSpeed);
    }



}
