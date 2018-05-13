using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FightTheAbyss
{
    public class WizardBehaviour : FightTheAbyss.EnemyScript
    {

        const float speed = 4;
        const float attackSpeed = 2;
        const float MAXHP = 100;
        const float MAXINVUL = .5f;
        const int RANGE = 5;
        const int DAMAGE = 20;

        float currentHP;
        float attackTime = 0;
        float remainingInvul = 0;
        bool chasing = false;
        bool playerInRange = false;
        public Transform player;
        static Animator anim;
        static CharacterController controller;
        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            anim.SetBool("idle_normal", true);
            currentHP = MAXHP;
        }

        // Update is called once per frame
        void Update()
        {
            if (remainingInvul > 0)
                remainingInvul -= Time.deltaTime;

            if (anim.GetBool("dead")) {
                Destroy(this.gameObject);
            }
            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward); 

            if (!chasing && angle < 50)
                chasing = true;

            if (Vector3.Distance(player.position, this.transform.position) < 20 && chasing)
            {

                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                anim.SetBool("idle_normal", false);
                attackTime = anim.GetFloat("attackTime");
                if (attackTime > 0)
                {
                    anim.SetFloat("attackTime", attackTime -= Time.deltaTime);
                    if (attackTime < (attackSpeed - attackSpeed / 4))
                        anim.SetBool("attack_short_001", false);
                }
                if (direction.magnitude > 4)
                {

                    if (!anim.GetBool("lockedInPlace"))
                        controller.SimpleMove(speed * this.transform.forward);
                    anim.SetBool("move_forward_fast", true);
                    anim.SetBool("idle_combat", false);
                }
                else
                {
                    anim.SetBool("move_forward_fast", false);
                    anim.SetBool("idle_combat", true);

                    if (attackTime <= 0)
                    {
                        anim.SetBool("attack_short_001", true);
                        anim.SetFloat("attackTime", attackSpeed);
                    }
                }
            }
            else
            {
                anim.SetBool("idle_normal", true);
                anim.SetBool("idle_combat", false);
                anim.SetBool("move_forward_fast", false);
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
                else
                {
                    anim.SetBool("dying", true);
                    GetComponentInChildren<PotionSpawn>().DropPotion(1, this.transform.position, this.transform.rotation);
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
    }

}