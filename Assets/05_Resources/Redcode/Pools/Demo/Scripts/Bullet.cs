using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    public Vector3 firPos;

    void Update()
    {
        transform.position += Vector3.forward;
    }

    void Init()
    {
        transform.position = firPos;
    }

    public void OnCreatedInPool()
    {
    }

    public void OnGettingFromPool()
    {
        Init();
    }
}
