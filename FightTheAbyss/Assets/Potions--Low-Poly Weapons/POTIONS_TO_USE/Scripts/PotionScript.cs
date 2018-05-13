using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FightTheAbyss;

namespace FightTheAbyss
{
    public class PotionScript : MonoBehaviour
    {

        // You can change this value to change potion type on unity
        public int potionType = HEALTH_POTION;

        // Drinking sound (must be audioSource of the gameObject)
        public AudioSource drinkPotionSound;

        const int HEALTH_POTION = 0;
        const int UNLIMITED_STAMINA = 1;
        const int UNLIMITED_CROSSBOW = 2;

        const int HEALTH_POTION_VALUE = 40;
        const int UNLIMITED_STAMINA_TIME = 10; // in seconds
        const int UNLIMITED_CROSSBOW_TIME = 10; // in seconds

        Crossbow crossbow;
        States states;

        private void Start()
        {
            drinkPotionSound = GetComponents<AudioSource>()[0];
        }

        private void deActivateStaminaBuff()
        {
            // Deactivate the buff
            states.staminaBuff = false;
            // Destroy the potion
            Destroy(gameObject);
        }

        // Used to destroy this object after a said ammount of time
        // Use this to destroy potions of inmediate use after drinking sound is finished
        private IEnumerator DestroyObjectAfterTime(float timeToDestroy)
        {
            yield return new WaitForSeconds(timeToDestroy);
            // Destroy the potion
            Destroy(gameObject);

        }

        private void deActivateCrossbowBuff()
        {
            // Deactivate the buff
            crossbow.noRechargingMode = false;
            // Destroy the potion
            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider col)
        {
            if ((col.CompareTag("Player")) && (Input.GetKeyDown(KeyCode.E)))
            {
                // Get player gameObject
                GameObject player = col.gameObject;

                // Do action based on potion
                switch (potionType)
                {
                    case HEALTH_POTION:
                        // Get player variables an functions
                        states = player.GetComponent<States>();
                        // Health portions only work if the character has lower than maximum life
                        if (states.health < states.healthMaxValue)
                        {
                            // Play potion drinking sound
                            drinkPotionSound.Play();
                            // Add the value of the health potion
                            states.health = states.health + HEALTH_POTION_VALUE;
                            // If the health increase is bigger than maxValue, get it down to maxValue
                            if (states.health > states.healthMaxValue)
                            {
                                states.health = states.healthMaxValue;
                            }
                            // Make the potion invisible and untangible until its effects pass
                            gameObject.GetComponent<Renderer>().enabled = false;
                            // Wait until the sound for potion drink is over to destroy the potion
                            StartCoroutine(DestroyObjectAfterTime(drinkPotionSound.clip.length));
                        }
                        break;

                    case UNLIMITED_STAMINA:
                        // Get player variables an functions
                        states = player.GetComponent<States>();
                        // If buff was active, you can't drink the potion
                        if (states.staminaBuff == false)
                        {
                            // Play potion drinking sound
                            drinkPotionSound.Play();
                            // Activate buff
                            states.staminaBuff = true;
                            // Deactivate buff after time has passed
                            Invoke("deActivateStaminaBuff", UNLIMITED_STAMINA_TIME);
                            // Make the potion invisible and untangible until its effects wear off to destroy it
                            gameObject.GetComponent<Renderer>().enabled = false;
                        }
                        break;

                    case UNLIMITED_CROSSBOW:

                        // Get player crossbow
                        crossbow = player.GetComponent<Weapons>().crossbow;
                        // If buff was active, you can't drink the potion
                        if (crossbow.noRechargingMode == false)
                        {
                            // Play potion drinking sound
                            drinkPotionSound.Play();
                            // Activate buff
                            crossbow.noRechargingMode = true;
                            // Deactivate buff after time has passed
                            Invoke("deActivateCrossbowBuff", UNLIMITED_CROSSBOW_TIME);
                            // Make the potion invisible and untangible until its effects wear off to destroy it
                            gameObject.GetComponent<Renderer>().enabled = false;
                        }
                        break;

                    default:
                        Debug.LogError("Potion type is unidentified: " + potionType.ToString());
                        break;
                }
            }
        }

    }
}
