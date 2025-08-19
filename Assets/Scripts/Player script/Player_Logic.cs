using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class Player_Logic : MonoBehaviour
{
    
    [SerializeField] CharacterController controls;
    public PlayerInput playerinput;

    //variables
    private Vector2 _Input;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private float _movementSpeed = 5;
    private float _gravity = -9.81f;
    [SerializeField] private float gravityScale = 1;
    private float _velocity;
    private float _jumpVelocity = 20f;


    //functions
    public void onMovementInput(InputAction.CallbackContext context) //for movement function
    {
        //movement of character
        _Input = context.ReadValue<Vector2>();
        _currentMovement.x = _Input.x;
        _currentMovement.y = _Input.y;
        _isMovementPressed = _Input.x != 0 || _Input.y != 0;
    }

    void applyGravity() // for gravity function
    {
        if (controls.isGrounded && _velocity<0)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * gravityScale * Time.deltaTime;
        }
        _currentMovement.y += _velocity;
    }
    

    void Awake()
    {
        controls = GetComponent<CharacterController>();
        playerinput = new PlayerInput();
        //to start the movement of character with keyboard
        playerinput.CharacterControls.Move.started += onMovementInput;
        //to stop the movement of character with keyboard
        playerinput.CharacterControls.Move.canceled += onMovementInput;
        //to start the movement of character with controller
        playerinput.CharacterControls.Move.performed += onMovementInput;
    }


    // Update is called once per frame
    void Update()
    {
        var targetAngle = Mathf.Atan2(_Input.x,_Input.y)*Mathf.Rad2Deg;
        transform.rotation = quaternion.Euler(0,targetAngle,0);
        applyGravity();
        controls.Move(_currentMovement * _movementSpeed * Time.deltaTime);
    }
    void OnEnable()
    {
        //enable player character controles
        playerinput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        //disable player character contoles
        playerinput.CharacterControls.Disable();
    }
}
