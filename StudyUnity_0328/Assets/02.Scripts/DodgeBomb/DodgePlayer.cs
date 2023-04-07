using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgePlayer : CharacterMovement2D
{
    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        myAnim.SetFloat("MoveSpeed", x);
        transform.Translate(x * Time.deltaTime * MoveSpeed, 0, 0);
    }
}
