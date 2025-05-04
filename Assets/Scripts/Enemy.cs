using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDie = false;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float destroyDelay = 0.5f;
    Animator myAnimator;
    
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDie) return;
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
