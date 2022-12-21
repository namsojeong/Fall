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
    private Material material;
    private  RaycastHit hit;
    private  Vector3 endPos = Vector3.zero;
    private Vector3 startPos;

    private Color setColor = Color.cyan;

    private float delay = 2f;
    private int index = 0;

    private bool isHit = false;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        material = line.material;
        startPos = transform.GetChild(0).position;
        StartCoroutine(SelectPoint());
    }

    private void Update()
    {
        line.SetPosition(0, startPos);
        
        endPos = Vector3.Lerp(endPos, laserPoint[index].position, Time.deltaTime);
        endPos.y = 0;
        Vector3 dir = endPos - startPos;
        if (Physics.Raycast(startPos, dir, out hit, 100f, playerLayer))
        {
            if (isHit) return;
            Collider[] cols = Physics.OverlapSphere(hit.transform.position, 100f, playerLayer);
            if (cols.Length > 0)
            {
                setColor = Color.red;
                StartCoroutine(LaserHit());
            }
            else
            {
                setColor = Color.cyan;
            }
        }
        else
        {
            setColor = Color.cyan;
        }
        line.SetColors(setColor, setColor);
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
