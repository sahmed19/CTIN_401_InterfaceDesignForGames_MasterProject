using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    public Vector2Variable movementInput;
    public Vector2Variable mouseInput;
    public BoolVariable leftMouseDown;
    public BoolVariable leftMouseTriggered;

    public GameEvent rightMouseDown;
    
    // Update is called once per frame
    void LateUpdate()
    {
        GatherInput();
    }

    void GatherInput() {
        movementInput.SetValue(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);
        mouseInput.SetValue(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
        leftMouseDown.SetValue(Input.GetButton("Fire1"));
        leftMouseTriggered.SetValue(Input.GetButtonDown("Fire1"));

        if(Input.GetButtonDown("Fire2")) {
            rightMouseDown.Raise();
        }

    }
}
