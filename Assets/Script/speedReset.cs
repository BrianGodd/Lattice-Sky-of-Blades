using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedReset : MonoBehaviour
{
    [SerializeField] Transform tpto;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerCharacter.Instance.gameObject.transform.position.x>=-105)
            {
                PlayerCharacter.Instance.SetTransform(tpto.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerCharacter.Instance.gameObject.transform.position.x >= -104.5 && PlayerCharacter.Instance.CurrentVelocity.magnitude>5)
            {
                PlayerCharacter.Instance.SetTransform(tpto.position);
            }
        }
    }
}
