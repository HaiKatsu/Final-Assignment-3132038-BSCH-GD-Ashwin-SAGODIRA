using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float maxSpeed;

    public float acceleration;

    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public bool isSliding;
    public GameObject hitbox;
    public GameObject aparitionEffect;

    public bool isAttacking;
    public Animator anim;
    public GameObject[] healths;
    private float attackCooldown = 2f;
    private float lastAttackTime;

    private Collider2D col;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>(); // look for a component called Rigidbody2D and assign it to myRb
        anim = GetComponentInChildren<Animator>();
        lastAttackTime = -attackCooldown;
        col = GetComponent<Collider2D>();
        Instantiate(aparitionEffect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("IsDead") == true)
        {
            myRb.velocity = new Vector2(0, myRb.velocity.y);
            return;
        }
        else {
            anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x)); // sets the speed parameter in the animator to the absolute value of the player's x velocity

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && Mathf.Abs(myRb.velocity.x) < maxSpeed) // if the absolute value of the input is greater than 0.1, and player is not moving faster than max Speed
            {
               myRb.AddForce(new Vector2(Input.GetAxis("Horizontal")*acceleration, 0), ForceMode2D.Force); //gets Input value and multiplies it by acceleration in the x direction.
            }

            if (Input.GetAxis("Horizontal") < 0) // if the player is moving left
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0); // rotate the player 180 degrees
            }
            else if (Input.GetAxis("Horizontal") > 0) // if the player is moving right
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0); // rotate the player 0 degrees
            }

            if (isGrounded == true && isCrouching == false && isSliding == false && Mathf.Abs(myRb.velocity.x) == 0 && isAttacking == false)
            {
                anim.Play("idle");
            }

            //Debug the current annimation
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0));

            HandlingCrouchSliding();
            HandlingJumping();
            HandlingAttacks();
        }
    }

    private void HandlingAttacks()
    {
        if (isAttacking == true)
            myRb.velocity = new Vector2(myRb.velocity.x / 1f, myRb.velocity.y);

        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            anim.Play("attack_1");
            isAttacking = true;

            lastAttackTime = Time.time;

            StartCoroutine(ResetAttacking1Status());
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            anim.Play("attack_2");
            isAttacking = true;

            lastAttackTime = Time.time;

            StartCoroutine(ResetAttacking2Status());
        }
    }

    private IEnumerator ResetAttacking1Status()
    {
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    private IEnumerator ResetAttacking2Status()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        isGrounded = true;
        Time.timeScale = 1f;

        if (other.gameObject.tag == "Water")
        {
            maxSpeed = 2;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
        Time.timeScale = 0.75f;
        maxSpeed = 3;
        
    }

    private void HandlingJumping() {
        if (isGrounded == true && anim.GetCurrentAnimatorStateInfo(0).IsName("jump") == true)
        {
            anim.Play("idle");
        }

        if (isGrounded == true && Input.GetButtonDown("Jump")) // if the player is grounded and the jump button is pressed
        {
            anim.Play("jump"); // play the jump animation
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // add a force in the y direction
        }
    }

    private void HandlingCrouchSliding() {

        if (isSliding == true || isCrouching == true) {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else if (isSliding == false && isCrouching == false) {
            transform.localScale = new Vector3(3, 3, 3);
        }

        if (isCrouching == false && isGrounded == true && Input.GetKeyDown(KeyCode.LeftControl)) // if the player is not crouching, is grounded, and the crouch button is pressed
        {
            isCrouching = true; // set isCrouching to true
            

            if (Mathf.Abs(myRb.velocity.x) == 0 && isGrounded == true) {
                anim.Play("Crouch Idle");
                isSliding = false;
                maxSpeed = 1;
            }
            else {
                anim.Play("slide");
                isSliding = true;
                maxSpeed = 2;
            } 

        }
        else if (isCrouching == true && isGrounded == true && Input.GetKeyUp(KeyCode.LeftControl)) // if the player is crouching, is grounded, and the crouch button is released
        {
            isCrouching = false; // set isCrouching to false
            isCrouching = false;            
            isSliding = false;
            anim.Play("idle");
            maxSpeed = 3;
        }
    }

    public float getLastAttackTime()
    {
        return lastAttackTime;
    }

    public float getAttackCooldown()
    {
        return attackCooldown;
    }
    public Collider2D getCollider()
    {
        return col;
    }

    public Collider2D getHitbox()
    {
        return hitbox.GetComponent<Collider2D>();
    }
}
