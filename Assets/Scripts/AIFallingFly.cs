using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFallingFly : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    private float baseSpeed = 0f;
    [SerializeField] float falseTime = 1f;
    Rigidbody2D myRigidbody;
    private SpriteRenderer spriteRenderer;


    PlayerMovement  playerScript;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (baseSpeed == 0) return;
        myRigidbody.velocity = new Vector2 (0f, baseSpeed);
    }

    


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Falling());
        }
    }

    IEnumerator Falling()
    {
        int flashCount = 3;
        float flashDuration = 0.2f;
        
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
        }
        baseSpeed = moveSpeed;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    
}