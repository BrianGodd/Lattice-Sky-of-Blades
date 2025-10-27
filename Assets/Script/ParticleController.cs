using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public Collider co;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AreAllParticleSystemsStopped(this.transform)) co.enabled = false;
    }

    bool AreAllParticleSystemsStopped(Transform obj)
    {
        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.isPlaying)
            {
                return false;
            }
        }

        return true;
    }

}
