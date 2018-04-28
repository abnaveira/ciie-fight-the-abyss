using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {

    private States states;
    private Movement movement;
    [HideInInspector] public KnightCamera camManager;
    private Weapons weapons;

    private float horizontal;
    private float vertical;
    // Use this for initialization
    void Start()
    {
        camManager = KnightCamera.singleton;
        states = GetComponent<States>();
        movement = GetComponent<Movement>();
        weapons = GetComponent<Weapons>();

        states.Init();
        movement.Init(states,this);
        weapons.Init(states);

        FixPlayerMeshes();
        //camManager.transform.SetParent(states.anim.GetBoneTransform(HumanBodyBones.Chest));

    }
    void FixPlayerMeshes()
    {
        SkinnedMeshRenderer[] skinned = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skinned.Length; i++)
        {
            skinned[i].updateWhenOffscreen = true;
        }
    }

    void FixedUpdate()
    {
        states.FixedTick();
        FixedUpdateStatesFromInput();
        movement.FixedTick();

        camManager.FixedTick();
        weapons.FixedTick();
    }

    // Update is called once per frame
    void Update()
    {
        states.Tick();
        UpdateStatesFromInput();

    }
    void UpdateStatesFromInput()
    {
        if (states.grounded)
        {
            states.jumpInput = Input.GetKeyDown(Statics.Jump);
            states.walkRunInput = Input.GetKeyDown(Statics.WalkRunBut);
            states.sprintInput = Input.GetKey(Statics.SprintBut);
        }
        states.aimInput = Input.GetKey(Statics.MouseRightBut);
    }
    void FixedUpdateStatesFromInput()
    {
        horizontal = Input.GetAxis(Statics.Horizontal);
        vertical = Input.GetAxis(Statics.Vertical);

        Vector3 v = camManager.transform.forward * vertical;
        Vector3 h = camManager.transform.right * horizontal;

        v.y = 0;
        v.y = 0;

        states.horizontal = horizontal;
        states.vertical = vertical;
        states.onLocomotion = states.anim.GetBool(Statics.animOnLocomotion);
        //states.walking_running = Input.GetButton(MTPC_Statics.Fire3);


        //Vector3 moveDir = (h + v).normalized;


    }

}
