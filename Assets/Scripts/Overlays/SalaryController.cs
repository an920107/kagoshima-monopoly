using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalaryController : MonoBehaviour {

    [SerializeField] private Text descriptionText;

    public event SalaryCompleteEventHandler SalaryComplete;

    private BlockController blockController;
    private PlayerController playerController;

    public void SetUp(BlockController blockController, PlayerController playerController) {
        this.blockController = blockController;
        this.playerController = playerController;

        descriptionText.text = descriptionText.text.Replace("{0}", $"{blockController.Data.Price:#,#}");
    }

    public void Confirm() {
        SalaryComplete?.Invoke(this, new(blockController, playerController));
    }
}
