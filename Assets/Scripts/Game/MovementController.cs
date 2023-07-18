using UnityEngine;

public class MotivationController : MonoBehaviour {
    public Vector3 InitialPosition { get; private set; }

    private bool isMoving = false;
    private Vector3 departure;
    private Vector3 destination;
    private Vector3 acceleration;
    private Vector3 valocity;
    private float time;
    private float movedTime;

    void Start() {
        InitialPosition = gameObject.transform.position;
    }

    void Update() {
        if (isMoving) {
            Vector3 current = gameObject.transform.position;
            if (movedTime * 2 < time)
                valocity += acceleration * Time.deltaTime;
            else
                valocity -= acceleration * Time.deltaTime;
            gameObject.transform.position += valocity * Time.deltaTime;
            movedTime += Time.deltaTime;
            if (movedTime > time || (current - departure).magnitude > (destination - departure).magnitude) {
                gameObject.transform.position = destination;
                isMoving = false;
            }
        }
    }

    public void MoveTo(Vector3 destination, float time) {
        departure = gameObject.transform.position;
        this.destination = destination;
        this.time = time;
        acceleration = (destination - departure) * 4 / time / time;
        movedTime = 0f;
        valocity = Vector3.zero;
        isMoving = true;
    }

    public void MoveToInitialPosition(float time) {
        MoveTo(InitialPosition, time);
    }
}
