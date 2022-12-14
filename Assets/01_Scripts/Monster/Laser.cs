using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;

public class Laser : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject hitParticle;
    public GameObject startPos;
    public GameObject player;

    private LineRenderer line;
    private  RaycastHit hit;
    private  Vector3 endPos = Vector3.zero;

    private float laserSpeed = 2f;
    private float slowSpeed;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

    }
    private void Update()
    {
        line.SetPosition(0, startPos.transform.position);
        
        endPos = Vector3.Lerp(endPos, player.transform.position, Time.deltaTime * laserSpeed);
        endPos.y = 0;
        Vector3 dir = endPos - startPos.transform.position;
        Debug.DrawRay(startPos.transform.position, dir*100f, Color.red);
        if (Physics.Raycast(startPos.transform.position, dir, out hit, 100f, playerLayer))
        {

            Collider[] cols = Physics.OverlapSphere(hit.transform.position, 100f, playerLayer);
            if (cols.Length > 0)
            {
                line.SetColors(Color.red, Color.red);
            PlayerController.Instance.SlowSpeed(true);
            }
            else
            {
                line.SetColors(Color.cyan, Color.cyan);
            PlayerController.Instance.SlowSpeed(false);
            }
        }
        else
        {
                line.SetColors(Color.cyan, Color.cyan);
            PlayerController.Instance.SlowSpeed(false);
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

}
