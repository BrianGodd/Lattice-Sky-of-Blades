using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    // Start is called before the first frame update
    public float boostForce = 20f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        var playerDirection = PlayerCharacter.Instance.transform.forward;
        PlayerCharacter.Instance.AddForce(playerDirection * boostForce);
        Debug.Log("Speed Boost Activated");
    }
}
