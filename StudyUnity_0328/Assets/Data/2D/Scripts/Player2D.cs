using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : CharacterMovement2D, IBattle
{
    public LayerMask enemyMask; 

    public void OnDamage(float dmg)
    {
        myAnim.SetTrigger("Damage");
    }

    public bool IsLive
    {
        get
        {
            return true;
        }
    }

    public void OnAttack()
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(transform.position + Forward() * 0.5f, 0.5f); // (플레이어 현재 트랜스폼 + 전방벡터 * 0.5f 거리 지점)부터 (0.5f 반지름) 사이에 있는 모든 지점
        foreach(Collider2D col in list)
        {
            col.transform.GetComponent<IBattle>()?.OnDamage(AttackPoint);
        }
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        AirCheck();
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

        if(Input.GetKeyDown(KeyCode.Space) && !myAnim.GetBool("isAir"))
        {
            //myRigid2D.AddForce(new Vector2(0, 400.0f));     // Rigidbody2D 물리를 이용한 점프 구현
            Jump(1.0f, 3.0f);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Drop();
        }
    }

    

    


}
