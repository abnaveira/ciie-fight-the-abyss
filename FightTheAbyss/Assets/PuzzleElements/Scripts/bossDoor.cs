﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class bossDoor : MonoBehaviour
    {
        public GameObject finalBoss;
        public Transform finalBossPosition;
        public GameObject sceneChangeEmpty;
        public GameObject player;

        public GameObject finalDoor;

        public GameObject upperTorch;
        public GameObject leftTorch;
        public GameObject rightTorch;

        private Torchelight scriptUpperTorch;
        private Torchelight scriptLeftTorch;
        private Torchelight scriptRightTorch;

        private MoveDoor scriptBossDoor;
        private ReptileBehaviour scriptFinalBoss;

        private bool puzzleSolved = false;
        private bool colliseumTowersFinished = false;
        private bool floatingTowersFinished = false;

        // Use this for initialization
        void Start()
        {
            scriptUpperTorch = upperTorch.GetComponent<Torchelight>();
            scriptLeftTorch = leftTorch.GetComponent<Torchelight>();
            scriptRightTorch = rightTorch.GetComponent<Torchelight>();

            scriptBossDoor = finalDoor.GetComponent<MoveDoor>();
            scriptFinalBoss = finalBoss.GetComponent<ReptileBehaviour>();
        }

        public void puzzleIsSolved()
        {
            puzzleSolved = true;
            scriptUpperTorch.IntensityLight = 1;
        }

        public void colliseumTowersIsFinished()
        {
            colliseumTowersFinished = true;
            scriptLeftTorch.IntensityLight = 1;
        }

        public void floatingTowersIsFinished()
        {
            floatingTowersFinished = true;
            scriptRightTorch.IntensityLight = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (puzzleSolved && !colliseumTowersFinished && !floatingTowersFinished)
            {
                scriptBossDoor.moveDoorDown();

                scriptFinalBoss.player = player.transform;
                scriptFinalBoss.changeSceneEmpty = sceneChangeEmpty;
                Instantiate(finalBoss, finalBossPosition.position, finalBossPosition.rotation);

                // Destroy the script, it is not needed anymore
                Destroy(this);
            }
        }
    }
}
