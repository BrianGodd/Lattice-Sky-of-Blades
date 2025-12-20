using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phasetrigger : MonoBehaviour
{
     [SerializeField]int Tostate = 0;
    [SerializeField]    PhaseMannger phaseMannger;
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
        if (other.CompareTag("Player"))
        {

            phaseMannger.changeState(Tostate); // Change to phase 0 when player enters the trigger
            print("taggg");
            Destroy(gameObject);
        }
    }


    
}
