using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    
    [System.Serializable]
    public class Movement {
        public float speed = 5.0f;
        public float acceleration = 3.0f;
        public Vector3 velocity;
        public Rigidbody rigidbody;
        public bool isGrounded;
        public bool airJumped = false;
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
    public BoolVariable jumpButtonInput;

    void Start() {
        movement.rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        Turning();
    }

    void FixedUpdate() {
        Locomotion();
        VerticalMovement();
    }

    public void AddRecoil(float r) {
        turningAndAiming.aimingDirection.x += Random.Range(-.2f, .2f) * r;
        turningAndAiming.recoilY -= r;
    }

    public void Jump(float jumpForce) {
        if(movement.isGrounded || movement.airJumped) {
            Vector3 lateralizedVelocity = movement.rigidbody.velocity;
            lateralizedVelocity.y = 0f;
            movement.rigidbody.velocity = lateralizedVelocity;
            movement.rigidbody.AddForce(Vector3.up * jumpForce);
        }
    }

    void Locomotion() {
        Vector3 targetVelocity = 
        (movementInput.x * transform.right + movementInput.y * transform.forward) * movement.speed * Time.fixedDeltaTime;
        movement.velocity = Vector3.Lerp(movement.velocity, targetVelocity, movement.acceleration * Time.fixedDeltaTime);
        movement.velocity.y = movement.rigidbody.velocity.y;
        movement.rigidbody.velocity = movement.velocity;
        //movement.controller.Move(movement.velocity);
    }

    void VerticalMovement() {

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

    //Collisions

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == ("Ground"))
        {
            movement.isGrounded = true;
            movement.airJumped = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == ("Ground"))
        {
            movement.isGrounded = false;
        }
    }

}
