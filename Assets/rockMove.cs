using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockMove : MonoBehaviour
{
    Vector3 v,start,target;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + v * Time.deltaTime);
        if ((transform.position - start).magnitude > (target-start).magnitude + 2)
        {
            Destroy(gameObject);
        }
    }

    public void init(Vector3 speed,Vector3 start,Vector3 end)
    {
        this.start = start;
        this.target = end;
        v = speed;
    }
}
