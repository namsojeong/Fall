using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;

public enum MonsterState
{
    IDLE,
    WALK,
    ATTACK,
    DAMAGE
}
public class BombMonster : MonoBehaviour
{
    private Transform target = null; // 타겟

    // Component
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;

    public LayerMask targetLayerMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        ResetMonster();
    }

    #region HP

    private const int MAXHP = 2;
    private int curHp = MAXHP;
    public int GetHP => curHp;
    public void SetHP(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }

    #endregion

    #region SET

    public void ResetMonster()
    {
        agent.speed = Define.MONSTER_SPEED;
        agent.stoppingDistance = 5f;
    }

    #endregion

    #region GET
    public float distance => GetDistance(); // 타겟과의 거리
    public Vector3 dir => GetDirection(); // 타겟과의 거리
    public Transform targetPos => target.transform; // 타겟과의 거리

    // 타겟과의 거리 구하기
    private float GetDistance()
    {
        SerachTarget();
        return Vector3.Distance(target.transform.position, transform.position);
    }

    // 타겟과의 방향 구하기
    private Vector3 GetDirection()
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
        Collider[] cols = Physics.OverlapSphere(transform.position, Define.MONSTER_SERCH_RANGE, targetLayerMask);
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
    public int hashWalk = Animator.StringToHash("Walk");
    [HideInInspector]
    public int hashAttack = Animator.StringToHash("Attack");
    [HideInInspector]
    public int hashDamage = Animator.StringToHash("Damage");

    // 이동 애니메이션
    public void ChangeState(MonsterState state, bool isOn)
    {
        switch (state)
        {
            case MonsterState.IDLE:
                {
                    anim.SetBool(hashWalk, false);
                    anim.SetBool(hashAttack, false);
                    anim.SetBool(hashDamage, false);
                }
                break;
            case MonsterState.WALK:
                {
                    anim.SetBool(hashWalk, isOn);
                }
                break;
            case MonsterState.ATTACK:
                {
                    anim.SetBool(hashAttack, isOn);
                }
                break;
            case MonsterState.DAMAGE:
                {
                    anim.SetBool(hashDamage, isOn);
                }
                break;
        }
    }

    #endregion

    private float bombPower =20.0f;

    public void Bomb()
    {
        agent.enabled = false;
        ChangeState(MonsterState.ATTACK, true);
    }

}
