using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Mono.Cecil;
using System.Security.Claims;
using Cinemachine;

[RequireComponent(typeof(PlayerInput))]
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
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool isRun = false;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private float bombPower;

    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform firePos;
    private Transform camTransform;

    private PlayerInput input;
    private Vector3 playerVelocity;
    private Rigidbody rigid;
    public GameObject model;
    public Transform FallPos;
    public bool _isBoss = false;
    public bool isGround;
    private RaycastHit hit;
    float hitMaxDist = 0.1f;
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

    #region HP

    private CharacterHP playerHP;
    public bool isDefault = true;

    #endregion

    #region Sound

    private string gunSound = "gun";
    [SerializeField] AudioClip playerAudio;

    #endregion

    public bool jumpactionbool;
    private void Start()
    {
        input= GetComponent<PlayerInput>();
        playerHP = GetComponent<CharacterHP>();
        rigid = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
        aimAction = input.actions["Aim"];
    }

    void Update()
    {
        isGround = CheckIsGround();
        DefaultSetting();
        if (jumpAction.triggered && isGround)
        {
            rigid.AddForce(Vector3.up * 300f);
        }
        if (isDefault)
        {
            DefaultSceneRotate();
            DefaultSceneMove();
            if (shootAction.triggered)
            {
                SoundManager.Instance.SFXPlay(playerAudio);
                DefaultSceneShootGun();
            }
        }
        else
        {
            GameSceneMove();
            GameSceneRotate();
            playerHP.HPSlide();
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerHP.ReviveHP();
            }
            if (shootAction.triggered && aimAction.IsPressed())
            {
                SoundManager.Instance.SFXPlay(playerAudio);
                GameSceneShootGun();
            }
        }
    }

    public bool CheckIsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if(Physics.Raycast(transform.position, Vector3.down, out hit, hitMaxDist))
        {
            return true;
        }
        return false;
    }

    #region Setting
    void DefaultSetting()
    {
        model.transform.position = transform.position;
        playerSpeed = Input.GetKey(KeyCode.LeftShift) ? playerRunSpeed : playerWalkSpeed;

        if (CheckIsGround() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        
        if (isBomb)
        {
            playerVelocity.y = bombPower;
        }
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

        transform.position += move * Time.deltaTime * playerSpeed;
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
        transform.position += move.normalized * (Time.deltaTime * playerSpeed);
        if (aimAction.IsPressed())
        {
            SoundManager.Instance.SFXPlay(playerAudio);
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
        Vector3 playerpos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousepos = Camera.main.WorldToScreenPoint(Input.mousePosition);
        if (Physics.Raycast(transform.position + Vector3.up, mousepos- playerpos, out hit, Mathf.Infinity))
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
            Debug.Log(bulletController.target);
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
        rigid.AddForce(Vector3.up * bombPower);
        Hit(damage);
    }


    #endregion

    #region HP


    public void Hit(int damage)
    {
        playerHP.Hit(damage);
        if (playerHP.IsDead)
        {
            ScoreManager.Instance.SaveScore();
            UI.Instance.ChangeScene(SceneState.GAMEOVER);
        }
    }

    #endregion

    #region SET

    public void SlowSpeed(bool isL)
    {
        isLaser = isL;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Gate")
        {
            UI.Instance.ChangeScene(SceneState.VS);
        }
    }
}