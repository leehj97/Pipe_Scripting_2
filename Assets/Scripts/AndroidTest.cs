using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidTest : MonoBehaviour
{
    private Quaternion rotateVector;
    private float plusRotate = 0.5f;
    void FixedUpdate()
    {
        rotateVector = Quaternion.Euler(new Vector3(plusRotate, plusRotate, plusRotate));
        gameObject.GetComponent<Transform>().localRotation = rotateVector;
        plusRotate += 0.5f;
    }
}
