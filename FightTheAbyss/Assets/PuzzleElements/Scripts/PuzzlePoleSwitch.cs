using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class PuzzlePoleSwitch : MonoBehaviour
    {

        public AudioSource activateLever;               // Sound to activate the lever
        public AudioSource shutDownLever;              // Sound to shut down the lever

        public Transform LeverPole, ActiveLever, TurnedOffLever;    // Pole of the lever, and transforms with rotations
        public float leverSpeed = 5F;               // Lever moving speed speed

        [HideInInspector]
        public bool _open = false;                  // Is the lever opened

        // Use this for initialization
        void Start()
        {
            AudioSource[] sounds = GetComponents<AudioSource>();
            shutDownLever = sounds[0];
            activateLever = sounds[1];
        }

        // Update is called once per frame
        void Update()
        {
            if (_open)
            {
                LeverSwitched(ActiveLever.rotation);
            }
            else
            {
                LeverSwitched(TurnedOffLever.rotation);

            }
        }

        // Rotate the LeverPole to the requested rotation
        void LeverSwitched(Quaternion toRot)
        {
            // Move lever
            if (LeverPole.rotation != toRot)
            {
                LeverPole.rotation = Quaternion.Lerp(LeverPole.rotation, toRot, Time.deltaTime * leverSpeed);
            }
        }

        private void OnTriggerStay(Collider col)
        {
            if ((col.CompareTag("Player")) && (Input.GetKeyDown(KeyCode.E)))
            {
                if (_open)
                {
                    _open = false;
                    shutDownLever.Play();
                }
                else
                {
                    _open = true;
                    activateLever.Play();
                }
            }
        }


    }
}
