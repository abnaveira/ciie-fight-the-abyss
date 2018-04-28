using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : MonoBehaviour {
    [Header("Inputs")]
    public float horizontal;
    public float vertical;
    public bool jumpInput;
    public bool walkRunInput;
    public bool sprintInput;
    public bool aimInput;

    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;
    public float sprintSpeed;
    public float jumpForce;

    [Header("States")]
    public bool grounded;
    public bool onLocomotion;
    public bool busy;

    public enum CharStates {Idle,Moving,OnAir }
    public CharStates currentState;
    public enum LocomotionStates {Walking,Running}
    public LocomotionStates locomotionState = LocomotionStates.Running;


    #region references
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public CapsuleCollider collider;
    #endregion

    #region auxiliarVariables
    [HideInInspector] public bool skipGroundCheck;
    #endregion


    public void Init()
    {
        anim = this.GetComponent<Animator>();
        collider = this.GetComponent<CapsuleCollider>();
        rigid = this.GetComponent<Rigidbody>();
        rigid.angularDrag = 999;
        rigid.drag = 4;
        rigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        anim.applyRootMotion = false;
        componentExists(anim, "animator");
        componentExists(rigid, "rigidbody");
        componentExists(collider, "capsuleCollider");
    }
    private void componentExists(Object obj, string name)
    {
        if (obj == null)
            Debug.Log("No " + name + " found !!");
    }

    public void Tick()
    {
        grounded = isGrounded();
        UpdateLocomotion();

    }

    private bool isGrounded()
    {
        if (skipGroundCheck)
            return false;
        Vector3 origin = transform.position + (transform.up * Statics.GroundCheckStartPointOffset);
        RaycastHit hit = new RaycastHit();
        bool isHit = findGround(origin, ref hit);
        //Si no hay colision probamos desde otros origenes un poco desviados por si acaso
        if (!isHit)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 newOrigin = origin;
                switch (i)
                {
                    case 0:
                        newOrigin += transform.forward / 4;
                        break;
                    case 1:
                        newOrigin += transform.right / 4;
                        break;
                    case 2:
                        newOrigin += -transform.right / 4;
                        break;
                    case 3:
                        newOrigin += -transform.forward / 4;
                        break;
                }
                isHit = findGround(newOrigin, ref hit);
                if (isHit)
                    break;
            }
        }
        if (isHit)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y + Statics.GroundOffset;
            transform.position = targetPosition;
        }
        return isHit;
    }

    private bool findGround(Vector3 origin, ref RaycastHit hit)
    {
        Debug.DrawRay(origin, -transform.up * Statics.DebugGroundCheckRayDistance, Color.red);
        return Physics.Raycast(origin, -transform.up, out hit, Statics.GroundCheckDistance);
    }

    public void FixedTick() {
        UpdateState();
    }


    private void UpdateLocomotion()
    {
        if (grounded)
        {
            if (walkRunInput)
            {
                if (locomotionState == LocomotionStates.Walking)
                {
                    locomotionState = LocomotionStates.Running;
                }else if (locomotionState == LocomotionStates.Running)
                {
                    locomotionState = LocomotionStates.Walking;
                }
            }
        }
    }

    private void UpdateState()
    {
        if (busy)
            return;
        if (!grounded)
        {
            currentState = CharStates.OnAir;
            return;
        }
        if (horizontal != 0 || vertical != 0)
            currentState = CharStates.Moving;
        else
        {
            currentState = CharStates.Idle;
        }
    }

    //Devuelve que pie está delante
    public Vector3 LegFrontPosition()
    {
        Vector3 ll = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
        Vector3 rl = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
        Vector3 rel_ll = transform.InverseTransformPoint(ll);
        Vector3 rel_rr = transform.InverseTransformPoint(rl);

        bool right = rel_ll.z > rel_rr.z;
        if (right)
            return rl;
        else
            return ll;
    }


}
