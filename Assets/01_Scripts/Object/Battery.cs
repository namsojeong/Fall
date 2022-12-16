using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    private Image batteryUI;
    public List<Sprite> batterySprites;
    private int curBattery = 0;

    private void Awake()
    {
        batteryUI = GetComponent<Image>();

        curBattery = 0;
    }

    public void AddBattery()
    {
        curBattery++;
        UpdateBattery();
    }

    private void UpdateBattery()
    {
        batteryUI.sprite = batterySprites[curBattery];
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddBattery();
            gameObject.SetActive(false);
        }
    }
}
