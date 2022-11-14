using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// FSM�� State ���¸� �����س��� ��ũ��Ʈ
/// </summary>
public class BaseState
{
    // state �̸�
    public string name;
    protected StateMachine stateMachine;

    // State ������
    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    // ���µ�
    public virtual void Enter() { } // ���� ���� ��
    public virtual void UpdateLogic() { } // Update
    public virtual void UpdateLate() { } // LateUpdate
    public virtual void Exit() { } // ���� ������ ��

    // ���� ���� üũ�ϴ� �Լ�
    public virtual void CheckDistance() { }

    // �ִϸ��̼� ���� �Լ���
    public virtual void SetAnim() { }
    public virtual void SetAnim(bool isPlay) { }
}
