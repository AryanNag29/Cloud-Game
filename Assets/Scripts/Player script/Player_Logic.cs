using System.Diagnostics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;


public class Player_Logic : MonoBehaviour
{
    [SerializeField] CharacterController controls;
    public PlayerInput playerinput;

    //variables
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;

    void Awake()
    {
        controls = GetComponent<CharacterController>();
        playerinput = new PlayerInput();
        playerinput.CharacterControls.Move.started += Context =>
        {   
            //movement of character
            currentMovementInput = Context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.y = currentMovementInput.y;
        };
    }

    // Update is called once per frame
    void Update()
    {

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
