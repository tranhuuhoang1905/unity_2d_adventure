using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWallSlide : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement playerMovement;
    void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerMovement.UpdateWallSliding(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerMovement.UpdateWallSliding(false);
        }
    }
}
