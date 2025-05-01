using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = -1f;
    Rigidbody2D myRigidbody;
    private bool isDie = false;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float destroyDelay = 0.5f;
    Animator myAnimator;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDie) return;
        myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
    //         if (playerMovement != null)
    //         {
    //             playerMovement.KillEnemyJump(); // Gọi nhảy lại
    //         }

    //         EnemyDie();
    //     }
    // }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Ground")) return;
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 ((Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    public void EnemyDie()
    {
        isDie = true;
        myAnimator.SetTrigger("Dying");
        // 1. Tắt tất cả collider để ngăn va chạm tiếp tục
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // 2. Nhảy lên rồi rơi xuống
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, jumpForce); // ✅ đẩy lên trục Y
            rb.gravityScale = 7f;                     // cho rơi tự nhiên
            rb.simulated = true;
            rb.isKinematic = false;
            rb.freezeRotation = false;               // tuỳ chọn: cho phép quay nếu thích
        }

        // 3. Biến mất sau delay
        Destroy(gameObject, destroyDelay);
    }
}
