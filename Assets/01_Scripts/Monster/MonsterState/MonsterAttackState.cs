using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterAttackState : StateMachineBehaviour
{
    BombMonster monster;

    float delayTIme = 3.0f;

    private void Bomb()
    {
        monster.agent.enabled = false;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<BombMonster>();
        Bomb();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider[] cols = Physics.OverlapSphere(monster.transform.position, Define.MONSTER_ATTACK_DAMAGE_RANGE, monster.targetLayerMask);
        if (cols[0] != null)
        {
            Debug.Log("Bomb");
            //cols[0].gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * bombPower * Time.deltaTime);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckTargetDamage();
    }

    float bombPower = 10.0f;
    private void CheckTargetDamage()
    {
        monster.gameObject.SetActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
