using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFly : MonoBehaviour
{
    [SerializeField] float moveSpeed = -1f;
    Rigidbody2D myRigidbody;

    PlayerMovement  playerScript;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Ground")) return;
        moveSpeed = -moveSpeed;
        if(playerScript != null)
        {
            playerScript.SetPlatformVelocity(myRigidbody.velocity*(-1));
        }
        FlipEnemyFacing();
        
    }

    
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 ((Mathf.Sign(myRigidbody.velocity.x)), 1f);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.SetPlatformVelocity(myRigidbody.velocity);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerScript != null)
            {
                playerScript.SetPlatformVelocity(Vector2.zero);
                playerScript = null;
            }
        }
    }
}
