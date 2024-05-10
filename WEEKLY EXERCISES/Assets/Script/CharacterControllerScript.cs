using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScriptTest : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;
    public bool secondaryJump;
    public float secondaryJumpForce;
    public float SecondaryJumpTime;


    // Start is called before the first frame update
    void StartTest()
    {
        myRb = GetComponent<Rigidbody2D>(); // look for the rigidbody component
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            myRb.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration, 0), ForceMode2D.Force);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            StartCoroutine(SecondaryJumpTest());
        }

        if (isGrounded == false && Input.GetButton("Jump"))
        {
            myRb.AddForce(new Vector2(0, secondaryJumpForce), ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2DTest(Collider2D other) // as long as the player is touching the ground, the player can jump
    {
        isGrounded = true;
    }

    private void OnTriggerExit2DTest(Collider2D other) // if the player is not touching the ground, the player cannot jump
    {
        isGrounded = false;
    }

    IEnumerator SecondaryJumpTest() // if the player is not touching the ground, the player can jump again
    {
        secondaryJump = true;
        yield return new WaitForSeconds(SecondaryJumpTime); // Wait for a certain amount of time
        secondaryJump = false;
        yield return null;
    }
}
