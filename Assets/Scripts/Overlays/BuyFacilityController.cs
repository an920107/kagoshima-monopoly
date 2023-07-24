using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFacilityController : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text tollsText;

    public event BuyFacilityCompleteEventHandler BuyFacilityComplete;

    private BlockController blockController;
    private PlayerController playerController;

    public void SetUp(BlockController blockController, PlayerController playerController) {
        this.blockController = blockController;
        this.playerController = playerController;

        titleText.text = blockController.Data.Title;
        priceText.text = $"¢D{blockController.Data.Price:#,#}";
        tollsText.text = string.Join("\n", blockController.Data.Tolls.ConvertAll<string>(
            new(x => $"¢D{x:#,#}")));
    }

    public void Confirm() {
        BuyFacilityComplete?.Invoke(this, new(true, blockController, playerController));
    }

    public void Cancel() {
        BuyFacilityComplete?.Invoke(this, new(false));
    }
}
