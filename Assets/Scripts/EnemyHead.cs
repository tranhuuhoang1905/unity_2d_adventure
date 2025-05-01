using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    // Start is called before the first frame update
    EnemyMovement enemyMovement;
    void Start()
    {
        enemyMovement = transform.parent.GetComponent<EnemyMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.collider.CompareTag("Player"))
    //     {
    //         Rigidbody2D playerRb = collision.collider.attachedRigidbody;
    //         Debug.Log($"check playerRb.velocity.y: {playerRb.velocity.y}");
    //         if (playerRb == null || playerRb.velocity.y !=0) return;
            
    //         PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
    //         if (enemyMovement == null || playerMovement == null) return;
    //         if (enemyMovement == null) return;
    //         if (playerMovement == null) return;
    //         playerMovement.KillEnemyJump();
    //         enemyMovement.EnemyDie();
    //     }
    // }



    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.collider.CompareTag("Player"))
    //     {
    //         float playerY = collision.collider.transform.position.y;
    //         float enemyY = transform.position.y;
    //         if (playerY - enemyY < 0.45f) return;

    //         PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
    //         if (enemyMovement == null || playerMovement == null) return;
    //         if (enemyMovement == null) return;
    //         if (playerMovement == null) return;
    //         playerMovement.KillEnemyJump();
    //         enemyMovement.EnemyDie();
    //     }
    // }
}
