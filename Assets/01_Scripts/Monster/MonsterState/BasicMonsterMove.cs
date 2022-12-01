using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Runtime.InteropServices.WindowsRuntime;
/// <summary>
/// �ٰŸ� ������ �̵� ��ũ��Ʈ
/// </summary>
public class BasicMonsterMove : BaseState
{
    BasicCloseMonster monster;
    Transform target;

    // Move ������
    public BasicMonsterMove(BasicCloseMonster stateMachine) : base("MOVE", stateMachine)
    {
        monster = (BasicCloseMonster)stateMachine;
    }

    #region MOVE

    private void SetMove(bool isMove)
    {
        //monster.agent.isStopped = !isMove;
    }

    #endregion

    #region ANIMATION

    public override void SetAnim(bool isPlay)
    {
        base.SetAnim();
        //monster.MoveAnimation(isPlay);
    }

    #endregion

    #region STATE


    // �ٸ� STATE�� �Ѿ�� ����
    public override void CheckDistance()
    {
        base.CheckDistance();
    }

    // NavMesh�� �̿��Ͽ� �̵�
    // �����ִٸ� isStopped=false
    // �ִϸ��̼� ����
    // Ÿ�� �Ѿư��� ����
    public override void Enter()
    {
        base.Enter();

        SetMove(true);
        SetAnim(true);
    }

    // Ÿ�� ��� ã���� �Ѿư��� + �Ĵٺ���
    // ���� �Ÿ��� ���� ���� ��ȯ
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //target = monster.SerachTarget();

    }

    // ���� ������ ��
    // �ִϸ��̼� ���� ������ ���߱�
    public override void Exit()
    {
        base.Exit();
        SetAnim(false);
        SetMove(false);
    }
    #endregion
}
