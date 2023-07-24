using OverlayEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayMoneyController : MonoBehaviour {

    [SerializeField] private Text descriptionText;

    public event PayMoneyCompleteEventHandler PayMoneyComplete;

    private BlockController blockController;
    private PlayerController fromPlayerController;
    private PlayerController toPlayerController;

    public void SetUp(BlockController blockController, PlayerController fromPlayerController, PlayerController toPlayerController) {
        this.blockController = blockController;
        this.fromPlayerController = fromPlayerController;
        this.toPlayerController = toPlayerController;

        descriptionText.text = descriptionText.text.Replace(
            "{0}", $"ª±®a {toPlayerController.Number + 1}");

        if (blockController.Data.Type == BlockType.Land) {
            descriptionText.text = descriptionText.text.Replace(
                "{1}", $"¢D{blockController.Data.Tolls[toPlayerController.Lands[blockController.Number]]:#,#}");
        } else {
            descriptionText.text = descriptionText.text.Replace(
                "{1}", $"¢D{blockController.Data.Tolls[toPlayerController.Facilities.Count - 1]:#,#}");
        }
    }

    public void Confirm() {
        PayMoneyComplete?.Invoke(this, new(blockController, fromPlayerController, toPlayerController));
    }
}
