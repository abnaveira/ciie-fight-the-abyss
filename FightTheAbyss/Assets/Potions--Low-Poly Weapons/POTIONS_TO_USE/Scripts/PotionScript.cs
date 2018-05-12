using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FightTheAbyss;

public class PotionScript : MonoBehaviour {

    // You can change this value to change potion type on unity
    public int potionType = HEALTH_POTION;

    const int HEALTH_POTION = 0;
    const int UNLIMITED_STAMINA = 1;
    const int UNLIMITED_CROSSBOW = 2;

    const int HEALTH_POTION_VALUE = 40;
    const int UNLIMITED_STAMINA_TIME = 10; // in seconds
    const int UNLIMITED_CROSSBOW_TIME = 10; // in seconds

    Crossbow crossbow;
    States states;

    private void deActivateStaminaBuff()
    {
        states.staminaBuff = false;
    }

    private void deActivateCrossbowBuff()
    {
        crossbow.noRechargingMode = false;
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
                        // Add the value of the health potion
                        states.health = states.health + HEALTH_POTION_VALUE;
                        // If the health increase is bigger than maxValue, get it down to maxValue
                        if (states.health > states.healthMaxValue)
                        {
                            states.health = states.healthMaxValue;
                        }
                        // Destroy the potion
                        Destroy(gameObject);
                    }
                    break;
                case UNLIMITED_STAMINA:
                    // Get player variables an functions
                    states = player.GetComponent<States>();
                    // Activate buff
                    states.staminaBuff = true;
                    // Deactivate buff after time has passed
                    Invoke("deActivateStaminaBuff", UNLIMITED_STAMINA_TIME);
                    // Destroy the potion
                    Destroy(gameObject);

                    break;
                case UNLIMITED_CROSSBOW:
                    // Get player crossbow
                    crossbow = player.GetComponent<Crossbow>();
                    // Activate buff
                    crossbow.noRechargingMode = true;
                    // Deactivate buff after time has passed
                    Invoke("deActivateCrossbowBuff", UNLIMITED_CROSSBOW_TIME);

                    // YOOOOOOOOOOOOOOOOOO, check if destroy the object destroys the invoke
                    // YOOOOOOOOOOOOOOOOOO, pregunta shield a Adri
                    // Check if destroying

                    // Destroy the potion
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    /*
     Para spawnear las potis 
            Vector3 origin = transform.position + (Vector3.up * Statics.GroundCheckStartPointOffset);

            RaycastHit hit = new RaycastHit();
            bool isHit = findGround(origin, ref hit);
     */
}
