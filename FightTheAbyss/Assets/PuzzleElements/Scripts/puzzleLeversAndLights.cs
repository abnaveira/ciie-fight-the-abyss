using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class puzzleLeversAndLights : MonoBehaviour
    {
        public GameObject lever1Object;
        public GameObject lever2Object;
        public GameObject lever3Object;
        public GameObject lever4Object;
        public GameObject lever5Object;

        public GameObject torch1;
        public GameObject torch2;
        public GameObject torch3;

        public GameObject ColliseumTowersDoor;
        public GameObject FloatingPlatformsDoor;

        public AudioSource puzzleSolved;

        private bool circuit3And;
        private bool Light3And;
        private bool Light2Or;
        private bool Light1And;

        private Torchelight scriptTorch1;
        private Torchelight scriptTorch2;
        private Torchelight scriptTorch3;

        private PuzzlePoleSwitch scriptLever1;
        private PuzzlePoleSwitch scriptLever2;
        private PuzzlePoleSwitch scriptLever3;
        private PuzzlePoleSwitch scriptLever4;
        private PuzzlePoleSwitch scriptLever5;

        private MoveDoor scriptColliseumDoor;
        private MoveDoor scriptFloatingDoor;

        private bool puzzleIsSolved = false;


        private void Start()
        {
            puzzleSolved = GetComponents<AudioSource>()[0];

            scriptTorch1 = torch1.GetComponent<Torchelight>();
            scriptTorch2 = torch2.GetComponent<Torchelight>();
            scriptTorch3 = torch3.GetComponent<Torchelight>();

            scriptLever1 = lever1Object.GetComponent<PuzzlePoleSwitch>();
            scriptLever2 = lever2Object.GetComponent<PuzzlePoleSwitch>();
            scriptLever3 = lever3Object.GetComponent<PuzzlePoleSwitch>();
            scriptLever4 = lever4Object.GetComponent<PuzzlePoleSwitch>();
            scriptLever5 = lever5Object.GetComponent<PuzzlePoleSwitch>();

            scriptColliseumDoor = ColliseumTowersDoor.GetComponent<MoveDoor>();
            scriptFloatingDoor = FloatingPlatformsDoor.GetComponent<MoveDoor>();
        }

        void ChangeLightBools()
        {
            circuit3And = scriptLever4._open && scriptLever5._open;
            Light3And = (!scriptLever3._open) && circuit3And;
            Light2Or = scriptLever2._open || (!scriptLever3._open);
            Light1And = (!scriptLever1._open) && circuit3And;


            scriptTorch1.IntensityLight = Light1And ? 1 : 0;
            scriptTorch2.IntensityLight = Light2Or ? 1 : 0;
            scriptTorch3.IntensityLight = Light3And ? 1 : 0;
            
            if (Light1And && Light2Or && Light3And)
            {
                puzzleSolved.Play();
                puzzleIsSolved = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (puzzleIsSolved)
            {
                scriptColliseumDoor.moveDoorDown();
                scriptFloatingDoor.moveDoorDown();
                // Destroy the script, it is not needed anymore
                Destroy(this);
            } else
            {
                ChangeLightBools();
            }
        }

    }
}