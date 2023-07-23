using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int AtBlock { get; set; }

    private Queue<Vector3> dest = new();

    public Color SkinColor {
        get => GetComponentInChildren<Renderer>().material.color;
        set => GetComponentInChildren<Renderer>().material.color = value;
    }

    void Awake() {
        AtBlock = 0;
    }

    void Start() {

    }

    void FixedUpdate() {

    }

    void Update() {

    }

    void OnCollisionEnter(Collision collision) {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<PlayerController>() == null && GetComponent<Rigidbody>().velocity.y < 0) {
            GetComponent<MeshCollider>().isTrigger = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (dest.Count > 0) {
                gameObject.transform.position = dest.Peek();
                dest.Dequeue();
                Jump(dest);
            }
        }
    }

    public void Jump(Queue<Vector3> dest) {
        this.dest = dest;
        if (dest.Count == 0)
            return;
        GetComponent<MeshCollider>().isTrigger = true;
        Vector3 displacement = dest.Peek() - gameObject.transform.position;

        float t = 0.5f, g = Physics.gravity.y;
        GetComponent<Rigidbody>().AddForce(
            displacement.x / t, -g * t * t, displacement.z / t, ForceMode.VelocityChange);
    }
}
