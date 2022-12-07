using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemytest : MonoBehaviour
{
    private Rigidbody _enemyRigid;
    private Transform _playerTransform;
    void Start()
    {
        _playerTransform = GameObject.Find("Player 1").transform;
        Debug.Log(_playerTransform);
        _enemyRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log("ºÎµúÈû");
        if(collision.gameObject.CompareTag("Bullet"))
        {
            _enemyRigid.AddForce(-_playerTransform.position + Vector3.up*1000, ForceMode.Impulse);
        }
    }
}
