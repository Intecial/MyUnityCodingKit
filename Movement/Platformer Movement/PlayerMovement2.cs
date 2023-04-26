using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private Vector2 direction;

    [Header("Vertical Movement")]
    [SerializeField]
    private float jumpForce = 15f;
    [SerializeField]
    private float jumpDelay = 0.25f;
    private float jumpTimer;

    [SerializeField]
    private float coyoteTimeDelay = 0.25f;

    [SerializeField]
    private float maximumFallSpeed = 50f;


    [SerializeField]
    private bool isFacingRight = true;

    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask groundLayer; 
        [SerializeField]
    private Animator animator;

    [SerializeField]
    private ParticleSystem dust;
    

    [Header("Physics")]
    [SerializeField]
    private float MaxSpeed = 7f;
    [SerializeField]
    private float linearDrag = 4f; 
    [SerializeField]
    private float gravity = 1f;
    [SerializeField]
    private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField]
    private bool onGround = false;
    [SerializeField]
    private float groundLength = 0.6f;
    [SerializeField]
    private Vector3 colliderOffset;



    private float lastJump = 0;

    private float lastOnGround = 0;

    const string IDLE = "idle";
    const string JUMP = "Jump";
    const string FALL = "fall";

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();      
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position -     colliderOffset, Vector2.down, groundLength, groundLayer);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));    
        if(Input.GetButtonDown("Jump")){
            jumpTimer = Time.time + jumpDelay;
        }
        if(onGround){
            ChangeAnimationState(IDLE);
        }
        Debug.Log(rb.velocity.y);
        
    }
    private void FixedUpdate() {
        moveCharacter(direction.x);
        if((jumpTimer > Time.time)){
            if(onGround){
                Jump();
            }
            else if(Mathf.Abs(lastOnGround - Time.time) < coyoteTimeDelay && Mathf.Abs(lastOnGround - Time.time) > 0f ){
                Jump();
            }
        }

        modifyPhysics();
    }

    void moveCharacter(float horizontal){
        rb.AddForce(horizontal * moveSpeed * Vector2.right);

        if((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight)){
            flip();
        }

        if(Mathf.Abs(rb.velocity.x) > MaxSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxSpeed, rb.velocity.y);
        }
    }
    private void modifyPhysics(){
        bool isChangingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if(onGround){
            if(Mathf.Abs(direction.x) < 0.4f || isChangingDirection){
                rb.drag = linearDrag;
            } else {
                rb.drag = 0;
            }
            rb.gravityScale = 0;
            lastOnGround = Time.time;
        }
        else{
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0){
                rb.gravityScale = gravity * fallMultiplier;
                ChangeAnimationState(FALL);
            } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")){
                rb.gravityScale = gravity * (fallMultiplier / 2);
                ChangeAnimationState(FALL);
            }

            if(rb.velocity.y < -maximumFallSpeed){
                    rb.velocity = new Vector2(rb.velocity.x, -maximumFallSpeed);
            }
            

        }
    }    
    void flip(){
        createDust();
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void Jump(){
        // lastOnGround = 0;
        createDust();
        animator.Play(JUMP);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
        lastJump = Time.time;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
    void ChangeAnimationState(string newState){
        animator.Play(newState);
    }

    void createDust(){
        dust.Play();
    }
}
