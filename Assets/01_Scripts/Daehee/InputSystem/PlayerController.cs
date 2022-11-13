using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake() => Instance = this;
    public float playerSpeed = 0f;

    [SerializeField]
    private float playerWalkSpeed = 10.0f;
    [SerializeField]
    private float playerRunSpeed = 20.0f;
    [SerializeField]
    private float playerStopSpeed = 0.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 8f;
    [SerializeField]
    private bool isRun = false;

    private Transform camTransform;

    public CharacterController controller;
    private PlayerInput input;
    private Vector3 playerVelocity;
    public bool groundedPlayer;


    #region InputAction
    private InputAction moveAction;
    public InputAction jumpAction;

    float time = 0f;

    #endregion

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input= GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
    }

    void Update()
    {
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
        

    }
}