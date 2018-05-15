using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss { 
    public class Weapons : MonoBehaviour {
        private Weapon equipedWeapon;
        public Crossbow crossbow;
        public Axe axe;
        [SerializeField]  private List<Weapon> weaponsList = new List<Weapon>();
        #region References
        States states;
        KnightCamera camera;
        IkHand ik;
        #endregion
        // Use this for initialization
        public void Init(States states, IkHand ik) {
            switch (states.weaponSelected) { 
               case States.WeaponSelected.Crossbow: equipedWeapon = crossbow;
                    break;
               case States.WeaponSelected.Axe: equipedWeapon = axe;
                    break;
               default: equipedWeapon = crossbow;
                    break;
             }
            this.states = states;
            this.camera = KnightCamera.singleton;
            this.ik = ik;
            equipedWeapon.weaponReference.SetParent(equipedWeapon.equipedReferenceHolder);
            equipedWeapon.weaponReference.localPosition = Vector3.zero;
            equipedWeapon.weaponReference.localRotation = Quaternion.identity;
            crossbow.init(ik,states);
            axe.init(ik, states);
            equipedWeapon.start();
	    }

        public void FixedTick()
        {
            checkWeaponChange();
            equipedWeapon.FixedTick();
        }


        public void Tick()
        {
            equipedWeapon.secondaryAction(states.aimInput);
            equipedWeapon.Tick();
            equipedWeapon.primaryAction(states.shootInput);
        }




    
        private void checkWeaponChange()
        {
            Weapon aux=null;
            if(states.weaponSelected==States.WeaponSelected.Crossbow && !equipedWeapon.Equals(crossbow))
            {
                aux = crossbow;
            }
            else if (states.weaponSelected == States.WeaponSelected.Axe && !equipedWeapon.Equals(axe))
            {
                aux = axe;
            }
            if (aux != null)
            {
                
                states.axeDefense = false;

                states.animationEventPlaySoundSource2("WeaponChange");
                equipedWeapon.weaponReference.SetParent(equipedWeapon.notEquipedWeaponHolder);
                equipedWeapon.weaponReference.localRotation = Quaternion.identity;
                equipedWeapon.weaponReference.localPosition = Vector3.zero;
                equipedWeapon = aux;
                equipedWeapon.weaponReference.SetParent(aux.equipedReferenceHolder);
                equipedWeapon.weaponReference.localRotation = Quaternion.identity;
                equipedWeapon.weaponReference.localPosition = Vector3.zero;
                equipedWeapon.start();
            }
        }

        public void animationEventStartRecharging()
        {
            states.animationEventPlaySoundSource2("CrossbowRecharge");

            crossbow.startCrossBowAnim();
        }

        public void animationEventPutArrow()
        {
            crossbow.recharge();
        }
        public void animationEventRechargeEnd()
        {
            crossbow.charging = false;
        }
    }





    [System.Serializable]
    public abstract class Weapon
    {
        public Transform notEquipedWeaponHolder;
        public Transform equipedReferenceHolder;
        public Transform weaponReference;


        abstract public void primaryAction(bool input);
        abstract public void secondaryAction(bool input);
        abstract public void init(IkHand ik,States states);
        abstract public void Tick();
        abstract public void FixedTick();
        abstract public void start();
   
    }

    [System.Serializable]
    public class Axe : Weapon
    {
        public Transform movementReference;
        States states;
        IkHand ik;
        private float timeCombo=0;
        private enum ComboAttacks {attack1,attack2,attack3}
        private ComboAttacks  previousAttack;
        //Usado para que el sonido de bloquear suene solo una vez. No se puede poner en un animationEvent porque está en loop.
        private bool playOnce;
        public override void init(IkHand ik, States states)
        {
            this.ik = ik;
            this.states = states;
        }
        public override void start()
        {
            states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.toAxeIdle));
        }

        public override void Tick()
        {
            weaponReference.position = movementReference.position;
            weaponReference.rotation = movementReference.rotation;
        
        }

        public override void FixedTick()
        {
            if (states.anim.GetBool(Statics.animAxeCombo))
            {
                timeCombo += Time.fixedDeltaTime;
                if (timeCombo > 3f)
                {
                    timeCombo = 0;
                    states.anim.SetBool(Statics.animAxeCombo, false);
                }
            }
        }

        public override void primaryAction(bool input)
        {

            if (input)
            {
                if (!states.anim.GetBool(Statics.animAxeCombo)) { 
                    states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.axeAttack1));
                    previousAttack = ComboAttacks.attack1;
                }
                else if(previousAttack==ComboAttacks.attack1 && states.anim.GetInteger(Statics.animSpecialTypeLayer2)==0)
                {
                    states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.axeAttack2));
                    previousAttack = ComboAttacks.attack2;
                    timeCombo = 0;
                }else if (previousAttack == ComboAttacks.attack2 && states.anim.GetInteger(Statics.animSpecialTypeLayer2) == 0)
                {
                    states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.axeAttack3));
                    previousAttack = ComboAttacks.attack3;
                    timeCombo = 0;
                    states.anim.SetBool(Statics.animAxeCombo, false);
                }

            }
        }

        public override void secondaryAction(bool input)
        {
        
            states.anim.SetBool(Statics.animAxeWeaponDefense, input);
            if (input)
            {
                states.lockSprint = true;
                if (!playOnce)
                {
                    states.axeDefense = true;
                    states.animationEventPlaySoundSource2("Shield");
                    playOnce = true;
                }
            }
            else
            {
                if (states.axeDefense)
                {
                    states.axeDefense = false;
                }
                if (states.stamina > 0)
                {
                    if (playOnce)
                    {
                        playOnce = false;
                    }
                    states.lockSprint = false;
                }
            }

        }
    
    }

    [System.Serializable]
    public class Crossbow : Weapon
    {
        public bool noRechargingMode;
        public Transform defaultPosition;
        public Transform aimPosition;
        public Transform chargePosition;
        public Transform leftIKTarget;
        public Transform rightIKTarget;
        public Transform shootDirection;
        public Transform arrowPosition;
        public Transform carcaj;
        public Animator crossbowAnimator;
        private States states;
        private float lefpPercentage=0;
        private Vector3 lerpInit;

        public List<Transform> arrows;
        int arrowNumber = 0;
        public bool charging=false;
        bool onTransition = false;
        Transform endTransition;
        enum PositionStates {Aim,Default,Recharge}
        PositionStates positionState=PositionStates.Default;
        float t;
        IkHand ik;
        KnightCamera camera;
        float chromaticAberrationIntensity = 0;

        public override void primaryAction(bool input)
        {
            if (input && !charging) {
                int i = arrowNumber + 1;
                string name = "Flecha" + i.ToString("0");
                Transform flecha = arrowPosition.Find(name);
                if (flecha == null) { 
                    return;
                    Debug.Log("No se encuentra la flecha");
                }
                flecha.parent = null;
                Flecha flechaScript = flecha.GetComponent<Flecha>();
                Vector3 dir = shootDirection.position - flecha.position;
                flechaScript.shoot(dir);
                states.animationEventPlaySoundSource2("CrossbowFire");
                onTransition = true;
                chromaticAberrationIntensity = 0f;
                endTransition = chargePosition;
                positionState = PositionStates.Recharge;
                charging = true;
                if (noRechargingMode)
                {
                    recharge();
                    charging = false;
                }
                else {
                    crossbowAnimator.SetInteger(Statics.crossAnimSpecialType, Statics.GetCrossbowAnimSpecialType(Statics.CrossbowAnimSpecials.disparar));
                    states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.recharge));
                }
            }
        }
    

        public override void init(IkHand ik,States states)
        {
            this.camera = KnightCamera.singleton;
            this.ik = ik;
            this.states = states;
        }
        public override void start()
        {
            ik.leftIKTarget = leftIKTarget;
            ik.rightIKTarget = rightIKTarget;
            states.anim.SetInteger(Statics.animSpecialTypeLayer2, Statics.GetAnimSpecialTypeLayer2(Statics.AnimSpecialsLayer2.toAim));

        }


        public void startCrossBowAnim()
        {
            crossbowAnimator.SetInteger(Statics.crossAnimSpecialType, Statics.GetCrossbowAnimSpecialType(Statics.CrossbowAnimSpecials.tensar));
        }


        public override void Tick()
        {
            if (onTransition)
            {
                weaponReference.position = Vector3.MoveTowards(weaponReference.position, endTransition.position,Time.deltaTime*2);
                UnityEngine.PostProcessing.ChromaticAberrationModel.Settings settings = camera.postProccesingBehaviour.profile.chromaticAberration.settings;
                settings.intensity = Mathf.Lerp(settings.intensity, chromaticAberrationIntensity, Time.deltaTime * 20);
                camera.postProccesingBehaviour.profile.chromaticAberration.settings = settings;
                if (weaponReference.position == endTransition.position)
                {
                    onTransition = false;
                }
            }else if (charging)
            {
                weaponReference.position = chargePosition.position;
            }
            HandleRotation();
        }

        public override void FixedTick()
        {
 
        }

        private void HandleRotation()
        {
            Vector3 dir = camera.pivot.forward;
            Quaternion rot = Quaternion.LookRotation(dir);
            weaponReference.rotation = rot;
        }

        public void recharge()
        {
            Transform arrow;
            int n;
            if (arrowNumber < arrows.Count - 1)
            {
                n = arrowNumber + 1;
            }
            else
            {
                n = 0;
            }
            arrowNumber = n;
            arrow = arrows[n];
            Rigidbody rigid = arrow.GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            rigid.isKinematic = true;
            //arrow.GetComponent<SphereCollider>().isTrigger = true;
            arrow.GetComponent<Flecha>().onAir = false ;
            arrow.GetComponent<TrailRenderer>().enabled = false;
            arrow.SetParent(arrowPosition);
            arrow.localPosition = Vector3.zero;
            arrow.localRotation = Quaternion.identity;

            int n2 = n + 1;
            if (n == arrows.Count - 1){
                n2 = 0;
            }
            Transform arrow2 = arrows[n2];
            Rigidbody rigid2 = arrow2.GetComponent<Rigidbody>();
            rigid2.velocity = Vector3.zero;
            rigid2.isKinematic = true;
            //arrow2.GetComponent<SphereCollider>().isTrigger = true;
            arrow2.GetComponent<Flecha>().onAir = false;
            arrow2.GetComponent<TrailRenderer>().enabled = false;
            arrow2.SetParent(carcaj);
            arrow2.localPosition =Vector3.zero;
            arrow2.rotation = Quaternion.LookRotation(Vector3.up);
        }

        public override void secondaryAction(bool input)
        {
            if (input && !charging && !onTransition)
            {
                if (positionState == PositionStates.Aim)
                {
                    weaponReference.position = aimPosition.position;
                    states.lockSprint = true;
                
               
                }
                else
                {
                    chromaticAberrationIntensity = 1f;
                    onTransition = true;
                    endTransition = aimPosition;
                    positionState =PositionStates.Aim;
                }
            }
            else if(!input && !charging && !onTransition)
            {
                if (positionState==PositionStates.Default)
                {
                    weaponReference.position = defaultPosition.position;
                    if (states.stamina > 0)
                    {
                        states.lockSprint = false;
                    }
                }
                else
                {
                    chromaticAberrationIntensity = 0;
                    onTransition = true;
                    endTransition = defaultPosition;
                    positionState = PositionStates.Default;
                }
            }
        }
    }
}
