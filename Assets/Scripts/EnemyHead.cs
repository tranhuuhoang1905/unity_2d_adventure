using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    // Start is called before the first frame update
    AIMovement aiMovement;
    void Start()
    {
        aiMovement = transform.parent.GetComponent<AIMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
    }

}
