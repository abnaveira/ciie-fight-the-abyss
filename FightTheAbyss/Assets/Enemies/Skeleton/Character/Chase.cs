using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Chase : MonoBehaviour {
    float speed = 3;
    int attackState = Animator.StringToHash("Base Layer.Attack");
    bool seenYou = false;
    public Transform player;
    static Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        
        if (!seenYou && angle < 50)
            seenYou = true;        
        if (Vector3.Distance(player.position, this.transform.position) < 20 && seenYou)
        {
            
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            anim.SetBool("isIdle", false);
            if (direction.magnitude > 1)
            {
                if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash != attackState)
                    this.transform.Translate(0, 0, speed * Time.deltaTime);
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking", false);
            }
            else
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", true);
            }
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", false);
            seenYou = false;
        }
	}
}
