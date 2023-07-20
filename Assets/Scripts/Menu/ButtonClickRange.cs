using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickRange : MonoBehaviour {

    [SerializeField] private float alphaThreshold = 0.1f;

    void Start() {
        GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }
}
