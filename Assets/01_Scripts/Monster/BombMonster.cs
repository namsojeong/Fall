using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    IDLE,
    WALK,
    ATTACK,
    DAMAGE
}
public class BombMonster : MonoBehaviour
{

    private Transform target = null; // Ÿ��


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
    public Transform targetPos => target.transform; // Ÿ�ٰ��� �Ÿ�

    // Ÿ�ٰ��� �Ÿ� ���ϱ�
    private float GetDistance() { return Vector3.Distance(target.transform.position, transform.position); }

    // Ÿ�ٰ��� ���� ���ϱ�
    private Vector3 GetDirection()
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
    public int hashWalk = Animator.StringToHash("Walk");
    public int hashAttack = Animator.StringToHash("Attack");
    public int hashDamage = Animator.StringToHash("Damage");


    // �̵� �ִϸ��̼�
    public void ChangeState(MonsterState state, bool isOn)
    {
        switch(state)
        {
            case MonsterState.IDLE:
                break;
            case MonsterState.WALK:
        anim.SetBool(hashWalk, isOn);
                break;
            case MonsterState.ATTACK:
        anim.SetBool(hashAttack, isOn);
                break;
            case MonsterState.DAMAGE:
        anim.SetBool(hashDamage, isOn);
                break;
        }
    }

    #endregion
}
