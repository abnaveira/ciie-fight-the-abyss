using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class changeRespawnPoint : MonoBehaviour
    {

        public GameObject fallCollider;
        public Transform newRespawnPoint;
        OnFall scriptOnFall;

        // Use this for initialization
        void Start()
        {
            scriptOnFall = fallCollider.GetComponent<OnFall>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                scriptOnFall.changeRespawnPoint(newRespawnPoint);
            }
        }
    }
}
