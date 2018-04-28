using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statics  {
    #region movementBehaviours
    //Move
    public static float GroundCheckStartPointOffset = 0.7f;//usado en groundChecks para que la componente vertical no quede por debajo del suelo.
    public static float GroundCheckDistance = 0.7f;//distancia para el groundCheck
    public static float GroundOffset = 0;//La distancia a la que queremos estar del suelo
    public static float PercentageOfVelocityForMovingBackwards = 0.7f;//Que porcentaje de la velocidad 'normal' se usa para ir de espaldas
    public static float WalkToIdleLerpFraction = 0.3f; //Entre 0 y 1. 0 tarda mas en parar y 1 para en seco
    public static float IdleToWalkLerpFraction = 0.05f;//Lo mismo que WalkToIdleLerpFraction pero para para empezar a andar.
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
    #endregion

    #region animatorVariables
    public static string animHorizontal = "horizontal";
    public static string animVertical = "vertical";
    public static string animOnLocomotion = "onLocomotion";
    public static string animSpecialType = "specialType";
    public static string animDistanceToGround = "distanceToGround";
    public static string animSprint = "sprint";

    #endregion

    #region CameraValues 
    public static string DefaultCameraId = "default";
    public static string NormalCameraID = "normal";
    public static float DefaultCameraMinAngle = 80;
    public static float DefaultCameraMaxAngle = 80;
    #endregion


    #region AnimationStateBehaviours
    public static float DistanceToGroundMax = 100f;
    public static float ExtraGravityMultiplier = 3;
    #endregion

    #region CameraReferencePositions
    public static string CameraReferencePositionContainer = "ReferenceCameraPositions";
    public static string CameraOriginalReferencePosition = "OriginalReference";
    #endregion

    #region Debugg
    public static float DebugGroundCheckRayDistance = 0.5f;//cuan largo es el ray que se muestra en el debug para groundChecks
    #endregion



    public static int GetAnimSpecialType(AnimSpecials i)
    {
        int r = 0;
        switch (i)
        {
            case AnimSpecials.jump: r = 1; break;
            case AnimSpecials.soft_fall: r = 2; break;
            case AnimSpecials.roll_fall: r = 3; break;
            case AnimSpecials.hard_fall: r = 4; break;
            case AnimSpecials.jump_idle: r = 5; break;

            default:
                break;
        }
        return r;
    }

    public enum AnimSpecials
    {
        jump, soft_fall, hard_fall, roll_fall, jump_idle
    }


}
