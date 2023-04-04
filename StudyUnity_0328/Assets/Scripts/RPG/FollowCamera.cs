using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public LayerMask crashMask;

    public Transform myTarget;          // ī�޶� ����ٴ� Ÿ��
    Vector3 Dir = Vector3.zero;         // ī�޶�� Ÿ�� ������ ���� (Ÿ�ٿ��� ī�޶� �ٶ󺸴� �������� �Ŀ� ����)
    float Dist;                         // ī�޶�� Ÿ�� ������ �Ÿ�
    float targetDist = 0.0f;            // ī�޶� Transform�� ������ ���� ����ǵ��� ����
    public float RotSpeed = 5.0f;
    public float ZoomSpeed = 10.0f;
    float minZoom = 1.0f;
    float maxZoom = 10.0f;

    private void Awake()
    {
        transform.LookAt(myTarget);
        rotX = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);  // ī�޶� ��ġ���� �ʱ� ���������� �־��ֱ�
        Dir = transform.position - myTarget.position;                     // Ÿ�ٿ��� ī�޶� �ٶ󺸴� ����
        Dist = Dir.magnitude;                                             // �Ÿ� ���
        Dir.Normalize();                                                  // ����ȭ

        Dir = transform.InverseTransformDirection(Dir);                   // ������� ���ϴ� �Լ�, �������� ���͸� ���� �� �ִ�;
    }

    void Start()
    {
    }

    Quaternion rotX = Quaternion.identity;              // ī�޶� ��ü�� ���� X��
    Quaternion rotY = Quaternion.identity;              // ī�޶� ��ü�� ���� Y��

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float y = Input.GetAxis("Mouse X") * RotSpeed;
            float x = Input.GetAxis("Mouse Y") * RotSpeed;

            //Quaternion rot = Quaternion.Euler(x, y, 0);                                   // ī�޶��� �������� �����Ǿ����� �ʾƼ� ���̵� �ʿ��� ȸ���� �̻��ϴ�
            //Quaternion rot = Quaternion.Euler(0, y, 0) * Quaternion.Euler(x, 0, 0);

            rotX *= Quaternion.Euler(x, 0, 0);
            rotY *= Quaternion.Euler(0, y, 0);

            // X�� ȸ�� ���� (���Ʒ� ����)
            float angle = rotX.eulerAngles.x;
            angle = ClampAngle(angle, -60.0f, 80.0f);
            rotX = Quaternion.Euler(angle, 0, 0);


        }
        targetDist -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, minZoom, maxZoom);

        // ���� Dist ���� ���� ����
        Dist = Mathf.Lerp(Dist, targetDist, Time.deltaTime * 3.0f);

        // ī�޶� ���� �Ʒ��� �������� �� ī�޶� ĳ���� ������ ���ܿ��� ó��
        // RayCast ����ؼ� �浹�ϴ� ��ü�� ����Ǿ� �ִ� ����ũ(crashMask)�� Ȯ���Ͽ��� ray�� �߰ߵ� ��� ī�޶� transform.position ���� hit.point�� ����  

        Vector3 dir = rotY * rotX * Dir;            // ���������� ����� ȸ�� ��
        float radius = 0.5f;                        // ī�޶�
        if (Physics.Raycast(new Ray(myTarget.position, dir), out RaycastHit hit, Dist + radius, crashMask))
        {
            //transform.position = hit.point + (- dir * radius); // �浹�� ���������� ī�޶� ��ġ ����
            Dist = hit.distance - radius;                        // ray�� ������ �������� �ε��� ���������� �Ÿ� - ������ Cam radius
        }
        
        transform.position = myTarget.position + dir * Dist;

        // ī�޶��� ���� ���͸� Ÿ���� ���ϰ� �� ������
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
