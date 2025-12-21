using UnityEngine;
using KinematicCharacterController;
using System;
using Unity.VisualScripting;


public enum CrouchInput
{
    None,
    Toggle
}

public enum Stance
{
    Stand,
    Crouch,
    Slide,
    Dash
}

public struct CharacterState
{
    public bool Grounded;
    public Stance Stance;
    // public Vector3 Acceleration;
}
public struct CharacterInput
{
    public Vector2 Move;
    public Quaternion Rotation;
    public bool Jump;
    public bool JumpSustain;
    public CrouchInput Crouch;
    public bool Attack;
    public bool Dash;
}
public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    public static PlayerCharacter Instance { get; private set; }
    public event Action OnMagicJump;
    public event Action OnJump;
    public event Action OnLand;
    
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private Transform root;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float walkResponse = 10f;
    [SerializeField] private float crouchResponse = 10f;
    [SerializeField] private float airSpeed = 15f;
    [SerializeField] private float airAcceleration = 75f;

    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [Range(0f, 1f)]
    [SerializeField] private float jumpSustainGravity = 0.4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float slideStartSpeed = 25f;
    [SerializeField] private float slideEndSpeed = 15f;
    [SerializeField] private float slideFriction = 0.8f;
    [SerializeField] private float slideSteerAcceleration = 5f;
    [SerializeField] private float slideGravity = -30f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchHeightResponse = 10f;
    [Range(0f, 1f)]
    [SerializeField] private float standCameraTargetHeight = 0.9f;
    [Range(0f, 1f)]
    [SerializeField] private float crouchCameraTargetHeight = 0.7f;

    [Header("Early Jump")]
    [SerializeField] private float earlyJumpWindow = 0.3f;
    [SerializeField] private float earlyJumpForwardBoost = 10f;
    [Header("Fast Fall")]
    [SerializeField] private float fastFallSpeed = 20f;
    
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 40f;
    // [SerializeField] private float dashMinSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    // [SerializeField] private AnimationCurve dashSpeedCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [Header("External Forces")]
    // [SerializeField] private float externalForceFriction = 60f;

    [Header("Debug")]
    [SerializeField] private Vector3 testForce = new Vector3(0, 10, 0);

    private CharacterState _state;
    private CharacterState _tempState;
    private CharacterState _lastState;
    // private Stance _stance;
    private Quaternion _requestedRotation;
    private Vector3 _requestedMovement;
    private bool _requestedJump;
    private bool _requestedJumpSustain;
    private bool _requestedCrouch;
    private bool _crouchJustPressed;
    private bool _requestedDash;
    private float _timeSinceGrounded;
    private float _timeSinceUngrounded;
    private float _timeSinceJumpRequested;  
    private bool _ungroundedDueToJump;
    private bool _jumpBuffered;
    private Vector3 _externalForce;
    private float _dashTimer;
    private Vector3 _dashDirection;
    private Vector3 _dashVelocity;
    private Vector3 _dashBufferedForces;
    // private Vector3 _externalVelocity;

    // [SerializeField] private float minMagicSpeed;
    
    // Public Properties
    public bool IsGrounded => _state.Grounded;
    public bool IsWalking => _state.Grounded && _state.Stance == Stance.Stand && _requestedMovement.sqrMagnitude > 0.01f;
    public Stance CurrentStance => _state.Stance;
    public Vector3 CurrentVelocity => motor.Velocity;
    public CharacterState CurrentState => _state;

    public void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    public void Initialize()
    {
        motor.CharacterController = this;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.Rotation;

        _requestedMovement = new Vector3(input.Move.x, 0, input.Move.y);
        _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
        _requestedMovement = input.Rotation * _requestedMovement;


        var wasRequestingJump = _requestedJump;
        _requestedJump = _requestedJump || input.Jump;
        if(_requestedJump && !wasRequestingJump)
        {
            _timeSinceJumpRequested = 0f;
        }

        _requestedJumpSustain = input.JumpSustain;
        
        var previousCrouch = _requestedCrouch;
        _requestedCrouch = input.Crouch switch
        {
            CrouchInput.Toggle => !_requestedCrouch,
            CrouchInput.None => _requestedCrouch,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        // Track when crouch is newly pressed
        if (_requestedCrouch && !previousCrouch)
        {
            _crouchJustPressed = true;
        }
        
        _requestedDash = _requestedDash || input.Dash;
    }

    public void UpdateBody(float deltaTime)
    {
        var currentHeight = motor.Capsule.height;
        var normalizedHeight = currentHeight / standHeight;
        var cameraTargetHeight = currentHeight * 
        (
            _state.Stance == Stance.Stand ? standCameraTargetHeight : crouchCameraTargetHeight
        );
        var rootTargetScale = new Vector3(1, normalizedHeight, 1);

        cameraTarget.localPosition = Vector3.Lerp
        (
            a: cameraTarget.localPosition, 
            b: new Vector3(cameraTarget.localPosition.x, cameraTargetHeight, cameraTarget.localPosition.z),
            t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
        );
        root.localScale = Vector3.Lerp
        (
            a: root.localScale, 
            b: rootTargetScale, 
            t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
        );
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        // _state.Acceleration = Vector3.zero;

        if (motor.GroundingStatus.IsStableOnGround)
        {
            _timeSinceUngrounded = 0f;
            _ungroundedDueToJump = false;
            
            // Clear crouch just pressed when grounded to prevent false fast fall triggers
            _crouchJustPressed = false;

            var groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;


            // start slide
            {
                var moving = groundedMovement.sqrMagnitude > 0f;
                var crouching = _state.Stance == Stance.Crouch;
                var wasStanding = _lastState.Stance == Stance.Stand;
                var wasInAir = _lastState.Grounded == false;
                if (moving && crouching && (wasStanding || wasInAir))
                {
                    Debug.Log("Start Slide");
                    _state.Stance = Stance.Slide;

                    var slideSpeed = Mathf.Max(currentVelocity.magnitude, slideStartSpeed);
                    currentVelocity = motor.GetDirectionTangentToSurface
                    (
                        direction: currentVelocity,
                        surfaceNormal: motor.GroundingStatus.GroundNormal
                    ) * slideSpeed;
                }
            }

            //Move
            if (_state.Stance == Stance.Stand || _state.Stance == Stance.Crouch)
            {
                var speed = _state.Stance == Stance.Stand ? walkSpeed : crouchSpeed;
                var response = _state.Stance == Stance.Stand ? walkResponse : crouchResponse;

                var targetVelocity = groundedMovement * speed;

                
                var moveVelocity = Vector3.Lerp
                (
                    a: currentVelocity,
                    b: targetVelocity,
                    t: 1f - Mathf.Exp(-response * deltaTime)
                );
                // _state.Acceleration = (moveVelocity - currentVelocity);
                currentVelocity = moveVelocity;
            }
            //continue slide
            else
            {
                currentVelocity -= currentVelocity * (slideFriction * deltaTime);

                // slope
                {
                    var force = Vector3.ProjectOnPlane
                    (
                        vector: -motor.CharacterUp,
                        planeNormal: motor.GroundingStatus.GroundNormal
                    ) * slideGravity;

                    currentVelocity -= force * deltaTime;
                }

                //steer
                {
                    var currentSpeed = currentVelocity.magnitude;
                    var targetVelocity = groundedMovement * currentSpeed;
                    var steerForce = (targetVelocity - currentVelocity) * slideSteerAcceleration * deltaTime;
                    currentVelocity += steerForce;
                    currentVelocity = Vector3.ClampMagnitude(currentVelocity, currentSpeed);
                }

                if (currentVelocity.magnitude <= slideEndSpeed)
                {
                    _state.Stance = Stance.Crouch;
                }
            }
        }
        else
        {
            _timeSinceUngrounded += deltaTime;
            // Fast fall when crouch is pressed in air (only on new press)
            if (_crouchJustPressed)
            {
                Debug.Log("Fast Fall");
                var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                currentVelocity += motor.CharacterUp * (-fastFallSpeed - currentVerticalSpeed);
                _crouchJustPressed = false;
            }

            if(_requestedMovement.sqrMagnitude > 0f)
            {
                var planarMovement = Vector3.ProjectOnPlane(_requestedMovement, motor.CharacterUp).normalized * _requestedMovement.magnitude;
                var currentPlanarVelocity = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);
                var movementForce = planarMovement * airAcceleration * deltaTime;

                if(currentPlanarVelocity.magnitude < airSpeed)
                {
                    var targetPlanarVelocity = currentPlanarVelocity + movementForce;
                    targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, airSpeed);
                    movementForce = targetPlanarVelocity - currentPlanarVelocity;
                }
                else if(Vector3.Dot(currentPlanarVelocity, movementForce) > 0f)
                {
                    var constrainedMovementForce = Vector3.ProjectOnPlane(movementForce, currentPlanarVelocity.normalized);
                    movementForce = constrainedMovementForce;
                }

                currentVelocity += movementForce;
            }
            var effectiveGravity = gravity;
            var verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            if (_requestedJumpSustain && verticalSpeed > 0f)
            {
                effectiveGravity *= jumpSustainGravity;
            }
            currentVelocity += motor.CharacterUp * effectiveGravity * deltaTime;
        }

        if (_requestedJump)
        {
            var grounded = motor.GroundingStatus.IsStableOnGround;
            var canCayoteJump = _timeSinceUngrounded < coyoteTime && !_ungroundedDueToJump;

            if(grounded || canCayoteJump)
            {
                if(!grounded)
                {
                    Debug.Log("Coyote Jump");
                }
                _requestedJump = false;
                _requestedCrouch = false;
                motor.ForceUnground(time: 0.1f);
                _ungroundedDueToJump = true;

                var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, jumpSpeed);
                currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
                
                OnJump?.Invoke();

                // Apply forward boost if jumped within early jump window
                if (_timeSinceGrounded < earlyJumpWindow || _jumpBuffered)
                {
                    Debug.Log("Early Jump Boost");
                    var forward = Vector3.ProjectOnPlane(_requestedRotation * Vector3.forward, motor.CharacterUp).normalized;
                    currentVelocity += forward * earlyJumpForwardBoost;
                    if(_jumpBuffered) _jumpBuffered = false;
                    OnMagicJump?.Invoke();
                    // GetComponent<PlayerPlatformer_New>().MagicJump();
                }
            }
            else
            {
                //jump buffer
                _timeSinceJumpRequested += deltaTime;
                _jumpBuffered = _timeSinceJumpRequested < coyoteTime;
                _requestedJump = _jumpBuffered;
            }
        }
        
        // Handle dash
        if (_state.Stance == Stance.Dash)
        {
            _dashTimer += deltaTime;
            
            if (_dashTimer >= dashDuration)
            {
                // Apply any accumulated buffered forces from during the dash
                currentVelocity = _dashBufferedForces;
                _dashBufferedForces = Vector3.zero;
                _state.Stance = Stance.Stand;
                Debug.Log("End Dash");
            }
            else
            {
                _dashVelocity = _dashDirection * dashSpeed;
                currentVelocity = _dashVelocity + _dashBufferedForces;
            }
        }
        
        if(_requestedDash && _state.Stance != Stance.Dash)
        {
            Debug.Log("Dash");
            _requestedDash = false;
            _state.Stance = Stance.Dash;
            _dashTimer = 0f;
            
            var desiredDashDirection = (_requestedRotation * Vector3.forward).normalized;
            // var currentPlanarVelocity = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);
            // var currentSpeed = currentPlanarVelocity.magnitude;
            
            // Blend dash speed based on direction alignment when moving faster than dash speed
            // if (currentSpeed > dashSpeed)
            // {
            //     var directionDot = Vector3.Dot(desiredDashDirection, currentPlanarVelocity.normalized);
            //     // directionDot ranges from -1 (opposite) to 1 (same direction)
            //     // When same direction (1), use current speed; when different (0 or negative), use min speed
            //     var blendFactor = Mathf.Max(0f, directionDot);
            //     var effectiveDashSpeed = Mathf.Lerp(dashMinSpeed, currentSpeed, blendFactor);
            //     dashSpeed = effectiveDashSpeed;  
            // }
            
            _dashDirection = desiredDashDirection;
            _dashVelocity = Vector3.zero;
            _dashBufferedForces = Vector3.zero;
            motor.ForceUnground(0.1f);
        }

        // Apply external forces
        if(_externalForce != Vector3.zero){
            // if(_state.Stance == Stance.Dash)
            // {
            //     // Accumulate buffered forces during dash to apply when dash ends
            //     _dashBufferedForces += _externalForce;
            // }
            // else
            // {
            //     _externalVelocity += _externalForce;
            // }
            currentVelocity += _externalForce;
            _externalForce = Vector3.zero;
            motor.ForceUnground(0.1f);
        }
        
        // Apply external velocity universally across all states (except dash)
        // if (_state.Stance != Stance.Dash && _externalVelocity.sqrMagnitude > 0.01f)
        // {
        //     currentVelocity += _externalVelocity;
        //     Debug.Log("Applying External Velocity: "+_externalVelocity);
        //     // Apply friction to external velocity based on state
        //     _externalVelocity -= _externalVelocity * (externalForceFriction * deltaTime);
            
        //     if (_externalVelocity.sqrMagnitude < 0.01f)
        //     {
        //         _externalVelocity = Vector3.zero;
        //     }
        // }

        // Debug.Log(_requestedCrouch);
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        // Debug.Log("Updating Rotation"+_requestedRotation);
        var forward = Vector3.ProjectOnPlane(_requestedRotation * Vector3.forward, motor.CharacterUp).normalized;
        currentRotation = Quaternion.LookRotation(forward, motor.CharacterUp);
    }


    public void BeforeCharacterUpdate(float deltaTime)
    {
        _tempState = _state;
        if(_requestedCrouch && _state.Stance == Stance.Stand)
        {
            _state.Stance = Stance.Crouch;
            motor.SetCapsuleDimensions(
                height: crouchHeight, 
                radius: motor.Capsule.radius,
                yOffset: crouchHeight * 0.5f
            );
        }
    }
    public void AfterCharacterUpdate(float deltaTime)
    {
        if(!_requestedCrouch && _state.Stance == Stance.Crouch)
        {
            _state.Stance = Stance.Stand;
            motor.SetCapsuleDimensions(
                height: standHeight, 
                radius: motor.Capsule.radius,
                yOffset: standHeight * 0.5f
            );
        }

        var wasGrounded = _state.Grounded;
        _state.Grounded = motor.GroundingStatus.IsStableOnGround;
        
        // Reset timer when landing
        if (!wasGrounded && _state.Grounded)
        {
            _timeSinceGrounded = 0f;
            OnLand?.Invoke();
        }
        else if (_state.Grounded)
        {
            _timeSinceGrounded += deltaTime;
        }
        
        _lastState = _tempState;
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider){}

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport){}

    public void PostGroundingUpdate(float deltaTime)
    {
        if(!motor.GroundingStatus.IsStableOnGround && _state.Stance == Stance.Slide)
        {
            _state.Stance = Stance.Crouch;
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport){}

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     AddForce(testForce);
        // }
    }

    public void SetTransform(Vector3 position, Quaternion? rotation = null)
    {
        motor.SetPosition(position);
        if (rotation.HasValue)
        {
            motor.SetRotation(rotation.Value);
        }
        motor.BaseVelocity = Vector3.zero;
        _state.Stance = Stance.Stand;
    }

    public void AddForce(Vector3 force)
    {
        _externalForce += force;
    }
}
