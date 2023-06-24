using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int number { get; set; }

    private GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainCamera.GetComponent<MotivationController>().Move(1.5f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            gameObject.transform.position -= new Vector3(0f, 0.5f, 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameObject.transform.position += new Vector3(0f, 0.5f, 0f);

            Vector3 pos = gameObject.transform.position;
            mainCamera.GetComponent<MotivationController>().Move(new Vector3(pos.x, 15f, pos.z - 10f), 1.5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
    }
}
