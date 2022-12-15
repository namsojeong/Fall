using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultController : MonoBehaviour
{
    //public static PlayerDefaultController Instance { get; private set; }
    public CharacterController controller;
    private PlayerInput input;
    private Rigidbody defaultPlayerRB;
    
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    public bool groundedPlayer;
    private Vector3 playerVelocity;
    private Vector2 inputVec;
    

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction shootAction;
    
    public GameObject bulletPrefab;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];
        defaultPlayerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    
    
}
