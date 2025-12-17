using Unity.Cinemachine;
using UnityEngine;

public class CameraSpringExtension : CinemachineExtension
{

    [Min(0.01f)]
    [SerializeField] private float halflife = 0.075f;

    [SerializeField] private float frequency = 18f;

    [SerializeField] private float angularDisplacement = 2f;
    [SerializeField] private float linearDisplacement = 0.05f;

    public Vector3 _springPosition;
    public Vector3 _springVelocity;
    bool _initialized = false;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        // Only modify the *final* camera state, after Body + Aim (PanTilt) are done
        if (stage != CinemachineCore.Stage.Finalize)
            return;

        if (deltaTime <= 0f)
            return;

        if (!_initialized)
        {
            _springPosition = state.RawPosition;
            _springVelocity = Vector3.zero;
            _initialized = true;
        }

        Vector3 targetPos = state.RawPosition;

        // Your spring update
        Spring(ref _springPosition, ref _springVelocity, targetPos, halflife, frequency, deltaTime);

        // Offset between spring position and raw camera position
        Vector3 offset = _springPosition - targetPos;

        // Project offset onto "up" to decide how much to tilt
        Vector3 up = state.ReferenceUp;
        float springHeight = Vector3.Dot(offset, up);

        // How much to pitch the camera based on vertical spring motion
        float pitchTilt = -springHeight * angularDisplacement;

        // Apply positional bob
        state.RawPosition += offset * linearDisplacement;

        // Apply rotational tilt (around camera's local X axis)
        Quaternion camRot = state.RawOrientation;
        Vector3 right = camRot * Vector3.right;
        Quaternion tiltRot = Quaternion.AngleAxis(pitchTilt, right);
        state.RawOrientation = tiltRot * camRot;
    }

    public void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halflife, float frequency, float timeStep)
    {
        float dampingRatio = -Mathf.Log(0.5f) / (frequency * halflife);
        float f = 1.0f + 2.0f * timeStep * dampingRatio * frequency;
        float oo = frequency * frequency;
        float hoo = timeStep * oo;
        float hhoo = timeStep * hoo;
        float detInv = 1.0f / (f + hhoo);
        Vector3 detX = f * current + timeStep * velocity + hhoo * target;
        Vector3 detV = velocity + hoo * (target - current);
        current = detX * detInv;
        velocity = detV * detInv;
    }
}
