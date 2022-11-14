using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
/// <summary>
/// FSM 로직
/// </summary>
public class StateMachine : MonoBehaviour
{
    BaseState curState; // 현재 상태

    protected virtual BaseState GetInitState() { return null; }
    protected virtual float GetDistance() { return 0.0f; }
    protected virtual Vector3 GetDirection() { return Vector3.zero; }


    private void Start()
    {
        // 기본 상태 가져오기
        curState = GetInitState();

        // 기본 상태가 있다면 시작
        if (curState != null)
        {
            curState.Enter();
        }
    }

    private void Update()
    {
        // State Update 해주기
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
        // 물리를 위해 혹시 모르니까 LateUpdate도 구현 해주기
        if (curState != null)
        {
            curState.UpdateLate();
        }
    }

    // 상태 바꾸기 사용할 때 아래와 같은 형식으로 쓰면 됨
    // stateMachine.ChangeState(((BasicMonster)stateMachine).idleState);
    public void ChangeState(BaseState newState)
    {
        // State 끝내기
        curState.Exit();

        // State 새로 할당
        curState = newState;
        curState.Enter();
    }


}
