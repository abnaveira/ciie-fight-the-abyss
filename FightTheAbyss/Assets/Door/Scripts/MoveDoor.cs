using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class MoveDoor : MonoBehaviour
    {

        public Transform DoorUp, DoorDown;      // Door and transforms with positions
        public float DoorSpeed = 1F;            // Door moving speed

        public bool isDoorUp = true;            // Indicates if the wall is up

        void Update()
        {
            if (isDoorUp)
            {
                moveDoor(DoorUp.position);
            }
            else
            {
                moveDoor(DoorDown.position);
            }
        }

        // Move door to the requested position
        void moveDoor(Vector3 position)
        {
            // Move wall
            if (this.transform.position != position)
            {
                
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, Time.deltaTime * DoorSpeed);
            }
        }

        public void moveDoorDown()
        {
            isDoorUp = false;
        }

        public void moveDoorUp()
        {
            isDoorUp = true;
        }

    }
}
