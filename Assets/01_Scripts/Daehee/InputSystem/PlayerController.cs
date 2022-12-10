using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake() => Instance = this;

    #region WALK_BULLET
    public float playerSpeed = 0f;

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

    #region HP

    private CharacterHP playerHP;
    public Image hpImage;
    public Text hpText;
    public float slideSpeed;

    #endregion

    private void Start()
    {
        #region Walk
        controller = GetComponent<CharacterController>();
        input= GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];

        #endregion

        playerHP = GetComponent<CharacterHP>();
    }

    void Update()
    {
        #region Walk
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

        if (Input.GetKey(KeyCode.LeftShift) && move != Vector3.zero) { 
            playerSpeed = playerRunSpeed;
        }
        else if (move != Vector3.zero)
        {
            playerSpeed = playerWalkSpeed;
        }
        else
        {
            playerSpeed = playerStopSpeed;
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        #endregion

        HPSlide();

        if(Input.GetKeyDown(KeyCode.V))
        {
            Hit();
        }
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
        GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, firePos);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = camTransform.position + camTransform.forward * bulletHitMissDistance;
            bulletController.hit = true;
        }
    }

    #endregion


    #region Bomb

    public void Bomb()
    {
        if (isBomb) return;
        isBomb = true;
        Hit();
        StartCoroutine(BombPower());
    }

    private IEnumerator BombPower()
    {
        playerVelocity.y += Mathf.Sqrt(5f * -3.0f * gravityValue);
        yield return new WaitForSeconds(0.3f);
        isBomb = false;
    }

    #endregion

    #region HP

    private void HPSlide()
    {
        hpText.text = String.Format($"{playerHP.HP}%");
        hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, playerHP.HP/playerHP.max_hp, Time.deltaTime * slideSpeed);
    }


    public void Hit()
    {
        playerHP.Hit(10);
        if (playerHP.IsDead)
        {
            UiManager.Instance.ChangeScene("GameOver");
        }
    }

    #endregion
}