using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class showDiag : MonoBehaviour
{

    [SerializeField] bool show = false;
    [SerializeField] GameObject diag;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            diag.SetActive(show);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            diag.SetActive(show);
        }
    }
}
