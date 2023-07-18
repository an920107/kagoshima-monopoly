using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingController : MonoBehaviour {

    [SerializeField] private GameObject colorSettingPrefab;

    private List<GameObject> colorSettings = new();

    public int PlayerNumber { get; private set; }

    void Awake() {
        PlayerNumber = 0;
    }

    void OnEnable() {
        for (int i = 0; i < PlayerNumber; i++) {
            var prefab = Instantiate(colorSettingPrefab);
            colorSettings.Add(prefab);
            prefab.transform.SetParent(
                GetComponentInChildren<HorizontalLayoutGroup>().gameObject.transform, false);
        }
    }

    void OnDisable() {
        PlayerNumber = 0;
        foreach (var prefab in colorSettings) {
            Destroy(prefab);
        }
    }

    public void SetPlayerNumber(int num) {
        PlayerNumber = num;
    }
}
