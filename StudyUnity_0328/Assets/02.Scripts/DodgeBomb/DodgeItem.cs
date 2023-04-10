using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeItem : MonoBehaviour
{
    public enum Type
    {
        Bomb, Score, None
    }


    private Type myType = Type.None;
    public LayerMask crashMask;
    public float DropSpeed = 3.0f;
    public Sprite[] list;


    void Start()
    {
        
    }

    public void SetType(Type type)
    {
        switch (type)
        {
            case Type.Bomb:
                break;
            case Type.Score:
                break;
        }

    }

    void Update()
    {
        transform.Translate(0, -DropSpeed * Time.deltaTime, 0);        
        //transform.Translate(Vector3.down * DropSpeed * Time.deltaTime);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer) & crashMask) != 0)
        {
            Destroy(this.gameObject);
        }
    }
}
