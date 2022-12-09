using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;

public class Laser : MonoBehaviour
{
    public GameObject hitParticle;
    public GameObject startPos;
    public GameObject player;
    private LineRenderer line;
    private  RaycastHit hit;
    private  Vector3 endPos = Vector3.zero;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

    }
    private void Update()
    {
        line.SetPosition(0, startPos.transform.position);
        Physics.Raycast(startPos.transform.position, transform.forward, out hit);
        endPos = Vector3.Lerp(endPos, player.transform.position, Time.deltaTime);
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
