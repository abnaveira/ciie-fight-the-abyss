using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleSwitch : MonoBehaviour {

    public AudioSource activateLever;               // Sound to activate the lever
    public AudioSource shutDownLever;              // Sound to shut down the lever

    public Transform LeverPole, ActiveLever, TurnedOffLever;    // Pole of the lever, and transforms with rotations
    public float leverSpeed = 5F;				// Lever moving speed speed

    public bool hasWall = false;                // Indicates if the lever has an associated wall

    public Transform MovableWall, WallUp, WallDown;     // Wall and transforms with positions
    public float wallSpeed = 1F;                // Wall moving speed

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
            if (hasWall)
            {
                moveWall(WallDown.position);
            }
        }
        else
        {
            LeverSwitched(TurnedOffLever.rotation);
            if (hasWall)
            {
                moveWall(WallUp.position);
            }
            
        }
    }

    // Rotate the LeverPole to the requested rotation
    void LeverSwitched(Quaternion toRot)
    {
        // Move lever
        if (LeverPole.rotation != toRot)
        {
            LeverPole.rotation = Quaternion.Lerp(LeverPole.rotation, toRot, Time.deltaTime * leverSpeed);
        }
    }

    // Move wall to the requested position
    void moveWall(Vector3 position)
    {
        // Move wall
        if (MovableWall.position != position)
        {
            MovableWall.position = Vector3.MoveTowards(MovableWall.position, position, Time.deltaTime * wallSpeed);
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
