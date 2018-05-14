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

        // Environmental variables
        public Transform player;
        static Animator anim;
        static CharacterController controller;

        // Set static environmental variables, initialise animation and health
        void Start()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            anim.SetBool("isIdle", true);
            currentHP = MAXHP;
        }

        // Update is called once per frame
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
            if (Vector3.Distance(player.position, this.transform.position) < 20 && chasing)
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
                        controller.SimpleMove(SPEED * this.transform.forward);
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
                anim.SetBool("isIdle", true);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRunning", false);
                chasing = false;
            }
        }

        public override void TakeDamage(int amount)
        {
            chasing = true;
            if (remainingInvul <= 0)
            {
                remainingInvul = MAXINVUL;
                currentHP -= amount;
                if (currentHP > 0)
                    anim.SetBool("damage_001", true);
                else if(!anim.GetBool("dying"))
                {
                    anim.SetBool("dying", true);
                    GetComponentInChildren<PotionSpawn>().DropPotion(1, this.transform.position, this.transform.rotation);
                }
            }
        }

        private void CastAttack()
        {            
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
            int i = 0;            
            while (i < hitColliders.Length)
            {                
                if(hitColliders[i].CompareTag("Player"))
                    player.gameObject.GetComponent<FightTheAbyss.States>().health -= DAMAGE;
                i++;
            }
        }
    }
}