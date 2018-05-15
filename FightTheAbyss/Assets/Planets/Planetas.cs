using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planetas : MonoBehaviour {
    public float velocidadRotacion;
    public bool orbitarAlrededorDeObjeto;
    public Transform centroDeOrbita;
    public float velocidadOrbita;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.Rotate(0, velocidadRotacion * Time.fixedDeltaTime, 0);
        if (orbitarAlrededorDeObjeto) {
            transform.RotateAround(centroDeOrbita.position,new Vector3(0,1,0),velocidadOrbita*Time.fixedDeltaTime);
        }
	}
}
