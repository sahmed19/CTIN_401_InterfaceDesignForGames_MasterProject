using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    public Vector2Variable movementInput;
    public Vector2Variable mouseInput;
    public BoolVariable fire1Down;
    public BoolVariable fire1Pressed;
    public BoolVariable jumpButtonDown;

    public GameEvent rightMouseDownEvent;
    public GameEvent jumpButtonDownEvent;
    
    // Update is called once per frame
    void LateUpdate()
    {
        GatherInput();
    }

    void GatherInput() {
        movementInput.SetValue(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);
        mouseInput.SetValue(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
        fire1Down.SetValue(Input.GetButton("Fire1"));
        fire1Pressed.SetValue(Input.GetButtonDown("Fire1"));
        
        jumpButtonDown.SetValue(Input.GetButtonDown("Jump"));

        if(Input.GetButtonDown("Fire2")) {
            rightMouseDownEvent.Raise();
        }

        if(Input.GetButtonDown("Jump")) {
            jumpButtonDownEvent.Raise();
        }

    }
}
