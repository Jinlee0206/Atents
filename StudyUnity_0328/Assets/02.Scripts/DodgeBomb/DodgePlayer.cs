using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgePlayer : CharacterMovement2D
{
    bool isActive = true;
    public void SetActive(bool act)
    {
        isActive = act;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(isActive)
        {
            float x = Input.GetAxis("Horizontal");
            myAnim.SetFloat("MoveSpeed", x);
            transform.Translate(x * Time.deltaTime * MoveSpeed, 0, 0);
        }
    }
}
