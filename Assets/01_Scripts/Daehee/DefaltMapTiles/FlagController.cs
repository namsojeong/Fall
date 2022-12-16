using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoSingleton<FlagController>
{
    [SerializeField] private GameObject gate;
    [SerializeField] private List<GameObject> _flagGrp;
    [SerializeField] List<Sprite> batterySprites;

    private Image batteryUI;

    private int curBattery = 0;

    void Start()
    {
        batteryUI = GetComponent<Image>();
        gate.SetActive(false);
    }

    void Update()
    {
        CheckIsDone();
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


    private void CheckIsDone()
    {
        bool done = true;
        foreach (GameObject _flag in _flagGrp)
        {
            if (_flag.activeSelf)
                done = false;
        }
        GameManager.Instance.IsBoss = done;

        if (done)
        {
            gate.SetActive(true);
        }
    }

}
