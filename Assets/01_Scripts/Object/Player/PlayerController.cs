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
    private Animator anim;

    #endregion

    #region Sound

    private string gunSound = "gun";
    [SerializeField] AudioClip playerAudio;

    #endregion

    public bool jumpactionbool;
    private void Start()
    {
        input = GetComponent<PlayerInput>();
        playerHP = GetComponent<CharacterHP>();
        anim = GetComponentInChildren<Animator>();
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
       
        Jump();
        AnimationStateCheck();
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

    private void Jump()
    {
        if (jumpAction.triggered && isGround)
        {
            rigid.AddForce(Vector3.up * 300f);
        }
    }

    public bool CheckIsGround()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, hitMaxDist))
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

    #endregion

    #region Shoot

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
        bullet.transform.position = firePos.position;
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

    #region ANIMATION

    private void AnimationStateCheck()
    {
        CheckGround();
        PlayerMoveAnim();
        PlayerJumpAnim();
        PlayerShotAnim();
    }

    private void CheckGround()
    {
        if (CheckIsGround() && !jumpAction.triggered)
        {
            anim.SetBool("isGround", true);
        }
    }

    void PlayerShotAnim()
    {
        if (shootAction.triggered && aimAction.IsPressed())
            anim.SetTrigger("shot");
    }

    void PlayerMoveAnim()
    {
        anim.SetFloat("speed", GetPlayerSpeed);
    }
    void PlayerJumpAnim()
    {
        if (jumpAction.triggered)
            anim.SetTrigger("jump");
    }
    #endregion

}