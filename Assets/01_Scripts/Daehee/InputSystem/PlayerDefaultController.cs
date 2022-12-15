using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultController : MonoBehaviour
{
    //public static PlayerDefaultController Instance { get; private set; }
    public CharacterController controller;
    private PlayerInput input;
    private Rigidbody defaultPlayerRB;
    
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    public bool groundedPlayer;
    private Vector3 playerVelocity;
    private Vector2 inputVec;
    private Vector2 playerPos;
    private float speed;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    private float radLook;
    private float angle;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction shootAction;
    
    public GameObject bulletPrefab;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
        defaultPlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        groundedPlayer = controller.isGrounded;
        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = 0;
        }
        if(shootAction.triggered)
        {
            ShootGunBoss();
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
        }
        if (Input.GetKey(KeyCode.LeftShift) && defaultPlayerRB.velocity != Vector3.zero) 
        {
            speed=20f;
        }
        else if (defaultPlayerRB.velocity != Vector3.zero)
        {
            speed = 10f;
        }
        else
        {
            speed = 0f;
        }
        MovePlayer();
        RotatePlayer();
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        controller.Move(move.normalized * (Time.deltaTime * speed));
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
