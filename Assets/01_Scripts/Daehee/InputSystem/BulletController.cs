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

    private void OnEnable()
    {
        StartCoroutine(DeleteBullet());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        ObjectPool.Instance.ReturnObject(PoolObjectType.BULLET, gameObject);
        StopCoroutine(DeleteBullet());
    }

    private IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(timeToDestroy);
        ObjectPool.Instance.ReturnObject(PoolObjectType.BULLET, gameObject);
    }
}
