using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    
    [System.Serializable]
    public class Movement {
        public float speed = 5.0f;
        public float acceleration = 3.0f;
        public Vector3 velocity;
        public CharacterController controller;
    }

    [System.Serializable]
    public class TurningAndAiming {
        public Camera playerCamera;
        [Range(10.0f, 100.0f)]
        public float sensitivity = 50.0f;
        public Vector2 sensitivityMultiplier;
        [Range(45.0f, 90.0f)]
        public float range = 90f;
        public bool invertY;
        public Vector2 aimingDirection;
        public float recoilY = 0f;
    }

    public Movement movement;
    public TurningAndAiming turningAndAiming;

    public Vector2Variable movementInput;
    public Vector2Variable mouseInput;

    void Start() {
        movement.controller = GetComponent<CharacterController>();
    }

    void Update() {
        Locomotion();
        Turning();
    }

    public void AddRecoil(float r) {
        turningAndAiming.aimingDirection.x += Random.Range(-.2f, .2f) * r;
        turningAndAiming.recoilY -= r;
    }


    void Locomotion() {
        Vector3 targetVelocity = 
        (movementInput.x * transform.right + movementInput.y * transform.forward) * movement.speed * Time.deltaTime;
        movement.velocity = Vector3.Lerp(movement.velocity, targetVelocity, movement.acceleration * Time.deltaTime);
        movement.controller.Move(movement.velocity);
    }

    void Turning() {
        //Set Turning Values
        float turnAmountX = 
            mouseInput.x * turningAndAiming.sensitivity * turningAndAiming.sensitivityMultiplier.x * Time.deltaTime;
        float turnAmountY = (turningAndAiming.invertY? 1f : -1f) * 
            mouseInput.y * turningAndAiming.sensitivity * turningAndAiming.sensitivityMultiplier.y * Time.deltaTime;
        
        turningAndAiming.aimingDirection.x += turnAmountX;
        turningAndAiming.aimingDirection.y = Mathf.Clamp(turningAndAiming.aimingDirection.y + turnAmountY, -turningAndAiming.range, turningAndAiming.range);
        
        //X - Player Object
        transform.localRotation = 
            Quaternion.Euler(turningAndAiming.aimingDirection.x * Vector3.up);

        //Y - Camera Object

        turningAndAiming.playerCamera.transform.localRotation = 
            Quaternion.Euler((turningAndAiming.aimingDirection.y + turningAndAiming.recoilY) * Vector3.right);

        //Recoil
        turningAndAiming.recoilY = Mathf.Lerp(turningAndAiming.recoilY, 0f, Time.deltaTime * 2.0f);

    }

}
