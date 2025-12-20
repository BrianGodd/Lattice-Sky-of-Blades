using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;


public class RockMovingPlatform : MonoBehaviour, IMoverController
{
    public PhysicsMover Mover;
    Vector3 v,start,target;
    private Transform _transform;

    public void init(Vector3 speed,Vector3 start,Vector3 end)
    {
        this.start = start;
        this.target = end;
        v = speed;
    }

    private void Start()
    {
        _transform = this.transform;

        Mover.MoverController = this;
    }

    // This is called every FixedUpdate by our PhysicsMover in order to tell it what pose it should go to
    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        goalPosition = _transform.position + v * Time.deltaTime;
        goalRotation = _transform.rotation;
    }

    void Update()
    {
        if ((transform.position - start).magnitude > (target-start).magnitude + 2)
        {
            Destroy(gameObject);
        }
    }

}
