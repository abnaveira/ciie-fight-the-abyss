using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleSwitch : MonoBehaviour {

    public AudioSource activateLever;               // Sound to activate the lever
    public AudioSource shutDownLever;              // Sound to shut down the lever

    public Transform LeverPole, ActiveLever, TurnedOffLever;	// Lid, Lid open rotation, Lid close rotationS
    public float openSpeed = 5F;				// Opening speed

    [HideInInspector]
    public bool _open = false;                  // Is the lever opened

    // Use this for initialization
    void Start () {
        AudioSource[] sounds = GetComponents<AudioSource>();
        shutDownLever = sounds[0];
        activateLever = sounds[1];
    }
	
	// Update is called once per frame
	void Update () {
        if (_open)
        {
            LeverSwitched(ActiveLever.rotation);
        }
        else
        {
            LeverSwitched(TurnedOffLever.rotation);
        }
    }

    // Rotate the LeverPole to the requested rotation
    void LeverSwitched(Quaternion toRot)
    {
        if (LeverPole.rotation != toRot)
        {
            LeverPole.rotation = Quaternion.Lerp(LeverPole.rotation, toRot, Time.deltaTime * openSpeed);
        }
    }


    private void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
                if (_open)
                {
                    _open = false;
                    shutDownLever.Play();
                }
                else
                {
                    _open = true;
                    activateLever.Play();
                }
        }
    }


}
