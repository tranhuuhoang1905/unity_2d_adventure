using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCrusherMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = -5f;
    Rigidbody2D myRigidbody;
    bool isrun = true;
    [SerializeField] float currentSpeed;
    [SerializeField] float acceleration = 5f;     // gia tốc rơi
    [SerializeField] float maxSpeed = -10f;         // tốc độ tối đa (an toàn)
    private bool isStop = false;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (currentSpeed <0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        if (isStop)
        {
             myRigidbody.velocity = new Vector2 (0f,0f);            
        }
        else
        {
            myRigidbody.velocity = new Vector2 (0f,currentSpeed);
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         StartCoroutine(turnAround());
    //     }
    // }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("TrapBlock")) return;
        
        StartCoroutine(turnAround());
        
    }
    
    IEnumerator turnAround(){
        isStop = true;
        yield return new WaitForSeconds(1);
        isStop = false;
        currentSpeed = Mathf.Sign(currentSpeed)*Mathf.Abs(moveSpeed)*(-1);
    }
}

