using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusRowController : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private Text playerName;
    [SerializeField] private Text money;
    [SerializeField] private GameObject row;
    [SerializeField] private GameObject itemPrefab;

    public void SetUp(List<BlockController> blockControllers, PlayerController playerController) {
        player.GetComponent<Renderer>().material.color = playerController.SkinColor;
        playerName.text = $"ª±®a {playerController.Number + 1}";
        money.text = $"¢D{playerController.Money:#,#}";
        foreach (var landIndex in playerController.Lands.Keys)
            Instantiate(itemPrefab, row.transform).GetComponent<StatusItemController>().SetUp(
                blockControllers[landIndex]);
        foreach (var facilityIndex in playerController.Facilities)
            Instantiate(itemPrefab, row.transform).GetComponent<StatusItemController>().SetUp(
                blockControllers[facilityIndex]);
    }
}
