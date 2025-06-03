using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    //[SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [Header("Ground Check")]
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] LayerMask groundMask;
    private bool grounded;
    [SerializeField] private Vector2 groundVelocity;
    [Header("Speed Settings")]
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float startSpeed = 3.0f;
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 1.0f;
    [SerializeField] private float maxFallSpeed = 30.0f;
    private float jumpMagnitude = 3.0f;
    private bool jumpCutted;
    [SerializeField] private float jumpHangTreshold = 0.5f;
    [SerializeField] private float jumpCut = 0.7f;
    private bool jumpState = false;
   

    private float horizontalMove = 0.0f;
    [Header("Gravity")]
    [SerializeField] private float hangGravityMult = 0.7f;
    [SerializeField] private float fallGravityMult = 2f;
    [SerializeField] private float normalGravity = 1.0f;


    bool isFacingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Flip();
        CheckGround();
        Run();
        if (rb.linearVelocity.y < 0.0f || jumpCutted)
        {
            rb.gravityScale = normalGravity * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        if (grounded)
        {
            rb.gravityScale = normalGravity;
            jumpCutted = false; 
            jumpState = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var val = context.ReadValue<Vector2>();
        horizontalMove = val.x;
        //Debug.Log(val.x);
        if (context.canceled)
        {
            horizontalMove = 0.0f;
        }
    }

    void Run()
    {
        //if (MathF.Abs(groundVelocity.x) > MathF.Abs(rb.velocity.x))
        //{
        //    rb.velocity = new Vector2(groundVelocity.x, rb.velocity.y);
        //}
        float targetSpeed = horizontalMove * maxSpeed + groundVelocity.x;
        float speedDif = targetSpeed - rb.linearVelocity.x;

        //if no input we decel
        float accelRate = (MathF.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDif * accelRate;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        if (grounded)
        {
            if (horizontalMove != 0.0f)
            {
                animator.SetTrigger("running");
            } else
            {
                animator.SetTrigger("stopped");
            }
        } 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        animator.SetTrigger("jumped");
        var value = context.ReadValue<float>();
        if (context.performed && grounded)
        {
            float realForce = jumpForce;
            if (rb.linearVelocity.y < 0.0f)
            {
                realForce -= rb.linearVelocity.y;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + ((realForce + (0.5f * Time.fixedDeltaTime)) / rb.mass));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpState = true;   
        }
         Debug.Log($"Hold value {context.phase})");
        if (context.canceled)
        {
            if (rb.linearVelocity.y > 0.0f)
            {
                rb.AddForce(Vector2.down * rb.linearVelocity.y * jumpCut , ForceMode2D.Impulse);
                jumpCutted = true;
            }
        }
    }

    void CheckGround()
    {
        Collider2D[] collisions = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask);
        grounded = collisions.Length > 0;
        groundVelocity = Vector2.zero;
        foreach (Collider2D collision in collisions) {
            var collisionRB = collision.GetComponent<Rigidbody2D>();
            if (collisionRB != null)
            {
                groundVelocity = collisionRB.linearVelocity;
            }
        }
    }

    void Flip()
    {
        if (horizontalMove > 0.0f && !isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = true;
        } else if (horizontalMove < 0.0f && isFacingRight) 
        {

            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = false;
        }
    }
}
