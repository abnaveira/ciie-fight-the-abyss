using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public class IkHand : MonoBehaviour
    {

        private Animator anim;

        public float ikWeight = 1;

        public Transform leftIKTarget;
        public Transform rightIKTarget;

        public Transform lookPosition;

        Vector3 lhPos;
        Vector3 rhPos;

        Quaternion lhRot;
        Quaternion rhRot;

        public float lh_weight;
        public float rh_weight;

        public float lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight;

        public void Start()
        {
            anim = GetComponent<Animator>();

        }
        public void FixedUpdate()
        {
            lh_weight = anim.GetFloat(Statics.animLeftHandIK);
            rh_weight = anim.GetFloat(Statics.animRightHandIK);
        }


        void OnAnimatorIK(int layerIndex)
        {

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, lh_weight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, lh_weight);

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_weight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_weight);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftIKTarget.position);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightIKTarget.position);

            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftIKTarget.rotation);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightIKTarget.rotation);

            anim.SetLookAtWeight(lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
            anim.SetLookAtPosition(lookPosition.position);

        }
    }
}

