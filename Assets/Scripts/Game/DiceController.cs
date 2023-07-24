using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    [SerializeField] private AudioClip collisionAudio;
    [SerializeField] private float audioCdTime = 0.5f;

    public bool IsStill { get; private set; }
    public int Value { get; private set; }

    private AudioSource audioSource;
    private Rigidbody rb;
    private float currentCdTime = 0;

    void Awake() {
        IsStill = true;
    }

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Throw();
    }

    void FixedUpdate() {
        if (!IsStill && transform.localPosition.y < 0.4f && rb.velocity.magnitude < 0.6f)
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

        if (currentCdTime > 0f)
            currentCdTime -= Time.deltaTime;

    }

    void OnCollisionEnter(Collision collision) {
        if (currentCdTime <= 0f && rb.velocity.magnitude > 2f) {
            audioSource.PlayOneShot(collisionAudio);
            currentCdTime = audioCdTime;
        }
    }

    public void Throw() {
        gameObject.transform.rotation = Random.rotation;
        gameObject.transform.position = new Vector3(
            Random.Range(-10f, 10f), Random.Range(30f, 40f), Random.Range(-70f, -80f));
        rb.velocity = Vector3.zero;
        rb.AddForce(0f, 0f, Random.Range(160f, 190f));
        rb.AddTorque(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
        IsStill = false;
    }
}
