
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class SkeletonBehaviour : EnemyScript
    {
        // Character constants
        const float SPEED = 3;
        const float attackSpeed = 2;
        const float MAXHP = 60;
        const float MAXINVUL = .5f;
        const int RANGE = 2;
        const int DAMAGE = 15;

        // Character variables
        float currentHP;
        float attackTime = 0;
        float remainingInvul = 0;
        bool chasing = false;
        bool playerInRange = false;
        bool alive = true;
        AudioSource attackSound;
        AudioSource deathSound;
        AudioSource hitSound;
        Vector3 origin;

        // Environmental variables
        public Transform player;
        public float maximumDistance;
        public float potionProbability;
        private Animator anim;
        private CharacterController controller;

        // Set static environmental variables, initialise animation and health
        void Start()
        {
            this.anim = GetComponent<Animator>();
            this.controller = GetComponent<CharacterController>();
            this.anim.SetBool("isIdle", true);
            this.currentHP = MAXHP;
            AudioSource[] sounds = GetComponents<AudioSource>();
            this.deathSound = sounds[0];
            this.attackSound = sounds[1];
            this.hitSound = sounds[2];
            this.origin = this.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            // Check to decrease invulnerability counter
            if (remainingInvul > 0)
                remainingInvul -= Time.deltaTime;

            // Check to destroy if dead.
            if (anim.GetBool("dead"))
            {
                Destroy(this.gameObject);
            }

            // Check sight angle to emulate the monster seeing you
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);


            // If it hasn't seen you before, start chase
            if (!chasing && angle < 50)
                chasing = true;

            // If we are close enough and chasing, make character move
            if (Vector3.Distance(player.position, this.transform.position) < 20 && chasing && !IsFarFromHome(direction))
            {

                // This prevents the monster from swiveling upwards and downwards
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                // Monster is no longer idle
                anim.SetBool("isIdle", false);

                // If too far for an attack
                if (direction.magnitude > RANGE)
                {
                    // This stops the enemy from slipping along the ground with still legs
                    if (!anim.GetBool("lockedInPlace"))
                        controller.SimpleMove(SPEED * this.transform.forward + 10 * Vector3.down);
                    anim.SetBool("isRunning", true);
                    anim.SetBool("isAttacking", false);
                }
                else
                {   // If close enough for an attack, start combat
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isAttacking", true);
                }
            }
            else
            {
                Vector3 difference = origin - this.transform.position;
                if (difference.magnitude > 2)
                {
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isRunning", true);
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(difference), 0.1f);
                    controller.SimpleMove(SPEED * this.transform.forward + 10 * Vector3.down);
                }
                else
                {
                    anim.SetBool("isRunning", false)
;
                    anim.SetBool("isIdle", true);
                }
                chasing = false;
            }
        }

        public override void TakeDamage(int amount)
        {
            // Make it aware of your presence
            chasing = true;
            if (alive)
            {
                if (remainingInvul <= 0)
                {
                    remainingInvul = MAXINVUL;
                    currentHP -= amount;
                    if (currentHP > 0)
                    {
                        anim.SetBool("damage_001", true);
                        hitSound.Play();
                    }
                    else
                    {
                        alive = false;
                        controller.detectCollisions = false;
                        anim.SetBool("dying", true);
                        GetComponentInChildren<PotionSpawn>().DropPotion(this.potionProbability, this.transform.position, this.transform.rotation);
                        deathSound.Play();
                    }
                }
            }
        }


        private void CastAttack()
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 2);
            int i = 0;
            attackSound.Play();
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Player"))
                    player.gameObject.GetComponent<FightTheAbyss.States>().health -= DAMAGE;
                i++;
            }
        }

        private bool IsFarFromHome(Vector3 direction)
        {
            if (maximumDistance > 0)
            {
                Vector3 difference = origin - (this.transform.position + (direction / direction.magnitude));
                return difference.magnitude > maximumDistance;
            }
            else
                return false;
        }
    }
}
