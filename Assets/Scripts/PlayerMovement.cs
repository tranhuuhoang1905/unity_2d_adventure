using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpEnemyKillSpeed = 5f;

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
    bool isWallJump = false;
    public GameObject dustBurstPrefab;
    public GameObject dustWallPrefab;
    ParticleSystem ps;
    ParticleSystem psWall;

    [SerializeField] float wallGravity = 0f;
    private bool isWallSliding = false;
    private GameSession gameSession;
    public bool isImmortal = false;
    private SpriteRenderer spriteRenderer;
    private Vector2 platformVelocity = Vector2.zero;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        
        ps = dustBurstPrefab.GetComponent<ParticleSystem>();
        psWall = dustWallPrefab.GetComponent<ParticleSystem>();
        
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Drag();
        Run();
        FlipSprite();
        AnimationAction();
    }
    void Drag()
    {
        myRigidbody.drag = isWallSliding ? 5f : 0f;
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
            JumpAction();
        }
    }

    void JumpAction()
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
    }
    public void AutoJump(float jumpSpeed)
    {
        currentJumpCount = 1;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        isJumpUp = true;
        isDoubleJumpUp = false;
        myAnimator.SetBool("isJumpUp", true);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity +  new Vector2(platformVelocity.x, 0f);;
    }
    
    void AnimationAction()
    {
        bool playerHasHorizontalSpeed = IsRuning();
        bool playerIsTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        
        isFalling = CheckFalling();

        isWallJump = CheckWallJump();
        if (playerIsTouchingGround && CheckIsGround())
        {
            currentJumpCount = 0;
        }
        
        if (isFalling || isWallJump ||playerIsTouchingGround || CheckIsGround())
        {
            isJumpUp = false;
            isDoubleJumpUp = false;
        }
        ParticalGroundAction(
            ( playerIsTouchingGround && playerHasHorizontalSpeed) 
            || 
            (!playerIsTouchingGround && !isFalling)
        );
        ParticalWallAction(isWallJump);
        myAnimator.SetBool("isWallJump", isWallJump);

        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        myAnimator.SetBool("isFalling", isFalling);
        myAnimator.SetBool("isJumpUp", isJumpUp);
        myAnimator.SetBool("isDoubleJumpUp", isDoubleJumpUp);
        myAnimator.SetBool("isGround", playerIsTouchingGround || CheckIsGround());
    }
    bool CheckFalling()
    {
        return myRigidbody.velocity.y < -0.1f && !CheckWallJump();
    }

    bool CheckWallJump()
    {
        
        bool playerIsTouchingGround = CheckIsGround();
        return !playerIsTouchingGround  && isWallSliding ;
    }
    bool CheckIsGround()
    {
        return myRigidbody.velocity.y < 0.1f && myRigidbody.velocity.y > -0.1f;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = IsRuning();
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    bool IsRuning()
    {
        return Mathf.Abs(myRigidbody.velocity.x - platformVelocity.x) > Mathf.Epsilon;
    }

    void TakeLife()
    {
        if (isImmortal) return;

        if (gameSession)
        {
            if (gameSession.GetPlayerLives() == 1){
                isAlive = false;
                myAnimator.SetTrigger("Dying");
            }

            gameSession.ProcessPlayerDeath();
        }
        
        StartCoroutine(IgnoreEnemyCollisionTemporarily());
    }

    IEnumerator IgnoreEnemyCollisionTemporarily()
    {
        int flashCount = 5;
        float flashDuration = 0.2f;
        isImmortal = true;
        isAlive = false;
        
        myRigidbody.velocity = Vector2.zero;
        for (int i = 0; i < flashCount; i++)
        {
            yield return new WaitForSeconds(flashDuration);
            if (i == 1 && gameSession.GetPlayerLives() > 0){
                isAlive = true;
            }
            spriteRenderer.color = Color.red; // Tùy – đỏ báo thương, hoặc Color.clear
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
        }

        spriteRenderer.color = Color.white; // Trả lại bình thường
        isImmortal = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            myRigidbody.velocity = Vector2.zero;
            currentJumpCount = 0;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies") || other.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            if (isImmortal) return;
            TakeLife();
        }
        
    }

    public void UpdateWallSliding(bool status)
    {
        isWallSliding = status;
        if(status)
        {
            myRigidbody.velocity = Vector2.zero;
            currentJumpCount = 0;
            myRigidbody.gravityScale = wallGravity ;
        }
        else
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            if (isImmortal) return;
            myRigidbody.velocity = Vector2.zero;
            TakeLife();
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f && Mathf.Abs(rb.velocity.y) < 0.1f) 
                {
                    currentJumpCount = 0;
                    return;
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f && Mathf.Abs(rb.velocity.y) < 0.1f) 
                {
                    Enemy enemy = collision.transform.GetComponent<Enemy>();
                    if(enemy != null)
                    {
                        AutoJump(jumpEnemyKillSpeed);
                        enemy.EnemyDie();
                    }
                    return;
                }
            }
        }
    }

    public void SetPlatformVelocity(Vector2 velocity)
    {
        platformVelocity = velocity;
    }

}
