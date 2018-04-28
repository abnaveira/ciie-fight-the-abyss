using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoolStateMachine : StateMachineBehaviour {

    public string parameter;
    public bool status;
    public bool resetOnExit;

    public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool(parameter, status);
    }

    public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (resetOnExit)
            animator.SetBool(parameter, !status);
    }
}
