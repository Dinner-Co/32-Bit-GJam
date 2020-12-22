using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlatformerController2D : MonoBehaviour
{

    public bool wallRun;
    public bool wallJump;
    public bool dash;
    public bool chargeJump;
    public bool wallRestore;

    private int extraJumps;
    public int jumpValue;

    public float speed;
    public float wallClingSpeed;
    public float jumpForce;
    private float moveInput;

    private bool clinging;

    private Rigidbody2D rb;
    private bool faceR = true;

    private bool isGrounded;
    public Transform groundCheck;
    public Transform frontCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public bool isTouchingFront;

    private bool wallJumping;
    public float wallFling;
    public float wallJumpHeight;
    public float wallJumpTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = jumpValue;
    }

    void Flip()
    {
        faceR = !faceR;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {

        //Draws a cricle. If any thing in the ground layer goes in the circle the variable is set to true
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (faceR == false && moveInput > 0)
        {
            Flip();
        }
        else if (faceR == true && moveInput < 0)
        {
            Flip();
        }
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;

    }

    void Update()
    {

        if (isGrounded == true)
        {
            extraJumps = jumpValue;
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (isTouchingFront == true && isGrounded == false && moveInput != 0)
        {
            clinging = true;
        }
        else
        {
            clinging = false;
        }

        if (clinging == true && wallJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallClingSpeed, float.MaxValue));
        }

        if (Input.GetButtonDown("Jump") && clinging == true)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (wallJumping == true && wallJump == true)
        {
            // rb.velocity == new Vector2(wallFling * -moveInput, wallJumpHeight);
        }

    }


}
