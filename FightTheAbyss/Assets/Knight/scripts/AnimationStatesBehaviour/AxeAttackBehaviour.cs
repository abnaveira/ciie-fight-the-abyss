using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public class AxeAttackBehaviour : StateMachineBehaviour
    {
        private Transform axe;
        private States states;
        private Weapons weapons;
        private Transform collisionStart;
        private Transform collisionEnd;
        private RaycastHit hit;
        private bool playOnce;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (states == null)
            {
                states = animator.transform.GetComponent<States>();
            }
            if (weapons == null)
            {
                weapons = animator.transform.GetComponent<Weapons>();
            }
            if (axe == null)
            {
                axe = weapons.axe.weaponReference;
            }
            if (collisionStart == null)
            {
                collisionStart = axe.Find(Statics.AxeCollisionDetectStart);
            }
            if (collisionEnd == null)
            {
                collisionEnd = axe.Find(Statics.AxeCollisionDetectEnd);
            }

            hit = new RaycastHit();

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (Physics.Linecast(collisionStart.position, collisionEnd.position, out hit, states.collisionsLayer, QueryTriggerInteraction.Ignore)
                && animator.GetBool(Statics.animAxeCheckAttackCollision))
            {
                states.objectHitWithAxe = true;
                if (!playOnce) { 
                    states.animationEventPlaySoundSource2("AxeHit");
                    playOnce = true;
                }
                

            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            states.objectHitWithAxe = false;
            playOnce = false;
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    }
}
