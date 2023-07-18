using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchingController : MonoBehaviour {
    [SerializeField] private GameObject current;
    [SerializeField] private GameObject next;

    public void Switch() {
        if (next != null)
            next.SetActive(true);
        current.SetActive(false);
    }
}
