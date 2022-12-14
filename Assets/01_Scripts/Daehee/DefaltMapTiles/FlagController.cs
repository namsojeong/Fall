using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _flagGrp;
    [SerializeField] private bool isDone = false;
    void Start()
    {
        isDone = GameManager.Instance.IsBoss;
    }

    // Update is called once per frame
    void Update()
    {
        isDone = CheckIsDone();
        if (isDone)
            GameManager.Instance.IsBoss = isDone;
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
