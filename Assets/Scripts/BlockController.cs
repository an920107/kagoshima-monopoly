using System;
using System.Text.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro descriptionText;

    public int Number { get; set; }
    public BlockData Data { get; set; }

    private GameObject mainCamera;

    void Start() {
        mainCamera = GameObject.Find("MainCamera");
        titleText = Instantiate(titleText);
        descriptionText = Instantiate(descriptionText);
        titleText.gameObject.transform.SetParent(gameObject.transform);
        descriptionText.gameObject.transform.SetParent(gameObject.transform);
        titleText.gameObject.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        descriptionText.gameObject.transform.localPosition = new Vector3(0f, 0.6f, 0f);

        if ((Data.Location & (BlockLocation.Left | BlockLocation.LeftBottom)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 90);
        if ((Data.Location & (BlockLocation.Top | BlockLocation.LeftTop)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 180);
        if ((Data.Location & (BlockLocation.Right | BlockLocation.RightTop)) > 0)
            gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), 270);

        if ((Data.Location & (
                BlockLocation.RightBottom |
                BlockLocation.LeftBottom |
                BlockLocation.LeftTop |
                BlockLocation.RightTop)) > 0) {
            titleText.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), 45);
            descriptionText.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), 45); 
        }
        descriptionText.gameObject.transform.Translate(new Vector3(0f, -0.5f, 0f), Space.Self);

        titleText.text = Data.Title;
        descriptionText.text = Data.Description;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            mainCamera.GetComponent<MotivationController>().MoveToInitialPosition(1.5f);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            gameObject.transform.position -= new Vector3(0f, 0.5f, 0f);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            gameObject.transform.position += new Vector3(0f, 0.5f, 0f);

            Vector3 pos = gameObject.transform.position;
            mainCamera.GetComponent<MotivationController>().MoveTo(new Vector3(pos.x, 15f, pos.z - 10f), 1.5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
    }
}

public class BlockData {
    [JsonPropertyName("type")]
    public string TypeString { get => Type.ToString(); set => Type = Enum.Parse<BlockType>(value); }

    [JsonPropertyName("location")]
    public string LocationString { get => Location.ToString(); set => Location = Enum.Parse<BlockLocation>(value); }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonIgnore]
    public BlockType Type { get; set; }

    [JsonIgnore]
    public BlockLocation Location { get; set; }
}

public enum BlockLocation {
    Left = 1,
    Right = 2,
    Top = 4,
    Bottom = 8,
    LeftTop = 16,
    RightTop = 32,
    LeftBottom = 64,
    RightBottom = 128
}

public enum BlockType {
    Land,
    Facility,
    Start,
    Event,
    Stop,
}