using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSODAVector3ToPosition : MonoBehaviour
{
    
    public Vector3Variable targetVector3;

    void LateUpdate() {
        targetVector3.value = transform.position;
    }

}
