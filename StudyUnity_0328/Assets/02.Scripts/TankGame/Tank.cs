using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public float Speed = 1.0f;
    public float RotSpeed = 360.0f;
    public float TopRotSpeed = 90.0f;
    public Transform myTop = null;
    public Transform myCannon = null;
    public Transform myMuzzle = null;
    public Bomb myBomb = null;
    public GameObject orgBomb = null;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * RotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * RotSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            myTop.Rotate(Vector3.down * TopRotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            myTop.Rotate(Vector3.up * TopRotSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            myCannon.Rotate(Vector3.left * TopRotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {            
            myCannon.Rotate(Vector3.right * TopRotSpeed * Time.deltaTime);           
        }

        Vector3 angle = myCannon.localRotation.eulerAngles;
        if(angle.x > 180.0f)
        {
            angle.x -= 360.0f;
        }
        /*
        if (angle.x > 15.0f)
        {
            angle.x = 15.0f;
        }
        if (angle.x < -60.0f)
        {
            angle.x = -60.0f;
        }
        */
        angle.x = Mathf.Clamp(angle.x, -60.0f, 15.0f);
        myCannon.localRotation = Quaternion.Euler(angle);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            myBomb.OnFire();
            myBomb = null;

            /*
            GameObject obj = Instantiate(orgBomb);
            obj.transform.SetParent(myMuzzle);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = myMuzzle.localRotation;
            */            
            GameObject obj = Instantiate(orgBomb, myMuzzle);
            myBomb = obj.GetComponent<Bomb>();
        }
    }
}
