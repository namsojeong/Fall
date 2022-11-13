using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 8f;
    private Transform camTransform;

    private CharacterController controller;
    private PlayerInput input;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    #region InputAction
    private InputAction moveAction;
    private InputAction jumpAction;
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
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x,0,input.y);
        move = move.x * camTransform.right.normalized + move.z*camTransform.forward.normalized;
        move.y = 0;
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