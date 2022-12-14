using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        PlayerMoveAnim();
        PlayerJumpAnim();
    }

    void PlayerMoveAnim()
    {
        float speed = PlayerController.Instance.GetPlayerSpeed;
        animator.SetFloat("speed", speed);
    }
    void PlayerJumpAnim()
    {
        bool isground = PlayerController.Instance.jumpAction.triggered;
        if (isground)
            animator.SetTrigger("jump");
    }
}
