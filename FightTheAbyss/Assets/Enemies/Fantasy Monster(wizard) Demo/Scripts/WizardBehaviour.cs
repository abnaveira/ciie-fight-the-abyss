using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FightTheAbyss
{
    public class WizardBehaviour : FightTheAbyss.EnemyScript
    {
        // Character constants
        const float SPEED = 4;
        const float attackSpeed = 2;
        const float MAXHP = 100;
        const float MAXINVUL = .5f;
        const int RANGE = 5;
        const int DAMAGE = 20;

        // Character variables
        float currentHP;
        float attackTime = 0;
        float remainingInvul = 0;
        bool chasing = false;
        bool playerInRange = false;
        bool playOnce = true;
        Vector3 origin;
        AudioSource attackSound;
        AudioSource deathSound;
        AudioSource hitSound;

        // Environmental variables
        public Transform player;
        public float maximumDistance;
        private Animator anim;
        private CharacterController controller;



        // Set static environmental variables, initialise animation and health
        void Start()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            anim.SetBool("idle_normal", true);
            currentHP = MAXHP;
            AudioSource[] sounds = GetComponents<AudioSource>();
            deathSound = sounds[0];
            attackSound = sounds[1];
            hitSound = sounds[2];
            origin = this.transform.position;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Check to decrease invulnerability counter
            if (remainingInvul > 0)
                remainingInvul -= Time.deltaTime;

            // Check to destroy if dead.
            if (anim.GetBool("dead")) {
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
                anim.SetBool("idle_normal", false);
                attackTime = anim.GetFloat("attackTime");

                // Check for attack timer decreasing
                if (attackTime > 0)
                {
                    anim.SetFloat("attackTime", attackTime -= Time.deltaTime);
                    if (attackTime < (attackSpeed - attackSpeed / 4))
                        anim.SetBool("attack_short_001", false);
                }

                // If too far for an attack
                if (direction.magnitude > 4)
                {
                    // This stops the enemy from slipping along the ground with still legs
                    if (!anim.GetBool("lockedInPlace"))
                        controller.SimpleMove(SPEED * this.transform.forward);
                    anim.SetBool("move_forward_fast", true);
                    anim.SetBool("idle_combat", false);
                }
                else
                {   // If close enough for an attack, start combat
                    anim.SetBool("move_forward_fast", false);
                    anim.SetBool("idle_combat", true);
                    // Check if an attack can be made and make it
                    if (attackTime <= 0 && (!anim.GetBool("dying")))
                    {
                        anim.SetBool("attack_short_001", true);
                        anim.SetFloat("attackTime", attackSpeed);
                        attackSound.Play();
                    }
                }
            }
            else
            {   // If nothing in range, stand still
                anim.SetBool("idle_normal", true);
                anim.SetBool("idle_combat", false);
                anim.SetBool("move_forward_fast", false);
                chasing = false;

                Vector3 difference = origin - this.transform.position;
                if (difference.magnitude > 2)
                {
                    anim.SetBool("idle_normal", false);
                    anim.SetBool("idle_combat", false);
                    anim.SetBool("move_forward_fast", true);
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(difference), 0.1f);
                    controller.SimpleMove(SPEED * this.transform.forward);
                }
                else
                {
                    anim.SetBool("move_forward_fast", false);
                    anim.SetBool("idle_normal", true);
                }
                chasing = false;
            }
        }

        // Called whenever the player attacks
        public override void TakeDamage(int amount)
        {
            // Make it aware of your presence
            chasing = true;
            if (!anim.GetBool("dying"))
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
                        anim.SetBool("dying", true);
                        GetComponentInChildren<PotionSpawn>().DropPotion(1, this.transform.position, this.transform.rotation);
                        deathSound.Play();
                    }
                }
            }
        }

        private void CastAttack()
        {
            if (playerInRange)
            {
                player.gameObject.GetComponent<FightTheAbyss.States>().health -= DAMAGE;
            }

        }

        public void SetPlayerInRange(bool value)
        {
            playerInRange = value;
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