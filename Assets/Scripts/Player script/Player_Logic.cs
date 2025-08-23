using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class Player_Logic : MonoBehaviour
{

    [SerializeField] CharacterController controls;
    public PlayerInput playerinput;

    //variables
    [Header("Character Movement")]
    //movement variable
    private Vector2 _Input;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private float _movementSpeed = 5;
    [Header("Gravity and Jump")]
    //gravity variable
    private float _gravity = -9.8f;
    private float _groundedVelocity = -0.05f;
    [SerializeField] private float fallMultiplier = 1.7f;
    private float _velocity;
    //jump variable
    private bool isJumpPressed = false;
    private float _jumpVelocity = 20f;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 3f;
    private float _maxJumpTIme = 0.5f;
    private bool _isjumping = false;


    #region OnMovement

    //functions
    public void onMovement(InputAction.CallbackContext context) //for movement function
    {
        //movement of character
        _Input = context.ReadValue<Vector2>();
        _currentMovement.x = _Input.x;
        _currentMovement.y = _Input.y;
        _isMovementPressed = _Input.x != 0 || _Input.y != 0;
    }

    #endregion


    #region OnJump
    private void setupJumpVariable()
    {
        float timeToApex = _maxJumpTIme / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
    }

    private void handleJumping()
    {
        if (!_isjumping && controls.isGrounded && isJumpPressed)
        {
            _isjumping = true;
            _currentMovement.y = _initialJumpVelocity * 0.5f;
        }
        else if (!isJumpPressed && _isjumping && controls.isGrounded)
        {
            _isjumping = false;
        }

    }

    public void onJump(InputAction.CallbackContext context)
    {
        //jump movement read
        isJumpPressed = context.ReadValueAsButton();
        UnityEngine.Debug.Log(isJumpPressed);
    }

    #endregion

    #region Gravity

    void applyGravity() // for gravity function
    {
        if (controls.isGrounded)
        {
            _currentMovement.y += _groundedVelocity;
        }
        else
        {
            float previousYVelocity = _currentMovement.y;
            float newYVelocity = _currentMovement.y + (_gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f; // mathf.pow for the gravity because gravity always change so it need to multiply with delta time twice like 9.81 m/s^2
            _currentMovement.y = nextYVelocity;
        }
    }

    #endregion


    #region Awake 

    void Awake()
    {
        controls = GetComponent<CharacterController>();
        playerinput = new PlayerInput();
        //to start the movement of character with keyboard
        playerinput.CharacterControls.Move.started += onMovement;
        //to stop the movement of character with keyboard
        playerinput.CharacterControls.Move.canceled += onMovement;
        //to start the movement of character with controller
        playerinput.CharacterControls.Move.performed += onMovement;

        //player jump read 
        playerinput.CharacterControls.Jump.started += onJump;
        playerinput.CharacterControls.Jump.canceled += onJump;
        setupJumpVariable();
    }

    #endregion


    #region Update

    // Update is called once per frame
    void Update()
    {
        var targetAngle = Mathf.Atan2(_Input.x, _Input.y) * Mathf.Rad2Deg;
        transform.rotation = quaternion.Euler(0, targetAngle, 0);
        applyGravity();
        handleJumping();
        controls.Move(_currentMovement * _movementSpeed * Time.deltaTime);
    }

    #endregion


    #region  OnEnable

    void OnEnable()
    {
        //enable player character controles
        playerinput.CharacterControls.Enable();
    }

    #endregion


    #region OnDisable

    void OnDisable()
    {
        //disable player character contoles
        playerinput.CharacterControls.Disable();
    }

    #endregion

}
