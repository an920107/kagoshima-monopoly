using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotivationController : MonoBehaviour
{
    public Vector3 initialPosition { get; private set; }

    private bool isMoving = false;
    private Vector3 departure;
    private Vector3 destination;
    private Vector3 acceleration;
    private Vector3 valocity;
    private float time;
    private float movedTime;


    void Start()
    {
        initialPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 current = gameObject.transform.position;
            if (movedTime * 2 < time)
                valocity += acceleration * Time.deltaTime;
            else
                valocity -= acceleration * Time.deltaTime;
            gameObject.transform.position += valocity * Time.deltaTime;
            movedTime += Time.deltaTime;
            if ((current - departure).magnitude > (destination - departure).magnitude)
            {
                gameObject.transform.position = destination;
                isMoving = false;
            }
        }
    }

    public void Move(Vector3 destination, float time)
    {
        departure = gameObject.transform.position;
        this.destination = destination;
        this.time = time;
        acceleration = (destination - departure) * 4 / time / time;
        movedTime = 0f;
        valocity = Vector3.zero;
        isMoving = true;
    }

    public void Move(float time)
    {
        Move(initialPosition, time);
    }
}
