using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spring : MonoBehaviour
{

    Vector3 forceV;
    [SerializeField] Transform from, to;
    [SerializeField] float force;

    // Start is called before the first frame update
    void Start()
    {
        forceV = (to.position - from.position).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerCharacter.Instance.AddForce(forceV);
        }
    }

}
