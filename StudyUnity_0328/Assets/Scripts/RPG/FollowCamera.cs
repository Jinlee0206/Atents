using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public LayerMask crashMask;

    public Transform myTarget;          // 카메라가 따라다닐 타겟
    Vector3 Dir = Vector3.zero;         // 카메라와 타겟 사이의 벡터 (타겟에서 카메라를 바라보는 방향으로 후에 설정)
    float Dist;                         // 카메라와 타겟 사이의 거리
    float targetDist = 0.0f;            // 카메라 Transform을 보간에 의해 변경되도록 설정
    public float RotSpeed = 5.0f;
    public float ZoomSpeed = 10.0f;
    float minZoom = 1.0f;
    float maxZoom = 10.0f;

    private void Awake()
    {
        transform.LookAt(myTarget);
        rotX = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);  // 카메라 위치값을 초기 설정값으로 넣어주기
        Dir = transform.position - myTarget.position;                     // 타겟에서 카메라를 바라보는 방향
        Dist = Dir.magnitude;                                             // 거리 계산
        Dir.Normalize();                                                  // 정규화

        Dir = transform.InverseTransformDirection(Dir);                   // 역행렬을 구하는 함수, 절대축의 벡터를 얻을 수 있다;
    }

    void Start()
    {
    }

    Quaternion rotX = Quaternion.identity;              // 카메라 자체의 기준 X축
    Quaternion rotY = Quaternion.identity;              // 카메라 자체의 기준 Y축

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float y = Input.GetAxis("Mouse X") * RotSpeed;
            float x = Input.GetAxis("Mouse Y") * RotSpeed;

            //Quaternion rot = Quaternion.Euler(x, y, 0);                                   // 카메라의 기준축이 설정되어있지 않아서 사이드 쪽에서 회전이 이상하다
            //Quaternion rot = Quaternion.Euler(0, y, 0) * Quaternion.Euler(x, 0, 0);

            rotX *= Quaternion.Euler(x, 0, 0);
            rotY *= Quaternion.Euler(0, y, 0);

            // X축 회전 제한 (위아래 제어)
            float angle = rotX.eulerAngles.x;
            angle = ClampAngle(angle, -60.0f, 80.0f);
            rotX = Quaternion.Euler(angle, 0, 0);


        }
        targetDist -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, minZoom, maxZoom);

        // 실제 Dist 값을 선형 보간
        Dist = Mathf.Lerp(Dist, targetDist, Time.deltaTime * 3.0f);

        // 카메라 지면 아래로 내려갔을 때 카메라를 캐릭터 쪽으로 땡겨오는 처리
        // RayCast 사용해서 충돌하는 물체로 저장되어 있는 마스크(crashMask)를 확인하여서 ray가 발견될 경우 카메라 transform.position 값을 hit.point로 변경  

        Vector3 dir = rotY * rotX * Dir;            // 최종적으로 계산한 회전 값
        float radius = 0.5f;                        // 카메라
        if (Physics.Raycast(new Ray(myTarget.position, dir), out RaycastHit hit, Dist + radius, crashMask))
        {
            //transform.position = hit.point + (- dir * radius); // 충돌한 지점까지로 카메라 위치 고정
            Dist = hit.distance - radius;                        // ray가 시작한 지점에서 부딪힌 지점까지의 거리 - 가상의 Cam radius
        }
        
        transform.position = myTarget.position + dir * Dist;

        // 카메라의 전방 벡터를 타겟을 향하게 끔 재조정
        //transform.forward = -Dir;
        //transform.rotation = Quaternion.LookRotation(-Dir);
        transform.LookAt(myTarget);
    }

    void LateUpdate()
    {
        
    }

    private float ClampAngle(float angle, float minAngle, float maxAngle)
    {
        if (angle > 180.0f) angle -= 360.0f;

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }
}
