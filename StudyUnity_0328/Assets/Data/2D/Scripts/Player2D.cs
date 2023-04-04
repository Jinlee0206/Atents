using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : CharacterProperty2D
{

    void Start()
    {
    }

    void Update()
    {
        Vector2 dir = Vector2.zero;
        dir.x = Input.GetAxisRaw("Horizontal");

        if (!Mathf.Approximately(dir.x, 0.0f))
        {
            myAnim.SetBool("isMoving", true);
            if (dir.x < 0.0f)
            {
                myRenderer.flipX = true;
            }
            else
            {
                myRenderer.flipX = false;
            }
        }
        else
        {
            myAnim.SetBool("isMoving", false);
        }

        //transform.position = new Vector2(x, y) * Time.deltaTime * MoveSpeed;
        transform.Translate(new Vector2(dir.x * Time.deltaTime * MoveSpeed, 0));

        if (Input.GetMouseButtonDown(0))
        {
            if (!myAnim.GetBool("isAttacking"))
            {
                myAnim.SetTrigger("Attack");
            }
            
        }
    }
}
