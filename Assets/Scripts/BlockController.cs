using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshPro titleText;

    public int Number { get; set; }
    public BlockLocation Location { get; set; }
    public string Title { get => titleText.text; set => titleText.text = value; }

    private GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        titleText = Instantiate(titleText);
        titleText.gameObject.transform.parent = gameObject.transform;
        titleText.gameObject.transform.localPosition = new Vector3(0f, 1f, 0f);

        if ((Location & (BlockLocation.Left | BlockLocation.LeftDown)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 90);
        if ((Location & (BlockLocation.Up | BlockLocation.LeftUp)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 180);
        if ((Location & (BlockLocation.Right | BlockLocation.RightUp)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 270);

        if ((Location & (
                BlockLocation.RightDown |
                BlockLocation.LeftDown |
                BlockLocation.LeftUp |
                BlockLocation.RightUp)) > 0)
            GetComponentInChildren<TextMeshPro>().gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), 45);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainCamera.GetComponent<MotivationController>().MoveToInitialPosition(1.5f);
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
            mainCamera.GetComponent<MotivationController>().MoveTo(new Vector3(pos.x, 15f, pos.z - 10f), 1.5f);
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

public enum BlockLocation
{
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
    LeftUp = 16,
    RightUp = 32,
    LeftDown = 64,
    RightDown = 128
}

public enum BlockType
{
    Land,
    Facility,
    Event,
}