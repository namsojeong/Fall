using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            FlagController.Instance.AddBattery();
            gameObject.SetActive(false);
        }
    }
}
