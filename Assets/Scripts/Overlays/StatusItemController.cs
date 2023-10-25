using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusItemController : MonoBehaviour {

    [SerializeField] private Text title;
    [SerializeField] private Text price;

    public void SetUp(BlockController blockController) {
        title.text = blockController.Data.Title;
        price.text = $"¢D{blockController.Data.Price}";
    }
}
