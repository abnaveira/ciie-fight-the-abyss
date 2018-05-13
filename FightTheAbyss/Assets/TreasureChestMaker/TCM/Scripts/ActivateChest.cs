using UnityEngine;
using System.Collections;
using FightTheAbyss;


public class ActivateChest : MonoBehaviour {

	public Transform lid, lidOpen, lidClose;	// Lid, Lid open rotation, Lid close rotation
	public float openSpeed = 5F;				// Opening speed
	public bool canClose;                       // Can the chest be closed

    public bool spawnsPotions = true;

    public AudioSource openChest;               // Sound to open the chest
    public AudioSource closeChest;              // Sound to close the chest

    [HideInInspector]
	public bool _open;							// Is the chest opened
    private bool _potionSpawned = false;         // Bool to control if one potion was spawned

    private void Start()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        openChest = sounds[0];
        closeChest = sounds[1];
    }

    void Update () {
		if(_open){
			ChestClicked(lidOpen.rotation);
		}
		else{
			ChestClicked(lidClose.rotation);
		}
	}
	
	// Rotate the lid to the requested rotation
	void ChestClicked(Quaternion toRot){
		if(lid.rotation != toRot){
			lid.rotation = Quaternion.Lerp(lid.rotation, toRot, Time.deltaTime * openSpeed);
		} else if (canClose)
        {
            // If the movement is finished and the lid can't close, disable the script
            Destroy(this);
        }
	}

    IEnumerator spawnChestPotion()
    {
        // We wait a little to spawn the potion, so the character doesn't drink it right away
        yield return new WaitForSeconds(0.01f);
        // We are spawning a potion, can't spawn more
        _potionSpawned = true;
        // Get the script of potion spawning
        PotionSpawn potionInstance = GetComponent<PotionSpawn>();
        Vector3 potionPosition = transform.position;
        // Put the potion a little higher than the chest
        potionPosition.y += 0.5f;
        // Drop a potion with a probability of 1 in designated position
        potionInstance.DropPotion(1.0f, potionPosition, transform.rotation);
    }

    private void OnTriggerStay(Collider col)
    {
        if ((col.CompareTag("Player")) && (Input.GetKeyDown(KeyCode.E)))
        {
            if (canClose)
            {
                if (_open)
                {
                    _open = false;
                    closeChest.Play();
                }
                else
                {
                    // Only spawn potions once
                    if ((spawnsPotions) && (!_potionSpawned))
                    {
                        StartCoroutine(spawnChestPotion());
                    }
                    _open = true;
                    openChest.Play();
                }

            }
            else
            {
                if (!_open) { 
                    // Only spawn potions once
                    if ((spawnsPotions) && (!_potionSpawned))
                    {
                        StartCoroutine(spawnChestPotion());
                    }
                    _open = true;
                    openChest.Play();
                }
            }
        }
    }

}
