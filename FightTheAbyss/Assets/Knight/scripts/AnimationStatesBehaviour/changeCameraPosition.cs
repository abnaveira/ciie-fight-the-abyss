using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCameraPosition : StateMachineBehaviour {
    //Podria hacerlo pasando el nombre del objeto y luego buscandolo. No se que es más eficiente.
    public string positionName;
    public bool resetPositionOnExit;
    private Transform positionObject;
    private KnightCamera camera;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        camera = KnightCamera.singleton;
        positionObject = camera.getReferenceCameraPosition(positionName);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (positionObject.position == camera.transform.position)
            return;
        camera.transform.position = Vector3.Lerp(camera.transform.position, positionObject.position, Time.deltaTime * 4);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (resetPositionOnExit)
            camera.setCameraToOriginalPosition();
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
