using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class OnCollideMoveDoor : MonoBehaviour
    {

        public GameObject door;
        public float doorSpeed = 5f;
        private MoveDoor scriptDoor;

        // Use this for initialization
        void Start()
        {
            scriptDoor = door.GetComponent<MoveDoor>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Accelerate speed of the door
                scriptDoor.changeDoorSpeed(doorSpeed);
                // Moves door up after player enters collision
                scriptDoor.moveDoorUp();
                // After moving the door, the object and script are not needed
                Destroy(this.gameObject);
            }
        }
    }
}