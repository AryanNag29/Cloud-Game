using System.Diagnostics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Player_Logic : MonoBehaviour
{
    [SerializeField] CharacterController controls;
    public PlayerInput playerinput;

    //variables
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private float _movementSpeed = 5;
    private float _gravity = -9.81f;
    [SerializeField] private float gravityScale = 1;
    private float _velocity;


    //functions
    void onMovementInput(InputAction.CallbackContext Context) //for movement function
    {
        //movement of character
        _currentMovementInput = Context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.y = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
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
