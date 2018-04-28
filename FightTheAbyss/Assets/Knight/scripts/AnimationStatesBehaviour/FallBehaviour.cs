using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBehaviour : StateMachineBehaviour {

    private float distanceToGround;
    private States states;
    private float initialHeight;
    private float finalHeight;
    private RaycastHit hit;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponent<States>();
        Physics.Raycast(states.LegFrontPosition(), -Vector3.up, out hit, Statics.DistanceToGroundMax);
        distanceToGround = hit.distance;
        animator.SetFloat(Statics.animDistanceToGround, distanceToGround);
        initialHeight = animator.transform.position.y;
        finalHeight = initialHeight;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!states.grounded)
        {
            if (Physics.Raycast(states.LegFrontPosition(), -Vector3.up, out hit, Statics.DistanceToGroundMax))
                distanceToGround = hit.distance;
            else
                distanceToGround = Statics.DistanceToGroundMax;
            Debug.DrawRay(states.LegFrontPosition(), -Vector3.up * Statics.DistanceToGroundMax, Color.cyan);
            animator.SetFloat(Statics.animDistanceToGround, distanceToGround);
        }
        else
        {
            animator.SetFloat(Statics.animDistanceToGround, 0);
            distanceToGround = 0;
        }
        applyExtraGravityForce();


        /*if (distanceToGround <= 0)
        {
            finalHeight = animator.transform.position.y;
            float diff = initialHeight - finalHeight;

            if (diff < MTPC_Statics.ToSoftFall)
                animator.SetInteger(MTPC_Statics.animSpecialType,
                    MTPC_Statics.GetAnimSpecialType(MTPC_Statics.AnimSpecials.soft_fall));
            else if (diff < MTPC_Statics.toRollFall)
                animator.SetInteger(MTPC_Statics.animSpecialType,
                    MTPC_Statics.GetAnimSpecialType(MTPC_Statics.AnimSpecials.roll_fall));
            else
                animator.SetInteger(MTPC_Statics.animSpecialType,
                    MTPC_Statics.GetAnimSpecialType(MTPC_Statics.AnimSpecials.hard_fall));
        }*/
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //states.hasJumped = false;
       // states.waitToJumpAgainCoroutine();
    }


    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void applyExtraGravityForce()
    {
        Vector3 extraForce = (Physics.gravity * Statics.ExtraGravityMultiplier) - Physics.gravity;
        states.rigid.AddForce(extraForce);
    }
}
