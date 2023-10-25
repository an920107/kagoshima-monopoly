using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailController : MonoBehaviour {

    public event JailCompleteEventHandler JailComplete;

    private PlayerController playerController;

    public void SetUp(PlayerController playerController) {
        this.playerController = playerController;
    }

    public void Confirm() {
        JailComplete?.Invoke(this, new(playerController));
    }
}
