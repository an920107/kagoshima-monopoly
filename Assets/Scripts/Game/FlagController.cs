using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {

    [SerializeField] private GameObject surface;

    void Start() {
        transform.localPosition = new Vector3(0.33f, 0.57f, -0.33f);
        transform.Rotate(0f, 180f, 0f);
    }

    public Color SurfaceColor {
        get => surface.GetComponent<Renderer>().material.color;
        set => surface.GetComponent<Renderer>().material.color = new Color(value.r, value.g, value.b, 0.7f);
    }
}
