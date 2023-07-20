using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSettingController : MonoBehaviour {

    public int ColorIndex { get; set; }

    private ColorSettingController parentController;

    void Start() {
        parentController = GetComponentInParent<ColorSettingController>();
        ColorIndex = 0;
        Next();
    }

    public void Next() {
        EventSystem.current.SetSelectedGameObject(null);
        ColorIndex = parentController.GetNextColorIndex(ColorIndex);
        var colorTup = parentController.GetColorFromIndex(ColorIndex);
        gameObject.GetComponent<Image>().color = colorTup.Item1;
        gameObject.GetComponentInChildren<Text>().color = colorTup.Item2;
    }
}
