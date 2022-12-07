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
        Destroy(gameObject, timeToDestroy);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,target, speed* Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Debug.Log(contact.thisCollider.gameObject);
       // GameObject.Instantiate(bulletDecal, contact.point, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);
    }
}
