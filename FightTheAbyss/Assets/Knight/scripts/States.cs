﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FightTheAbyss { 
    public class States : MonoBehaviour {
        [Header("Inputs")]
        public float horizontal;
        public float vertical;
        public bool jumpInput;
        public bool walkRunInput;
        public bool sprintInput;
        public bool aimInput;
        public bool shootInput;

        [Header("Stats")]
        public float healthMaxValue=100;
        public float health;
        public float staminaMaxValue;
        public float stamina;
        public float walkSpeed;
        public float runSpeed;
        public float onAirSpeed;
        public float sprintSpeed;
        public float jumpForce;
        public LayerMask collisionsLayer;

        [Header("States")]
        public bool grounded;
        public bool onLocomotion;
        public bool objectHitWithAxe;
        public bool lowObstacleFordward;
        public bool axeDefense;
        public bool busy;
        public enum CharStates {Idle,Moving,OnAir }
        public CharStates currentState;
        public enum LocomotionStates {Walking,Running}
        public LocomotionStates locomotionState = LocomotionStates.Running;
        public enum WeaponSelected {Crossbow,Axe}
        public WeaponSelected weaponSelected = WeaponSelected.Crossbow;
        public bool lockSprint;
        public bool staminaBuff = false;    // Stamina Buff makes the character not lose stamina


        [System.Serializable]
        public struct SoundStruct
        {
            public string key;
            public AudioClip value;
        }
        public SoundStruct [] sounds;
        #region references
        [Header("References")]
        public RectTransform healthUI;
        public RectTransform staminaUI;
        public Texture bloodStain1,bloodStain2,bloodStain3;
        [HideInInspector] public Animator anim;
        [HideInInspector] public Rigidbody rigid;
        [HideInInspector] public CapsuleCollider collider;
        private KnightCamera camera;
        [HideInInspector] public AudioSource[] audioSources;
        [HideInInspector] public AudioSource audioSource1,audioSource2;

        #endregion

        #region auxiliarVariables
        [HideInInspector] public bool skipGroundCheck;
        private float healthForStain1, healthForStain2, healthForStain3;
        [HideInInspector] public Vector3 lowObstacleClimbPos;

        #endregion


        public void Init()
        {
            health = healthMaxValue;
            anim = this.GetComponent<Animator>();
            collider = this.GetComponent<CapsuleCollider>();
            rigid = this.GetComponent<Rigidbody>();
            camera = KnightCamera.singleton;
            audioSources = this.GetComponents<AudioSource>();
            if (audioSources.Length == 2) { 
                audioSource1 = audioSources[0];
                audioSource2 = audioSources[1];
            }
            else
            {
                Debug.LogError("There must be exactly 2 Audio Sources");
            }
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            anim.applyRootMotion = false;
            componentExists(anim, "animator");
            componentExists(rigid, "rigidbody");
            componentExists(collider, "capsuleCollider");
            healthForStain1 = healthMaxValue / 2;
            healthForStain2 = healthMaxValue / 3;
            healthForStain3 = healthMaxValue / 4;
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
            //Debug.Log(anim.GetFloat(Statics.animDistanceToGround));
        }
        public void FixedTick()
        {
            handleObstacleFordward();

            UpdateState();
            UpdateHealth();
            UpdateStamina();
        }

        public void takeDamage(float damage)
        {
            if (axeDefense)
            {
                float val = damage-stamina;
                if (val > 0)
                {
                    health -= val;
                    stamina = 0;
                }
                else
                {
                    stamina -= damage;
                }
            }
            else
            {
                health -= damage;
            }
        }

        private void UpdateHealth()
        {
        
            Vector3 scale = healthUI.transform.localScale;
            if (health <= 0)
            {
                UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                settings.intensity = 0;
                settings.mask = null;
                camera.postProccesingBehaviour.profile.vignette.settings = settings;
                scale.x = 0;
                health = 0;
                SceneManagement.lastScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(SceneManagement.deathScene);

                
            }else if (health >= healthMaxValue)
            {
                scale.x = 1;
                health = healthMaxValue;
                if (camera.postProccesingBehaviour.profile.vignette.settings.intensity != 0)
                {
                    UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                    settings.intensity = 0;
                    settings.mask = null;
                    camera.postProccesingBehaviour.profile.vignette.settings = settings;
                }
            }
            else { 
                scale.x = health / healthMaxValue;
                if (health < healthForStain3)
                {
                    UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                    settings.intensity = 1;
                    settings.mask = bloodStain3;
                    camera.postProccesingBehaviour.profile.vignette.settings = settings;
                }else if (health < healthForStain2)
                {
                    UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                    settings.intensity = 1;
                    settings.mask = bloodStain2;
                    camera.postProccesingBehaviour.profile.vignette.settings = settings;
                }
                else if (health < healthForStain1)
                {
                    UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                    settings.intensity = 1;
                    settings.mask = bloodStain1;
                    camera.postProccesingBehaviour.profile.vignette.settings = settings;
                }
                else
                {
                    if (camera.postProccesingBehaviour.profile.vignette.settings.intensity != 0) { 
                        UnityEngine.PostProcessing.VignetteModel.Settings settings = camera.postProccesingBehaviour.profile.vignette.settings;
                        settings.intensity = 0;
                        settings.mask = null;
                        camera.postProccesingBehaviour.profile.vignette.settings = settings;
                    }
                }

            }
            healthUI.transform.localScale = scale;
        }

        private void UpdateStamina()
        {
            if (anim.GetBool(Statics.animSprint) && !lockSprint)
            {
                // Stamina Buff makes the character not lose stamina
                if (!staminaBuff && onLocomotion)
                {
                    stamina -= Statics.StaminaLossFromSprint;
                }
            }else if (stamina <= staminaMaxValue && !sprintInput)
            {
                stamina += Statics.StaminaGain;
            }
            Vector3 scale = staminaUI.transform.localScale;
            if (stamina <= 0)
            {
                scale.x = 0;
                stamina = 0;
                lockSprint = true;
            }
            else if (stamina >= staminaMaxValue)
            {
                scale.x = 1;
                stamina = staminaMaxValue;
            }
            else
            {
                scale.x = stamina / staminaMaxValue;
            }
            staminaUI.transform.localScale = scale;
        }

        private void handleObstacleFordward()
        {
            Vector3 direction = (camera.transform.forward * vertical + camera.transform.right * horizontal).normalized;
            Vector3 origin = transform.position + (transform.up * Statics.ObstacleForwardVerticalOffset);
            RaycastHit hit=new RaycastHit();
            if( findObstacle(origin, direction, Statics.DistanceToCheckDistanceFordward,ref hit))
            {
                Vector3 origin2 = origin + (transform.up * 0.7f);
                origin2 += direction * 0.5f;

                if (findObstacle(origin2, -transform.up,0.3f,ref hit))
                {
                    lowObstacleFordward = true;
                    skipGroundCheck = true;
                    rigid.isKinematic = true;
                    lowObstacleClimbPos = hit.point;
                }
                
            }
            
            
        }

        private bool findObstacle(Vector3 origin, Vector3 direction, float distanceToCheck,ref RaycastHit hit)
        {
            RaycastHit hit2 = new RaycastHit();
            int numberOfHits = 0;
            for (int i = -1; i < 2; i++)
            {
                Vector3 newOrigin = origin;
                newOrigin += transform.right * (i * 0.3f);
                Debug.DrawRay(newOrigin, direction * distanceToCheck, Color.cyan);
                if (i == 0)
                {
                    if (Physics.Raycast(newOrigin, direction, out hit, distanceToCheck))
                        numberOfHits++;
                }
                else
                {
                    if (Physics.Raycast(newOrigin, direction, out hit2, distanceToCheck))
                        numberOfHits++;
                }
            }
            return numberOfHits >= 2;
        }
        private bool isGrounded()
        {
            if (skipGroundCheck)
                return false;


            Vector3 origin = transform.position + (Vector3.up * Statics.GroundCheckStartPointOffset);

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
                Vector3 targetPosition = this.transform.position;
                targetPosition.y = hit.point.y + Statics.GroundOffset;
                this.transform.position =  targetPosition;

            }
            anim.SetBool(Statics.animGrounded, isHit);
            return isHit;
        }

        private bool findGround(Vector3 origin, ref RaycastHit hit)
        {

            Debug.DrawRay(origin, -Vector3.up * Statics.DebugGroundCheckRayDistance, Color.red);
            return Physics.Raycast(origin, -Vector3.up, out hit, Statics.GroundCheckDistance,collisionsLayer);
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
            if (!grounded && !lowObstacleFordward)
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

        public void animationEventPlaySoundSource1(string key)
        {
            for (int i= 0 ; i < sounds.Length; i++)
            {
                if (sounds[i].key.Equals(key))
                {
                    audioSource1.clip = sounds[i].value;
                    audioSource1.Play();
                }
            }
        }
        public void animationEventPlaySoundSource2(string key)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].key.Equals(key))
                {
                    audioSource2.clip = sounds[i].value;
                    audioSource2.Play();
                }
            }
        }
        //Este animation event se usa para que no empiece a detectar el golpe justo al principio de la animacion 
        //si no que lo haga cuando esté haciendo el swing.
        public void animationEventAxeCheckAttackCollision(int value)
        {
            if(value == 0)
            {
                anim.SetBool(Statics.animAxeCheckAttackCollision, false);
            }
            else
            {
                anim.SetBool(Statics.animAxeCheckAttackCollision, true);
            }
        }

    }
}
