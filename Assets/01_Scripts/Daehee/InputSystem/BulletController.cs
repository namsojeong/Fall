using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeleteBullet());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(DeleteBullet());
        ObjectPool.Instance.ReturnObject(PoolObjectType.BULLET, gameObject);
    }

    private IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(timeToDestroy);
        ObjectPool.Instance.ReturnObject(PoolObjectType.BULLET, gameObject);
    }
}
