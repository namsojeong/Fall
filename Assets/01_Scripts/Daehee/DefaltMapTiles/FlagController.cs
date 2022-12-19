using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoSingleton<FlagController>
{
    [SerializeField] private GameObject gate;
    [SerializeField] private List<GameObject> _flagGrp;
    [SerializeField] private List<AudioSource> audioSource;
    [SerializeField] List<Sprite> batterySprites;
    [SerializeField] List<GameObject> battery;
    [SerializeField] List<Transform> gateSpawner;
    [SerializeField] List<Transform> batterySpawner;
    Queue<Transform> curBatterySpawner;

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
        gate.transform.position = gateSpawner[Random.Range(0, gateSpawner.Count)].position;
        
        gate.SetActive(false);
    }

    void Update()
    {
        CheckIsDone();
    }

    private void ResetBattery()
    {
        for(int i=0; i < battery.Count; i++)
        {
            int index = Random.Range(0, batterySpawner.Count);
            battery[i].transform.position = batterySpawner[index].position;
            batterySpawner.RemoveAt(index);
        }
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
