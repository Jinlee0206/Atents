using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeItem : MonoBehaviour
{
    public float dropSpeed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(0, -dropSpeed * Time.deltaTime, 0);        
    }
}
