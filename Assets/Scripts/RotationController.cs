using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public Quaternion initialRotation { get; private set; }

    private bool isRotating = false;
    private Vector3 axis;
    private Quaternion departure;
    private float deltaDegree;
    private float acceleration;
    private float valocity;
    private float time;
    private float rotatedTime;
    private float rotatedDegree;

    void Start()
    {
        initialRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        if (isRotating)
        {
            if (rotatedTime * 2 < time)
                valocity += acceleration * Time.deltaTime;
            else
                valocity -= acceleration * Time.deltaTime;
            gameObject.transform.Rotate(axis, valocity * Time.deltaTime);
            rotatedDegree += valocity * Time.deltaTime;
            rotatedTime += Time.deltaTime;
            if (rotatedTime > time || Math.Abs(rotatedDegree) >= Math.Abs(deltaDegree))
            {
                gameObject.transform.rotation = departure;
                gameObject.transform.Rotate(axis, deltaDegree);
                isRotating = false;
            }
        }
    }

    public void Rotate(Vector3 axis, float deltaDegree, float time)
    {
        if (isRotating)
            return;

        departure = gameObject.transform.rotation;
        this.axis = axis;
        this.deltaDegree = deltaDegree;
        this.time = time;
        acceleration = deltaDegree * 4 / time / time;
        rotatedTime = 0f;
        rotatedDegree = 0f;
        valocity = 0f;
        isRotating = true;
    }
}
