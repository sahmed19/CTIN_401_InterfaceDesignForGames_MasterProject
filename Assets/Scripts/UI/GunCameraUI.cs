using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCameraUI : MonoBehaviour
{

    Camera gunCamera;

    float kick = 0f;
    
    void Start() {
        DisableCursor();
        gunCamera = GetComponent<Camera>();
    }

    void Update() {
        FOVKicker();
    }

    public void Shoot(float fovKick) {
        kick += fovKick;
    }

    void FOVKicker() {
        gunCamera.fieldOfView = 90f + kick;
        kick = Mathf.Lerp(kick, 0f, Time.deltaTime * 5.0f);
    }

    void DisableCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
