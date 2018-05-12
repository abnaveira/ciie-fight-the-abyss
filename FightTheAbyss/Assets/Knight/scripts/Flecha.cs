using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FightTheAbyss
{
    public class Flecha : MonoBehaviour
    {
        Rigidbody rigid;
        TrailRenderer trail;
        [HideInInspector] public bool onAir;
        private RaycastHit hit;
        void Awake()
        {
            rigid = this.GetComponent<Rigidbody>();
            trail = this.GetComponent<TrailRenderer>();
            hit = new RaycastHit();
        }
        void FixedUpdate()
        {
            if (onAir)
            {
                Vector3 vel = rigid.velocity;
                //vel.y -= Time.fixedDeltaTime* Statics.ArrowFallFactor;

                rigid.velocity = vel;
                this.transform.rotation = Quaternion.LookRotation(vel);
                Debug.DrawRay(transform.position, rigid.velocity * 0.5f, Color.red);
                if (Physics.Raycast(transform.position, rigid.velocity, out hit, 1f))
                {
                    onAir = false;
                    Rigidbody rigid = this.GetComponent<Rigidbody>();
                    rigid.velocity = Vector3.zero;
                    this.transform.position = hit.point;
                    rigid.isKinematic = true;
                    //coll.isTrigger = true;
                }

            }
        }
        void Update()
        {
            if (onAir)
            {

            }
        }

        public void shoot(Vector3 dir)
        {
            trail.enabled = true;
            onAir = true;
            //coll.isTrigger = false;
            rigid.isKinematic = false;
            rigid.velocity = dir.normalized * Statics.ArrowInitialForce;

        }

        /*void OnCollisionEnter(Collision collision)
        {

        }*/
    }
}