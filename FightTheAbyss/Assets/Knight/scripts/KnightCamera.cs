using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
namespace FightTheAbyss
{
    public class KnightCamera : MonoBehaviour
    {
        public Transform referencePoint;
        #region Variables
        [SerializeField] float turnSpeed = 1.5f;
        public bool lockCursor;
        #endregion

        #region References
        [HideInInspector] public Transform parent;
        [HideInInspector] public Transform pivot;
        [HideInInspector] public Transform camTrans;
        [HideInInspector] public PostProcessingBehaviour postProccesingBehaviour;
        #endregion

        static public KnightCamera singleton;



        #region Internal Variables
        float x;
        float y;
        float lookAngleX;
        float lookAngleY;
        float tiltAngle;
        float smoothX = 0;
        float smoothY = 0;

        Vector3 originalPosition;
        float offset;
        bool resetOriginalPos;
        #endregion

        private void Awake()
        {
            singleton = this;
        }

        void Start()
        {
            if (Camera.main.transform == null)
                Debug.Log("You don't have a 'MainCamera");
            postProccesingBehaviour = Camera.main.transform.GetComponent<PostProcessingBehaviour>();
            camTrans = Camera.main.transform.parent;
            pivot = camTrans.parent;
            resetOriginalPos = true;
            offset = Vector3.Distance(this.transform.position, referencePoint.position);
            parent = this.transform.parent;
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void Tick()
        {
            this.transform.position = referencePoint.position;

        }

        public void setCameraToOriginalPosition()
        {
            resetOriginalPos = true;
        }

        //Llamada desde Movement
        public Vector3 HandleRotation()
        {
            x = Input.GetAxis(Statics.MouseX);
            y = Input.GetAxis(Statics.MouseY);

            float targetTurnSpeed = turnSpeed;

            lookAngleX += x * targetTurnSpeed;
            lookAngleY += y * targetTurnSpeed;

            //reset the look angle when it does a full circle
            if (lookAngleX > 360) lookAngleX = 0;
            if (lookAngleX < -360) lookAngleX = 0;

            if (lookAngleY > 360) lookAngleY = 0;
            if (lookAngleY < -360) lookAngleY = 0;
            return new Vector3(-lookAngleY, lookAngleX, 0);
        }

    }
}


