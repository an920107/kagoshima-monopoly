using System;
using UnityEngine;

public class RotationController : MonoBehaviour {
    public Quaternion InitialRotation { get; private set; }

    private bool isRotating = false;
    private Vector3 axis;
    private Quaternion departure;
    private float angle;
    private float acceleration;
    private float valocity;
    private float time;
    private float rotatedTime;
    private float rotatedAngle;

    void Start() {
        InitialRotation = gameObject.transform.rotation;
    }

    void Update() {
        if (isRotating) {
            if (rotatedTime * 2 < time)
                valocity += acceleration * Time.deltaTime;
            else
                valocity -= acceleration * Time.deltaTime;
            gameObject.transform.Rotate(axis, valocity * Time.deltaTime);
            rotatedAngle += valocity * Time.deltaTime;
            rotatedTime += Time.deltaTime;
            if (rotatedTime > time || Math.Abs(rotatedAngle) >= Math.Abs(angle)) {
                gameObject.transform.rotation = departure;
                gameObject.transform.Rotate(axis, angle);
                isRotating = false;
            }
        }
    }

    public void Rotate(Vector3 axis, float angle, float time, bool animation = true) {
        if (isRotating)
            return;

        departure = gameObject.transform.rotation;
        this.axis = axis;
        this.angle = angle;
        this.time = time;
        acceleration = animation ? (angle * 4 / time / time) : 0;
        rotatedTime = 0f;
        rotatedAngle = 0f;
        valocity = 0f;
        isRotating = true;
    }
}
