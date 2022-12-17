using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoSingleton<FlagController>
{
    [SerializeField] private GameObject gate;
    [SerializeField] private List<GameObject> _flagGrp;
    [SerializeField] private List<AudioSource> audioSource;
    [SerializeField] List<Sprite> batterySprites;

    private Image batteryUI;

    private int curBattery = 0;
    private void Awake()
    {
        EventManager.StartListening("BatterySound", OnBatterySound);
    }

    void Start()
    {
        batteryUI = GetComponent<Image>();
        ResetBattery();
        gate.SetActive(false);
    }

    void Update()
    {
        CheckIsDone();
    }

    private void ResetBattery()
    {
        curBattery = 0;
        UpdateBattery();
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
    private void OnBatterySound(EventParam eventParam)
    {
        audioSource[eventParam.intParam].Play();
    }

    private void OnDestroy()
    {
        EventManager.StopListening("BatterySound", OnBatterySound);
    }
}
