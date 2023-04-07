using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement2D : CharacterProperty2D
{
    float defaultForward = 1.0f;
    Coroutine coJump = null;
    bool isDown = false;                // ���� �ִ�ġ�� �����ؼ� �������� �ִ� ���
    float dropDist = -1.0f;             // �ٴ�üũ�� �����ϱ� �����ϴ� ���� (������ �Ÿ�)
    float dropHeight = 0.0f;            // �������� �����ϴ� ������ ����

    protected void MoveByDir(Vector2 dir, UnityAction done = null)
    {
        StartCoroutine(MovingByDirection(dir, done));
    }

    protected void Jump(float totalTime, float maxHeight)
    {
        if (coJump != null) StopCoroutine(coJump);
        coJump = StartCoroutine(Jumping(totalTime, maxHeight));
    }

    protected void Drop()
    {
        Jump(0.5f, 0.5f);                       // ������ �� ��¦ �����ϴ� ȿ���� ó��
        dropHeight = transform.position.y;      // ���� ���̸� dropHeight�� ����
        dropDist = 1.5f;
    }

    protected void Attack(Transform target)
    {
        StartCoroutine(Attacking(target));
    }

    // �� ���� �����̰� �ϴ� �ڷ�ƾ �Լ�
    IEnumerator MovingByDirection(Vector2 dir, UnityAction done)
    {
        bool cliff = false;
        myAnim.SetBool("isMoving", true);
        while (!cliff)
        {
            //transform.Translate(dir * MoveSpeed * Time.deltaTime, Space.World); // Translate �̿��� �ܼ� �̵�

            if (!myAnim.GetBool("isAir")) // ���߿� ���ִ� ���°� �ƴ϶��
            {
                Vector3 pos = transform.position + (Vector3)dir * MoveSpeed * Time.deltaTime;
                cliff = checkCliff(pos + Vector3.up * 0.5f);
                if (cliff)
                {
                    break;
                }
                transform.position = pos;
            }
            
            yield return null;
        }
        myAnim.SetBool("isMoving", false);
        done?.Invoke();
    }

    // �ﰢ�Լ��� �ڷ�ƾ�� �̿��� ��������
    IEnumerator Jumping(float totalTime, float maxHeight)    // ü�� �ð�, �ְ� ����
    {
        isDown = false;
        float t = 0.0f; // ó������ �ð�
        float orgHeight = transform.position.y; // ó�� ���� �� ����

        myAnim.SetTrigger("Jump");

        while (t <= totalTime)
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


    //// �Ʒ� ����Ű�� S�� ������ �� ����
    //IEnumerator Droping()
    //{
    //    Jump(1.0f, 1.0f);
    //    dropDist = 1.5f;
    //    yield return null;
    //}

    public LayerMask crashMask;                 // �ٴ��� �Ǵ� ����ũ
    // ���̸� ���� �ٴڿ� �����ߴ��� Ȯ���ϴ� �Լ�
    protected void AirCheck()
    {
        Vector2 orgPos = transform.position + Vector3.up * 0.5f;
        Vector2 dir = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(orgPos, dir, 1.0f, crashMask);
        Debug.DrawLine(orgPos, orgPos + dir * 1.0f, Color.red);
        if (hit.collider != null && dropDist < 0.0f)
        {
            if (isDown)
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

            if (dropDist > 0.0f)
            {
                float dist = dropHeight - transform.position.y;
                if (dist > dropDist)
                {
                    dropDist = -1.0f;
                }

                //dropDist -= delta;        // ����
            }
        }
    }

    bool checkCliff(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 1.0f, crashMask);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    protected void Turn()
    {
        myRenderer.flipX = !myRenderer.flipX;
    }

    protected Vector3 Forward(){ return myRenderer.flipX? transform.right * defaultForward : -transform.right; }

    IEnumerator Attacking(Transform target)
    {
        float playTime = 0.0f;
        while(target!= null)
        {
            playTime += Time.deltaTime;
            Vector3 dir = target.position - transform.position; // �ڽſ��Լ� ��󿡰Է� ���ϴ� ����
            if(dir.magnitude <= AttackRange)
            {
                if(playTime >= AttackDelay)
                {
                    myAnim.SetTrigger("Attack");
                    playTime = 0.0f;
                }
            }
            // ���� ���� ������ ����� ����� �� -> �߰��ؾ��Ѵ�
            else
            {
                dir.y = 0.0f;                                   // x�� �����̵��� �ϰԲ� y���� 0���� ����� ��
                float dist = dir.magnitude - AttackRange;
                if (dist < 0.0f) dist = 0.0f;
                dir.Normalize();
                float delta = MoveSpeed * Time.deltaTime;
                if(delta > dist) delta = dist;
                transform.Translate(dir * delta);
            }
            yield return null;
        }
    }
}
