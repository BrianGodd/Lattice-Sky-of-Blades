using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpBasic : MonoBehaviour
{
    [SerializeField] float Tptime;
    [SerializeField]GameObject tpto;

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
       PlayerCharacter.Instance.SetTransform(tpto.transform.position);
    }

}
