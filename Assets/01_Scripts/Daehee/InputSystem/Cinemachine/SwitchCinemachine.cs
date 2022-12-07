using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCinemachine : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;
    [SerializeField]
    private int priorityBoostAmount = 9;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;


    public static CinemachineVirtualCamera vCam;
    private InputAction aimAction;
    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        aimAction = input.actions["Aim"];
    }

    void OnEnable()
    {
        aimAction.performed += _ => StartAim();   
        aimAction.canceled += _ => CancelAim();   
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    private void CancelAim()
    {
        vCam.Priority += priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;

    }

    private void StartAim()
    {
        vCam.Priority -= priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

}
