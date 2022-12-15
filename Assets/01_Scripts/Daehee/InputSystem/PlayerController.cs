using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Mono.Cecil;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake() => Instance = this;

    private float playerSpeed = 0f;
    public float GetPlayerSpeed => playerSpeed;
    public void SetPlayerSpeed(float value) => playerSpeed = value;
    private Vector2 playerPos;
    private float radLook;
    private float angle;

    #region WALK_BULLET

    [SerializeField] private float playerWalkSpeed = 10.0f;
    [SerializeField] private float playerRunSpeed = 20.0f;
    [SerializeField] private float playerStopSpeed = 0.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private bool isRun = false;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private float bombPower;

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
    public InputAction aimAction;

    float time = 0f;

    #endregion

    #endregion

    private bool isBomb = false;
    private bool isLaser = false;
    public bool isLooking = false;

    #region HP

    private CharacterHP playerHP;
    public Image hpImage;
    public Text hpText;
    public float slideSpeed;

    #endregion

    #region Sound

    private string gunSound = "gun";
    [SerializeField] AudioClip playerAudio;

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
        aimAction = input.actions["Aim"];
    }

    void Update()
    {
        DefaultSetting();
        if (SceneManager.GetActiveScene().name == "DefaultGameScene")
        {
            DefaultSceneRotate();
            DefaultSceneMove();
        }
        else
        {
            GameSceneMove();
            GameSceneRotate();
            HPSlide();
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerHP.ReviveHP();
            }
        }
    }

    #region Setting
    void DefaultSetting()
    {
        isLooking = aimAction.IsPressed();
        model.transform.position = transform.position;
        playerSpeed = Input.GetKey(KeyCode.LeftShift) ? playerRunSpeed : playerWalkSpeed;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (isBomb)
        {
            playerVelocity.y = bombPower;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }
    #endregion

    #region Rotate
    private void DefaultSceneRotate()
    {
        playerPos = Camera.main.WorldToScreenPoint(transform.position);
        radLook = Mathf.Atan2(Input.mousePosition.y - playerPos.y, Input.mousePosition.x - playerPos.x);
        angle = radLook * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 120, 0);
    }
    void GameSceneRotate()
    {
        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Move
    private void GameSceneMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
        move.y = 0;
        if (aimAction.IsPressed() && isLooking)
        {
            SoundManager.instance.SFXPlay(gunSound, playerAudio);
            GameSceneShootGun();
        }

        float speed;
        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
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
        if (isLaser)
        {
            speed -= 5;
        }
        playerSpeed = speed;

        controller.Move(move * Time.deltaTime * playerSpeed);
    }

    private void DefaultSceneMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        float speed;
        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
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
        playerSpeed = speed;
        controller.Move(move.normalized * (Time.deltaTime * playerSpeed));
        if (aimAction.IsPressed() && isLooking)
        {
            SoundManager.instance.SFXPlay(gunSound, playerAudio);
            DefaultSceneShootGun();
        }
    }
    #endregion

    #region Shoot

    private void DefaultSceneShootGun()
    {
        RaycastHit hit;
        GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
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
    private void GameSceneShootGun()
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