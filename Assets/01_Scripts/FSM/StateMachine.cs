using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
/// <summary>
/// FSM ����
/// </summary>
public class StateMachine : MonoBehaviour
{
    BaseState curState; // ���� ����

    protected virtual BaseState GetInitState() { return null; }
    protected virtual float GetDistance() { return 0.0f; }
    protected virtual Vector3 GetDirection() { return Vector3.zero; }


    private void Start()
    {
        // �⺻ ���� ��������
        curState = GetInitState();

        // �⺻ ���°� �ִٸ� ����
        if (curState != null)
        {
            curState.Enter();
        }
    }

    private void Update()
    {
        // State Update ���ֱ�
        if (curState != null)
        {
            curState.UpdateLogic();

            curState.CheckDistance();
        }
        else
        {
            Debug.Log("Error");
        }
    }

    private void LateUpdate()
    {
        // ������ ���� Ȥ�� �𸣴ϱ� LateUpdate�� ���� ���ֱ�
        if (curState != null)
        {
            curState.UpdateLate();
        }
    }

    // ���� �ٲٱ� ����� �� �Ʒ��� ���� �������� ���� ��
    // stateMachine.ChangeState(((BasicMonster)stateMachine).idleState);
    public void ChangeState(BaseState newState)
    {
        // State ������
        curState.Exit();

        // State ���� �Ҵ�
        curState = newState;
        curState.Enter();
    }


}
