using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class OnClearAreaMoveDoorUp : MonoBehaviour
    {

        public GameObject Enemy1 = null;
        public GameObject Enemy2 = null;
        public GameObject Enemy3 = null;
        public GameObject Enemy4 = null;
        public GameObject Enemy5 = null;
        public GameObject Enemy6 = null;

        public GameObject door;
        public float doorSpeed = 1f;

        public GameObject mainLogic;
        public bool isFloatingPlatformsNotColliseum = true;

        private MoveDoor scriptDoor;
        private bossDoor mainLogicScript;
        private bool playerEnteredArea = false;

        // Use this for initialization
        void Start()
        {
            scriptDoor = door.GetComponent<MoveDoor>();
            mainLogicScript = mainLogic.GetComponent<bossDoor>();
        }

        public void PlayerHasEnteredTheArea()
        {
            playerEnteredArea = true;
        } 

        // Update is called once per frame
        void Update()
        {
            if (playerEnteredArea)
            {
                // If enemies are dead, are is cleared
                if ((Enemy1 == null) && (Enemy2 == null) && (Enemy3 == null) &&
                    (Enemy4 == null) && (Enemy5 == null) && (Enemy6 == null))
                {
                    // Signal mainLogic script: this area was cleared
                    if (isFloatingPlatformsNotColliseum)
                    {
                        mainLogicScript.floatingTowersIsFinished();
                    }
                    else
                    {
                        mainLogicScript.colliseumTowersIsFinished();
                    }
                    // Change speed of the door
                    scriptDoor.changeDoorSpeed(doorSpeed);
                    // Moves door down after player kills all enemies
                    scriptDoor.moveDoorDown();
                    // After moving the door, the object and script are not needed
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
