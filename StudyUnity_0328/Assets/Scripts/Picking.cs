using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picking : MonoBehaviour
{    
    public LayerMask pickMask;
    public LayerMask ememyMask;
    public float MoveSpeed = 2.0f;
    public float Velocity = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        targetRot = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {        
        if(Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pickMask))
            {
                //if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))                
                if(((1 << hit.transform.gameObject.layer) & ememyMask) ==  0)
                {                    
                    StopAllCoroutines();
                    //transform.position = hit.point;
                    targetPos = transform.position;
                    targetRot = transform.rotation.eulerAngles;
                    StartCoroutine(MovingToPos(hit.point));
                }
            }
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, Velocity * Time.deltaTime);
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), 10.0f * Time.deltaTime);
    }

    Vector3 targetPos;
    IEnumerator MovingToPos(Vector3 pos)
    {        
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;//Vector3.Distance(pos, transform.position);        
        dir.Normalize();
        StartCoroutine(Rotating(dir));

        /*
        //È¸Àü
        float d = Vector3.Dot(transform.forward, dir);
        float r = Mathf.Acos(d);
        // y : 180 = x : pi
        // y / 180 = x / pi;
        // y = 180 * ( x / pi );
        float angle = 180.0f * (r / Mathf.PI);
        angle = Vector3.Angle(transform.forward, dir);

        float rotDir = 1.0f;
        if(Vector3.Dot(transform.right,dir) < 0.0f)
        {
            rotDir = -1.0f;
        }

        transform.Rotate(Vector3.up * rotDir * angle);
        */

        while (dist > 0.0f)
        {
            float delta = MoveSpeed * Time.deltaTime;
            if(dist - delta < 0.0f)
            {
                delta = dist;
            }
            //transform.Translate(dir * delta, Space.World);
            targetPos += dir * delta;            
            dist -= delta;
            yield return null;
        }        
    }

    Vector3 targetRot;
    IEnumerator Rotating(Vector3 dir)
    {
        float d = Vector3.Dot(transform.forward, dir);
        float r = Mathf.Acos(d);
        float angle = r * Mathf.Rad2Deg;
        float rotDir = 1.0f;
        if(Vector3.Dot(transform.right,dir) < 0.0f)
        {
            rotDir = -1.0f;
        }

        while(angle > 0.0f)
        {
            float delta = 360.0f * Time.deltaTime;
            if(angle - delta < 0.0f)
            {
                delta = angle;
            }
            //transform.Rotate(Vector3.up * rotDir * delta);
            targetRot.y += rotDir * delta;
            angle -= delta;
            yield return null;
        }
    }
}
