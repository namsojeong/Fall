using DG.Tweening;
using MonsterLove.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombMonster : MonoBehaviour
{

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
    Rigidbody rigid;
    Collider collider;

    // Layer
    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask;

    private float moveSpeed = 5.0f;
    private float moveRange = 50.0f;
    private float attackRange = 5.0f;
    private float colRadius = 100f;
    private int attackPower = 20;
    private float bombPower = 20.0f;
    private float bombDistance = 10.0f;

    private FlashHit hitFlash;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        hitFlash = GetComponent<FlashHit>();

        target = SearchTarget();
        fsm = StateMachine<States>.Initialize(this, States.Idle);
    }

    private void OnEnable()
    {
        ResetMonster();
    }

    private void ResetMonster()
    {
        agent.enabled = true;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        fsm.ChangeState(States.Idle);
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
    private void Idle_Enter()
    {

    }
        
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

    #region COLLISION

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            fsm.ChangeState(States.Hit);   
        }
    }

    private void Bomb()
    {
        Vector3 direction = -dir.normalized;
        Vector3 destination = transform.position + transform.up * bombDistance + direction * bombDistance;
        agent.enabled = false;
        rigid.DOKill();
        rigid.DOMove(destination, 0.3f);
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
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, targetLayerMask);
        if (cols.Length>0)
        {
            cols[0].gameObject.GetComponent<PlayerController>().Bomb(attackPower, bombPower);
        }
    }

    #endregion

    #region HIT

    private void Hit_Enter()
    {
        AnimationPlay(hashHit, true);
        hitFlash.DamageEffect();
        Bomb();
    }

    #endregion;

    #region DIE

    private void Die_Enter()
    {
        SetMove(false);
        MonsterDie();
    }

    private void MonsterDie()
    {
        ObjectPool.Instance.ReturnObject(PoolObjectType.BOMB_MONSTER, gameObject);
    }

    #endregion

}
