using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private List<GameObject> Panels;

    void OnEnable() {
        if (Panels != null && Panels.Count > 0)
            Panels[0].SetActive(true);
        for (int i = 1; i < Panels.Count; i++)
            Panels[i].SetActive(false);
    }

    public void ExitApplication() {
        Application.Quit();
    }
}
