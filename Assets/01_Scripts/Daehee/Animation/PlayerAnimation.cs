using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    CharacterController playerController;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<CharacterController>();
    }


    void Update()
    {
        IsGroundCheck();
        PlayerMoveAnim();
        PlayerJumpAnim();
        PlayerShotAnim();
    }

    void IsGroundCheck()
    {
            if(playerController.isGrounded)
                animator.SetBool("isGround", true);
    }

    void PlayerShotAnim()
    {
        bool isShot;
        isShot = SceneManager.GetActiveScene().name == "DefaultGameScene" ? 
            PlayerDefaultController.Instance.shootAction.triggered : PlayerController.Instance.shootAction.triggered;
        if(isShot)  
            animator.SetTrigger("shot");
    }

    void PlayerMoveAnim()
    {
        float speed;
        if (SceneManager.GetActiveScene().name == "DefaultGameScene")
            speed = PlayerDefaultController.Instance.Speed;
        else
            speed = PlayerController.Instance.GetPlayerSpeed;
        animator.SetFloat("speed", speed);
    }
    void PlayerJumpAnim()
    {
        bool isground;
        isground = SceneManager.GetActiveScene().name == "DefaultGameScene" ?
            PlayerDefaultController.Instance.jumpAction.triggered : PlayerController.Instance.jumpAction.triggered;
        if (isground)
            animator.SetTrigger("jump");
    }
}
