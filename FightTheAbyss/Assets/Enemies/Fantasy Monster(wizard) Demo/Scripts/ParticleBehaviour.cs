using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class ParticleBehaviour : MonoBehaviour
    {
        WizardBehaviour parentScript;

        void Start()
        {
            parentScript = this.transform.parent.GetComponent<WizardBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                parentScript.SetPlayerInRange(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                parentScript.SetPlayerInRange(false);
        }
    }
}