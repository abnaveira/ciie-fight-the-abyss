using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class OnCollideMoveDoor : MonoBehaviour
    {

        public GameObject door;
        public float doorSpeed = 5f;
        public GameObject clearLogic;

        private MoveDoor scriptDoor;
        private OnClearAreaMoveDoorUp scriptClearLogic;


        // Use this for initialization
        void Start()
        {
            scriptDoor = door.GetComponent<MoveDoor>();
            scriptClearLogic = clearLogic.GetComponent<OnClearAreaMoveDoorUp>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Accelerate speed of the door
                scriptDoor.changeDoorSpeed(doorSpeed);
                // Moves door up after player enters collision
                scriptDoor.moveDoorUp();
                // Signal clearLogic, so it starts to check if you killed enemies
                scriptClearLogic.PlayerHasEnteredTheArea();
                // After moving the door, the object and script are not needed
                Destroy(this.gameObject);
            }
        }
    }
}