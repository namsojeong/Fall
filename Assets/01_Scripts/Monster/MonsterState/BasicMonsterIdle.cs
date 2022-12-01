using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// (�⺻) �ٰŸ� ���� IDLE ���� 
/// </summary>
public class BasicMonsterIdle : BaseState
{
    BasicCloseMonster monster;
    Transform target = null;

    // IDLE ���� ����
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

    // ���� ���� ��
    // Ÿ�� ã�� ����
    public override void Enter()
    {
        base.Enter();
    }

    // Ÿ���� �ִٸ�
    // �Ÿ��� ���� ���� ��ȯ
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //target = monster.SerachTarget();
    }

    // ���� ������ ��
    public override void Exit()
    {
        base.Exit();
    }

    #endregion
}
