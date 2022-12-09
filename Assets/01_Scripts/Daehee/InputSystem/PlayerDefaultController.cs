using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultController : MonoBehaviour
{
    public CharacterController controller;
    private PlayerInput input;
    private InputAction moveAction;

    private Vector2 inputVec;
    private Vector2 playerPos;
    private float radLook;
    private float angle;

    public GameObject bulletPrefab;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move.y = 0;
        controller.Move(move.normalized * Time.deltaTime * 10f);

        playerPos = Camera.main.WorldToScreenPoint(transform.position);
        radLook = Mathf.Atan2(Input.mousePosition.y - playerPos.y, Input.mousePosition.x - playerPos.x);
        angle = radLook * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 120, 0);

        if(Input.GetMouseButtonDown(0))
        {
            ShootGunBoss();
        }
    }
    private void ShootGunBoss()
    {
        Debug.Log("shoot");
        RaycastHit hit;
        GameObject bullet = Instantiate(bulletPrefab, transform.position,Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = transform.position + transform.forward * 1000;
            bulletController.hit = false;
        }
    }
}
