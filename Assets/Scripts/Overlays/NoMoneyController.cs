using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoneyController : MonoBehaviour {

    public event NoMoneyCompleteEventHandler NoMoneyComplete;

    public void Confirm() {
        NoMoneyComplete?.Invoke(this, new());
    }
}
