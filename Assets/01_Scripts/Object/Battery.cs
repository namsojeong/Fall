using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    EventParam eventParam = new EventParam();
    public int id;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            eventParam.intParam = id;
            EventManager.TriggerEvent("BatterySound", eventParam);
            FlagController.Instance.AddBattery();
            gameObject.SetActive(false);
        }
    }
}
