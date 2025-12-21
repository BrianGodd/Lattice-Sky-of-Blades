using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spring : MonoBehaviour
{

    Vector3 forceV;
    [SerializeField] Transform from, to;
    [SerializeField] float force;
    bool isCD = false;

    // Start is called before the first frame update
    void Start()
    {
        forceV = (to.position - from.position).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isCD)
        {

            PlayerCharacter.Instance.AddForce(forceV);
            StartCoroutine(CD());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isCD)
        {
            PlayerCharacter.Instance.AddForce(forceV);
            StartCoroutine(CD());
        }
    }


    IEnumerator CD()
    {
        isCD = true;
        yield return new WaitForSeconds(0.2f);
        isCD = false;
    }

}
