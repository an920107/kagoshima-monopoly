using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour {

    [SerializeField] private GameObject column;
    [SerializeField] private GameObject rowPrefab;

    public event StatusCompleteEventHandler StatusComplete;

    public void SetUp(List<BlockController> blockControllers, List<PlayerController> playerControllers) {
        foreach (var pc in playerControllers)
            Instantiate(rowPrefab, column.transform).GetComponent<StatusRowController>().SetUp(
                blockControllers, pc);
    }

    public void Confirm() {
        StatusComplete?.Invoke(this, new());
    }
}
