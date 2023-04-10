using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public Vector2 MoveArea;            // 이동 범위 (x축 기준 왼쪽 끝을 x, 오른쪽 끝을 y로 잡는다)
    float myDir = 0.0f;                 // 이동 방향
    [Header ("우주선 속도")]
    [Range(0.0f, 3.0f)]
    public float moveSpeed = 1.0f;             // 이동 속도

    void Start()
    {
        // 이동 방향 랜덤생성
        switch (Random.Range(0, 2))
        {
            case 0:
                myDir = -1.0f;
                break;
            case 1:
                myDir = 1.0f;
                break;
        }

        StartCoroutine(Dropping(2.0f));
    }

    void Update()
    {
        transform.Translate(Vector2.right * myDir * moveSpeed * Time.deltaTime);
        if (transform.position.x <= MoveArea.x)
        {
            myDir *= -1.0f;
            transform.position = new Vector3(MoveArea.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= MoveArea.y)
        {
            myDir *= -1.0f;
            transform.position = new Vector3(MoveArea.y, transform.position.y, transform.position.z);
        }
    }

    IEnumerator Dropping (float delay)
    {
        while(true)
        {
            //Instantiate(Resources.Load("Item"), transform.position, Quaternion.identity);

            GameObject obj = Instantiate(Resources.Load("Item"), transform.position, Quaternion.identity) as GameObject;
            int count = System.Enum.GetValues(typeof(DodgeItem.Type)).Length;
            obj.GetComponent<DodgeItem>().SetType((DodgeItem.Type)Random.Range(0, count - 1));
            yield return new WaitForSeconds(delay);
        }
    }

}