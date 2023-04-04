using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseEarth : MonoBehaviour
{
    public static DefenseEarth Instance = null;
    public Transform myEarth = null;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawning()
    {
        while(myEarth != null)
        {            
            Vector3 pos = Vector3.zero;
            /*
            while (Mathf.Approximately(pos.magnitude, 0.0f))
            {
                pos.x = Random.Range(-1.0f, 1.0f);
                pos.y = Random.Range(-1.0f, 1.0f);
            }
            Vector3 rndDir = (pos - myEarth.position).normalized;
            */
            Vector3 rndDir = new Vector3(0, 1, 0);
            float angle = Random.Range(0.0f, 360.0f);
            rndDir = Quaternion.Euler(0, 0, angle) * rndDir;

            pos = myEarth.position + rndDir * 4.0f;            
            GameObject obj = Instantiate(Resources.Load("DefenseEarth/Meteo"),pos,Quaternion.identity) as GameObject;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
