using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 근거리 Monster의 기본 FSM
/// </summary>
public class BasicCloseMonster : StateMachine
{
    private Transform target = null; // 타겟

    // 상태 스크립트
    public BasicMonsterIdle idleState;
    public BasicMonsterMove moveState;

    // Component
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;

    // 필요 변수 => 나중에 SO로 뽑을 예정
    private float walkingSpeed = Define.MONSTER_SPEED;
    private float colRadius = Define.MONSTER_SERCH_RANGE; // 
    public float moveRange = Define.MONSTER_MOVE_RANGE;
    
    public LayerMask targetLayerMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();


        // 상태 할당
        idleState = new BasicMonsterIdle(this);
        moveState = new BasicMonsterMove(this);

        ResetMonster();
    }

    #region SET

    public void ResetMonster()
    {
        agent.speed = walkingSpeed;
    }

    #endregion

    #region GET
    public float distance => GetDistance(); // 타겟과의 거리
    public Vector3 dir => GetDirection(); // 타겟과의 거리
    protected override BaseState GetInitState() { return idleState; } // 기본 State 가져오기

    // 타겟과의 거리 구하기
    protected override float GetDistance() {   return Vector3.Distance(target.transform.position, transform.position); }

    // 타겟과의 방향 구하기
    protected override Vector3 GetDirection()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        return dir;
    }

    #endregion

    #region TARGET

    // 타겟 구하기
    public Transform SerachTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, colRadius, targetLayerMask);
        if (cols.Length > 0)
        {
            target = cols[0].gameObject.transform;
            return target;
        }
        else return null;
    }
    // 타겟 쳐다보기
    public void LookTarget(Transform target)
    {
        Vector3 dir = GetDirection();
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        transform.rotation = rot;
    }
    #endregion

    #region ANIMATION

    // 애니메이션 Hash
    [HideInInspector]
    public int hashWalk = Animator.StringToHash("IsWalk");
    

    // 이동 애니메이션
    public void MoveAnimation(bool isOn)
    {
        anim.SetBool("IsWalk", isOn);
    }

    #endregion

}
