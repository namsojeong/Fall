using DG.Tweening;
using MonsterLove.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.TextCore.Text;

//public enum MonsterState
//{
//    IDLE,
//    WALK,
//    ATTACK,
//    DAMAGE
//}
public class BombMonster : MonoBehaviour
{
    //private Transform target = null; // 타겟

    //// Component
    //[HideInInspector]
    //public NavMeshAgent agent;
    //[HideInInspector]
    //public Animator anim;
    //[HideInInspector]
    //public Rigidbody rigid;

    //public LayerMask targetLayerMask;

    //CharacterHP monsterHP;

    //private void Awake()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //    anim = GetComponent<Animator>();
    //    rigid = GetComponent<Rigidbody>();
    //    monsterHP = GetComponent<CharacterHP>();

    //    ResetMonster();
    //}

    //#region HP

    //private void Die()
    //{

    //}

    //#endregion

    //#region SET

    //public void ResetMonster()
    //{
    //    agent.speed = Define.MONSTER_SPEED;
    //    agent.stoppingDistance = 5f;
    //}

    //#endregion

    //#region GET
    //public float distance => GetDistance(); // 타겟과의 거리
    //public Vector3 dir => GetDirection(); // 타겟과의 거리
    //public Transform targetPos => target.transform; // 타겟과의 거리

    //// 타겟과의 거리 구하기
    //private float GetDistance()
    //{
    //    SerachTarget();
    //    return Vector3.Distance(target.transform.position, transform.position);
    //}

    //// 타겟과의 방향 구하기
    //private Vector3 GetDirection()
    //{
    //    Vector3 dir = target.position - transform.position;
    //    dir.y = 0;
    //    return dir;
    //}

    //#endregion

    //#region TARGET

    //// 타겟 구하기
    //public Transform SerachTarget()
    //{
    //    Collider[] cols = Physics.OverlapSphere(transform.position, Define.MONSTER_SERCH_RANGE, targetLayerMask);
    //    if (cols.Length > 0)
    //    {
    //        target = cols[0].gameObject.transform;
    //        return target;
    //    }
    //    else return null;
    //}
    //// 타겟 쳐다보기
    //public void LookTarget(Transform target)
    //{
    //    Vector3 dir = GetDirection();
    //    Quaternion rot = Quaternion.LookRotation(dir.normalized);
    //    transform.rotation = rot;
    //}
    //#endregion

    //#region ANIMATION

    //// 애니메이션 Hash
    //[HideInInspector]
    //public int hashWalk = Animator.StringToHash("Walk");
    //[HideInInspector]
    //public int hashAttack = Animator.StringToHash("Attack");
    //[HideInInspector]
    //public int hashDamage = Animator.StringToHash("Damage");

    //// 이동 애니메이션
    //public void ChangeState(MonsterState state, bool isOn)
    //{
    //    switch (state)
    //    {
    //        case MonsterState.IDLE:
    //            {
    //                anim.SetBool(hashWalk, false);
    //                anim.SetBool(hashAttack, false);
    //                anim.SetBool(hashDamage, false);
    //            }
    //            break;
    //        case MonsterState.WALK:
    //            {
    //                anim.SetBool(hashWalk, isOn);
    //            }
    //            break;
    //        case MonsterState.ATTACK:
    //            {
    //                anim.SetBool(hashAttack, isOn);
    //            }
    //            break;
    //        case MonsterState.DAMAGE:
    //            {
    //                anim.SetBool(hashDamage, isOn);
    //            }
    //            break;
    //    }
    //}

    //#endregion

    //private float bombPower =20.0f;

    //public void Bomb()
    //{
    //    agent.enabled = false;
    //    ChangeState(MonsterState.ATTACK, true);
    //}

    public enum States
    {
        Idle,
        Walk,
        Attack,
        Hit,
        Die
    }

    StateMachine<States> fsm;

    private Transform target = null; // 타겟

    // Component
    Animator anim;
    NavMeshAgent agent;
    Collider collider;

    // Layer
    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask;

    private float moveSpeed = 5.0f;
    private float moveRange = 50.0f;
    private float attackRange = 13.0f;
    private float colRadius = 100f;
    private int attackPower = 20;
    private float bombPower = 50.0f;
    private float height = 50.0f;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();

        target = SearchTarget();
        fsm = StateMachine<States>.Initialize(this, States.Idle);

        ResetMonster();
    }

    private void ResetMonster()
    {
    }

    #region GET

    public float distance => GetDistance();
    public Vector3 dir => GetDirection();

    // 타겟과의 거리 구하기
    private float GetDistance() { return Vector3.Distance(transform.position, target.position); }

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
    public Transform SearchTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, colRadius, targetLayerMask);
        if (cols.Length > 0)
        {
            return cols[0].gameObject.transform;
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

    int hashAttack = Animator.StringToHash("Attack");
    int hashWalk = Animator.StringToHash("Walk");
    int hashHit = Animator.StringToHash("Damage");

    public void AnimationPlay(int hash, bool isOn)
    {
        anim.SetBool(hash, isOn);
    }

    public void AnimationPlay(int hash)
    {
        anim.SetTrigger(hash);
    }

    #endregion

    #region IDLE

    private void CheckDistanceIdle()
    {
        if (distance <= attackRange)
        {
            fsm.ChangeState(States.Attack);
        }
        else if (distance <= moveRange)
        {
            fsm.ChangeState(States.Walk);
        }
    }

    private void Idle_Update()
    {
        CheckDistanceIdle();
    }

    #endregion

    #region WALK

    private void SetMove(bool isMove)
    {
        agent.isStopped = !isMove;
    }

    private void Move()
    {
        if (target == null) return;
        LookTarget(target);
        agent.SetDestination(target.position);
    }

    private void CheckDistanceWalk()
    {
        if (distance <= attackRange)
        {
            fsm.ChangeState(States.Attack);
        }
    }

    private void Walk_Enter()
    {
        AnimationPlay(hashWalk, true);
        SetMove(true);
    }

    private void Walk_Update()
    {
        CheckDistanceWalk();

        Move();
    }

    private void Walk_Exit()
    {
        SetMove(false);
        AnimationPlay(hashWalk, false);
    }

    #endregion

    #region ATTACK


    private void Attack_Enter()
    {
        AnimationPlay(hashAttack, true);
    }

    private void Attack_Update()
    {
    }

    private void Attack_Exit()
    {
        AnimationPlay(hashAttack, false);
    }

    public void Attack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, Define.MONSTER_ATTACK_DAMAGE_RANGE, targetLayerMask);
        if (cols.Length>0)
        {
            cols[0].gameObject.GetComponent<PlayerController>().Bomb();
        }
    }

    #endregion

    #region DIE

    private void Die_Enter()
    {
        SetMove(false);
        MonsterDie();
    }

    private void MonsterDie()
    {
        gameObject.SetActive(false);
    }

    #endregion

}
