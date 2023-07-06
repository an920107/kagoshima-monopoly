using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        Throw();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            Throw();
    }

    public void Throw() {
        gameObject.transform.rotation = Random.rotation;
        gameObject.transform.position = new Vector3(0f, 40f, -80f);
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(0f, 0f, 150f);
    }
}
