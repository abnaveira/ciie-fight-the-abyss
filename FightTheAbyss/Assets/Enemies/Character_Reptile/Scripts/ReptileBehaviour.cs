using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightTheAbyss
{
    public class ReptileBehaviour : EnemyScript
    {
        const float WALKSPEED = 4;
        const float RUNSPEED = 7.5f;
        const float MAXHP = 250;
        const float MAXINVUL = .5f;
        const int RANGE = 4;
        const int DAMAGE1 = 35;
        const int DAMAGE2 = 22;
        const int RUNRANGE = 25;

        // Character variables
        float currentHP;
        float attackTime = 0;
        float remainingInvul = 0;
        bool switchAttack = false;
        bool chasing = false;
        bool playerInRange = false;

        public Transform player;
        private Animator anim;
        private CharacterController controller;

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            anim.SetBool("isIdle", true);
            currentHP = MAXHP;
        }


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
            if (direction.magnitude < 50 && chasing)
            {

                // This prevents the monster from swiveling upwards and downwards
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                // Monster is no longer idle
                anim.SetBool("isIdle", false);
                // If too far for an attack
                if (direction.magnitude > RANGE)
                {                    
                    // Run from closer range, walk from far away
                    if (direction.magnitude > RUNRANGE)
                    {
                        if (!anim.GetBool("lockedInPlace"))
                            controller.SimpleMove(WALKSPEED * this.transform.forward);
                        anim.SetBool("isWalking", true);
                        anim.SetBool("isRunning", false);
                    }
                    else
                    {
                        if (!anim.GetBool("lockedInPlace"))
                            controller.SimpleMove(RUNSPEED * this.transform.forward);
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isRunning", true);
                    }
                    anim.SetBool("attack1", false);
                    anim.SetBool("attack2", false);

                }
                else
                {   // If close enough for an attack, start combat
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", false);
                    if (!anim.GetBool("lockedInPlace"))
                    {
                        if (switchAttack = !switchAttack)
                        {
                            anim.SetBool("attack1", false);
                            anim.SetBool("attack2", true);
                        }
                        else
                        {
                            anim.SetBool("attack1", true);
                            anim.SetBool("attack2", false);
                        }
                    }
                }
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
                    anim.SetBool("damaged", true);
                else if (!anim.GetBool("dying"))
                {
                    anim.SetBool("dying", true);
                    //GetComponentInChildren<PotionSpawn>().DropPotion(1, this.transform.position, this.transform.rotation);
                }
            }
        }

        private void Attack(float dmg)
        {
            Vector3 pos = transform.position;
            pos.z += 2;
            Collider[] hitColliders = Physics.OverlapSphere(pos, 4);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].CompareTag("Player"))
                    player.gameObject.GetComponent<FightTheAbyss.States>().health -= dmg;
                i++;
            }
        }
        
        public void CastAttack1()
        {
            Attack(DAMAGE1);
        }

        public void CastAttack2()
        {
            Attack(DAMAGE2);
        }
    }
}
