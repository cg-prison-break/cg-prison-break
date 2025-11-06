using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")] public float MoveSpeed = 4.0f;
    public float SprintSpeed = 6.0f;
    public float RotationSpeed = 1.0f;
    public float SpeedChangeRate = 10.0f;

    [Space(10)] public float JumpHeight = 1.2f;
    public float Gravity = -9.81f;

    [Space(10)] public float JumpTimeout = 0.1f;
    public float FallTimeout = 0.2f;

    [Header("Player Grounding")] public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 90.0f;
    public float BottomClamp = -90.0f;
    
    
    private float _cineMachineTargetPitch;

    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 50.0f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


    private PlayerInput _playerInput;
    private CharacterController _controller;
    private PlayerController _input;
    private GameObject _mainCamera;


    private const float _threshold = 0.01f;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KB and Mouse";
#else
                return false;
#endif
        }
    }

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerController>();
        _playerInput = GetComponent<PlayerInput>();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    // Update is called once per frame
    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void CameraRotation()
    {
        if (_input.look.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            
            _cineMachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;
            
            // clamp vertical max
            _cineMachineTargetPitch = ClampAngle(_cineMachineTargetPitch, BottomClamp, TopClamp);
            
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cineMachineTargetPitch, 0.0f, 0.0f);
            
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    
    private void Move()
    {
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
        
        
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;
        
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        
        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1.0f;
        
        
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
            
            // round to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        
        else
        {
            _speed = targetSpeed;
        }
        
        
        
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        
        
        if (_input.move != Vector2.zero)
        {
            inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
        }
        
        if (_controller.enabled)
        {
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }
    
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;
            
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2.0f;
            }
            
            
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
            }
            
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        
        else
        {
            _jumpTimeoutDelta = JumpTimeout;
            
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            
            _input.jump = false;
        }
        
        
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    
    
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}