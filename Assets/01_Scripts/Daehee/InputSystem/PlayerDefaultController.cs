using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultController : MonoBehaviour
{
    public CharacterController controller;
    private PlayerInput input;
    private Rigidbody _playerRigid;
    
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    public bool groundedPlayer;
    private Vector3 playerVelocity;
    private Vector2 inputVec;
    private Vector2 playerPos;
    private float radLook;
    private float angle;

    public InputAction moveAction;
    public InputAction jumpAction;
    
    public GameObject bulletPrefab;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        _playerRigid = GameObject.Find("Player 1").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
        groundedPlayer = controller.isGrounded;
        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = 0;
        }
        if(Input.GetMouseButtonDown(0))
        {
            ShootGunBoss();
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
        }
        _playerRigid.velocity = playerVelocity;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        controller.Move(move.normalized * (Time.deltaTime * 10f));
    }

    private void RotatePlayer()
    {
        playerPos = Camera.main.WorldToScreenPoint(transform.position);
        radLook = Mathf.Atan2(Input.mousePosition.y - playerPos.y, Input.mousePosition.x - playerPos.x);
        angle = radLook * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 120, 0);

    }
    private void ShootGunBoss()
    {
        RaycastHit hit;
        GameObject bullet = Instantiate(bulletPrefab, transform.position+Vector3.up,Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = transform.position + transform.forward * 1000;
            bulletController.hit = false;
        }
        Debug.Log("ASdfsdf");
    }
}
