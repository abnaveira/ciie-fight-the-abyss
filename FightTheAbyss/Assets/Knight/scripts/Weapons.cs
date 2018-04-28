using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {
    [SerializeField] WeaponStats equipedWeapon;

    [SerializeField]  private List<WeaponStats> weaponsList = new List<WeaponStats>();

    #region References
    States states;
    KnightCamera camera;
    #endregion
    // Use this for initialization
    public void Init (States states) {
        this.states = states;
        this.camera = KnightCamera.singleton;
	}
	
	public void FixedTick () {
        HandleRotation();
       
        if (states.aimInput)
        {
            if(equipedWeapon.weaponReference.position!= equipedWeapon.aimPosition.position) { 
                equipedWeapon.weaponReference.position = Vector3.Lerp(equipedWeapon.weaponReference.position, equipedWeapon.aimPosition.position, Time.deltaTime * 4);
            }
        }
        else
        {
            if (equipedWeapon.weaponReference.position != equipedWeapon.defaultPosition.position)
            {
                equipedWeapon.weaponReference.position = Vector3.Lerp(equipedWeapon.weaponReference.position, equipedWeapon.defaultPosition.position, Time.deltaTime * 4);
            }
        }
        
	}

    private void HandleRotation()
    {
        Vector3 dir = camera.pivot.forward;
        Quaternion rot = Quaternion.LookRotation(dir);
        equipedWeapon.weaponReference.rotation = Quaternion.Slerp(equipedWeapon.weaponReference.rotation, rot, Time.deltaTime * 4);
    }
    private WeaponStats searchWeapon(string name)
    {
        WeaponStats weaponResult=null; 
        foreach(WeaponStats weapon in weaponsList)
        {
            if (weapon.name.Equals(name))
            {
                weaponResult = weapon;
            }
        }
        return weaponResult;
    }
}
[System.Serializable]
public class WeaponStats
{
    [Header("Name")]
    public string name;
    [Header("Reference")]
    public Transform weaponReference;
    [Header("DefaultPosition")]
    public Transform defaultPosition;
    [Header("AimPosition")]
    public Transform aimPosition;
}
