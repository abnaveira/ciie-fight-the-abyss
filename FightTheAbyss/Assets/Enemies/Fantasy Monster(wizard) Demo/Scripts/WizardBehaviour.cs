using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehaviour : EnemyScript {

    const float speed = 4;
    const float attackSpeed = 2;
    const float HP = 100;
    const int RANGE = 5;
    const int DAMAGE = 20;

    float currentHP;
    float attackTime = 0;
    int attackState = Animator.StringToHash("Base Layer.attack_short_001");
    bool seenYou = false;
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
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        anim.SetBool("damage_001", false);
        if (!seenYou && angle < 50)
            seenYou = true;
        if (Vector3.Distance(player.position, this.transform.position) < 20 && seenYou)
        {

            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            anim.SetBool("idle_normal", false);
            attackTime = anim.GetFloat("attackTime");
            if(attackTime > 0)
            {                
                anim.SetFloat("attackTime", attackTime -= Time.deltaTime);
                if(attackTime < (attackSpeed - attackSpeed/4))
                    anim.SetBool("attack_short_001", false);
            }
            if (direction.magnitude > 4)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != attackState)
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
                    anim.SetFloat("attackTime",attackSpeed);
                }
            }
        }
        else
        {
            anim.SetBool("idle_normal", true);
            anim.SetBool("idle_combat", false);
            anim.SetBool("move_forward_fast", false);
            seenYou = false;
        }
    }

    public override void TakeDamage(int amount)
    {
        seenYou = true;
        currentHP -= amount;
        anim.SetBool("damage_001", true);
    }

    private void CastAttack()
    {
        print("I am attacking");
        if (playerInRange)
        {
            player.gameObject.GetComponent<FightTheAbyss.States>().health -= DAMAGE;
            print("I have attacked the player");
        }

    }

    public void SetPlayerInRange(bool value)
    {
        print("Setting player to " + value);
        playerInRange = value;
    }
}
