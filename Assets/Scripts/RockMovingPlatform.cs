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



    // public PlayableDirector Director;

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
        // // Remember pose before animation
        // Vector3 _positionBeforeAnim = _transform.position;
        // Quaternion _rotationBeforeAnim = _transform.rotation;

        // // Update animation
        // EvaluateAtTime(Time.time);

        // // Set our platform's goal pose to the animation's
        // goalPosition = _transform.position;
        // goalRotation = _transform.rotation;

        // // Reset the actual transform pose to where it was before evaluating. 
        // // This is so that the real movement can be handled by the physics mover; not the animation
        // _transform.position = _positionBeforeAnim;
        // _transform.rotation = _rotationBeforeAnim;
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

    // public void EvaluateAtTime(double time)
    // {
    //     Director.time = time % Director.duration;
    //     Director.Evaluate();
    // }
}
