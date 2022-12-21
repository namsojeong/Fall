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
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]

public class PlayerDefaultController : MonoBehaviour
{

    public GameObject model;

    private Vector3 playerVelocity;

    private float hitMaxDist = 0.1f;
    private float radLook;
    private float angle;

    public Transform firePos;
    private Transform camTransform;

    private Animator anim;
    private Rigidbody rigid;

    private bool isBomb = false;
    private bool isLaser = false;

    #region Speed
    public float PlayerSpeed { get { return playerSpeed; } set { playerSpeed = value; } }
    private float playerSpeed = 0f;

    [SerializeField] private float playerWalkSpeed = 5.0f;
    [SerializeField] private float playerRunSpeed = 10.0f;
    [SerializeField] private float playerStopSpeed = 0.0f;

    #endregion

    #region InputAction

    public bool jumpactionbool;

    private PlayerInput input;
    private InputAction moveAction;
    public InputAction jumpAction;
    public InputAction shootAction;
    public InputAction aimAction;

    #endregion

    #region Sound

    [SerializeField] AudioSource walk;
    [SerializeField] AudioClip playerAudio;
    [SerializeField] AudioClip doorSound;
    private string gunSound = "gun";

    #endregion

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();

        camTransform = Camera.main.transform;

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
    }

    void Update()
    {
        model.transform.position = transform.position;

        if (jumpAction.triggered && CheckIsGround())
        {
            Jump();
        }
        Move();

        CheckGround();
        PlayerMoveAnim();
        PlayerJumpAnim();
        //PlayerShotAnim();

        if(transform.position.y <= -10f)
        {
            PlayerPrefs.SetInt("SCORE", 0);
            UI.Instance.ChangeScene(SceneState.GAMEOVER);
        }

    }

    #region Move 

    public bool CheckIsGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, hitMaxDist))
        {
            return true;
        }
        return false;
    }

 private void Jump()
    {
       
            rigid.AddForce(Vector3.up * 300f);
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, playerVelocity.y, input.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.15f);

        float speed;
        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero)
        {
           WalkPlay(true, 2.5f);
            speed = playerRunSpeed;
        }
        else if (move != Vector3.zero)
        {
           WalkPlay(true, 2);
            speed = playerWalkSpeed;
        }
        else
        {
            WalkPlay(false, 2);
            speed = playerStopSpeed;
        }
        playerSpeed = speed;

        transform.position += move.normalized * (Time.deltaTime * playerSpeed);
    }

    public void WalkPlay(bool isPlay, float pitch)
    {
        walk.pitch = pitch;
        if (!isPlay)
        {
            walk.Stop();
        }
        else if (!walk.isPlaying)
        {
            walk.Play();

        }
    }

    #endregion

    #region Bomb

    public void Bomb(int damage, float bombPower)
    {
        isBomb = true;
        rigid.AddForce(Vector3.up * bombPower);
    }

    #endregion

    #region Animation

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
        anim.SetFloat("speed", PlayerSpeed);
    }
    void PlayerJumpAnim()
    {
        if (jumpAction.triggered)
            anim.SetTrigger("jump");
    }

    #endregion

    #region Collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Gate"))
        {
            StartCoroutine(DoorDelay());
        }
    }

    private IEnumerator DoorDelay()
    {
        SoundManager.Instance.SFXPlay(doorSound);
        yield return new WaitForSeconds(0.5f);
        UI.Instance.ChangeScene(SceneState.VS);
    }
    #endregion
}

