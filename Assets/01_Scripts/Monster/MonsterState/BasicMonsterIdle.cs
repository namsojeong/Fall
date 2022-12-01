using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// (기본) 근거리 몬스터 IDLE 상태 
/// </summary>
public class BasicMonsterIdle : BaseState
{
    BasicCloseMonster monster;
    Transform target = null;

    // IDLE 상태 정의
    public BasicMonsterIdle(BasicCloseMonster stateMachine) : base("IDLE", stateMachine)
    {
        monster = (BasicCloseMonster)stateMachine;
    }

    #region STATE

    public override void CheckDistance()
    {
        base.CheckDistance();
        if (target == null) return;

    }

    // 상태 시작 시
    // 타겟 찾기 시작
    public override void Enter()
    {
        base.Enter();
    }

    // 타겟이 있다면
    // 거리에 따라 상태 전환
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //target = monster.SerachTarget();
    }

    // 상태 끝났을 시
    public override void Exit()
    {
        base.Exit();
    }

    #endregion
}
