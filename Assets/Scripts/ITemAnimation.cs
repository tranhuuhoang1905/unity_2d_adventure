using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITemAnimation : MonoBehaviour
{
    
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            myAnimator.SetTrigger("Action");
        }
    }
}
