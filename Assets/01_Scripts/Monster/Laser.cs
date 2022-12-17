using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;

public class Laser : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject hitParticle;
    public List<Transform> laserPoint;

    private LineRenderer line;
    private  RaycastHit hit;
    private  Vector3 endPos = Vector3.zero;
    private Vector3 startPos;

    private float laserSpeed = 2f;
    private float slowSpeed;
    private float delay = 2f;
    int index = 0;

    bool isHit = false;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        startPos = transform.GetChild(0).position;
        StartCoroutine(SelectPoint());
    }

    private void Update()
    {
        line.SetPosition(0, startPos);
        
        endPos = Vector3.Lerp(endPos, laserPoint[index].position, Time.deltaTime);
        endPos.y = 0;
        Vector3 dir = endPos - startPos;
        Debug.DrawRay(startPos, dir*100f, Color.red);
        if (Physics.Raycast(startPos, dir, out hit, 100f, playerLayer))
        {
            if (isHit) return;
            Collider[] cols = Physics.OverlapSphere(hit.transform.position, 100f, playerLayer);
            if (cols.Length > 0)
            {
                line.SetColors(Color.red, Color.red);
                StartCoroutine(LaserHit());
            }
            else
            {
                line.SetColors(Color.cyan, Color.cyan);
            }
        }
        else
        {
            line.SetColors(Color.cyan, Color.cyan);
            
        }
        line.SetPosition(1, endPos);
        LaserEffect();
    }

    private void LaserEffect()
    {
        hitParticle.transform.position = endPos;
        hitParticle.transform.rotation = Quaternion.LookRotation(hit.normal);
        hitParticle.SetActive(true);
    }

    IEnumerator SelectPoint()
    {
        while(true)
        {
            index = Random.Range(0, laserPoint.Count);
            yield return new WaitForSeconds(delay);
        }

    }

    IEnumerator LaserHit()
    {
        PlayerController.Instance.Hit(5);
        isHit = true;
        yield return new WaitForSeconds(delay);
        isHit = false;
    }

}
