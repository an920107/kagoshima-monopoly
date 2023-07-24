using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour {

    [SerializeField] private GameObject buyLand;
    [SerializeField] private GameObject buyFacility;
    [SerializeField] private GameObject noMoney;
    [SerializeField] private GameObject payMoney;

    private readonly Stack<GameObject> prefabs = new();

    public void OnDisable() {
        Clear();
    }

    public void Clear() {
        while (prefabs.Count > 0)
            Destroy(prefabs.Pop());
    }

    public void ShowBuyLand(
                BlockController blockController,
                PlayerController playerController,
                out BuyLandController controller) {

        gameObject.SetActive(true);
        prefabs.Push(Instantiate(buyLand, transform));
        controller = prefabs.Peek().GetComponent<BuyLandController>();
        controller.SetUp(blockController, playerController);
    }

    public void ShowBuyFacility(
                BlockController blockController,
                PlayerController playerController,
                out BuyFacilityController controller) {

        gameObject.SetActive(true);
        prefabs.Push(Instantiate(buyFacility, transform));
        controller = prefabs.Peek().GetComponent<BuyFacilityController>();
        controller.SetUp(blockController, playerController);
    }

    public void ShowNoMoney(out NoMoneyController controller) {
        gameObject.SetActive(true);
        prefabs.Push(Instantiate(noMoney, transform));
        controller = prefabs.Peek().GetComponent<NoMoneyController>();
    }

    public void ShowPayMoney(
            BlockController blockController,
            PlayerController fromPlayerController,
            PlayerController toPlayerController,
            out PayMoneyController controller) {

        gameObject.SetActive(true);
        prefabs.Push(Instantiate(payMoney, transform));
        controller = prefabs.Peek().GetComponent<PayMoneyController>();
        controller.SetUp(blockController, fromPlayerController, toPlayerController);
    }
}
