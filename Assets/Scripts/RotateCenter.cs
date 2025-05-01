using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCenter : MonoBehaviour
{
    public Transform centerPoint;   // Điểm xoay – ví dụ là cái block vuông
    public float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
