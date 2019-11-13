using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    
    public GameEvent gunShot;

    public BoolReference leftMouseDown;
    public BoolReference leftMouseTriggered;

    public IntVariable equippedAmmo;
    public IntVariable reserveAmmo;

    public Vector2Variable movementInput;

    public FloatVariable accuracyModifier;

    public float fireRate = .1f;
    public bool fireModeAuto = false;
    public GameObject muzzle;
    public LayerMask shootableMask;

    private float timer = 0f;

    Camera mainCamera;
    public GameObject testHole;

    float recoilAccuracyModifier;

    void Start() {
        mainCamera = Camera.main;
    }

    void Update() {
        CheckForShot();
        AccuracyCheck();
    }

    void CheckForShot() {

        timer += Time.deltaTime;

        if(timer > fireRate && equippedAmmo > 0 &&
        (   (!fireModeAuto && leftMouseDown) ||
            (fireModeAuto && leftMouseTriggered))) {
            gunShot.Raise();

        }

    }

    void AccuracyCheck() {
        recoilAccuracyModifier = Mathf.Lerp(recoilAccuracyModifier, 0f, Time.deltaTime * 5.0f);
        accuracyModifier.value = .5f * movementInput.sqrMagnitude + recoilAccuracyModifier;
    }

    public void Shoot() {
        
        recoilAccuracyModifier += .5f;

        RaycastHit shootHit;

        Vector3 recoilOffset = (transform.right * Random.Range(-1f, 1f) + transform.up * Random.Range(-1f, 1f)) * accuracyModifier * .2f;

        if(Physics.Raycast(transform.position + recoilOffset, transform.forward, out shootHit, 10f, shootableMask.value)) {
            testHole.transform.position = shootHit.point;
        }
        
        timer = 0f;
        equippedAmmo--;
    }

}
