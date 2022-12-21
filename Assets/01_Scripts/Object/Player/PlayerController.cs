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

    #region PLAYER

    [SerializeField] private float playerWalkSpeed = 10.0f;
    [SerializeField] private float playerRunSpeed = 20.0f;
    [SerializeField] private float playerStopSpeed = 0.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private float hitMaxDist = 0.1f;


    [SerializeField] private bool isRun = false;
    public bool isGround;

    public Transform firePos;
    public Transform FallPos;
    private Transform camTransform;

    public GameObject model;

    private Animator anim;
    private RaycastHit hit;
    private Rigidbody rigid;
    private Vector3 playerVelocity;

    private CharacterHP playerHP;

    #endregion

    #region InputAction

    private PlayerInput input;
    private InputAction moveAction;
    public InputAction jumpAction;
    public InputAction shootAction;

    #endregion

    #region MONSTER

    public bool _isBoss = false;
    private bool isBomb = false;
    private bool isLaser = false;
    private float bombPower;

    #endregion

    #region Sound

    private string gunSound = "gun";
    [SerializeField] AudioClip playerAudio;

    #endregion

    private void Start()
    {
        camTransform = Camera.main.transform;

        input = GetComponent<PlayerInput>();
        playerHP = GetComponent<CharacterHP>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
    }

    void Update()
    {
        isGround = CheckIsGround();
        Setting();

        if (jumpAction.triggered && isGround)
        {
            Jump();
        }
        AnimationStateCheck();
        GameSceneMove();
        GameSceneRotate();

        playerHP.HPSlide();

        if (shootAction.triggered)
        {
            SoundManager.Instance.SFXPlay(playerAudio);
            GameSceneShootGun();
        }
        if (transform.position.y <= -10f) {
            Respawn();
        }
    }

    private void Jump()
    {
        rigid.AddForce(Vector3.up * 300f);
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

    void Respawn()
    {
        Hit(30);
        transform.position = new Vector3(0, 0, 0);
    }

    void Setting()
    {
        model.transform.position = transform.position;
        playerSpeed = Input.GetKey(KeyCode.LeftShift) ? playerRunSpeed : playerWalkSpeed;
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
           Die();
        }
    }

    private void Die()
    {
            ScoreManager.Instance.SaveScore();
            UI.Instance.ChangeScene(SceneState.GAMEOVER);
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
        if (shootAction.triggered)
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