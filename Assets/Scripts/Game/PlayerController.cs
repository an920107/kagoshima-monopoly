using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] AudioClip collisionAudio;

    public event PlayerArrivedEventHandler PlayerArrived;

    public int Number { get; set; }
    public int AtBlock { get; set; }
    public int Money { get; set; }
    public Dictionary<int, int> Lands { get; set; }
    public List<int> Facilities { get; set; }
    public int StepLeft { get; set; }
    public bool IsPause { get; set; }

    private AudioSource audioSource;
    private Queue<Vector3> dest = new();

    public Color SkinColor {
        get => GetComponentInChildren<Renderer>().material.color;
        set => GetComponentInChildren<Renderer>().material.color = value;
    }

    void Awake() {
        AtBlock = 0;
        Money = 20000;
        Lands = new();
        Facilities = new();
        StepLeft = 0;
        IsPause = false;
    }

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<PlayerController>() == null && GetComponent<Rigidbody>().velocity.y < 0) {
            audioSource.PlayOneShot(collisionAudio);
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
        if (dest.Count == 0) {
            PlayerArrived?.Invoke(this, new());
            return;
        }
        GetComponent<MeshCollider>().isTrigger = true;
        Vector3 displacement = dest.Peek() - gameObject.transform.position;

        float t = 0.5f, g = Physics.gravity.y;
        GetComponent<Rigidbody>().AddForce(
            displacement.x / t, -g * t * t, displacement.z / t, ForceMode.VelocityChange);
    }
}
