using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    bool isFire = false;
    public float Speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFire)
        {
            float delta = Speed * Time.deltaTime;

            Ray ray = new Ray();
            ray.origin = transform.position;
            ray.direction = transform.forward;
            if(Physics.Raycast(ray, out RaycastHit hit, delta))
            {
                DestroyObject(hit.transform.gameObject);
            }

            transform.Translate(Vector3.forward * delta);
        }
    }

    public void OnFire()
    {
        isFire = true;
        transform.SetParent(null);
        //transform.parent = null;
        GetComponent<Collider>().isTrigger = false;

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");  
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay");
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb") return;
        Debug.Log("OnTriggerEnter");
        //DestroyObject(other.gameObject);
    }

    void DestroyObject(GameObject obj)
    {
        Vector3 temp = obj.transform.position;
        Destroy(obj);
        StartCoroutine(CreateDelay(temp));
    }

    IEnumerator CreateDelay(Vector3 temp)
    {
        yield return new WaitForSeconds(1.0f);

        GameObject org = Resources.Load("Target") as GameObject;
        if (org != null)
        {
            GameObject obj = Instantiate(org);
            temp.x = Random.Range(-8.0f, 8.0f);
            obj.transform.position = temp;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
    }
}
