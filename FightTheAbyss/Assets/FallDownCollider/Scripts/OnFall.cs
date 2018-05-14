using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class OnFall : MonoBehaviour
    {

        public GameObject player;
        public Transform respawnPoint;

        public void changeRespawnPoint(Transform newRespawnPoint)
        {
            respawnPoint = newRespawnPoint;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                col.gameObject.transform.position = respawnPoint.position;
                col.gameObject.GetComponent<States>().health -= 10;
            }
            else if (col.CompareTag("Enemy"))
            {
                Destroy(col.gameObject);
            }
        }
    }
}
