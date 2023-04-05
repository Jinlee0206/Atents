using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : CharacterProperty2D
{

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
            //myRigid2D.AddForce(new Vector2(0, 400.0f));     // Rigidbody2D ������ �̿��� ���� ����
            if (coJump != null) StopCoroutine(coJump);
            coJump = StartCoroutine(Jumping(1.0f, 3.0f));
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Droping());
        }
    }

    Coroutine coJump = null;
    bool isDown = false;            // ���� �ִ�ġ�� �����ؼ� �������� �ִ� ���
    // �ﰢ�Լ��� �̿��� ��������
    IEnumerator Jumping(float totalTime, float maxHeight)    // ü�� �ð�, �ְ� ����
    {
        isDown = false;
        float t = 0.0f; // ó������ �ð�
        float orgHeight = transform.position.y; // ó�� ���� �� ����

        myAnim.SetTrigger("Jump");

        while(t <= totalTime)
        {
            if (t >= totalTime * 0.5f) isDown = true;
            t += Time.deltaTime;
            float h = Mathf.Sin(Mathf.PI * (t / totalTime)) * maxHeight;    // t : totalTime = h : PI ��ʽ� �̿�

            // ������ �ϴ� ���߿� Ray�� ���� ������ ������ ������ ��

            Vector3 pos = new Vector3(transform.position.x, orgHeight + h, transform.position.z);

            if (isDown)
            {
                RaycastHit2D hit = 
                    Physics2D.Raycast(transform.position, Vector2.down, Vector3.Distance(transform.position, pos), crashMask);
                if (hit.collider != null)
                {
                    transform.position = hit.point;
                    yield break;
                }
            }

            transform.position = pos;   // ���� ���� ��ȭ�����༭ transform.position�� ����
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, orgHeight, transform.position.z);
    }

    float dropDist = 0.0f;

    // �Ʒ� ����Ű�� S�� ������ �� ����
    IEnumerator Droping()
    {
        dropDist = 1.0f;
        yield return null;
    }

    public LayerMask crashMask;                 // �ٴ��� �Ǵ� ����ũ
    // ���̸� ���� �ٴڿ� �����ߴ��� Ȯ���ϴ� �Լ�
    void AirCheck()
    {
        Vector2 orgPos = transform.position + Vector3.up * 0.05f; 
        Vector2 dir = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(orgPos, dir, 0.2f, crashMask);
        if (hit.collider != null && Mathf.Approximately(dropDist, 0.0f))
        {
            if(isDown)
            {
                if (coJump != null) StopCoroutine(coJump);
                transform.position = hit.point;
            }
            myAnim.SetBool("isAir", false); // ���̸� ���� �� ����� ���� �ִ�
        }
        else
        {
            myAnim.SetBool("isAir", true);                              // ���̸� ���� �� ����� ���� ����
            float delta = 9.8f * Time.deltaTime;
            transform.position += Vector3.down * delta; // ������ �߷� �۵� (Rigidbody ��� �÷��̾� ������ ����)

            if(dropDist > 0.0f)
            {
                dropDist -= delta;
            }
        }
    }


}
