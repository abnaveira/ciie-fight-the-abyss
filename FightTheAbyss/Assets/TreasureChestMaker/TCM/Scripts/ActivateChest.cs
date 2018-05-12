using UnityEngine;
using System.Collections;

public class ActivateChest : MonoBehaviour {

	public Transform lid, lidOpen, lidClose;	// Lid, Lid open rotation, Lid close rotation
	public float openSpeed = 5F;				// Opening speed
	public bool canClose;                       // Can the chest be closed

    public AudioSource openChest;               // Sound to open the chest
    public AudioSource closeChest;              // Sound to close the chest

    [HideInInspector]
	public bool _open;							// Is the chest opened

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
            this.enabled = false;
        }
	}

    private void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.E))
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
                    _open = true;
                    openChest.Play();
                }

            }
            else
            {
                if (!_open) { 
                    _open = true;
                    openChest.Play();
                }
            }
        }
    }

}
