using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] int maxJumpCount = 2; 
    int currentJumpCount = 0;
    
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    bool isJumpUp = false;
    bool isDoubleJumpUp = false;
    bool isFalling = true;
    public GameObject dustBurstPrefab;
    public GameObject dustWallPrefab;
    ParticleSystem ps;
    ParticleSystem psWall;

    [SerializeField] float wallGravity = 0f;
    private bool isWallSliding = false;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        
        ps = dustBurstPrefab.GetComponent<ParticleSystem>();
        psWall = dustWallPrefab.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Drag();
        Run();
        FlipSprite();
        // ClimbLadder();
        Die();
        AnimationAction();
    }
    void Drag()
    {
        myRigidbody.drag = isWallSliding ? 4f : 0f;
    }

    void ParticalGroundAction(bool active)
    {
        if (active)
        {
            if (!ps.isPlaying)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // reset đúng
                ps.Simulate(0f, true, true); 
                ps.Play();
            }
        }
        else
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void ParticalWallAction(bool active)
    {
        if (active)
        {
            Vector3 scale = psWall.transform.localScale;
            scale.x = Mathf.Sign(transform.localScale.x); // giống hướng nhân vật
            psWall.transform.localScale = scale;
            if (!psWall.isPlaying)
            {
                psWall.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // reset đúng
                psWall.Simulate(0f, true, true); 
                psWall.Play();
            }
        }
        else
        {
            psWall.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
    

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }
    
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed && currentJumpCount < maxJumpCount)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed); // không dùng +=
            if (isWallSliding)
            {
                currentJumpCount = 1;
            }
            else
            {
                currentJumpCount++;
            }
            
            Debug.Log($"check currentJumpCount:{currentJumpCount} ");
            
            if (currentJumpCount == 1)
            {
                isJumpUp = true;
                isDoubleJumpUp = false;
                myAnimator.SetBool("isJumpUp", true);
            }
            else
            {
                isDoubleJumpUp = true;
                myAnimator.SetBool("isDoubleJumpUp", isDoubleJumpUp);
            }

            // CreateDustJump(); // gọi bụi nếu bạn có hiệu ứng
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }
    
    void AnimationAction()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        bool playerIsTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        
        isFalling = CheckFalling();

        if (playerIsTouchingGround && CheckIsGround())
        {
            currentJumpCount = 0;
        }
        
        if (isFalling)
        {
            isJumpUp = false;
            isDoubleJumpUp = false;
        }
        ParticalGroundAction(
            ( playerIsTouchingGround && playerHasHorizontalSpeed) 
            || 
            (!playerIsTouchingGround && !isFalling)
        );

        ParticalWallAction(!playerIsTouchingGround && isWallSliding);

        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        myAnimator.SetBool("isFalling", isFalling);
        myAnimator.SetBool("isJumpUp", isJumpUp);
        myAnimator.SetBool("isDoubleJumpUp", isDoubleJumpUp);
        myAnimator.SetBool("isGround", playerIsTouchingGround);
    }
    bool CheckFalling()
    {
        return myRigidbody.velocity.y < -0.1f;
    }
    bool CheckIsGround()
    {
        return myRigidbody.velocity.y < 0.1f && myRigidbody.velocity.y > -0.1f;
    }

    void FlipSprite()
    {
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    // void ClimbLadder()
    // {
    //     if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
    //     { 
    //         myRigidbody.gravityScale = gravityScaleAtStart;
    //         myAnimator.SetBool("isClimbing", false);
    //         return;
    //     }
        
    //     Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
    //     myRigidbody.velocity = climbVelocity;
    //     myRigidbody.gravityScale = 0f;

    //     bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
    //     myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    // }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isWallSliding = true;
            myRigidbody.velocity = Vector2.zero;
            currentJumpCount = 0;
            myRigidbody.gravityScale = wallGravity ;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            isWallSliding = false;
            myRigidbody.gravityScale = gravityScaleAtStart;
        }
    }

}
