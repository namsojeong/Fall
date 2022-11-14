using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// FSM에 State 상태를 정의해놓은 스크립트
/// </summary>
public class BaseState
{
    // state 이름
    public string name;
    protected StateMachine stateMachine;

    // State 생성자
    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    // 상태들
    public virtual void Enter() { } // 상태 시작 시
    public virtual void UpdateLogic() { } // Update
    public virtual void UpdateLate() { } // LateUpdate
    public virtual void Exit() { } // 상태 끝났을 시

    // 상태 조건 체크하는 함수
    public virtual void CheckDistance() { }

    // 애니메이션 구조 함수들
    public virtual void SetAnim() { }
    public virtual void SetAnim(bool isPlay) { }
}
