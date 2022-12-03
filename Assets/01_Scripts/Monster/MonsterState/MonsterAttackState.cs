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
        Debug.Log("ATTACK"); 
        Bomb();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckTargetDamage();
    }

    private void CheckTargetDamage()
    {
        Collider[] cols = Physics.OverlapSphere(monster.transform.position, Define.MONSTER_ATTACK_DAMAGE_RANGE, monster.targetLayerMask);
        for(int i=0;i<cols.Length;i++)
        {
            Debug.Log("Bomb");
            //cols[i].GetComponent<Player>()?.Bomb();
            //cols[i].GetComponent<Turret>()?.Damage();
        }
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
