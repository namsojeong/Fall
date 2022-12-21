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
    PlayerController player;
    PlayerDefaultController playerDefault;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerController>();
        playerDefault = GetComponentInParent<PlayerDefaultController>();
    }


    void Update()
    {
        PlayerMoveAnim();
        PlayerJumpAnim();
        PlayerShotAnim();
    }

    void PlayerShotAnim()
    {
        if (player.shootAction.triggered)
            animator.SetTrigger("shot");
    }

    void PlayerMoveAnim()
    {
        animator.SetFloat("speed", player.GetPlayerSpeed);
    }
    void PlayerJumpAnim()
    {
        if (player.jumpAction.triggered)
            animator.SetTrigger("jump");
    }
}
