using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Mono.Cecil;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake() => Instance = this;

    private float playerSpeed = 0f;
    public float GetPlayerSpeed => playerSpeed;

    #region WALK_BULLET

    [SerializeField] private float playerWalkSpeed = 10.0f;
    [SerializeField] private float playerRunSpeed = 20.0f;
    [SerializeField] private float playerStopSpeed = 0.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private bool isRun = false;
    [SerializeField] private float bulletHitMissDistance = 25f;

    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform firePos;
    private Transform camTransform;

    public CharacterController controller;
    private PlayerInput input;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public GameObject model;
    public bool _isBoss = false;
    #region InputAction
    private InputAction moveAction;
    public InputAction jumpAction;
    public InputAction shootAction;

    float time = 0f;

    #endregion

    #endregion

    private bool isBomb = false;
    private bool isLaser = false;

    #region HP

    private CharacterHP playerHP;
    public Image hpImage;
    public Text hpText;
    public float slideSpeed;

    #endregion

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input= GetComponent<PlayerInput>();
        playerHP = GetComponent<CharacterHP>();
        camTransform = Camera.main.transform;
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
    }

    void Update()
    {
        model.transform.position = transform.position;
        playerSpeed = Input.GetKey(KeyCode.LeftShift) ? playerRunSpeed : playerWalkSpeed;
        
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x,0,input.y);
        move = move.x * camTransform.right.normalized + move.z*camTransform.forward.normalized;
        move.y = 0;
        if(Input.GetMouseButtonDown(0))
        {
            ShootGunBoss();
        }

        float speed;
        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero) {
            speed = playerRunSpeed;
        }
        else if (move != Vector3.zero)
        {
            speed = playerWalkSpeed;
        }
        else
        {
            speed = playerStopSpeed;
        }
        if(isLaser)
        {
            speed -= 5;
        }
        playerSpeed = speed;

        controller.Move(move * Time.deltaTime * playerSpeed);
        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if(isBomb)
        {
         //   playerVelocity.y = bombPower;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


        HPSlide();

        if(Input.GetKeyDown(KeyCode.R))
        {
            playerHP.ReviveHP();
        }
    }

    #region Shoot
    private void ShootGunBoss()
    {
        Debug.Log("shoot");
        RaycastHit hit;
        GameObject bullet = ObjectPool.Instance.GetObject(PoolObjectType.BULLET);
        SetBullet(bullet);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = camTransform.position + camTransform.forward * 1000;
            Debug.Log(bulletController.target);
            bulletController.hit = false;
        }
    }

    private void SetBullet(GameObject bullet)
    {
        bullet.transform.position = barrelTransform.position;
        bullet.transform.rotation = Quaternion.identity;
    }

    #endregion


    #region Bomb

    public void Bomb(int damage, float bombPower)
    {
        isBomb = true;
        Hit(damage);
        playerVelocity.y = bombPower;
        Debug.Log("Bomb");
    }


    #endregion

    #region HP

    private void HPSlide()
    {
        hpText.text = String.Format($"{playerHP.HP}%");
        hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, playerHP.HP/playerHP.max_hp, Time.deltaTime * slideSpeed);
    }


    public void Hit(int damage)
    {
        playerHP.Hit(damage);
        if (playerHP.IsDead)
        {
            UI.Instance.ChangeScene("GameOver");
        }
    }

    #endregion

    #region SET

    public void SlowSpeed(bool isL)
    {
        isLaser = isL;
    }

    #endregion
}