using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoSingleton<FlagController>
{
    [SerializeField] private List<GameObject> _flagGrp;
    [SerializeField] private bool isDone = false;
    [SerializeField] List<Sprite> batterySprites;

    private Image batteryUI;

    private int curBattery = 0;

    void Start()
    {
        isDone = GameManager.Instance.IsBoss;
        batteryUI = GetComponent<Image>(); 
    }

    void Update()
    {
        isDone = CheckIsDone();
        if (isDone)
            GameManager.Instance.IsBoss = isDone;
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


    bool CheckIsDone()
    {
        bool done = true;
        foreach (GameObject _flag in _flagGrp)
        {
            if (_flag.activeInHierarchy)
                done = false;
        }
        return done;
    }

}
