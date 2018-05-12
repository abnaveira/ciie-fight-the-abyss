using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public class Movement : MonoBehaviour
    {
        private States states;
        private Inputs inputs;

        private float velocityChange = 4;
        private bool doJump = false;
        public void Init(States st, Inputs inp)
        {
            states = st;
            inputs = inp;
        }

        public void FixedTick()
        {
            MovementNormal();

        }
        public void Tick()
        {
            HandleRotation_Normal();
            HandleDrag();
            HandleJump();
        }
        private void HandleDrag()
        {
            if (states.grounded)
            {
                states.rigid.drag = 4;
            }
            else
            {
                states.rigid.drag = 0;
            }
        }

        private void MovementNormal()
        {
            if (states.grounded && states.onLocomotion && !states.jumpInput)
            {

                float targetSpeed;
                if (states.sprintInput && states.vertical > 0 && !states.lockSprint)
                {
                    targetSpeed = states.sprintSpeed;
                }
                else if (states.locomotionState == States.LocomotionStates.Running)
                {
                    targetSpeed = states.runSpeed;
                }
                else
                {
                    targetSpeed = states.walkSpeed;
                }
                HandleVelocity_Normal(targetSpeed);
                HandleAnimations();

            }
            else if (states.currentState == States.CharStates.OnAir)
            {
                Vector3 v = inputs.camManager.pivot.transform.forward * states.vertical;
                Vector3 h = inputs.camManager.pivot.transform.right * states.horizontal;
                v.y = 0;
                h.y = 0;
                Vector3 targetVelocity = (h + v).normalized * states.onAirSpeed;
                targetVelocity.y = states.rigid.velocity.y;
                states.rigid.velocity = targetVelocity;
            }
        }

        private void HandleAnimations()
        {
            float h = states.horizontal;
            float v = states.vertical;
            if (states.locomotionState == States.LocomotionStates.Walking)
            {
                h *= 0.5f;
                v *= 0.5f;
            }
            states.anim.SetFloat(Statics.animHorizontal, h, 0.1f, Time.deltaTime);
            states.anim.SetFloat(Statics.animVertical, v, 0.1f, Time.deltaTime);
            states.anim.SetBool(Statics.animSprint, !states.lockSprint ? states.sprintInput : false);
        }

        private void HandleVelocity_Normal(float speed)
        {
            Vector3 curVelocity = states.rigid.velocity;
            Vector3 targetVelocity;
            float lerpFraction;

            if (states.horizontal != 0 || states.vertical != 0)
            {
                if (states.vertical < 0)
                    speed *= Statics.PercentageOfVelocityForMovingBackwards;
                Vector3 v = inputs.camManager.pivot.transform.forward * states.vertical;
                Vector3 h = inputs.camManager.pivot.transform.right * states.horizontal;
                lerpFraction = Statics.IdleToWalkLerpFraction;
                v.y = 0;
                h.y = 0;
                targetVelocity = (h + v).normalized * speed;
            }
            else
            {
                lerpFraction = Statics.WalkToIdleLerpFraction;
                targetVelocity = Vector3.zero;
                targetVelocity.y = states.rigid.velocity.y;
            }
            if (curVelocity == targetVelocity)
                return;
            Vector3 vel = Vector3.Lerp(curVelocity, targetVelocity, lerpFraction);
            states.rigid.velocity = vel;

        }


        private void HandleRotation_Normal()
        {
            Vector3 rotation = inputs.camManager.HandleRotation();
            rotation.x = Mathf.Clamp(rotation.x, -Statics.DefaultCameraMinAngle, Statics.DefaultCameraMaxAngle);

            Quaternion knightRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation.y, 0), 4 * Time.deltaTime);
            Quaternion pivotRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            if (states.horizontal != 0 && states.vertical != 0)
            {
                Vector3 v = inputs.camManager.pivot.transform.forward * states.vertical;
                Vector3 h = inputs.camManager.pivot.transform.right * states.horizontal;
                if (states.vertical < 0)
                {
                    v *= -1;
                    h *= -1;
                }
                Vector3 dir = (v + h).normalized;
                dir.y = 0;
                dir.Normalize();
                Debug.DrawRay(transform.position, dir);
                Quaternion targetRotation = Quaternion.LookRotation(dir, transform.up);
                knightRotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
            }

            transform.rotation = knightRotation;
            inputs.camManager.pivot.transform.rotation = pivotRotation;
        }

        private void HandleJump()
        {
            if (states.onLocomotion && states.jumpInput && states.grounded)
            {
                states.jumpInput = false;
                Vector3 vel = states.rigid.velocity;
                vel.y = states.jumpForce;
                states.rigid.velocity = vel;
                states.anim.SetInteger(Statics.animSpecialType, Statics.GetAnimSpecialType(Statics.AnimSpecials.jump));
            }
        }

    }
}
