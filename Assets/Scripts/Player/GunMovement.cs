using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    
    Vector3 lagOffset;
    Vector3 lagRotationOffset;

    public FloatReference lagAmount = new FloatReference(4.0f);
    public FloatReference lagRecoverySpeed = new FloatReference(5.0f);
    public FloatReference clamper = new FloatReference(.04f);

    Vector3 swayOffset;
    Vector3 swayRotationOffset;

    public FloatReference swayAmount = new FloatReference(.7f);
    public FloatReference swaySpeed = new FloatReference(9.0f);
    public FloatReference swayRecoverySpeed = new FloatReference(8.0f);

    Vector3 recoilOffset;
    Vector3 recoilRotationOffset;

    public FloatReference recoilAmount = new FloatReference(1.5f);
    public FloatReference recoilRecoverySpeed = new FloatReference(8f);

    public Vector2Reference mouseInput;
    public Vector2Reference movementInput;

    void Update() {
        GetPlayerInput();
        AimLag();
        MovementSway();
        Recoil();
        MoveAndRecover();
    }

    public void GetPlayerInput() {
        
    }

    public void RecoilFromShot() {
        recoilOffset += Vector3.back * recoilAmount * .01f;
        recoilRotationOffset += Vector3.right * recoilAmount * -0.5f;
    }

    void AimLag() {
        
        lagOffset += (Vector3.right * mouseInput.x + Vector3.up * mouseInput.y) * -lagAmount * .01f * Time.deltaTime;
        lagOffset = new Vector3(
            Mathf.Clamp(lagOffset.x, -clamper, clamper),
            Mathf.Clamp(lagOffset.y, -clamper, clamper),
            0f
        );
        lagRotationOffset += Vector3.forward * mouseInput.x * lagAmount * Time.deltaTime;
        lagOffset = Vector3.Lerp(lagOffset, Vector3.zero, lagRecoverySpeed * Time.deltaTime);
        lagRotationOffset = Vector3.Lerp(lagRotationOffset, Vector3.zero, lagRecoverySpeed * Time.deltaTime);
    }

    void MovementSway() {
        Vector3 targetSwayOffset = new Vector3(
            Mathf.Sin(Time.time * swaySpeed) * -.5f,
            -Mathf.Abs(Mathf.Cos(Time.time * swaySpeed))
        ) * swayAmount * .03f * Mathf.Clamp01(movementInput.sqrMagnitude);

        Vector3 targetSwayRotationOffset = Vector3.forward * 
        Mathf.Sin(Time.time * swaySpeed) * swayAmount * 3f * Mathf.Clamp01(movementInput.sqrMagnitude);

        swayOffset = Vector3.Lerp(swayOffset, targetSwayOffset, swayRecoverySpeed * Time.deltaTime);
        swayRotationOffset = Vector3.Lerp(swayRotationOffset, targetSwayRotationOffset, swayRecoverySpeed * Time.deltaTime);
    }

    void Recoil() {
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, recoilRecoverySpeed * Time.deltaTime);
        recoilRotationOffset = Vector3.Lerp(recoilRotationOffset, Vector3.zero, recoilRecoverySpeed * Time.deltaTime);
    }

    void MoveAndRecover() {
        transform.localPosition = Vector3.zero + lagOffset + swayOffset + recoilOffset;
        Vector3 euler = Vector3.zero + recoilRotationOffset + swayRotationOffset + lagRotationOffset;
        transform.localRotation = Quaternion.Euler(euler);
    }

}
