using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultCamSet : MonoBehaviour
{
    private Transform _playerTransform;
    void Start()
    {
        _playerTransform = GameObject.Find("Player 1").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position + new Vector3(0, 15, 6);
        transform.LookAt(_playerTransform);
    }
}
