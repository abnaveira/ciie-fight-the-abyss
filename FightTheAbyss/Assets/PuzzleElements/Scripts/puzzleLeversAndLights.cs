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

        public Transform Door1, Door2, Door1Down, Door2Down;     // Doors and transforms with positions
        public float doorSpeed = 1F;                // Door moving speed

        public AudioSource puzzleSolved;

        /*
        [HideInInspector]
        private bool lever1 = false;
        private bool lever2 = false;
        private bool lever3 = false;
        private bool lever4 = false;
        private bool lever5 = false;
        */

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
        }

        void ChangeLightBools()
        {
            /*
            circuit3And = lever4 && lever5;
            Light3And = (!lever3) && circuit3And;
            Light2Or = lever2 || (!lever3);
            Light1And = (!lever1) && circuit3And;
            */
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
                moveDoor(Door1, Door1Down.position);
                moveDoor(Door2, Door2Down.position);
                // If doors are on positon, destroy the script
                if ((Door1.position == Door1Down.position) && (Door2.position == Door2Down.position))
                {
                    Destroy(this);
                }
            } else
            {
                ChangeLightBools();
            }
        }

        // Move door to the requested position
        void moveDoor(Transform Door, Vector3 position)
        {
            // Move door
            if (Door.position != position)
            {
                Door.position = Vector3.MoveTowards(Door.position, position, Time.deltaTime * doorSpeed);
            }
            // DESTRUYE SCRIPT AL ACABAR DE MOVER LAS PAREDES
        }

    }
}