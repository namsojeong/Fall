using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultController : MonoBehaviour
{
    public CharacterController controller;
    private PlayerInput input;
    private InputAction moveAction;

    private Vector2 inputVec;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        inputVec = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputVec.x, 0, inputVec.y);
        controller.Move(move.normalized * Time.deltaTime * 10f);
    }
}
