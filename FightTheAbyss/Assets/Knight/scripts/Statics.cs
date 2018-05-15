using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public static class Statics
    {
        #region movementBehaviours
        //Move
        public static float GroundCheckStartPointOffset = 0.7f;//usado en groundChecks para que la componente vertical no quede por debajo del suelo.
        public static float GroundCheckDistance = 0.7f;//distancia para el groundCheck
        public static float GroundOffset = 0;//La distancia a la que queremos estar del suelo
        public static float PercentageOfVelocityForMovingBackwards = 0.7f;//Que porcentaje de la velocidad 'normal' se usa para ir de espaldas
        public static float WalkToIdleLerpFraction = 0.3f; //Entre 0 y 1. 0 tarda mas en parar y 1 para en seco
        public static float IdleToWalkLerpFraction = 0.05f;//Lo mismo que WalkToIdleLerpFraction pero para para empezar a andar.

        public static float ObstacleForwardVerticalOffset = 0.05f;//Para hacer el obstacle check
        public static float DistanceToCheckDistanceFordward = 0.5f;
        #endregion
        #region statsParameters
        public static float StaminaLossFromSprint = 0.1f;
        public static float StaminaGain = 0.05f;
        #endregion

        #region inputNames
        public static string Horizontal = "Horizontal";
        public static string Vertical = "Vertical";
        public static string MouseX = "Mouse X";
        public static string MouseY = "Mouse Y";
        public static KeyCode Jump = KeyCode.Space;
        public static string Fire3But = "Fire3";
        public static KeyCode WalkRunBut = KeyCode.LeftControl;
        public static KeyCode SprintBut = KeyCode.LeftShift;
        public static KeyCode MouseRightBut = KeyCode.Mouse1;
        public static KeyCode MouseLeftBut = KeyCode.F;
        public static KeyCode Button1 = KeyCode.Alpha1;
        public static KeyCode Button2 = KeyCode.Alpha2;
        #endregion

        #region animatorVariables
        public static string animHorizontal = "horizontal";
        public static string animVertical = "vertical";
        public static string animOnLocomotion = "onLocomotion";
        public static string animSpecialType = "specialType";
        public static string animSpecialTypeLayer2 = "specialTypeLayer2";
        public static string animDistanceToGround = "distanceToGround";
        public static string animSprint = "sprint";
        public static string animLeftHandIK = "leftHandIK";
        public static string animRightHandIK = "rightHandIK";
        public static string animAxeWeaponDefense = "axeWeaponDefense";
        public static string animAnimationSpeed = "animationSpeed";
        public static string animAxeCombo = "axeCombo";
        public static string animGrounded = "grounded";
        public static string animAxeCheckAttackCollision = "axeCheckAttackCollision";
        #endregion

        #region crossbowAnimatorVariables
        public static string crossAnimSpecialType = "specialType";
        #endregion

        #region CameraValues 
        public static string DefaultCameraId = "default";
        public static string NormalCameraID = "normal";
        public static float DefaultCameraMinAngle = 80;
        public static float DefaultCameraMaxAngle = 80;
        #endregion


        #region AnimationStateBehaviours
        public static float DistanceToGroundMax = 100f;
        public static float ExtraGravityMultiplier = 2;
        public static string AxeCollisionDetectStart = "CollisionDetectStart";
        public static string AxeCollisionDetectEnd = "CollisionDetectEnd";
        #endregion

        #region CameraReferencePositions
        public static string CameraReferencePositionContainer = "ReferenceCameraPositions";
        public static string CameraOriginalReferencePosition = "OriginalReference";
        #endregion

        #region Weapons
        public static float ArrowInitialForce = 50f;
        public static float ArrowFallFactor = 3.2f;
        #endregion

        #region Debugg
        public static float DebugGroundCheckRayDistance = 0.7f;//cuan largo es el ray que se muestra en el debug para groundChecks
        #endregion


        /***************************************************/
        public static int GetAnimSpecialType(AnimSpecials i)
        {
            int r = 0;
            switch (i)
            {
                case AnimSpecials.jump: r = 1; break;
                default:
                    break;
            }
            return r;
        }

        public enum AnimSpecials
        {
            jump
        }
        /********************************************************/
        public static int GetAnimSpecialTypeLayer2(AnimSpecialsLayer2 i)
        {
            int r = 0;
            switch (i)
            {
                case AnimSpecialsLayer2.recharge: r = 1; break;
                case AnimSpecialsLayer2.toAxeIdle: r = 2; break;
                case AnimSpecialsLayer2.toAim: r = 3; break;
                case AnimSpecialsLayer2.axeAttack1: r = 4; break;
                case AnimSpecialsLayer2.axeAttack2: r = 5; break;
                case AnimSpecialsLayer2.axeAttack3: r = 6; break;
                default:
                    break;
            }
            return r;
        }
        public enum AnimSpecialsLayer2
        {
            recharge, toAxeIdle, toAim, axeAttack1, axeAttack2, axeAttack3
        }

        /******************************************************/

        public enum CrossbowAnimSpecials
        {
            disparar, tensar
        }

        public static int GetCrossbowAnimSpecialType(CrossbowAnimSpecials i)
        {
            int r = 0;
            switch (i)
            {
                case CrossbowAnimSpecials.disparar: r = 1; break;
                case CrossbowAnimSpecials.tensar: r = 2; break;
                default: break;
            }
            return r;
        }
        /************************************************************************************/
        //Comprueba si dos posiciones son aproximadamente iguales, porque con ik nunca llega a la posicion exacta
        public static bool checkPositionApprox(Vector3 pos1, Vector3 pos2, float minDistance)
        {
            float dist = Vector3.Distance(pos1, pos2);
            return dist < minDistance;

        }

    }
}
