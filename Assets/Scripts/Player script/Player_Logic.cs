using System.Diagnostics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Logic : MonoBehaviour
{
    [SerializeField] CharacterController controls;
    public PlayerInput playerinput;

    //variables
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    float movementSpeed = 5;

    //functions
    void onMovementInput(InputAction.CallbackContext Context)
    {
        //movement of character
        currentMovementInput = Context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.y = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
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
        controls.Move(currentMovement *movementSpeed * Time.deltaTime);
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
