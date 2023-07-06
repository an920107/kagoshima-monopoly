using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    public bool IsStill { get; private set; }
    public int Value { get; private set; }

    private Rigidbody rb;

    void Start() {
        IsStill = true;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (!IsStill && transform.localPosition.y < 0.25f && rb.velocity.magnitude < 0.1f)
            IsStill = true;

        float maxY = float.MinValue;
        foreach (var sideTransform in GetComponentsInChildren<Transform>()) {
            if (sideTransform != gameObject.transform) {
                if (sideTransform.position.y > maxY) {
                    try {
                        Value = int.Parse(sideTransform.gameObject.name);
                    } catch {
                        continue;
                    }
                    maxY = sideTransform.position.y;
                }
            }
        }
    }

    public void Throw() {
        gameObject.transform.rotation = Random.rotation;
        gameObject.transform.position = new Vector3(
            Random.Range(-10f, 10f), Random.Range(30f, 40f), Random.Range(-70f, -80f));
        rb.velocity = Vector3.zero;
        rb.AddForce(0f, 0f, Random.Range(160f, 210f));
        IsStill = false;
    }
}
