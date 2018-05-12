using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public class Inputs : MonoBehaviour
    {

        private States states;
        private Movement movement;
        [HideInInspector] public KnightCamera camManager;
        private Weapons weapons;
        private IkHand ik;

        private float horizontal;
        private float vertical;
        // Use this for initialization
        void Start()
        {
            camManager = KnightCamera.singleton;
            states = GetComponent<States>();
            movement = GetComponent<Movement>();
            weapons = GetComponent<Weapons>();
            ik = GetComponent<IkHand>();

            states.Init();
            movement.Init(states, this);
            weapons.Init(states, ik);

            FixPlayerMeshes();

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
            weapons.FixedTick();
        }

        // Update is called once per frame
        void Update()
        {
            states.Tick();
            UpdateStatesFromInput();
            movement.Tick();
            //Si tenemos la ballesta es necesario llamar al update para la cámara y la ballesta antes del 
            //onIKAnimator porque si no hace movimientos demasiado bruscos
            if (states.weaponSelected == States.WeaponSelected.Crossbow)
            {
                camManager.Tick();
                weapons.Tick();
            }

        }


        void LateUpdate()
        {

            //Si tenemos el hacha hay que actualizar la cámara en el lateupdate porque el hacha se mueve con la animación (sin IK)
            //y no con la cámara.
            if (states.weaponSelected == States.WeaponSelected.Axe)
            {
                camManager.Tick();
                weapons.Tick();
            }
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
            states.shootInput = Input.GetKeyDown(Statics.MouseLeftBut);
            if (Input.GetKeyDown(Statics.Button1))
            {
                states.weaponSelected = States.WeaponSelected.Crossbow;
            }
            else if (Input.GetKeyDown(Statics.Button2))
            {
                states.weaponSelected = States.WeaponSelected.Axe;
            }
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
        }

    }
}
