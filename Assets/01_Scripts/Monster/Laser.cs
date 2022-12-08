using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float dis;
    LineRenderer line;
    GameObject hitParticle;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Vector3 hitPoint = hit.point;

            Debug.DrawLine(transform.position, transform.forward*hit.distance, Color.red);

            line.SetPosition(0, transform.position);
            line.SetPosition(1, hit.point);

            hitParticle.transform.position = hitPoint;
            hitParticle.transform.rotation = Quaternion.LookRotation(hit.normal);
            hitParticle.SetActive(true);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
        }
    }
}
