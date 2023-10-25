using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour {

    [SerializeField] private GameObject buyLand;
    [SerializeField] private GameObject buyFacility;
    [SerializeField] private GameObject noMoney;
    [SerializeField] private GameObject payMoney;
    [SerializeField] private GameObject status;
    [SerializeField] private GameObject salary;

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

    public void ShowStatus(
            List<BlockController> blockControllers,
            List<PlayerController> playerControllers,
            out StatusController controller) {

        gameObject.SetActive(true);
        prefabs.Push(Instantiate(status, transform));
        controller = prefabs.Peek().GetComponent<StatusController>();
        controller.SetUp(blockControllers, playerControllers);
    }

    public void ShowSalary(
        BlockController blockController,
        PlayerController playerController,
        out SalaryController controller) {
        
        gameObject.SetActive(true);
        prefabs.Push(Instantiate(salary, transform));
        controller = prefabs.Peek().GetComponent<SalaryController>();
        controller.SetUp(blockController, playerController);
    }
}
