using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// �ٰŸ� Monster�� �⺻ FSM
/// </summary>
public class BasicCloseMonster : StateMachine
{
    private Transform target = null; // Ÿ��

    // ���� ��ũ��Ʈ
    public BasicMonsterIdle idleState;
    public BasicMonsterMove moveState;

    // Component
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;

    // �ʿ� ���� => ���߿� SO�� ���� ����
    private float walkingSpeed = Define.MONSTER_SPEED;
    private float colRadius = Define.MONSTER_SERCH_RANGE; // 
    public float moveRange = Define.MONSTER_MOVE_RANGE;
    
    public LayerMask targetLayerMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();


        // ���� �Ҵ�
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
    public float distance => GetDistance(); // Ÿ�ٰ��� �Ÿ�
    public Vector3 dir => GetDirection(); // Ÿ�ٰ��� �Ÿ�
    protected override BaseState GetInitState() { return idleState; } // �⺻ State ��������

    // Ÿ�ٰ��� �Ÿ� ���ϱ�
    protected override float GetDistance() {   return Vector3.Distance(target.transform.position, transform.position); }

    // Ÿ�ٰ��� ���� ���ϱ�
    protected override Vector3 GetDirection()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        return dir;
    }

    #endregion

    #region TARGET

    // Ÿ�� ���ϱ�
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
    // Ÿ�� �Ĵٺ���
    public void LookTarget(Transform target)
    {
        Vector3 dir = GetDirection();
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        transform.rotation = rot;
    }
    #endregion

    #region ANIMATION

    // �ִϸ��̼� Hash
    [HideInInspector]
    public int hashWalk = Animator.StringToHash("IsWalk");
    

    // �̵� �ִϸ��̼�
    public void MoveAnimation(bool isOn)
    {
        anim.SetBool("IsWalk", isOn);
    }

    #endregion

}
