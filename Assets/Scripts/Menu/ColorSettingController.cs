using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSettingController : MonoBehaviour {

    [SerializeField] private GameObject colorSettingPrefab;

    private List<GameObject> colorSettings = new();
    private bool isReady = false;

    private readonly List<Tuple<Color, Color>> colorsPool = new() {
        new(new Color(1, 0, 0), new Color(0.9f, 0.9f, 0.9f)),
        new(new Color(1, 1, 0), new Color(0.2f, 0.2f, 0.2f)),
        new(new Color(0, 1, 0), new Color(0.2f, 0.2f, 0.2f)),
        new(new Color(0, 1, 1), new Color(0.2f, 0.2f, 0.2f)),
        new(new Color(0, 0, 1), new Color(0.9f, 0.9f, 0.9f)),
        new(new Color(1, 0, 1), new Color(0.2f, 0.2f, 0.2f)),
        new(new Color(0, 0, 0), new Color(0.9f, 0.9f, 0.9f)),
    };

    public int PlayerNumber { get; private set; }

    void Awake() {
        PlayerNumber = 0;
    }

    void Update() {
        if (isReady) {
            isReady = false;
            for (int i = 0; i < PlayerNumber; i++) {
                var prefab = Instantiate(colorSettingPrefab);
                colorSettings.Add(prefab);
                prefab.transform.SetParent(
                    GetComponentInChildren<HorizontalLayoutGroup>().gameObject.transform, false);
                prefab.GetComponentInChildren<Text>().text = "ª±®a " + (i + 1).ToString();
            }
        }
    }

    void OnDisable() {
        isReady = false;
        PlayerNumber = 0;
        foreach (var prefab in colorSettings)
            Destroy(prefab);
        colorSettings.Clear();
    }

    public void SetPlayerNumber(int num) {
        PlayerNumber = num;
        isReady = true;
    }

    public int GetNextColorIndex(int current) {
        bool returnFlag = true;
        for (int i = current; ; i++) {
            if (i == colorsPool.Count)
                i = 0;
            foreach (var cs in colorSettings) {
                if (i == cs.GetComponentInChildren<PlayerSettingController>().ColorIndex)
                    returnFlag = false;
            }
            if (returnFlag)
                return i;
            returnFlag = true;
        }
    }

    public Tuple<Color, Color> GetColorFromIndex(int index) {
        return colorsPool[index];
    }
}
