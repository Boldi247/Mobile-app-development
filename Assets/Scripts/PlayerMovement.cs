using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Joystick joystick;
    
    public Transform feetPos;
    public LayerMask whatIsGround;
    
    public float speed;
    public float jumpForce;
    private float moveInput;
    private bool isGrounded;
    public float checkRadius;
    public float jumpTime;
    private bool jumpedOnce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandleHorizontalMovement();
    }

    private void Update()
    {
        HandleVerticalMovement();
        UpdateAnimationUpdate();
    }

    //A function for handling the players horizontal movement.
    //!No movement until the joystick had been moved enough.
    private void HandleHorizontalMovement()
    {
        moveInput = joystick.Horizontal;
        Debug.Log(moveInput);
        if (moveInput <= .2f && moveInput >= -.2f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Math.Sign(moveInput) * speed, rb.velocity.y);
        }
    }

    //A function for handling the players vertical movement.
    //!Does not allow bunnyhopping.
    private void HandleVerticalMovement()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && joystick.Vertical >= .5f && jumpedOnce == false)
        {
            rb.velocity = Vector2.up * jumpForce;
            isGrounded=false;
            jumpedOnce = true;
        }

        if (joystick.Vertical < .5f) jumpedOnce = false;
    }

    //A function for updating the animations of the main character.
    private void UpdateAnimationUpdate()
    {
        //Flipping the character on the X axis.
        if (moveInput >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        //Setting the running/idle animation.
        if (moveInput <= .2f && moveInput >= -.2f)
        {
            animator.SetBool("running", false);
        }
        else
        {
            animator.SetBool("running", true);
        }
    }    

}
